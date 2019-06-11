#include "StdAfx.h"
#include "MethodMetadata.h"

MethodMetadata::MethodMetadata(FunctionID functionId, ICorProfilerInfo3 * pProfilerInfo):FunctionId(functionId)
{
	InitializeFields(pProfilerInfo);
	PopulateParameters();
}

void MethodMetadata::InitializeFields(ICorProfilerInfo3 * pProfilerInfo){
	HRESULT hr;

	hr = pProfilerInfo->GetTokenAndMetaDataFromFunction(this->FunctionId,IID_IMetaDataImport2,(LPUNKNOWN *) &this->_pMetaDataImport, &this->MethodMdToken);
	CheckError(hr);

	WCHAR methodName[NAME_BUFFER_SIZE];
	mdTypeDef classMdToken;
	hr = _pMetaDataImport->GetMethodProps(this->MethodMdToken,&classMdToken, methodName, NAME_BUFFER_SIZE, 0, 0, 0, 0, 0, 0);
	CheckError(hr);

	this->Name.append(methodName);
	InitializeContainingClass(pProfilerInfo, classMdToken);

}

void MethodMetadata::InitializeContainingClass(ICorProfilerInfo3 * pProfilerInfo, mdTypeDef classMdToken){
	HRESULT hr;

	ClassID classId;
	ModuleID moduleId;
	mdToken moduleMdToken;
	hr = pProfilerInfo->GetFunctionInfo2(this->FunctionId, 0,  &classId, &moduleId, &moduleMdToken,0,0,0);
	CheckError(hr);

	// Workaround: for generic classes is the classId = 0, http://blogs.msdn.com/b/davbr/archive/2010/01/28/generics-and-your-profiler.aspx
	bool genericClass = classId == 0;
	if(genericClass)
		classId = classMdToken;

	shared_ptr<ClassMetadata> pClassMetadata;
	if(ClassMetadata::ContainsCache(classId)){
		pClassMetadata = ClassMetadata::GetById(classId);
	}else{
		pClassMetadata = shared_ptr<ClassMetadata>(new ClassMetadata(classId, classMdToken, moduleId, moduleMdToken, pProfilerInfo, this->_pMetaDataImport, genericClass));
		ClassMetadata::AddMetadata(classId, pClassMetadata);
	}

	this->pContainingTypeMetadata = pClassMetadata;
	_isProfilingEnabled = pClassMetadata->IsProfilingEnabled();
}

void MethodMetadata::PopulateParameters(){
	HRESULT hr;

	HCORENUM paramsEnum = 0;
	ULONG reaEnumlCount = 0;
	mdParamDef paramMdTokenArray[ENUM_ARRAY_SIZE];
	do{
		hr = _pMetaDataImport->EnumParams(&paramsEnum, this->MethodMdToken, paramMdTokenArray,ENUM_ARRAY_SIZE,&reaEnumlCount);

		CheckError(hr);

		for(unsigned int i = 0; i < reaEnumlCount; i++){
			mdParamDef paramMdToken = paramMdTokenArray[i];
			WCHAR paramName[NAME_BUFFER_SIZE];
			hr = _pMetaDataImport->GetParamProps(paramMdToken,0,0, paramName, NAME_BUFFER_SIZE,0,0,0,0,0);

			CheckError(hr);

			wstring paramNameString;
			paramNameString.append(paramName);
			this->Parameters.push_back(paramNameString);
		}
	}while(reaEnumlCount > 0);
}

wstring MethodMetadata::ToString(){
	wstring wholeName;
	wholeName.append(L"[");
	wholeName.append(this->GetDefiningAssembly()->Name);
	wholeName.append(L"]");
	wholeName.append(this->pContainingTypeMetadata->ToString());
	wholeName.append(L".");
	wholeName.append(this->Name);
	wholeName.append(L"(");
	for ( vector<wstring>::iterator it = this->Parameters.begin(); it < this->Parameters.end(); it++ ){
		wholeName.append(*it);

		bool lastElement = (++it)-- == this->Parameters.end();
		if(lastElement)
			break;
		wholeName.append(L", ");
	}
	wholeName.append(L")");

	return wholeName;
}

shared_ptr<AssemblyMetadata> MethodMetadata:: GetDefiningAssembly(){
	shared_ptr<AssemblyMetadata> pAssemblyMetadata = this->pContainingTypeMetadata->pParentModuleMetadata->pAssemblyMetadata;
	return pAssemblyMetadata;
}

void MethodMetadata:: Serialize(SerializationBuffer * buffer){
	buffer->SerializeMetadataTypes(MetadataTypes_MethodMedatada);
	buffer->SerializeMetadataId(FunctionId);
	buffer->SerializeMdToken(MethodMdToken);
	buffer->SerializeWString(Name);
	buffer->SerializeUINT(Parameters.size());
	for(vector<wstring>::iterator it = Parameters.begin(); it != Parameters.end(); it++){
		buffer->SerializeWString(*it);
	}
	buffer->SerializeMetadataId(pContainingTypeMetadata->ClassId);
}

