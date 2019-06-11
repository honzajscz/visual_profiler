#pragma once
#include <map>
#include <cor.h>
#include <corprof.h>
#include <sstream>
#include "MethodMetadata.h"

template<class TTreeElem>
class CallTreeElemBase {
public:
	TTreeElem * pParent;
	FunctionID FunctionId;
	
	CallTreeElemBase(FunctionID functionId = 0, TTreeElem * pParent = NULL):FunctionId(functionId), pParent(pParent){}

	bool IsRootElem(){
		return FunctionId == 0 && pParent == NULL;
	}

	TTreeElem * GetChildTreeElem(FunctionID functionId){
		TTreeElem * childElem = _pChildrenMap[functionId].get();
		bool missingInMap = childElem == NULL;
		if(missingInMap){
			childElem = new TTreeElem(functionId, (TTreeElem*) this);
			_pChildrenMap[functionId] = shared_ptr<TTreeElem>(childElem);
		}

		return childElem;
	}

	virtual void ToString(wstringstream & wsout, wstring indentation = L"", wstring indentationString = L"   ") = 0;
	map<FunctionID,shared_ptr<TTreeElem>> * GetChildrenMap(){
		return &_pChildrenMap;
	}

protected:
	map<FunctionID,shared_ptr<TTreeElem>> _pChildrenMap;
};