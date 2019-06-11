#pragma once
#include "stdafx.h"
#include <cor.h>
#include <corprof.h>
#include <vector>
#include <string>
#include <map>
#include "AssemblyMetadata.h"
#include "ClassMetadata.h"
#include "MetadataBase.h"

using namespace std;
class MethodMetadata : public MetadataBase<FunctionID, MethodMetadata>
{
public:
	FunctionID FunctionId;
	mdMethodDef MethodMdToken;
	wstring Name;
	vector<wstring> Parameters;
	shared_ptr<ClassMetadata> pContainingTypeMetadata;

	MethodMetadata(FunctionID functionId, ICorProfilerInfo3 * pProfilerInfo);
	shared_ptr<AssemblyMetadata> GetDefiningAssembly();
	wstring ToString();
	void Serialize(SerializationBuffer * buffer);

private:
	
	IMetaDataImport2* _pMetaDataImport ;
		
	void InitializeFields(ICorProfilerInfo3 * pProfilerInfo);
	void InitializeContainingClass(ICorProfilerInfo3 * pProfilerInfo, mdTypeDef classMdToken);
	void PopulateParameters();
};

