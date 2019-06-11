#pragma once
#include "resource.h"       // main symbols
#include <iostream>
#include "CorProfilerCallbackBase.h"
#include "VisualProfilerBackend_i.h"
#include "TracingCallTree.h"
#include <fstream>
#include "SerializationBuffer.h"
#include "VisualProfilerAccess.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;
using namespace std;

class ATL_NO_VTABLE CTracingProfiler :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CTracingProfiler, &CLSID_TracingProfiler>,
	public ITracingProfiler,
	public CorProfilerCallbackBase
{
public:
	CTracingProfiler()
	{	
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_TRACINGPROFILER)

	DECLARE_NOT_AGGREGATABLE(CTracingProfiler)

	BEGIN_COM_MAP(CTracingProfiler)
		COM_INTERFACE_ENTRY(ITracingProfiler)
		COM_INTERFACE_ENTRY(ICorProfilerCallback)
		COM_INTERFACE_ENTRY(ICorProfilerCallback2)
		COM_INTERFACE_ENTRY(ICorProfilerCallback3)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	
	HRESULT FinalConstruct(){ 
		_profilerAccess.StartListeningAsync();
		return S_OK; 
	}

	void FinalRelease()
	{
		_profilerAccess.FinishProfiling();
		SerializationBuffer b;
		ModuleMetadata::SerializeMetadata(&b);
		
	}

	virtual HRESULT STDMETHODCALLTYPE Shutdown(){
				return S_OK;
	}

public:
	virtual HRESULT STDMETHODCALLTYPE Initialize(IUnknown *pICorProfilerInfoUnk) ;
	virtual HRESULT STDMETHODCALLTYPE ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId) ;
	virtual HRESULT STDMETHODCALLTYPE ThreadCreated(ThreadID threadId) ;
	virtual HRESULT STDMETHODCALLTYPE ThreadDestroyed(ThreadID threadId) ;
	virtual HRESULT STDMETHODCALLTYPE RuntimeThreadSuspended(ThreadID threadId) ;
	virtual HRESULT STDMETHODCALLTYPE RuntimeThreadResumed(ThreadID threadId) ;
	virtual HRESULT STDMETHODCALLTYPE ExceptionSearchFunctionEnter(FunctionID functionId);
	virtual HRESULT STDMETHODCALLTYPE ExceptionSearchCatcherFound(FunctionID functionId);

	static UINT_PTR STDMETHODCALLTYPE FunctionIdMapper(FunctionID functionId,void * clientData,  BOOL *pbHookFunction);

private:
	static void __stdcall FunctionEnterHook(FunctionIDOrClientID functionIDOrClientID);
	static void __stdcall FunctionLeaveHook(FunctionIDOrClientID functionIDOrClientID);

	//thread local storage static variables
	static __declspec(thread)  TracingCallTree * _pTracingCallTree;
	static __declspec(thread)  UINT _exceptionSearchCount;

	VisualProfilerAccess<TracingCallTree> _profilerAccess;
};

OBJECT_ENTRY_AUTO(__uuidof(TracingProfiler), CTracingProfiler)
