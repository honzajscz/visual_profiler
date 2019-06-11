#pragma once
#include "SerializationBuffer.h"
#include "MetadataTypes.h"
#include "stdafx.h"
#include <string>
#include "Utils.h"
#include "CallTreeBase.h"

#define BUFFER_SIZE 256
using namespace std;

template<class TCallTree>
class VisualProfilerAccess{

private:
	HANDLE _pipeHandle;
	HANDLE _listeningThread;
	bool _stopListening;

public:

	VisualProfilerAccess(){
		_pipeHandle = INVALID_HANDLE_VALUE;

		wstring pipeName = GetEnvirnomentalVariable(L"VisualProfiler.PipeName");
		bool pipeNameNotFound = pipeName.size() == 0;
		if(pipeNameNotFound)
			return;

		wstring pipeNameExtended(L"\\\\.\\pipe\\");
		pipeNameExtended.append(pipeName);

		_pipeHandle = CreateFile(pipeNameExtended.data(), GENERIC_READ| GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);
		//CheckError(_pipeHandle != INVALID_HANDLE_VALUE);
	}

	~VisualProfilerAccess(){
		if(IsHandleInvalid()) return ;
		CloseHandle(_pipeHandle);
	}

	bool IsHandleInvalid(){
		bool isHandleInvalid = _pipeHandle == INVALID_HANDLE_VALUE;
		return isHandleInvalid;
	}


	void StartListeningAsync(){
		if(IsHandleInvalid()) return ;

		_listeningThread = CreateThread(NULL,NULL,StartListening, this, NULL, NULL);
	}

	void StopListening(){
		if(IsHandleInvalid()) return ;

		_stopListening = true;
		CancelIoEx(_pipeHandle, NULL);
	}

	void CarryOutAction(Actions action, void * actionData){
		if(IsHandleInvalid()) return ;

		SerializationBuffer buffer;
		buffer.SerializeActions(action);
		
		switch(action){
		case Actions_SendingProfilingData:
			Actions_SendingProfilingDataMethod(&buffer, actionData);
			break;
		case Actions_ProfilingFinished:
			break;
		default:
			CheckError(false);
			break;
		}
		
		SendBuffer(&buffer);
	}

	void FinishProfiling(){
		if(IsHandleInvalid()) return ;

		StopListening();
		bool useShapshots= false;
		CarryOutAction(Actions_SendingProfilingData, &useShapshots);
		CarryOutAction(Actions_ProfilingFinished, NULL);
	}

private:

	void Actions_SendingProfilingDataMethod(SerializationBuffer * buffer, void * useSnapshot){
		bool useSnapshott =  *((bool *)useSnapshot);
		SerializeProfilingData(buffer, useSnapshott );
	}

	static DWORD WINAPI StartListening(void * data){
		VisualProfilerAccess * pThis = (VisualProfilerAccess *) data;
		pThis->_stopListening = false;
		while(!pThis->_stopListening){
			Commands command = pThis->WaitForCommand();
			bool useSnapshot = true;
			switch(command){
			case Commands_SendProfilingData:

				pThis->CarryOutAction(Actions_SendingProfilingData, &useSnapshot);
				break;
			case Commands_FinishProfiling:
				break;
			default:
				CheckError(false);
				break;
			}	
		}

		DWORD success = 1;
		return success;
	}

	void SerializeProfilingData(SerializationBuffer * buffer, bool useSnapshotBeforeFreshResults){
		SerializationBuffer bufferProfilingData;
		if(useSnapshotBeforeFreshResults){
			TCallTree::SerializeAllTreeSnapShots(&bufferProfilingData);
		}else{
			TCallTree::SerializeAllTrees(&bufferProfilingData);
		}

		SerializationBuffer bufferMetadata;
		AssemblyMetadata::SerializeMetadata(&bufferMetadata);
		ModuleMetadata::SerializeMetadata(&bufferMetadata);
		ClassMetadata::SerializeMetadata(&bufferMetadata);
		MethodMetadata::SerializeMetadata(&bufferMetadata);

		UINT metadataSizeUInt = SIZE_OF_UINT;
		UINT payloadSizeOfStream = metadataSizeUInt + bufferMetadata.Size() + bufferProfilingData.Size();
		buffer->SerializeUINT(payloadSizeOfStream);
		buffer->SerializeUINT(bufferMetadata.Size());

		bufferMetadata.CopyToAnotherBuffer(buffer);
		bufferProfilingData.CopyToAnotherBuffer(buffer);
	}

	void SendBuffer(SerializationBuffer * buffer){
		DWORD writtenBytesCount = 0;
		WriteFile(_pipeHandle,buffer->GetBuffer(),buffer->Size(),&writtenBytesCount,0);
//		CheckError(writtenBytesCount == buffer->Size());
	}

	Commands WaitForCommand(){
		Commands command = -1;
		DWORD bytesReadCount = -1;
		BOOL succeeded = ReadFile(_pipeHandle, &command, sizeof(Commands), &bytesReadCount, NULL);
		if(bytesReadCount == 0)
			return Commands_FinishProfiling;
		CheckError(bytesReadCount == sizeof(Commands));
		CheckError2(succeeded);

		return command;
	}
};

