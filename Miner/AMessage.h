#pragma once
#include "EMessageType.h"
#include "AData.h"

class AMessage : public AData {
    public:
        struct FEmbed {
            string title;
            string description;
            map<string,string> fields;
        };

    protected:
        string sender_name;
        string sender_nickname;
        string content;
        uint64_t sender_discord_id = 0;
        uint64_t message_id = 0;
        sys_time<milliseconds> timestamp;
        FEmbed embedded_message;
        EMessageType type = EMessageType::NOT_SET;

        AMessage(const string& senderName, const string& senderNickname, const string& content, uint64_t senderDiscordId, uint64_t messageId, const sys_time<milliseconds>& timestamp, const FEmbed& embeddedMessage) :
            AData("[AMessage]"), sender_name(senderName), sender_nickname(senderNickname), content(content), sender_discord_id(senderDiscordId), message_id(messageId), timestamp(timestamp), embedded_message(embeddedMessage) {
                SetMessageType();
        }

        private:
            void SetMessageType(){};
    public:
        [[nodiscard]] const string&                 GetContent() const { return content; }
        void                                        SetContent(const string& content) { this->content = content; }
        [[nodiscard]] const FEmbed&                 GetEmbeddedMessage() const { return embedded_message; }
        void                                        SetEmbeddedMessage(const FEmbed& embeddedMessage) { embedded_message = embeddedMessage; }
        [[nodiscard]] uint64_t                      GetMessageId() const { return message_id; }
        void                                        SetMessageId(const uint64_t messageId) { message_id = messageId; }
        [[nodiscard]] uint64_t                      GetSenderDiscordId() const { return sender_discord_id; }
        void                                        SetSenderDiscordId(const uint64_t senderDiscordId) { sender_discord_id = senderDiscordId; }
        [[nodiscard]] const string&                 GetSenderName() const { return sender_name; }
        void                                        SetSenderName(const string& senderName) { sender_name = senderName; }
        [[nodiscard]] const string&                 GetSenderNickname() const { return sender_nickname; }
        void                                        SetSenderNickname(const string& senderNickname) { sender_nickname = senderNickname; }
        [[nodiscard]] const sys_time<milliseconds>& GetTimestamp() const { return timestamp; }
        void                                        SetTimestamp(const sys_time<milliseconds>& timestamp) { this->timestamp = timestamp; }
        [[nodiscard]] EMessageType                  GetType() const { return type; }
        void                                        SetType(const EMessageType type) { this->type = type; }

        AMessage() : AData("[AMessage]"){} 

        friend class APlayer;
};
