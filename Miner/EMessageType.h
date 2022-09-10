#pragma once
enum class EMessageType {
    NOT_SET,
    PM_QUEUE,
    PM_LEAVE,
    BOT_QUEUE,
    BOT_LEAVE,
    BOT_VOTING_COMPLETE,
    BOT_TEAMS_PICKED,
    OTHER
};