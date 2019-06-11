#pragma once
#include "stdafx.h"
#include <cor.h>
#include <corprof.h>
#include <string>
#include <map>
#include "MetadataBase.h"
#include "AssemblyMetadata.h"

using namespace std;

class ModuleMetadata : public MetadataBase<ModuleID, ModuleMetadata>
{
public:
	ModuleID ModuleId;
	mdToken ModuleMdToken;
	wstring FileName;
	shared_ptr<AssemblyMetadata> pAssemblyMetadata;
	LPCBYTE BaseLoadAddress;  

	ModuleMetadata(ModuleID moduleId, mdToken moduleMdToken, ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2* pMetadataImport);
	virtual void Serialize(SerializationBuffer * buffer);
};

