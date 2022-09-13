#include "pch.h"
#include "PlayerSerializer.h"

#include "ACore.h"

shared_ptr<Serializers::PlayerSerializer> Serializers::PlayerSerializer::GetSingleton() {
    if(!singleton) singleton = shared_ptr<PlayerSerializer>(new PlayerSerializer());
    return singleton;
}

shared_ptr<Components::AData> Serializers::PlayerSerializer::SerializeFromMessages(const shared_ptr<Components::AMessage>& current_player_message, const shared_ptr<Components::AMessage>& current_bot_response_message) const {
    auto       new_player = make_shared<Components::APlayer>();
    smatch     link_name_match;
    const bool matched = regex_search(current_bot_response_message->GetEmbeddedMessage()->description, link_name_match, extract_name_from_link);
    new_player->AddName(current_player_message->GetSenderName(), current_player_message->GetSenderNickname(), matched ? link_name_match.str() : "");
    new_player->discord_id = current_player_message->GetSenderDiscordId();
    new_player->SetIsValid(true);
    return new_player;
}

shared_ptr<Serializers::PlayerSerializer> Serializers::PlayerSerializer::singleton;