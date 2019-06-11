#include "StdAfx.h"
#include "AssemblyMetadata.h"


AssemblyMetadata::AssemblyMetadata(AssemblyID assemblyId,ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2 * pMetadataImport):AssemblyId(assemblyId)
{
	HRESULT hr;

	WCHAR name[NAME_BUFFER_SIZE];
	hr = pProfilerInfo->GetAssemblyInfo(assemblyId, NAME_BUFFER_SIZE, 0, name, &this->AppDomainId, &this->MetadataModuleId);
	CheckError(hr);
	this->Name.append(name);
	
	IMetaDataAssemblyImport * pMetadataAssemblyImport = 0;
	hr = pMetadataImport->QueryInterface(IID_IMetaDataAssemblyImport, (void **)&pMetadataAssemblyImport);
	CheckError(hr);
	
	hr = pMetadataAssemblyImport->GetAssemblyFromScope(&this->AssemblyMdToken);
	CheckError(hr);

	BYTE * pBytes;
	ULONG  byteCount;
	hr = pMetadataImport->GetCustomAttributeByName(this->AssemblyMdToken, VISUAL_PROFILER_TARGET_ATTR, (const void**)&pBytes, &byteCount);
	bool visualProfilerTargetAttrApplied = hr == S_OK;
	if(!visualProfilerTargetAttrApplied){
		_isProfilingEnabled = false;
	}else{
		_isProfilingEnabled = true;
	}
}

void AssemblyMetadata::Serialize(SerializationBuffer * buffer){
	buffer->SerializeMetadataTypes(MetadataTypes_AssemblyMetadata);
	buffer->SerializeMetadataId(AssemblyId);
	buffer->SerializeMdToken(AssemblyMdToken);
	buffer->SerializeWString(Name);
	buffer->SerializeBool(IsProfilingEnabled());
}


