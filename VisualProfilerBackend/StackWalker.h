#pragma once
#include <cor.h>
#include <corprof.h>
#include <set>
#include "CriticalSection.h"
#include <vector>
#include <map>
#include <iostream>
#include "ThreadTimer.h"
#include "SamplingCallTreeElem.h"
#include "SamplingCallTree.h"

class StackWalker
{
public:
	StackWalker(ICorProfilerInfo3 * pProfilerInfo, UINT samplingPeriodMs);
	void RegisterThread(ThreadID threadId);
	void DeregisterThread(ThreadID threadId);
	void StartSampling();
	void StopSampling();

private:
	static DWORD WINAPI Sample(void * data);
	void DeregisterAllRegisteredThreads();
	static HRESULT __stdcall  MakeFrameWalk(FunctionID functionId, UINT_PTR ip, COR_PRF_FRAME_INFO frameInfo, ULONG32 contextSize, BYTE context[], void *clientData);

	bool _continueSampling;
	CriticalSection _criticalSection;
	ICorProfilerInfo3 * _pProfilerInfo;
	UINT _samplingPeriodMs;
	set<ThreadID> _registeredThreadIds;
	DWORD _samplingThreadId;
	HANDLE _samplingThreadHandle;
};

