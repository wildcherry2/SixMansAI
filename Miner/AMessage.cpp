#include "pch.h"
#include "AMessage.h"

void Components::AMessage::SetMessageType() {
    if(!is_bot) {
        type = content == "!q" ? EMessageType::PM_QUEUE : content == "!leave" ? EMessageType::PM_LEAVE : EMessageType::OTHER;
    }

    else {
        if(!embedded_message) is_valid = false;
        else {
            const auto& desc = embedded_message->description;
            if(desc.find(") has joined.") != string::npos)
                type = EMessageType::BOT_QUEUE;
            else if(desc.find(") has left (using command).") != string::npos)
                type = EMessageType::BOT_LEAVE;
            else if(desc.find("All players must join within 7 minutes and then teams will be chosen.\n**Vote result:**") != string::npos)
                type = EMessageType::BOT_VOTING_COMPLETE;
            else if(desc.find("You may now join the team channels") != string::npos)
                type = EMessageType::BOT_TEAMS_PICKED;
            else
                type = EMessageType::OTHER;
        }
    }

    is_valid = type != EMessageType::OTHER && type != EMessageType::NOT_SET;
}

string Components::AMessage::GetMessageTypeLabel() const {
    switch(type){
        case EMessageType::PM_QUEUE:
            return "PM_QUEUE";
        case EMessageType::PM_LEAVE:
            return "PM_LEAVE";
        case EMessageType::BOT_QUEUE:
            return "BOT_QUEUE";
        case EMessageType::BOT_LEAVE:
            return "BOT_LEAVE";
        case EMessageType::BOT_VOTING_COMPLETE:
            return "BOT_VOTING_COMPLETE";
        case EMessageType::BOT_TEAMS_PICKED:
            return "BOT_TEAMS_PICKED";
        case EMessageType::OTHER:
            return "OTHER";
        default:
            return "NOT_SET";
    }
}
