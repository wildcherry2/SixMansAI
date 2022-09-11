#pragma once
#include "AData.h"
#include "MessageSerializer.h"

namespace Components {
    class AMessage : public AData {
        public:
            struct FEmbed {
                string title;
                string description;
                map<string, string> fields;

                FEmbed(const string& title, const string& description, const map<string,string>& fields) : title(title), description(description), fields(fields) {}
            };

        private:
            string sender_name = "";
            string sender_nickname = "";
            string content;
            uint64_t sender_discord_id = 0;
            uint64_t message_id = 0;
            bool is_bot = false;
            sys_time<milliseconds> timestamp;
            shared_ptr<FEmbed> embedded_message;
            vector<string> emoji_reactions;
            vector<tuple<uint64_t, string, string>> mentions;
            EMessageType type = EMessageType::NOT_SET;

            void SetMessageType();
            string GetMessageTypeLabel() const;
        public:
            AMessage(const string& senderName, const string& senderNickname, const string& content, uint64_t senderDiscordId, uint64_t messageId, const sys_time<milliseconds>& timestamp, const shared_ptr<FEmbed> embeddedMessage, bool is_bot, const vector<string>& emoji_reactions, const vector<tuple<uint64_t, string, string>>& mentions) :
                AData("[AMessage]"), sender_name(senderName), sender_nickname(senderNickname), content(content), sender_discord_id(senderDiscordId), message_id(messageId), is_bot(is_bot), timestamp(timestamp),
                embedded_message(embeddedMessage), emoji_reactions(emoji_reactions), mentions(mentions) {
                SetMessageType();
            }

            AMessage() : AData("[AMessage]"){ is_valid = false; }

            [[nodiscard]] const EMessageType& GetType() const { return type; }
            [[nodiscard]] const shared_ptr<FEmbed> GetEmbeddedMessage() const { return embedded_message; }
            [[nodiscard]] const string& GetContent() const { return content; }
            [[nodiscard]] const string& GetSenderName() const { return sender_name; }
            [[nodiscard]] const string& GetSenderNickname() const { return sender_nickname; }
            [[nodiscard]] const sys_time<milliseconds>& GetTimestamp() const { return timestamp; }
            [[nodiscard]] const uint64_t& GetMessageId() const { return message_id; }
            [[nodiscard]] const uint64_t& GetSenderDiscordId() const { return sender_discord_id; }

            string ToString() const noexcept {
                return "Message " + std::to_string(message_id) + "\nSender: " + sender_name + "\nType: " + GetMessageTypeLabel() + "\nValid: " + std::to_string(is_valid);
            }

            friend Serializers::MessageSerializer;
    };
}
