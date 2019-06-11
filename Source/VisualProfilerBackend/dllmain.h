// dllmain.h : Declaration of module class.

class CVisualProfilerBackendModule : public ATL::CAtlDllModuleT< CVisualProfilerBackendModule >
{
public :
	DECLARE_LIBID(LIBID_VisualProfilerBackendLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_VISUALPROFILERBACKEND, "{13951F2B-57E1-4D41-AF0A-BFFB5F88E87B}")
};

extern class CVisualProfilerBackendModule _AtlModule;
