#pragma once
#include "pch.h"
#include "APlayer.h"

namespace Components {
    class APlayer;
}

namespace Components::Structs {
    struct FTeam {
        std::vector<APlayer> players;
    };

    struct FLobby {
        std::vector<FTeam> teams;
    };

}
