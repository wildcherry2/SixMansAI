#include "pch.h"
#pragma once
#include "ScoreReportSerializer.h"



shared_ptr<Components::AData> Serializers::ScoreReportSerializer::Serialize(const json& src) {
    return shared_ptr<AData>();
}

string Serializers::ScoreReportSerializer::Deserialize(const shared_ptr<AData>& component) {
    return string();
}

shared_ptr<Serializers::ScoreReportSerializer> Serializers::ScoreReportSerializer::GetSingleton() {
    if(singleton == nullptr) singleton = make_shared<ScoreReportSerializer>();
    return singleton;
}

Serializers::ScoreReportSerializer::ScoreReportSerializer() : ISerializer("[ScoreReportSerializer]"){
    
}

shared_ptr<Serializers::ScoreReportSerializer> Serializers::ScoreReportSerializer::singleton;