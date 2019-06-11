#pragma once
#include <cor.h>
#include <corprof.h>
#include <string>
#include <vector>
#include "MetadataBase.h"
#include "ModuleMetadata.h"

using namespace std;

class ClassMetadata : public MetadataBase<ClassID, ClassMetadata>
{
public:
	ClassID ClassId;
	mdTypeDef ClassMdToken;
	wstring Name;
	bool IsGeneric;
	shared_ptr<ModuleMetadata> pParentModuleMetadata;

	ClassMetadata(ClassID classId, mdTypeDef classMdToken, ModuleID moduleId, mdToken moduleMdToken, ICorProfilerInfo3 * pProfilerInfo, IMetaDataImport2* pMetadataImport,bool isGeneric);
	wstring ToString();
	void Serialize(SerializationBuffer * buffer);
};

