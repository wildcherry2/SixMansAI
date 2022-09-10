#pragma once
#include "AData.h"

namespace Components {
    class AMessage;
    class AQueue : public AData {
        private:
            int match_id = 0;
            shared_ptr<Lobby> lobby;
            shared_ptr<ScoreReport> result;

        protected:
            AQueue() : AData("[AQueue]") { is_valid = false; }
            AQueue(const int& match_id, const shared_ptr<AMessage> teams_picked_message, const shared_ptr<AMessage> score_report_message);

        public:
            [[nodiscard]] const shared_ptr<Lobby>& GetLobby() const { return lobby; }
            [[nodiscard]] const shared_ptr<ScoreReport>& GetResult() const { return result; }
            [[nodiscard]] int                            GetMatchId() const { return match_id; }
            void                                         SetLobby(const shared_ptr<Lobby> lobby) { this->lobby = lobby; }
            void                                         SetMatchId(const int matchId) { match_id = matchId; }
            void                                         SetResult(const shared_ptr<ScoreReport> result) { this->result = result; }
    };
}