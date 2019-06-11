#include "StdAfx.h"
#include "ClassMetadata.h"


ClassMetadata::ClassMetadata(ClassID classId, mdTypeDef classMdToken, ModuleID moduleId, mdToken moduleMdToken, ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2 * pMetadataImport, bool isGeneric)
{
    HRESULT hr;
	this->IsGeneric = isGeneric;
	this->ClassId = (classId != 0) ?classId : classMdToken;
	this->ClassMdToken = classMdToken;
	
	WCHAR name[NAME_BUFFER_SIZE];
	hr = pMetadataImport->GetTypeDefProps(this->ClassMdToken, name, NAME_BUFFER_SIZE, 0, 0, 0);
	CheckError(hr);
	this->Name.append(name);

	shared_ptr<ModuleMetadata> pModuleMetadata ;
	if(ModuleMetadata::ContainsCache(moduleId)){	
		pModuleMetadata = ModuleMetadata::GetById(moduleId);
	}
	else{
		pModuleMetadata = shared_ptr<ModuleMetadata>(new ModuleMetadata(moduleId, moduleMdToken, pProfilerInfo, pMetadataImport));
		ModuleMetadata::AddMetadata(moduleId, pModuleMetadata);
	}

	this->pParentModuleMetadata = pModuleMetadata;
	_isProfilingEnabled = pModuleMetadata->IsProfilingEnabled();
}

wstring ClassMetadata::ToString(){
	wstring wholeName;
	wholeName.append(this->Name);
	if(this->IsGeneric){
		wholeName.append(L"<>");
	}
	return wholeName;
}

void ClassMetadata::Serialize(SerializationBuffer * buffer){
	buffer->SerializeMetadataTypes(MetadataTypes_ClassMedatada);
	buffer->SerializeMetadataId(ClassId);
	buffer->SerializeMdToken(ClassMdToken);
	buffer->SerializeWString(Name);
	buffer->SerializeBool(IsGeneric);
	buffer->SerializeMetadataId(pParentModuleMetadata->ModuleId);
}

   
