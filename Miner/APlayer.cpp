#include "pch.h"
#include "APlayer.h"

void APlayer::AddName(std::convertible_to<std::string_view> auto && ...name_list) {
    for(auto str : std::initializer_list<std::string_view>{ name_list... }) {
        names.push_back(string(str));
    }
}

bool APlayer::HasName(const std::string& name) const {
    for(auto& it : names)
        if(it == name) return true;

    return false;
}

// NOTE: the returned entry will be nullptr if the record vector changes size; use it as a short-lived (local) object
const APlayer::FPlayerRecordEntry* APlayer::GetRecordEntry(const int& match_id) const {
    for(auto& rec : record)
        if(rec.match_id == match_id) return &rec;
    return nullptr;
}

const list<APlayer::FPlayerRecordEntry> APlayer::GetRecordsFromSeason(const Season& season) const {
    list<FPlayerRecordEntry> matches;
    for(auto& rec :record) {
        if(std::get<0>(rec.season) == std::get<0>(season) and std::get<1>(rec.season) == std::get<1>(season))
            matches.push_back(rec);
    }

    return matches;
}
