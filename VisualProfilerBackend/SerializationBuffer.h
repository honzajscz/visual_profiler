#pragma once

#include <cor.h>
#include <corprof.h>
#include <string>
#include "MetadataTypes.h"
#include "ProfilingDataTypes.h"
#include "Commands.h"
#include "Actions.h"
#include <iostream>

#define INITIAL_BUFFER_SIZE 0x2000
#define SIZE_OF_METADATATYPES sizeof(MetadataTypes)
#define SIZE_OF_PROFILINGDATATYPES sizeof(ProfilingDataTypes)
#define SIZE_OF_COMMANDS sizeof(Commands)
#define SIZE_OF_ACTIONS sizeof(Actions)
#define SIZE_OF_UINT_PTR sizeof(UINT_PTR)
#define SIZE_OF_MDTOKEN sizeof(mdToken)
#define SIZE_OF_WCHAR sizeof(WCHAR)
#define SIZE_OF_CHAR sizeof(CHAR)
#define SIZE_OF_UINT sizeof(UINT)
#define SIZE_OF_BOOL sizeof(bool)
#define SIZE_OF_ULONGLONG sizeof(ULONGLONG)
#define SIZE_OF_ULONG64 sizeof(ULONG64)
#define SIZE_OF_DWORD sizeof(DWORD)

using namespace std;

class SerializationBuffer
{
public:
	SerializationBuffer(void):_currentIndex(0),_bufferSize(INITIAL_BUFFER_SIZE){
		_buffer = new BYTE[INITIAL_BUFFER_SIZE];
		//ZeroBuffer(_buffer, _bufferSize);
	}

	~SerializationBuffer(void){
		delete [] _buffer;
		_bufferSize = _currentIndex= -1;
		_buffer = NULL;
	}

	void Clear(){
		bool bufferAlreadyUsed = 0 < _currentIndex ;
		if(bufferAlreadyUsed){
			delete [] _buffer;
			_buffer = new BYTE[INITIAL_BUFFER_SIZE];
			_bufferSize = INITIAL_BUFFER_SIZE;
			_currentIndex = 0;
		}
	}

	void SerializeMetadataId(const UINT_PTR & metadataId){
		CopyToBuffer(&metadataId, SIZE_OF_UINT_PTR);
	}

	void SerializeThreadId(const UINT_PTR & threadId){
		CopyToBuffer(&threadId, SIZE_OF_UINT_PTR);
	}

	void SerializeFunctionId(const UINT_PTR & functionId){
		CopyToBuffer(&functionId, SIZE_OF_UINT_PTR);
	}

	void SerializeMdToken(const mdToken & mdToken){
		CopyToBuffer(&mdToken, SIZE_OF_MDTOKEN);
	}

	void SerializeUINT(const UINT & uint){
		CopyToBuffer(&uint, SIZE_OF_UINT);
	}

	void SerializeBool(const bool & boolean){
		CopyToBuffer(&boolean, SIZE_OF_BOOL);
	}

	void SerializeWString(const wstring & str){
		UINT byteCountOfStr =  str.size() * SIZE_OF_WCHAR;
		SerializeUINT(byteCountOfStr);

		const WCHAR * wchars = str.data();
		CopyToBuffer(wchars, byteCountOfStr);
	}

	void SerializeDebugString(const string & str){
		UINT byteCountOfStr =  str.size() * SIZE_OF_CHAR;
		const CHAR * chars = str.data();
		CopyToBuffer(chars, byteCountOfStr);
	}

	void SerializeMetadataTypes(const MetadataTypes metadataType){
		CopyToBuffer(&metadataType, SIZE_OF_METADATATYPES);
	}

	void SerializeProfilingDataTypes(const ProfilingDataTypes profilingDataType){
		CopyToBuffer(&profilingDataType, SIZE_OF_PROFILINGDATATYPES);
	}

	void SerializeCommands(const Commands command){
		CopyToBuffer(&command, SIZE_OF_COMMANDS);
	}

	void SerializeActions(const Actions action){
		CopyToBuffer(&action, SIZE_OF_ACTIONS);
	}

	void SerializeULONGLONG(ULONGLONG & ull){
		CopyToBuffer(&ull, SIZE_OF_ULONGLONG);
	}

	void SerializeULONG64(ULONG64 & ul64){
		CopyToBuffer(&ul64, SIZE_OF_ULONG64);
	}

	void CopyToAnotherBuffer(SerializationBuffer * destinationBuffer){
		destinationBuffer->CopyToBuffer(_buffer, _currentIndex);
	}

	UINT Size(){
		return _currentIndex;
	}
	
	BYTE* GetBuffer(){
		return _buffer;
	}

private:
	void EnsureBufferCapacity(UINT requiredCapacity){
		bool capicityExceeded = _bufferSize < (_currentIndex + requiredCapacity );
		if(capicityExceeded){
			UINT newBufferSize = _bufferSize + INITIAL_BUFFER_SIZE + requiredCapacity;
			BYTE * newBuffer = new BYTE[newBufferSize];
		//	ZeroBuffer(newBuffer, newBufferSize);
			memcpy_s(newBuffer,newBufferSize, _buffer, _bufferSize);
			delete [] _buffer;
			_buffer = newBuffer;
			_bufferSize = newBufferSize;
		}
	}

	void CopyToBuffer(const void * sourceAddr, UINT numberOfBytes){
		EnsureBufferCapacity(numberOfBytes);
		BYTE* destinationAddr = _buffer + _currentIndex;
		memcpy(destinationAddr, sourceAddr, numberOfBytes);
		_currentIndex += numberOfBytes;
	}

	//TODO Remove this method, just debugging
	void ZeroBuffer(BYTE * startAddr, UINT byteCount){
		for(UINT i = 0; i < byteCount; i++){
			startAddr[i] = 0xAA;
		}
	}


private:
	UINT _currentIndex;
	BYTE* _buffer;
	UINT _bufferSize;
};

