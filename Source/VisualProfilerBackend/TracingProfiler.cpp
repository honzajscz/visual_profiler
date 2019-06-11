// TracingProfiler.cpp : Implementation of CTracingProfiler

#include "stdafx.h"
#include "TracingProfiler.h"
#include <iostream>
#include "TracingCallTree.h"

CTracingProfiler * tracingProfiler;

__declspec(thread)  TracingCallTree * CTracingProfiler::_pTracingCallTree = 0;
__declspec(thread)  UINT CTracingProfiler::_exceptionSearchCount = 0;

void __stdcall CTracingProfiler::FunctionEnterHook(FunctionIDOrClientID functionIDOrClientID){
	_pTracingCallTree->FunctionEnter(functionIDOrClientID.functionID);
}

void __stdcall CTracingProfiler::FunctionLeaveHook(FunctionIDOrClientID functionIDOrClientID){
	_pTracingCallTree->FunctionLeave();
}

void  _declspec(naked) FunctionEnter3Naked(FunctionIDOrClientID functionIDOrClientID)
{
	__asm
	{
		push    ebp                 // Create a frame
		mov     ebp,esp
		pushad                      // Save registers
		mov     eax,[ebp+0x08]      // pass functionIDOrClientID parameter to a function
		push    eax;
		call    CTracingProfiler::FunctionEnterHook // call a function
		popad                       // Restore registers
		pop     ebp                 // Restore EBP
		ret     4					// Return to caller and ESP = ESP+4 to clean the argument
	}
}

void _declspec(naked) FunctionLeave3Naked(FunctionIDOrClientID functionIDOrClientID)
{

	__asm
	{
		push    ebp                 // Create a frame
		mov     ebp,esp
		pushad                      // Save registers
		mov     eax,[ebp+0x08]      // pass functionIDOrClientID parameter to a function
		push    eax;
		call    CTracingProfiler::FunctionLeaveHook // call a function
		popad                       // Restore registers
		pop     ebp                 // Restore EBP
		ret     4					// Return to caller and ESP = ESP+4 to clean the argument
	}
}

void _declspec(naked) FunctionTailcall3Naked(FunctionIDOrClientID functionIDOrClientID)
{
	__asm
	{
		int 3 //jump to debugger
		ret    4
	}
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::Initialize( IUnknown *pICorProfilerInfoUnk) {
	CorProfilerCallbackBase::Initialize(pICorProfilerInfoUnk);
	DWORD mask = COR_PRF_MONITOR_ENTERLEAVE |  COR_PRF_MONITOR_THREADS | COR_PRF_MONITOR_EXCEPTIONS | COR_PRF_DISABLE_INLINING;      
	this->pProfilerInfo->SetEventMask(mask);

	FunctionEnter3* enterFunction = &FunctionEnter3Naked;
	FunctionLeave3* leaveFunction = &FunctionLeave3Naked;
	FunctionTailcall3* tailcallFuntion = &FunctionTailcall3Naked;
	this->pProfilerInfo->SetFunctionIDMapper2(&FunctionIdMapper, this);
	this->pProfilerInfo->SetEnterLeaveFunctionHooks3(enterFunction, leaveFunction , tailcallFuntion);

	tracingProfiler = this;
	//Beep(4000, 100);
	return S_OK;
}

UINT_PTR STDMETHODCALLTYPE CTracingProfiler::FunctionIdMapper(FunctionID functionId, void * clientData, BOOL *pbHookFunction)
{	 
	CTracingProfiler * profilerBase = (CTracingProfiler *) clientData;

	shared_ptr<MethodMetadata> pMethodMetadata;
	if(MethodMetadata::ContainsCache(functionId)){
		pMethodMetadata = MethodMetadata::GetById(functionId);
	}else{
		pMethodMetadata = shared_ptr<MethodMetadata>(new MethodMetadata(functionId, profilerBase->pProfilerInfo));
		MethodMetadata::AddMetadata(functionId, pMethodMetadata);
	}

	*pbHookFunction = pMethodMetadata->GetDefiningAssembly()->IsProfilingEnabled();
	//*pbHookFunction = true; //uncomment to track all CLR functions
	UINT_PTR internalCLRFunctionKey = functionId;
	return internalCLRFunctionKey;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::ThreadCreated(ThreadID threadId){
	_pTracingCallTree = TracingCallTree::AddThread(threadId, pProfilerInfo);
	_pTracingCallTree->GetTimer()->Start();

	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::ThreadDestroyed(ThreadID threadId){
	TracingCallTreeElem * activeElem = _pTracingCallTree->GetActiveCallTreeElem();
	while(!activeElem->IsRootElem()){
		_pTracingCallTree->FunctionLeave();
		activeElem = _pTracingCallTree->GetActiveCallTreeElem();
	}
	_pTracingCallTree->RefreshCallTreeBuffer(true);
	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::RuntimeThreadSuspended(ThreadID threadId){
	_pTracingCallTree->GetTimer()->Stop();
	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::RuntimeThreadResumed(ThreadID threadId){
	_pTracingCallTree->GetTimer()->Start();
	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId){
	bool needOSThreadInitialization = _pTracingCallTree == NULL || _pTracingCallTree->GetThreadId() != managedThreadId;
	if(needOSThreadInitialization){
		_pTracingCallTree =  TracingCallTree::GetCallTree(managedThreadId);
		//HANDLE osThreadHandle = GetCurrentThread();
		//_pTracingCallTree->SetOSThreadHandle(osThreadHandle);

		//update thread kernel and user mode time stamps when a new os thread is assigned to the managed thread 
		/*TracingCallTreeElem * pCallTreeElem = _pTracingCallTree->GetActiveCallTreeElem();
		if(pCallTreeElem != NULL){
			while(!pCallTreeElem->IsRootElem()){
				FILETIME dummy;
				GetThreadTimes(_pTracingCallTree->GetOSThreadHandle(),&dummy, &dummy, &pCallTreeElem->LastEnterKernelModeTimeStamp, &pCallTreeElem->LastEnterUserModeTimeStamp);
				pCallTreeElem = pCallTreeElem->pParent;
			}
		}*/
	}

	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::ExceptionSearchFunctionEnter(FunctionID functionId){
	_exceptionSearchCount++;
	return S_OK;
}

HRESULT STDMETHODCALLTYPE CTracingProfiler::ExceptionSearchCatcherFound(FunctionID functionId){
	for(UINT i = 0; i < _exceptionSearchCount - 1; i++){
		_pTracingCallTree->FunctionLeave();
	}
	_exceptionSearchCount = 0;
	return S_OK;
}

