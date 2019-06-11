#include "StdAfx.h"
#include "CorProfilerCallbackBase.h"

//------------------------------------------------------------------ICorProfilerCallback3------------------------------------------------------------------
HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::InitializeForAttach( 
	/* [in] */ IUnknown *pCorProfilerInfoUnk,
	/* [in] */ void *pvClientData,
	/* [in] */ UINT cbClientData) {	return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ProfilerAttachComplete( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ProfilerDetachSucceeded( void) { return S_OK;  }

//------------------------------------------------------------------ICorProfilerCallback2------------------------------------------------------------------
HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ThreadNameChanged( 
	/* [in] */ ThreadID threadId,
	/* [in] */ ULONG cchName,
	/* [in] */ 
	__in_ecount_opt(cchName)  WCHAR name[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::GarbageCollectionStarted( 
	/* [in] */ int cGenerations,
	/* [size_is][in] */ BOOL generationCollected[  ],
	/* [in] */ COR_PRF_GC_REASON reason) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::SurvivingReferences( 
	/* [in] */ ULONG cSurvivingObjectIDRanges,
	/* [size_is][in] */ ObjectID objectIDRangeStart[  ],
	/* [size_is][in] */ ULONG cObjectIDRangeLength[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::GarbageCollectionFinished( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::FinalizeableObjectQueued( 
	/* [in] */ DWORD finalizerFlags,
	/* [in] */ ObjectID objectID) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RootReferences2( 
	/* [in] */ ULONG cRootRefs,
	/* [size_is][in] */ ObjectID rootRefIds[  ],
	/* [size_is][in] */ COR_PRF_GC_ROOT_KIND rootKinds[  ],
	/* [size_is][in] */ COR_PRF_GC_ROOT_FLAGS rootFlags[  ],
	/* [size_is][in] */ UINT_PTR rootIds[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::HandleCreated( 
	/* [in] */ GCHandleID handleId,
	/* [in] */ ObjectID initialObjectId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::HandleDestroyed( 
	/* [in] */ GCHandleID handleId) { return S_OK;  }

//------------------------------------------------------------------ICorProfilerCallback------------------------------------------------------------------
HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::Initialize( 
	/* [in] */ IUnknown *pICorProfilerInfoUnk) 
{ 
	HRESULT hr;

	hr = pICorProfilerInfoUnk->QueryInterface(IID_ICorProfilerInfo3, (void **)  (LPVOID*)&pProfilerInfo);
	if (FAILED(hr))
		return E_FAIL;

	return S_OK; 
}

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::Shutdown( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AppDomainCreationStarted( 
	/* [in] */ AppDomainID appDomainId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AppDomainCreationFinished( 
	/* [in] */ AppDomainID appDomainId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AppDomainShutdownStarted( 
	/* [in] */ AppDomainID appDomainId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AppDomainShutdownFinished( 
	/* [in] */ AppDomainID appDomainId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AssemblyLoadStarted( 
	/* [in] */ AssemblyID assemblyId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AssemblyLoadFinished( 
	/* [in] */ AssemblyID assemblyId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AssemblyUnloadStarted( 
	/* [in] */ AssemblyID assemblyId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::AssemblyUnloadFinished( 
	/* [in] */ AssemblyID assemblyId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ModuleLoadStarted( 
	/* [in] */ ModuleID moduleId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ModuleLoadFinished( 
	/* [in] */ ModuleID moduleId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ModuleUnloadStarted( 
	/* [in] */ ModuleID moduleId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ModuleUnloadFinished( 
	/* [in] */ ModuleID moduleId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ModuleAttachedToAssembly( 
	/* [in] */ ModuleID moduleId,
	/* [in] */ AssemblyID AssemblyId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ClassLoadStarted( 
	/* [in] */ ClassID classId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ClassLoadFinished( 
	/* [in] */ ClassID classId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ClassUnloadStarted( 
	/* [in] */ ClassID classId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ClassUnloadFinished( 
	/* [in] */ ClassID classId,
	/* [in] */ HRESULT hrStatus) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::FunctionUnloadStarted( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITCompilationStarted( 
	/* [in] */ FunctionID functionId,
	/* [in] */ BOOL fIsSafeToBlock) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITCompilationFinished( 
	/* [in] */ FunctionID functionId,
	/* [in] */ HRESULT hrStatus,
	/* [in] */ BOOL fIsSafeToBlock) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITCachedFunctionSearchStarted( 
	/* [in] */ FunctionID functionId,
	/* [out] */ BOOL *pbUseCachedFunction) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITCachedFunctionSearchFinished( 
	/* [in] */ FunctionID functionId,
	/* [in] */ COR_PRF_JIT_CACHE result) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITFunctionPitched( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::JITInlining( 
	/* [in] */ FunctionID callerId,
	/* [in] */ FunctionID calleeId,
	/* [out] */ BOOL *pfShouldInline) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ThreadCreated( 
	/* [in] */ ThreadID threadId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ThreadDestroyed( 
	/* [in] */ ThreadID threadId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ThreadAssignedToOSThread( 
	/* [in] */ ThreadID managedThreadId,
	/* [in] */ DWORD osThreadId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingClientInvocationStarted( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingClientSendingMessage( 
	/* [in] */ GUID *pCookie,
	/* [in] */ BOOL fIsAsync) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingClientReceivingReply( 
	/* [in] */ GUID *pCookie,
	/* [in] */ BOOL fIsAsync) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingClientInvocationFinished( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingServerReceivingMessage( 
	/* [in] */ GUID *pCookie,
	/* [in] */ BOOL fIsAsync) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingServerInvocationStarted( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingServerInvocationReturned( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RemotingServerSendingReply( 
	/* [in] */ GUID *pCookie,
	/* [in] */ BOOL fIsAsync) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::UnmanagedToManagedTransition( 
	/* [in] */ FunctionID functionId,
	/* [in] */ COR_PRF_TRANSITION_REASON reason) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ManagedToUnmanagedTransition( 
	/* [in] */ FunctionID functionId,
	/* [in] */ COR_PRF_TRANSITION_REASON reason) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeSuspendStarted( 
	/* [in] */ COR_PRF_SUSPEND_REASON suspendReason) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeSuspendFinished( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeSuspendAborted( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeResumeStarted( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeResumeFinished( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeThreadSuspended( 
	/* [in] */ ThreadID threadId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RuntimeThreadResumed( 
	/* [in] */ ThreadID threadId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::MovedReferences( 
	/* [in] */ ULONG cMovedObjectIDRanges,
	/* [size_is][in] */ ObjectID oldObjectIDRangeStart[  ],
	/* [size_is][in] */ ObjectID newObjectIDRangeStart[  ],
	/* [size_is][in] */ ULONG cObjectIDRangeLength[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ObjectAllocated( 
	/* [in] */ ObjectID objectId,
	/* [in] */ ClassID classId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ObjectsAllocatedByClass( 
	/* [in] */ ULONG cClassCount,
	/* [size_is][in] */ ClassID classIds[  ],
	/* [size_is][in] */ ULONG cObjects[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ObjectReferences( 
	/* [in] */ ObjectID objectId,
	/* [in] */ ClassID classId,
	/* [in] */ ULONG cObjectRefs,
	/* [size_is][in] */ ObjectID objectRefIds[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::RootReferences( 
	/* [in] */ ULONG cRootRefs,
	/* [size_is][in] */ ObjectID rootRefIds[  ]) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionThrown( 
	/* [in] */ ObjectID thrownObjectId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionSearchFunctionEnter( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionSearchFunctionLeave( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionSearchFilterEnter( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionSearchFilterLeave( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionSearchCatcherFound( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionOSHandlerEnter( 
	/* [in] */ UINT_PTR __unused) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionOSHandlerLeave( 
	/* [in] */ UINT_PTR __unused) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionUnwindFunctionEnter( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionUnwindFunctionLeave( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionUnwindFinallyEnter( 
	/* [in] */ FunctionID functionId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionUnwindFinallyLeave( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionCatcherEnter( 
	/* [in] */ FunctionID functionId,
	/* [in] */ ObjectID objectId) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionCatcherLeave( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::COMClassicVTableCreated( 
	/* [in] */ ClassID wrappedClassId,
	/* [in] */ REFGUID implementedIID,
	/* [in] */ void *pVTable,
	/* [in] */ ULONG cSlots) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::COMClassicVTableDestroyed( 
	/* [in] */ ClassID wrappedClassId,
	/* [in] */ REFGUID implementedIID,
	/* [in] */ void *pVTable) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionCLRCatcherFound( void) { return S_OK;  }

HRESULT STDMETHODCALLTYPE CorProfilerCallbackBase::ExceptionCLRCatcherExecute( void) { return S_OK;  }