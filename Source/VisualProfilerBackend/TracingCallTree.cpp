#include "StdAfx.h"
#include "TracingCallTree.h"

TracingCallTree::TracingCallTree(ThreadID threadId, ICorProfilerInfo3 * profilerInfo):CallTreeBase<TracingCallTree, TracingCallTreeElem>(threadId, profilerInfo){
	_pActiveCallTreeElem = & _rootCallTreeElem;	
}

void TracingCallTree::FunctionEnter(FunctionID functionId){
	TracingCallTreeElem * prevActiveElem = _pActiveCallTreeElem;
	TracingCallTreeElem * nextActiveElem = _pActiveCallTreeElem->GetChildTreeElem(functionId);
			
	_timer.GetElapsedTimeIn100NanoSeconds(&nextActiveElem->LastEnterTimeStampHns);
	UpdateCycleTime(prevActiveElem, nextActiveElem);	

	nextActiveElem->EnterCount++;

	_pActiveCallTreeElem = nextActiveElem;
	RefreshCallTreeBuffer();
}

void TracingCallTree::FunctionLeave(){
	TracingCallTreeElem * prevActiveElem = _pActiveCallTreeElem;
	TracingCallTreeElem * nextActiveElem = _pActiveCallTreeElem->pParent;
	
	ULONGLONG actualTimeStamp;
	_timer.GetElapsedTimeIn100NanoSeconds(&actualTimeStamp);
	ULONGLONG funcitonDuration = actualTimeStamp - prevActiveElem->LastEnterTimeStampHns;
	prevActiveElem->WallClockDurationHns += funcitonDuration;
	
	prevActiveElem->LeaveCount++;

	UpdateCycleTime(prevActiveElem, nextActiveElem);

	_pActiveCallTreeElem = nextActiveElem;

	RefreshCallTreeBuffer();
}

void TracingCallTree::UpdateCycleTime(TracingCallTreeElem * prevActiveElem, TracingCallTreeElem* nextActiveElem){
	BOOL suc = QueryThreadCycleTime(_osThreadHandle,&nextActiveElem->LastCycleTime);
	CheckError2(suc);
	prevActiveElem->CycleTime += nextActiveElem->LastCycleTime - prevActiveElem->LastCycleTime;
}

TracingCallTreeElem * TracingCallTree::GetActiveCallTreeElem(){
	return _pActiveCallTreeElem;
}

void TracingCallTree::Serialize(SerializationBuffer * buffer){
	buffer->SerializeProfilingDataTypes(ProfilingDataTypes_Tracing);
	buffer->SerializeThreadId(_threadId);
	UpdateUserAndKernelModeDurations();
	buffer->SerializeULONGLONG(KernelModeDurationHns);
	buffer->SerializeULONGLONG(UserModeDurationHns);
	SerializeCallTreeElem(&_rootCallTreeElem, buffer);
}

void TracingCallTree::SerializeCallTreeElem(TracingCallTreeElem * elem, SerializationBuffer * buffer){
	buffer->SerializeFunctionId(elem->FunctionId);
	buffer->SerializeUINT(elem->EnterCount);
	buffer->SerializeUINT(elem->LeaveCount);

	bool functionNotFinished = elem->EnterCount != elem->LeaveCount;
	if(functionNotFinished){
		ULONGLONG actualTimeStamp;
		_timer.GetElapsedTimeIn100NanoSeconds(&actualTimeStamp);
		ULONGLONG funcitonDuration = actualTimeStamp - elem->LastEnterTimeStampHns + elem->WallClockDurationHns;
		buffer->SerializeULONGLONG(funcitonDuration); 
	}else{
		buffer->SerializeULONGLONG(elem->WallClockDurationHns);
	}
	
	buffer->SerializeULONGLONG(elem->CycleTime);
	
	map<FunctionID, shared_ptr<TracingCallTreeElem>> * pChildrenMap = elem->GetChildrenMap();
	UINT childrenSize = pChildrenMap->size();
	buffer->SerializeUINT(childrenSize);

	map<FunctionID, shared_ptr<TracingCallTreeElem>>::iterator it = pChildrenMap->begin();
	for(; it != pChildrenMap->end(); it++){
		TracingCallTreeElem * childElem = it->second.get();
		SerializeCallTreeElem(childElem, buffer);
	}
}
