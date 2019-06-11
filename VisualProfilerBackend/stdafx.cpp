// stdafx.cpp : source file that includes just the standard includes
// VisualProfilerBackend.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

void CheckError(HRESULT hr){
	if(SUCCEEDED(hr)){
		return;
	}

		DWORD lastError = GetLastError();
	HRESULT hrError = HRESULT_FROM_WIN32(lastError);
	__asm{
		int 3
	}
}

void CheckError(bool succeeded){
	if(succeeded){
		return;
	}
	DWORD lastError = GetLastError();
	HRESULT hrError = HRESULT_FROM_WIN32(lastError);
	__asm{
		int 3
	}
}

void CheckError2(BOOL succeeded){
	if(succeeded){
		return;
	}
	DWORD lastError = GetLastError();
	HRESULT hrError = HRESULT_FROM_WIN32(lastError);
	__asm{
		int 3
	}
}


void HandleError(wstring message){
	__asm{
		int 3
	}
}