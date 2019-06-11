#include "StdAfx.h"
#include "SamplingCallTree.h"
#include <iostream>

SamplingCallTree::SamplingCallTree(ThreadID threadId,ICorProfilerInfo3 * profilerInfo):CallTreeBase<SamplingCallTree, SamplingCallTreeElem>(threadId, profilerInfo){}


void SamplingCallTree::ProcessSamples(vector<FunctionID> * functionIdsSnapshot){
	SamplingCallTreeElem * treeElem = &_rootCallTreeElem;
	bool isProfilingEnabledOnElem = false;
	for(vector<FunctionID>::reverse_iterator it = functionIdsSnapshot->rbegin(); it < functionIdsSnapshot->rend(); it++){
		FunctionID functionId = *it;
		if(functionId == 0)
			continue;

		shared_ptr<MethodMetadata> pMethodMetadata = MethodMetadata::GetById(functionId);
		if(pMethodMetadata == NULL){
			pMethodMetadata = shared_ptr<MethodMetadata>(new MethodMetadata(functionId, _profilerInfo));
			MethodMetadata::AddMetadata(functionId, pMethodMetadata);
		}
	
		if(pMethodMetadata->GetDefiningAssembly()->IsProfilingEnabled()){
			isProfilingEnabledOnElem = true;
			treeElem = treeElem->GetChildTreeElem(functionId);
		}else{
			isProfilingEnabledOnElem = false;
		}
	}

#pragma region waitJoinSleep
/*
		FILETIME dummy, userNow, kernelNow;
			GetThreadTimes(OsThreadHandle,&dummy,&dummy,&userNow,&kernelNow);
			bool kernelChanged =false, userchanged = false;
	
			if(user.dwHighDateTime != userNow.dwHighDateTime && user.dwLowDateTime != userNow.dwLowDateTime){
				user = userNow;
				userchanged = true;
		}
			
			if(kernel.dwHighDateTime != kernelNow.dwHighDateTime && kernel.dwLowDateTime != kernelNow.dwLowDateTime){
				kernel = kernelNow;
				kernelChanged = true;
		}

			ULONG64 ticks;
			QueryThreadCycleTime(OsThreadHandle, &ticks);
			cout << "threadID=" << _threadId << ", ticks=" << ticks << endl;
			printFT(&userNow);
			printFT(&kernelNow);
			cout << "--------------" <<endl;
	if(!(userchanged || kernelChanged))
		return;
	}*/
#pragma endregion
	bool wasProfilingEnabledOnStackTopFrame = isProfilingEnabledOnElem;
	if(wasProfilingEnabledOnStackTopFrame){
		treeElem->StackTopOccurrenceCount++;
	}else{
		treeElem->LastProfiledFrameInStackCount++;
	}
}


void SamplingCallTree::ToString(wstringstream & wsout){
	wsout << "Thread Id = " << _threadId << ", Number of stack divisions = " << _rootCallTreeElem.GetChildrenMap()->size() <<  endl ;

	double durationSec = _timer.GetElapsedTimeIn100NanoSeconds()/1e7;
	double userModeSec = UserModeDurationHns/1e7;
	double kernelModeSec = KernelModeDurationHns/1e7;
	wsout << L"Twc=" << durationSec << L"s,Tum=" << userModeSec << L"s,Tkm=" << kernelModeSec << "s" << endl;

	_rootCallTreeElem.ToString(wsout);
}

void SamplingCallTree::Serialize(SerializationBuffer * buffer){
	buffer->SerializeProfilingDataTypes(ProfilingDataTypes_Sampling);
	buffer->SerializeThreadId(_threadId);
	
	ULONGLONG wallClockTime;
	_timer.GetElapsedTimeIn100NanoSeconds(&wallClockTime);
	buffer->SerializeULONGLONG(wallClockTime);
	
	UpdateUserAndKernelModeDurations();
	buffer->SerializeULONGLONG(KernelModeDurationHns);
	buffer->SerializeULONGLONG(UserModeDurationHns);

	SerializeCallTreeElem(&_rootCallTreeElem, buffer);
}

void SamplingCallTree::SerializeCallTreeElem(SamplingCallTreeElem * elem, SerializationBuffer * buffer){
	buffer->SerializeFunctionId(elem->FunctionId);
	buffer->SerializeUINT(elem->StackTopOccurrenceCount);
	buffer->SerializeUINT(elem->LastProfiledFrameInStackCount);
	
	map<FunctionID, shared_ptr<SamplingCallTreeElem>> * childrenMap =  elem->GetChildrenMap();
	UINT childrenSize = childrenMap->size();
	buffer->SerializeUINT(childrenSize);

	map<FunctionID, shared_ptr<SamplingCallTreeElem>>::iterator it = childrenMap->begin();
	
	for(; it != childrenMap->end(); it++){
		SamplingCallTreeElem * childElem = it->second.get();
		SerializeCallTreeElem(childElem, buffer);	
	}
}