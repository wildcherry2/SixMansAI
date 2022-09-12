#pragma once
#include "APlayer.h"
#include "ETeamTag.h"
#include "ISerializer.h"

namespace Serializers {
    class QueueSerializer : public ISerializer {
        private:
            static shared_ptr<QueueSerializer> singleton;
            QueueSerializer();

        public:
            shared_ptr<AData>                  Serialize(const json& src) override;
            string                             Deserialize(const shared_ptr<AData>& component) override;
            static shared_ptr<QueueSerializer> GetSingleton();
    };
}
