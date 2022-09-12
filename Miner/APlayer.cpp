#include "pch.h"
#include "APlayer.h"

void Components::APlayer::AddName(std::convertible_to<std::string_view> auto && ...name_list) {
    for(auto str : std::initializer_list<std::string_view>{ name_list... }) {
        names.push_back(string(str));
    }
}

bool Components::APlayer::HasName(const std::string& name) const {
    for(auto& it : names)
        if(it == name) return true;

    return false;
}

// NOTE: the returned entry will be nullptr if the record vector changes size; use it as a short-lived (local) object
const Components::APlayer::FPlayerRecordEntry* Components::APlayer::GetRecordEntry(const int& match_id) const {
    for(auto& rec : record)
        if(rec.match_id == match_id) return &rec;
    return nullptr;
}

const list<Components::APlayer::FPlayerRecordEntry> Components::APlayer::GetRecordsFromSeason(const Structs::FSeason& season) const {
    list<FPlayerRecordEntry> matches;
    for(auto& rec :record) {
        if(rec.season->month == season.month and rec.season->year == season.year)
            matches.push_back(rec);
    }

    return matches;
}
