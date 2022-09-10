#pragma once
#include "ISerializer.h"

namespace Serializers {
    class ScoreReportSerializer : public ISerializer {
        private:
            static shared_ptr<ScoreReportSerializer> singleton;
        public:
            shared_ptr<AData>                        Serialize(const json& src) override;
            string                                   Deserialize(const shared_ptr<AData>& component) override;
            static shared_ptr<ScoreReportSerializer> GetSingleton();
            ScoreReportSerializer();
    };
    shared_ptr<ScoreReportSerializer> ScoreReportSerializer::singleton;
}
