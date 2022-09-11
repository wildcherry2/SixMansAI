#include "pch.h"
#include "ACoordinator.h"
#include "AMessage.h"
#include "MessageSerializer.h"

Components::ACoordinator::ACoordinator() : AData("[ACoordinator]"){}

shared_ptr<Components::ACoordinator> Components::ACoordinator::GetSingleton() {
    if(singleton == nullptr) singleton = shared_ptr<ACoordinator>(new ACoordinator());
    return singleton;
}

void Components::ACoordinator::SerializeMessagesInFile(const path& file) {
    auto message_serializer = Serializers::MessageSerializer::GetSingleton();
    ifstream js_file(file);
    json parent;
    js_file >> parent;
    int count = 0;
    for(auto& msg : parent["messages"]) {
        auto res = dynamic_pointer_cast<AMessage>(message_serializer->Serialize(msg));
        //if(res) cout << res->ToString() << "\n\n";
        count++;
    }
    cout << std::to_string(count);
}

shared_ptr<Components::ACoordinator> Components::ACoordinator::singleton;
