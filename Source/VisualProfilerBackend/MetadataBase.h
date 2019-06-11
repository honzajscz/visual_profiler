#pragma once
#include <memory>
#include <map>
#include <cor.h>
#include <corprof.h>
#include "CriticalSection.h"
#include "SerializationBuffer.h"
#include "MetadataSerializer.h"

using namespace std;

template <class TId, class TMetadata>
class MetadataBase : public MetadataSerializer
{
public:
	static void AddMetadata(TId id, shared_ptr<TMetadata> pMetadata){
		if(!ContainsCache(id)){
			_criticalSection.Enter();
			{
				pMetadata->_isAlreadySerialized = false;
				_cacheMap[id] = pMetadata;
			}
			_criticalSection.Leave();
		}
	}

	static bool ContainsCache(TId id){
		bool contains;
		_criticalSection.Enter();
		{
			contains = _cacheMap[id] != NULL;
		}
		_criticalSection.Leave();
		return contains;
	}

	static shared_ptr<TMetadata> GetById(TId id){
		shared_ptr<TMetadata> pMetadata ;
		_criticalSection.Enter();
		{
			pMetadata = _cacheMap[id];
		}
		_criticalSection.Leave();
		return pMetadata;
	}

	static int CacheSize(){
		_criticalSection.Enter();
		{
			int cacheSize = _cacheMap.size();
		}
		_criticalSection.Leave();
		return cacheSize;
	}

	static void SerializeMetadata(SerializationBuffer * buffer){
		_criticalSection.Enter();
		{
			map<TId, shared_ptr<TMetadata>>::iterator it;
			for(it = _cacheMap.begin(); it != _cacheMap.end(); it++ ){
				TMetadata * pMetadata = it->second.get();
				bool skipSerialization = (pMetadata == NULL || pMetadata->_isAlreadySerialized || !pMetadata->IsProfilingEnabled());
				if(skipSerialization)
					continue;
				pMetadata->Serialize(buffer);
				pMetadata->_isAlreadySerialized = true;
			}
		}
		_criticalSection.Leave();
	}

	bool IsProfilingEnabled(){
		return _isProfilingEnabled;
	}
	

private:
	static map<TId, shared_ptr<TMetadata>> _cacheMap;
	static CriticalSection _criticalSection;
protected:
	bool _isProfilingEnabled;
};

template <class TId, class TMetadata>
map<TId, shared_ptr<TMetadata>> MetadataBase<TId, TMetadata>::_cacheMap;

template <class TId, class TMetadata>
CriticalSection MetadataBase<TId, TMetadata>::_criticalSection;