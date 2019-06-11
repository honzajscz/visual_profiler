#include "StdAfx.h"
#include "StackWalker.h"


StackWalker::StackWalker(ICorProfilerInfo3 * pProfilerInfo, UINT samplingPeriodMs):_pProfilerInfo(pProfilerInfo), _samplingPeriodMs(samplingPeriodMs){}

void StackWalker::RegisterThread(ThreadID threadId){
	_criticalSection.Enter();
	{
		_registeredThreadIds.insert(threadId);
	}
	_criticalSection.Leave();
	SamplingCallTree * callTree = SamplingCallTree::AddThread(threadId, _pProfilerInfo);
}

void StackWalker::DeregisterThread(ThreadID threadId){
	_criticalSection.Enter();
	{
		_registeredThreadIds.erase(threadId);
	}
	_criticalSection.Leave();

	SamplingCallTree * callTree = SamplingCallTree::GetCallTree(threadId);
	callTree->GetTimer()->Stop();
	callTree->UpdateUserAndKernelModeDurations();
	callTree->RefreshCallTreeBuffer(true);
}

DWORD WINAPI StackWalker::Sample(void * data){
	StackWalker * pThis = (StackWalker *) data;
	vector<FunctionID> functionIdsSnapshot;
	HRESULT hr = 0;

	while(pThis->_continueSampling){
		pThis->_criticalSection.Enter();
		{
			for(set<ThreadID>::iterator it = pThis->_registeredThreadIds.begin(); it != pThis->_registeredThreadIds.end(); it++){
				ThreadID threadId = *it;
				functionIdsSnapshot.clear();
				hr = pThis->_pProfilerInfo->DoStackSnapshot(threadId,&MakeFrameWalk,0, &functionIdsSnapshot,0,0);
				if(SUCCEEDED(hr)){
					SamplingCallTree * pCallTree = SamplingCallTree::GetCallTree(threadId);
					pCallTree->ProcessSamples(&functionIdsSnapshot);
					pCallTree->RefreshCallTreeBuffer();
				}
			}
		}

		pThis->_criticalSection.Leave();

		if(!pThis->_continueSampling) break;
		Sleep(pThis->_samplingPeriodMs);
	}

	DWORD success = 1;
	return success;
}

void StackWalker::DeregisterAllRegisteredThreads(){
	UINT registeredThreadIdsCount = _registeredThreadIds.size();
	ThreadID * threadIds = new ThreadID[registeredThreadIdsCount];

	UINT i = 0;
	for(set<ThreadID>::iterator it = _registeredThreadIds.begin(); it != _registeredThreadIds.end(); it++){
		ThreadID threadId = *it;
		threadIds[i++] = threadId;
	}

	for(i = 0; i < registeredThreadIdsCount; i++){
		ThreadID threadId = threadIds[i];
		DeregisterThread(threadId);
	}
}

HRESULT __stdcall  StackWalker::MakeFrameWalk(FunctionID functionId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData){
	vector<FunctionID>  * functionIds = (vector<FunctionID>  *) clientData;
	functionIds->push_back(functionId);
	return S_OK;
}

void StackWalker::StartSampling(){
	bool alreadyStarted = _continueSampling == true;
	if( alreadyStarted) return;
	_continueSampling = true;
	_samplingThreadHandle = CreateThread(NULL, 0, &Sample,this, 0, &_samplingThreadId);
}

void StackWalker::StopSampling(){
	_continueSampling = false;
	DeregisterAllRegisteredThreads();
	WaitForSingleObject(_samplingThreadHandle, INFINITE);
}
