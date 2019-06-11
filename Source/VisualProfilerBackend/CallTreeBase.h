#pragma once
#include <map>
#include <cor.h>
#include <corprof.h>
#include "ThreadTimer.h"
#include "CriticalSection.h"
#include <string>
#include <sstream>
#include "Utils.h"
#include "SerializationBuffer.h"

using namespace std;

template<class TCallTree, class TTreeElem>
class CallTreeBase{
protected :
	static map<ThreadID, shared_ptr<TCallTree>> _callTreeMap;
	static CriticalSection _criticalSection;

	TTreeElem _rootCallTreeElem;
	ThreadID _threadId;
	ThreadTimer _timer;
	ICorProfilerInfo3 * _profilerInfo;
	bool _refreshCallTreeBuffer;
	SerializationBuffer _callTreeBuffer;
	CriticalSection _instanceCriticalSection;
	HANDLE _osThreadHandle;
	DWORD _osThreadId;
	FILETIME CreationUserModeTimeStamp;
	FILETIME CreationKernelModeTimeStamp;

	virtual void SerializeCallTreeElem(TTreeElem * elem, SerializationBuffer * buffer) = 0;

public:

	ULONGLONG KernelModeDurationHns;
	ULONGLONG UserModeDurationHns;
	
		
	CallTreeBase(ThreadID threadId, ICorProfilerInfo3 * profilerInfo):_threadId(threadId),_refreshCallTreeBuffer(true),_osThreadId(0), _osThreadHandle(INVALID_HANDLE_VALUE), _profilerInfo(profilerInfo){
		_profilerInfo->GetThreadInfo(threadId, &_osThreadId);
		SetOsThreadInfo();
		GetTimer()->Start();
	};

	void RefreshCallTreeBuffer(bool force = false){
		if(force || _refreshCallTreeBuffer){
			_instanceCriticalSection.Enter();
			{
				_callTreeBuffer.Clear();
				Serialize(&_callTreeBuffer);
				_refreshCallTreeBuffer = false;
			}
			_instanceCriticalSection.Leave();
		}
	}

	void CopyCallTreeBufferToBuffer(SerializationBuffer * destinationBuffer){
		_instanceCriticalSection.Enter();
		{
			_callTreeBuffer.CopyToAnotherBuffer(destinationBuffer);
			_refreshCallTreeBuffer = true;
		}
		_instanceCriticalSection.Leave();
	}
	
	void UpdateUserAndKernelModeDurations(){
		FILETIME dummy;
		FILETIME currentUserModeTimeStamp;
		FILETIME currentKernelModeTimeStamp;

		BOOL success = GetThreadTimes(_osThreadHandle,&dummy, &dummy,&currentKernelModeTimeStamp, &currentUserModeTimeStamp);
		CheckError2(success);
		UserModeDurationHns = 0;
		KernelModeDurationHns = 0;
		SubtractFILETIMESAndAddToResult(&currentUserModeTimeStamp, &CreationUserModeTimeStamp, &UserModeDurationHns);
		SubtractFILETIMESAndAddToResult(&currentKernelModeTimeStamp, &CreationKernelModeTimeStamp, &KernelModeDurationHns);
	}

	ThreadID GetThreadId(){
		return _threadId;
	}

	ThreadTimer * GetTimer(){
		return &_timer;
	}

	virtual void ToString(wstringstream & wsout){
		wsout << "Thread Id = " <<_threadId << ", Number of stack divisions = " << _rootCallTreeElem.GetChildrenMap()->size() <<  endl ;
		_rootCallTreeElem.ToString(wsout);
	}

	virtual void Serialize(SerializationBuffer * buffer) = 0;

	static TCallTree * AddThread(ThreadID threadId, ICorProfilerInfo3 * profilerInfo){
		shared_ptr<TCallTree> pCallTree(new TCallTree(threadId, profilerInfo));
		_criticalSection.Enter();
		{
			_callTreeMap[threadId] = pCallTree;
		}
		_criticalSection.Leave();
		return pCallTree.get();
	}

	static TCallTree * GetCallTree(ThreadID threadId){
		shared_ptr<TCallTree> pCallTree;
		_criticalSection.Enter();
		{
			pCallTree = _callTreeMap[threadId];
		}
		_criticalSection.Leave();
		return pCallTree.get();
	}

	static map<ThreadID, shared_ptr<TCallTree>> * GetCallTreeMap(){
		return &_callTreeMap;
	}

	static void SerializeAllTrees(SerializationBuffer * buffer){
		_criticalSection.Enter();
		{
			map<ThreadID, shared_ptr<TCallTree>>::iterator it = _callTreeMap.begin();
			for(;it != _callTreeMap.end(); it++){
				TCallTree * pCallTree = it->second.get();
				pCallTree->Serialize(buffer);
			}
		}
		_criticalSection.Leave();
	}

	static void SerializeAllTreeSnapShots(SerializationBuffer * buffer){
		_criticalSection.Enter();
		{
			map<ThreadID, shared_ptr<TCallTree>>::iterator it = _callTreeMap.begin();
			for(;it != _callTreeMap.end(); it++){
				TCallTree * pCallTree = it->second.get();
				pCallTree->CopyCallTreeBufferToBuffer(buffer);
			}
		}
		_criticalSection.Leave();
	}

private:
	void SetOsThreadInfo(){
		_osThreadHandle = OpenThread(THREAD_QUERY_INFORMATION,false,_osThreadId);
		if(_osThreadHandle == NULL || _osThreadHandle == INVALID_HANDLE_VALUE){
			CheckError(false);
		}

		FILETIME dummy;
		BOOL success = GetThreadTimes(_osThreadHandle,&dummy, &dummy,&CreationKernelModeTimeStamp, &CreationUserModeTimeStamp);
		CheckError2(success);
	}

};
template<class TCallTree, class TTreeElem>
map<ThreadID, shared_ptr<TCallTree>> CallTreeBase<TCallTree, TTreeElem>::_callTreeMap;

template<class TCallTree, class TTreeElem>
CriticalSection CallTreeBase<TCallTree, TTreeElem>::_criticalSection;