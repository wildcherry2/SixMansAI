#include "pch.h"
#include "AQueue.h"

Components::AQueue::AQueue(const int& match_id, const shared_ptr<AMessage> teams_picked_message, const shared_ptr<AMessage> score_report_message) : AData("[AQueue]"), match_id(match_id){}
