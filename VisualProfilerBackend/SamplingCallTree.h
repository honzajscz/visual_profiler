#pragma once

#include "CallTreeBase.h"
#include "SamplingCallTreeElem.h"

class SamplingCallTree : public CallTreeBase<SamplingCallTree, SamplingCallTreeElem> {
public:
	SamplingCallTree(ThreadID threadId, ICorProfilerInfo3 * profilerInfo);
	void ProcessSamples(vector<FunctionID> * functionIdsSnapshot);
	
	virtual void Serialize(SerializationBuffer * buffer);
	virtual void ToString(wstringstream & wsout);

protected:
	void SerializeCallTreeElem(SamplingCallTreeElem * elem, SerializationBuffer * buffer);

private:
	#pragma region waitJoinSleep
	//FILETIME user;
	//FILETIME kernel;
	#pragma endregion

};