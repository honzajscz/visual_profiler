#include "StdAfx.h"
#include "ModuleMetadata.h"
#include "MetadataBase.h"

ModuleMetadata::ModuleMetadata(ModuleID moduleId, mdToken moduleMdToken, ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2* pMetadataImport)
{
	this->ModuleId = moduleId;
	this->ModuleMdToken = moduleMdToken;
	HRESULT hr;
	WCHAR fileName[NAME_BUFFER_SIZE];
	AssemblyID assemblyId;
	hr = pProfilerInfo->GetModuleInfo(moduleId, &this->BaseLoadAddress,NAME_BUFFER_SIZE,0,fileName, &assemblyId);
	CheckError(hr);

	this->FileName.append(fileName);

	shared_ptr<AssemblyMetadata> pAssemblyMetadata;
	if(AssemblyMetadata::ContainsCache(assemblyId)){
		pAssemblyMetadata = AssemblyMetadata::GetById(assemblyId);
	}else{
		pAssemblyMetadata = shared_ptr<AssemblyMetadata>(new AssemblyMetadata(assemblyId, pProfilerInfo, pMetadataImport));
		AssemblyMetadata::AddMetadata(assemblyId, pAssemblyMetadata);
	}
	this->pAssemblyMetadata = pAssemblyMetadata;
	_isProfilingEnabled = pAssemblyMetadata->IsProfilingEnabled();
}

void ModuleMetadata::Serialize(SerializationBuffer * buffer){
	buffer->SerializeMetadataTypes(MetadataTypes_ModuleMedatada);
	buffer->SerializeMetadataId(ModuleId);
	buffer->SerializeMdToken(ModuleMdToken);
	/*SerializationBuffer b;
	b.SerializeWString(FileName);*/
	buffer->SerializeWString(FileName);
	buffer->SerializeMetadataId(pAssemblyMetadata->AssemblyId);
}



