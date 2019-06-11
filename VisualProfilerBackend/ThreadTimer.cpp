#include "StdAfx.h"
#include "ThreadTimer.h"
#include "Utils.h"

ThreadTimer::ThreadTimer(){
	Reset();
}

void ThreadTimer::Start(){
	_isStopped = false;
	GetSystemTimeAsFileTime(&_startTime);
}

void ThreadTimer::GetElapsedTimeIn100NanoSeconds(ULONGLONG * elapsedTime){
	*elapsedTime = _elapsedTime;
	if(!_isStopped){
		SubtractCurrentFromStartAndAddElapsedTime(elapsedTime); 
	}
}

ULONGLONG ThreadTimer::GetElapsedTimeIn100NanoSeconds(){
	ULONGLONG elapsedTime;
	GetElapsedTimeIn100NanoSeconds(&elapsedTime);
	return elapsedTime;
}

void ThreadTimer::Stop(){
	SubtractCurrentFromStartAndAddElapsedTime(&_elapsedTime);
	_isStopped = true;
}

void ThreadTimer::Reset(){
	_elapsedTime = 0;
	_isStopped = true;	
}

void ThreadTimer::SubtractCurrentFromStartAndAddElapsedTime(ULONGLONG * result ){
	FILETIME currentTime;
	GetSystemTimeAsFileTime(&currentTime);
	SubtractFILETIMESAndAddToResult(&currentTime, &_startTime, result);
}
