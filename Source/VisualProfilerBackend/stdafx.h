// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#ifndef STRICT
#define STRICT
#endif

#include "targetver.h"

#define _ATL_APARTMENT_THREADED

#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

#define ATL_NO_ASSERT_ON_DESTROY_NONEXISTENT_WINDOW

#define ENUM_ARRAY_SIZE 20
#define NAME_BUFFER_SIZE 1024

#include "resource.h"
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
#include <string>


using namespace std;

void CheckError(HRESULT hr);
void CheckError(bool succeeded);
void CheckError2(BOOL succeeded);
void HandleError(wstring message);

