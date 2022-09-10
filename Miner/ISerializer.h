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
            ISerializer(const string& prefix = "[ISerializer]") : AData(prefix){}
            virtual shared_ptr<AData> Serialize(const json& src) = 0;
            virtual string Deserialize(const shared_ptr<Components::AData>& component) = 0;
    };
}
