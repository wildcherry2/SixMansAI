#pragma once
#include "ISerializer.h"

namespace Serializers {

    class PlayerSerializer : public ISerializer {
        private:
            static shared_ptr<PlayerSerializer> singleton;
            PlayerSerializer() : ISerializer("[PlayerSerializer]") {
                extract_name_from_link = regex(R"((?:(?!^\[|\]\().)+)", regex::ECMAScript);
            }

            regex extract_name_from_link;

        public:
            static shared_ptr<PlayerSerializer> GetSingleton();
            [[nodiscard]] shared_ptr<AData>     SerializeFromMessages(const shared_ptr<Components::AMessage>& current_player_message, const shared_ptr<Components::AMessage>& current_bot_response_message) const;
            //string                             Deserialize(const shared_ptr<AData>& component) override;
    };

}