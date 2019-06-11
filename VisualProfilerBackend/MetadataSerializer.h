#pragma once
#include "SerializationBuffer.h"

class MetadataSerializer
{
	bool IsAlreadySerialized(){
		return _isAlreadySerialized;
	}

	virtual void Serialize(SerializationBuffer * buffer)=0;

protected:
	bool _isAlreadySerialized;


};


