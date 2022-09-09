#pragma once
#include "pch.h"
#include "AMessage.h"
#include "ESeason.h"

using Season = tuple<EMonth, EYear>;
class APlayer : public AData {
    public:
        struct FPlayerRecordEntry {
            int  match_id = 0;
            bool bWon     = false;
            Season season;
            /* pointer to queue struct */
        };

        using Record = vector<FPlayerRecordEntry>;

        APlayer(const string& name, const string& nickname = "", const uint64_t& discord_id = 0) : AData("[APlayer]") {
            if(name.empty() or discord_id == 0) is_valid = false;
            else {
                names.push_back(name);
                if(!nickname.empty()) names.push_back(nickname);
            }
        }

        void AddName(std::convertible_to<std::string_view> auto&& ...name_list); /*{
            for(auto str : std::initializer_list<std::string_view>{ name_list... }) {
                names.push_back(string(str));
            }
        }*/
        bool HasName(const std::string& name) const;
        void IncrementTotalLosses() noexcept { total_losses++; }
        void IncrementTotalWins() noexcept { total_wins++; }

        [[nodiscard]] const FPlayerRecordEntry& GetRecordEntry(const int& match_id) const;
        [[nodiscard]] list<const FPlayerRecordEntry*>& GetRecordsFromSeason(const Season& season) const;
        [[nodiscard]] const int&                       GetTotalLosses() const { return total_losses; }
        [[nodiscard]] const int&                       GetTotalWins() const { return total_wins; }
        [[nodiscard]] const uint64_t&                  GetDiscordId() const { return discord_id; }

    private:
        vector<string> names;
        Record         record;
        int            total_wins   = 0;
        int            total_losses = 0;
        uint64_t discord_id = 0;
};
