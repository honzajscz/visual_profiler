#pragma once
#include <exception>
#include <string>

using namespace std;

class VisualProfilerException : public exception
{
public:
	HRESULT HResult;
	wstring Message;
	VisualProfilerException(){};
	VisualProfilerException(HRESULT HResult):HResult(HResult){}
	VisualProfilerException(wstring message):Message(message){}
	VisualProfilerException(HRESULT HResult, wstring message):HResult(HResult),Message(message){}
	
	virtual const char* what() const throw()
	{
		return "Visual Profiler Exception";
	}
}VISUAL_PROFILER_EXCEPTION;

