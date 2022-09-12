#include "pch.h"
#include "ACoordinator.h"

#define ON_LAPTOP

int main() {
    auto coord = Components::ACoordinator::GetSingleton();
    #ifndef ON_LAPTOP
        coord->SerializeMessagesInFile(R"(C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\rank-b\August2022.json)");
    #else
        coord->SerializeMessagesInFile(R"(C:\Users\tyler\source\repos\AI\SixMans\RawData\rank-b\June2022.json)");
    #endif

    return 0;
}