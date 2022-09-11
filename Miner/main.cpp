#include "pch.h"
#include "ACoordinator.h"

int main() {
    auto coord = Components::ACoordinator::GetSingleton();
    coord->SerializeMessagesInFile(R"(C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\rank-b\August2022.json)");
    return 0;
}