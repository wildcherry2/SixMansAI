#include "pch.h"
#include "ACoordinator.h"

#include "ACore.h"
#include "AMessage.h"
#include "MessageSerializer.h"

Components::ACoordinator::ACoordinator() : AData("[ACoordinator]") {}

shared_ptr<Components::ACoordinator> Components::ACoordinator::GetSingleton() {
    if (singleton == nullptr) singleton = shared_ptr<ACoordinator>(new ACoordinator());
    return singleton;
}

void Components::ACoordinator::SerializeMessagesInFile(const path& file) const {
    const auto message_serializer = Serializers::MessageSerializer::GetSingleton();
    const auto core               = ACore::GetSingleton();
    const auto& file_name = file.filename().string();

    Log("Serializing from {}...", file_name);
    try {
        ifstream js_file(file);
        json     parent;
        js_file >> parent;
        js_file.close();
        for (auto& msg : parent["messages"]) {
            if (auto current = dynamic_pointer_cast<AMessage>(message_serializer->Serialize(msg))) core->raw_messages.push_back(current);
        }

        Log("Messages from {} serialized!", file_name);

        int err = 0;
        for (auto it = core->raw_messages.begin(); it != core->raw_messages.end(); ++it) {
            if (const auto next = (it + 1); next != core->raw_messages.end() && it->get()->IsBot() && (it + 1)->get()->IsBot()) {
                const auto& this_type = it->get()->GetType();
                if (const auto& next_type = next->get()->GetType(); this_type == next_type) err++;
            }
        }

        Log("Error !q messages in {}: {} / {}", file_name, err, core->raw_messages.size());
    }
    catch (const exception& ex) { Log("FATAL EXCEPTION: {}", ex.what()); }
}

shared_ptr<Components::ACoordinator> Components::ACoordinator::singleton;
