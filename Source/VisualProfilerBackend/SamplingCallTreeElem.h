#pragma once
#include "CallTreeElemBase.h"

class SamplingCallTreeElem : public CallTreeElemBase<SamplingCallTreeElem>{
public:
	UINT StackTopOccurrenceCount;
	UINT LastProfiledFrameInStackCount;

	SamplingCallTreeElem(FunctionID functionId = 0, SamplingCallTreeElem * pParent = NULL);
	void ToString(wstringstream & wsout, wstring indentation = L"", wstring indentationString = L"   ");
};
