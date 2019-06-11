#pragma once
#include "MethodMetadata.h"
#include "CallTreeElemBase.h"


class TracingCallTreeElem: public CallTreeElemBase<TracingCallTreeElem> { 
public:
	UINT EnterCount;
	UINT LeaveCount;
	ULONGLONG LastEnterTimeStampHns;
	ULONGLONG WallClockDurationHns; //100 nanoseconds

	ULONG64 CycleTime;
	ULONG64 LastCycleTime;

	TracingCallTreeElem(FunctionID functionId = 0, TracingCallTreeElem * pParent = NULL);
	void ToString(wstringstream & wsout, wstring indentation = L"", wstring indentationString = L"   ");
};
