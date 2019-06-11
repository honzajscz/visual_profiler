#pragma once

class ThreadTimer
{
public:
	ThreadTimer();
	void Start();
	void GetElapsedTimeIn100NanoSeconds(ULONGLONG * elapsedTime);
	ULONGLONG GetElapsedTimeIn100NanoSeconds();
	void Stop();
	void Reset();

private:
	ULONGLONG _elapsedTime;
	FILETIME _startTime;
	bool _isStopped;
	void SubtractCurrentFromStartAndAddElapsedTime(ULONGLONG * result );
};


