#include "pch.h"
#include "MessageSerializer.h"
#include "AMessage.h"

#define JGet(js) RemoveQuotes((js).dump())

shared_ptr<Components::AData> Serializers::MessageSerializer::Serialize(const json& src) {
    auto ret      = make_shared<Components::AMessage>();
    ret->is_valid = SetTrivialData(src, ret);
    ret->is_valid &= SetEmbeds(src, ret);
    ret->is_valid &= SetReactions(src, ret);
    ret->is_valid &= SetMentions(src, ret);
    ret->is_valid &= SetTimestamp(src, ret);
    ret->SetMessageType();
    if (ret->is_valid && ret->type != EMessageType::OTHER && ret->type != EMessageType::NOT_SET) return ret;
    return nullptr;
}

string Serializers::MessageSerializer::Deserialize(const shared_ptr<AData>& component) { return ""; }

shared_ptr<Serializers::MessageSerializer> Serializers::MessageSerializer::GetSingleton() {
    if (singleton == nullptr) singleton = shared_ptr<MessageSerializer>(new MessageSerializer());
    return singleton;
}

Serializers::MessageSerializer::MessageSerializer() : ISerializer("[MessageSerializer]") {}

shared_ptr<Serializers::MessageSerializer> Serializers::MessageSerializer::singleton;

bool Serializers::MessageSerializer::SetTrivialData(const json& src, const shared_ptr<Components::AMessage>& msg) {
    try {
        auto sender_name       = src["author"]["name"].dump();
        auto sender_nickname   = src["author"]["nickname"].dump();
        auto content           = src["content"].dump();
        auto sender_discord_id = src["author"]["id"].dump();
        auto message_id        = src["id"].dump();
        auto is_bot            = src["author"]["isBot"].dump();

        RemoveQuotes(sender_name, sender_nickname, content, sender_discord_id, message_id, is_bot);

        msg->sender_name       = sender_name;
        msg->sender_nickname   = sender_nickname;
        msg->content           = content;
        msg->sender_discord_id = stoull(sender_discord_id);
        msg->message_id        = stoull(message_id);
        msg->is_bot            = is_bot == "true";
    }
    catch (const std::exception& ex) { return false; }

    return true;
}

bool Serializers::MessageSerializer::SetEmbeds(const json& src, const shared_ptr<Components::AMessage>& msg) {
    try {
        auto                                     the_embed = src["embeds"];
        shared_ptr<Components::AMessage::FEmbed> embed;
        if (!the_embed.empty()) {
            string              title       = the_embed["title"];
            string              description = the_embed["description"];
            map<string, string> fields;
            for (auto& it : the_embed["fields"]) { fields.insert(std::make_pair<string, string>(JGet(it["name"]), JGet(it["value"]))); }

            RemoveQuotes(title, description);
            embed = make_shared<Components::AMessage::FEmbed>(title, description, fields);
        }
        else embed = make_shared<Components::AMessage::FEmbed>("", "", map<string, string>());

        msg->embedded_message = embed;
    }
    catch (const std::exception& ex) { return false; }

    return true;
}

bool Serializers::MessageSerializer::SetReactions(const json& src, const shared_ptr<Components::AMessage>& msg) {
    try {
        vector<string> reaction_vector;
        auto           reactions = src["reactions"];
        for (auto& it : reactions) { reaction_vector.push_back(JGet(it["emoji"]["name"])); }

        msg->emoji_reactions = reaction_vector;
    }
    catch (const std::exception& ex) { return false; }

    return true;
}

bool Serializers::MessageSerializer::SetMentions(const json& src, const shared_ptr<Components::AMessage>& msg) {
    try {
        vector<tuple<uint64_t, string, string>> mentions_vector;
        auto                                    mentions = src["mentions"];
        for (auto& it : mentions) { mentions_vector.push_back(std::make_tuple(stoull(JGet(it["id"])), JGet(it["name"]), JGet(it["nickname"]))); }

        msg->mentions = mentions_vector;
    }
    catch (const std::exception& ex) { return false; }

    return true;
}

bool Serializers::MessageSerializer::SetTimestamp(const json& src, const shared_ptr<Components::AMessage>& msg) {
    try {
        istringstream          ts_str{JGet(src["timestamp"])};
        sys_time<milliseconds> ts;
        ts_str >> parse("%FT%T", ts);
        msg->timestamp = ts;
        if (ts.time_since_epoch().count() != 0) return true;
        return false;
    }
    catch (const std::exception& ex) { return false; }
}

#undef JGet