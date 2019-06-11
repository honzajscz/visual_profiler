#pragma once
#include "stdafx.h"
#include <cor.h>
#include <corprof.h>
#include <string>
#include "MetadataBase.h"
#define VISUAL_PROFILER_TARGET_ATTR L"VisualProfiler.ProfilingEnabledAttribute"

class AssemblyMetadata : public MetadataBase<AssemblyID, AssemblyMetadata>
{

public:
	AssemblyID AssemblyId;
	mdAssembly AssemblyMdToken;
	wstring Name;
	AppDomainID AppDomainId;
	ModuleID MetadataModuleId;

	
	AssemblyMetadata(AssemblyID assemblyId, ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2 * pMetadataImport);
	virtual void Serialize(SerializationBuffer * buffer);
	
};

