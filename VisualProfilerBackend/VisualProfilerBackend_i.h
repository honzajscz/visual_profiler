

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Sat Dec 31 06:24:34 2011
 */
/* Compiler settings for VisualProfilerBackend.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __VisualProfilerBackend_i_h__
#define __VisualProfilerBackend_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ISamplingProfiler_FWD_DEFINED__
#define __ISamplingProfiler_FWD_DEFINED__
typedef interface ISamplingProfiler ISamplingProfiler;
#endif 	/* __ISamplingProfiler_FWD_DEFINED__ */


#ifndef __ITracingProfiler_FWD_DEFINED__
#define __ITracingProfiler_FWD_DEFINED__
typedef interface ITracingProfiler ITracingProfiler;
#endif 	/* __ITracingProfiler_FWD_DEFINED__ */


#ifndef __SamplingProfiler_FWD_DEFINED__
#define __SamplingProfiler_FWD_DEFINED__

#ifdef __cplusplus
typedef class SamplingProfiler SamplingProfiler;
#else
typedef struct SamplingProfiler SamplingProfiler;
#endif /* __cplusplus */

#endif 	/* __SamplingProfiler_FWD_DEFINED__ */


#ifndef __TracingProfiler_FWD_DEFINED__
#define __TracingProfiler_FWD_DEFINED__

#ifdef __cplusplus
typedef class TracingProfiler TracingProfiler;
#else
typedef struct TracingProfiler TracingProfiler;
#endif /* __cplusplus */

#endif 	/* __TracingProfiler_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ISamplingProfiler_INTERFACE_DEFINED__
#define __ISamplingProfiler_INTERFACE_DEFINED__

/* interface ISamplingProfiler */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_ISamplingProfiler;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("19840906-C001-0000-0001-000000000001")
    ISamplingProfiler : public IUnknown
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct ISamplingProfilerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ISamplingProfiler * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ISamplingProfiler * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ISamplingProfiler * This);
        
        END_INTERFACE
    } ISamplingProfilerVtbl;

    interface ISamplingProfiler
    {
        CONST_VTBL struct ISamplingProfilerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISamplingProfiler_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ISamplingProfiler_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ISamplingProfiler_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ISamplingProfiler_INTERFACE_DEFINED__ */


#ifndef __ITracingProfiler_INTERFACE_DEFINED__
#define __ITracingProfiler_INTERFACE_DEFINED__

/* interface ITracingProfiler */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_ITracingProfiler;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("19840906-C001-0000-0001-000000000002")
    ITracingProfiler : public IUnknown
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct ITracingProfilerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITracingProfiler * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITracingProfiler * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITracingProfiler * This);
        
        END_INTERFACE
    } ITracingProfilerVtbl;

    interface ITracingProfiler
    {
        CONST_VTBL struct ITracingProfilerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITracingProfiler_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITracingProfiler_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITracingProfiler_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITracingProfiler_INTERFACE_DEFINED__ */



#ifndef __VisualProfilerBackendLib_LIBRARY_DEFINED__
#define __VisualProfilerBackendLib_LIBRARY_DEFINED__

/* library VisualProfilerBackendLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_VisualProfilerBackendLib;

EXTERN_C const CLSID CLSID_SamplingProfiler;

#ifdef __cplusplus

class DECLSPEC_UUID("19840906-C001-0000-000C-000000000001")
SamplingProfiler;
#endif

EXTERN_C const CLSID CLSID_TracingProfiler;

#ifdef __cplusplus

class DECLSPEC_UUID("19840906-C001-0000-000C-000000000002")
TracingProfiler;
#endif
#endif /* __VisualProfilerBackendLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


