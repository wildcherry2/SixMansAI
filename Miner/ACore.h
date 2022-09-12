#pragma once
#include "AData.h"
#include "AMessage.h"
#include "APlayer.h"

class ACore : public Components::AData{
	private:
		static shared_ptr<ACore> singleton;
	    ACore() : AData("[ACore]"){}
	public:
		static shared_ptr<ACore> GetSingleton();
	    vector<shared_ptr<Components::AMessage>> raw_messages;
	    vector<shared_ptr<Components::APlayer>> players;
	    vector<shared_ptr<Components::AQueue>> queues;
};
