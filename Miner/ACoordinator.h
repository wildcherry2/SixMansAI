#pragma once
#include "AData.h"
#include "AMessage.h"

namespace Components {

    class ACoordinator : public AData {
        private:
            static shared_ptr<ACoordinator> singleton;
            ACoordinator();

        public:
            static shared_ptr<ACoordinator> GetSingleton();
            void                            SerializeMessagesInFile(const path& file) const;
            void                            SerializePlayerNamesFromMessages() const;
    };

}
