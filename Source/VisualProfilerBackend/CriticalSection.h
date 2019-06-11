#pragma once
#include <Windows.h>
class CriticalSection{
public:
	
	CriticalSection(){
		InitializeCriticalSection(&m_CriticalSection);
	}

	~CriticalSection(){
		DeleteCriticalSection(&m_CriticalSection);
	}

	void Enter(){
		EnterCriticalSection(&m_CriticalSection);
	}

	void Leave(){
		LeaveCriticalSection(&m_CriticalSection);
	}

private:
	CRITICAL_SECTION m_CriticalSection;
};