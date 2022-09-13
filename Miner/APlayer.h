#pragma once
#include "AData.h"
#include "AQueue.h"
#include "FSeason.h"
#include "PlayerSerializer.h"

namespace Components {
    class AQueue;

    class APlayer : public AData {
        public:
            struct FPlayerRecordEntry {
                int                          match_id = 0;
                bool                         bWon     = false;
                shared_ptr<Structs::FSeason> season;
                shared_ptr<AQueue>           queue;
            };

            APlayer() : AData("[APlayer]") { is_valid = false; }

            APlayer(const string& name, const string& nickname = "", const uint64_t& discord_id = 0) : AData("[APlayer]") {
                if (name.empty() or discord_id == 0) is_valid = false;
                else {
                    names.push_back(name);
                    if (!nickname.empty()) names.push_back(nickname);
                }
            }

            void AddName(std::convertible_to<std::string_view> auto&& ...name_list) {
                for (auto str : std::initializer_list<std::string_view>{name_list...}) {
                    if (!str.empty() && none_of(names, [&](const string& in) { return in == str; })) 
                        names.push_back(string(str));
                }
            }

            bool HasName(const std::string& name) const;
            void IncrementTotalLosses() noexcept { total_losses++; }
            void IncrementTotalWins() noexcept { total_wins++; }

            [[nodiscard]] const FPlayerRecordEntry*      GetRecordEntry(const int& match_id) const;
            [[nodiscard]] const list<FPlayerRecordEntry> GetRecordsFromSeason(const Structs::FSeason& season) const;
            [[nodiscard]] const int&                     GetTotalLosses() const { return total_losses; }
            [[nodiscard]] const int&                     GetTotalWins() const { return total_wins; }
            [[nodiscard]] const uint64_t&                GetDiscordId() const { return discord_id; }

            friend shared_ptr<AData> Serializers::PlayerSerializer::SerializeFromMessages(const shared_ptr<AMessage>& current_player_message, const shared_ptr<AMessage>& current_bot_response_message) const;
        private:
            vector<string>             names;
            vector<FPlayerRecordEntry> record;
            int                        total_wins   = 0;
            int                        total_losses = 0;
            uint64_t                   discord_id   = 0;
    };
}
