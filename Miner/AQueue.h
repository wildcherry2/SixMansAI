#pragma once
#include "AData.h"
#include "APlayer.h"
#include "FScoreReport.h"
#include "FSeason.h"
#include "FTeam.h"

enum class ETeamTag;

namespace Components {
    class AMessage;

    class AQueue : public AData {
        private:
            int                               match_id = 0;
            shared_ptr<Structs::FLobby>       lobby;
            shared_ptr<Structs::FScoreReport> result;

        public:
            AQueue() : AData("[AQueue]") { is_valid = false; }
            AQueue(const int& match_id, shared_ptr<AMessage> teams_picked_message, shared_ptr<AMessage> score_report_message);

            [[nodiscard]] const shared_ptr<Structs::FLobby>&       GetLobby() const { return lobby; }
            [[nodiscard]] const shared_ptr<Structs::FScoreReport>& GetResult() const { return result; }
            [[nodiscard]] int                                      GetMatchId() const { return match_id; }
            void                                                   SetLobby(const shared_ptr<Structs::FLobby> lobby) { this->lobby = lobby; }
            void                                                   SetMatchId(const int matchId) { match_id = matchId; }
            void                                                   SetResult(const shared_ptr<Structs::FScoreReport> result) { this->result = result; }
    };
}
