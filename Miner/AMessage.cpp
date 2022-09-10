#include "pch.h"
#include "AMessage.h"

void Components::AMessage::SetMessageType() {
    if(!is_bot) {
        type = content == "!q" ? EMessageType::PM_QUEUE : content == "!leave" ? EMessageType::PM_LEAVE : EMessageType::OTHER;
    }

    else {
        if(embedded_message.fields.empty()) is_valid = false;
        else {
            const auto& desc = embedded_message.description;
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
