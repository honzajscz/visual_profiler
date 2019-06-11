#include "stdafx.h"
#include "SamplingProfiler.h"


HRESULT STDMETHODCALLTYPE CSamplingProfiler::Initialize(IUnknown *pICorProfilerInfoUnk) {
	CorProfilerCallbackBase::Initialize(pICorProfilerInfoUnk);

	DWORD mask =  COR_PRF_MONITOR_THREADS |  COR_PRF_ENABLE_STACK_SNAPSHOT ;
	pProfilerInfo->SetEventMask(mask);
//	Beep(2000, 200);
	_stackWalker = shared_ptr<StackWalker>(new StackWalker(pProfilerInfo, 1));
	_stackWalker->StartSampling();
	return S_OK;
}

HRESULT STDMETHODCALLTYPE CSamplingProfiler::Shutdown(){
	_stackWalker->StopSampling();
	return S_OK;
}

HRESULT STDMETHODCALLTYPE  CSamplingProfiler::ThreadCreated(ThreadID threadId) {
	_stackWalker->RegisterThread(threadId);
	return S_OK;
}

HRESULT STDMETHODCALLTYPE  CSamplingProfiler::ThreadDestroyed(ThreadID threadId) {
	_stackWalker->DeregisterThread(threadId);
	return S_OK;
}

HRESULT STDMETHODCALLTYPE  CSamplingProfiler::ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId) {
	return S_OK;
}


