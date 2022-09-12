﻿namespace Database.Database.Enums; 

public enum EMessageType {
    PLAYER_Q,
    PLAYER_LEAVE,
    VOTING_COMPLETE,
    SCORE_REPORT,
    TEAMS_PICKED,
    BOT_RESPONSE_TO_PLAYER_Q,
    BOT_RESPONSE_TO_PLAYER_LEAVE,
    BOT_LOBBY_CANCELLED,
    UNKNOWN
}