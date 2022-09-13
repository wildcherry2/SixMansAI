#pragma once
#include "pch.h"

#include "AData.h"

namespace Components {
    struct AData;
    class AMessage;
}

namespace Serializers {
    class ISerializer : public Components::AData {
        public:
            explicit                  ISerializer(const string& prefix = "[ISerializer]") : AData(prefix) {}
            virtual shared_ptr<AData> Serialize(const json& src){ return nullptr; }
            virtual shared_ptr<AData> Serialize(){ return nullptr; }
            virtual string            Deserialize(const shared_ptr<AData>& component){ return""; }
    };
}
