#pragma once
#include <cstdarg>

#include "ISerializer.h"

namespace Serializers {
    class MessageSerializer : public ISerializer {
        private:
            static shared_ptr<MessageSerializer> singleton;
            MessageSerializer();

            auto RemoveQuotes(std::convertible_to<string> auto&& ... in) {
                ([&] {
                    if((*in.begin()) == '\"') in.erase(in.begin());
                    if((*(in.end() - 1)) == '\"') in.erase(in.end() - 1);
                }(),...);
            }

            auto RemoveQuotes(std::convertible_to<string> auto&& in) {
                if((*in.begin()) == '\"') in.erase(in.begin());
                if((*(in.end() - 1)) == '\"') in.erase(in.end() - 1);
                return in;
            }

            bool SetTrivialData(const json& src, shared_ptr<Components::AMessage> msg);
            bool SetEmbeds(const json& src, shared_ptr<Components::AMessage> msg);
            bool SetReactions(const json& src, shared_ptr<Components::AMessage> msg);
            bool SetMentions(const json& src, shared_ptr<Components::AMessage> msg);
            bool SetTimestamp(const json& src, shared_ptr<Components::AMessage> msg);

        public:
            shared_ptr<AData>                  Serialize(const json& src) override;
            string                             Deserialize(const shared_ptr<AData>& component) override;
            static shared_ptr<MessageSerializer> GetSingleton();
    };
}