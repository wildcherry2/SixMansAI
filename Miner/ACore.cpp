#include "pch.h"
#include "ACore.h"

shared_ptr<ACore> ACore::GetSingleton() {
    if(singleton == nullptr) singleton = shared_ptr<ACore>(new ACore());
    return singleton;
}

shared_ptr<ACore> ACore::singleton;
