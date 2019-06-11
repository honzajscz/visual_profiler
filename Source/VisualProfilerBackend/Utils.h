#pragma once
#include <string>

void SubtractFILETIMESAndAddToResult(FILETIME * ft1, FILETIME * ft2, ULONGLONG * result );
wstring GetEnvirnomentalVariable(WCHAR * variableName);