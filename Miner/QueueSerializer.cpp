#include "pch.h"
#include "QueueSerializer.h"



Serializers::QueueSerializer::QueueSerializer() : ISerializer("[QueueSerializer]") {
    
}

shared_ptr<Components::AData> Serializers::QueueSerializer::Serialize(const json& src) {
    return shared_ptr<AData>();
}

string Serializers::QueueSerializer::Deserialize(const shared_ptr<AData>& component) {
    return string();
}


shared_ptr<Serializers::QueueSerializer> Serializers::QueueSerializer::GetSingleton() {
    if(singleton == nullptr) singleton = make_shared<QueueSerializer>();
    return singleton;
}
