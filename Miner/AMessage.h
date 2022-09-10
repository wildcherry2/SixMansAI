#pragma once
#include "AData.h"

namespace Components {
    class AMessage : public AData {
        public:
            struct FEmbed {
                string title;
                string description;
                map<string, string> fields;
            };

        protected:
            string sender_name;
            string sender_nickname;
            string content;
            uint64_t sender_discord_id = 0;
            uint64_t message_id = 0;
            bool is_bot = false;
            sys_time<milliseconds> timestamp;
            FEmbed embedded_message;
            EMessageType type = EMessageType::NOT_SET;

            AMessage(const string& senderName, const string& senderNickname, const string& content, uint64_t senderDiscordId, uint64_t messageId, const sys_time<milliseconds>& timestamp, const FEmbed& embeddedMessage, bool is_bot) :
                AData("[AMessage]"), sender_name(senderName), sender_nickname(senderNickname), content(content), sender_discord_id(senderDiscordId), message_id(messageId), timestamp(timestamp), embedded_message(embeddedMessage), is_bot(is_bot) {
                SetMessageType();
            }

        private:
            void SetMessageType();
        public:
            [[nodiscard]] const EMessageType& GetType() const { return type; }
            [[nodiscard]] const FEmbed& GetEmbeddedMessage() const { return embedded_message; }
            [[nodiscard]] const string& GetContent() const { return content; }
            [[nodiscard]] const string& GetSenderName() const { return sender_name; }
            [[nodiscard]] const string& GetSenderNickname() const { return sender_nickname; }
            [[nodiscard]] const sys_time<milliseconds>& GetTimestamp() const { return timestamp; }
            [[nodiscard]] const uint64_t& GetMessageId() const { return message_id; }
            [[nodiscard]] const uint64_t& GetSenderDiscordId() const { return sender_discord_id; }

            AMessage() : AData("[AMessage]") { is_valid = false; }
    };
}