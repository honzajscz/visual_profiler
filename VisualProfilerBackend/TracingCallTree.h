#pragma once

#include "TracingCallTreeElem.h"
#include "CallTreeBase.h"

using namespace std;

class TracingCallTree : public CallTreeBase<TracingCallTree, TracingCallTreeElem>
{
private:
	TracingCallTreeElem * _pActiveCallTreeElem;

public:

	TracingCallTree(ThreadID threadId, ICorProfilerInfo3 * profilerInfo);
	void FunctionEnter(FunctionID functionId);
	void FunctionLeave();
	TracingCallTreeElem * GetActiveCallTreeElem();
	virtual void Serialize(SerializationBuffer * buffer);

private:
	void UpdateCycleTime(TracingCallTreeElem * prevActiveElem, TracingCallTreeElem* nextActiveElem);

protected:
	void SerializeCallTreeElem(TracingCallTreeElem * elem, SerializationBuffer * buffer);
};
