#pragma once
#include "pch.h"
struct AData {
    protected:
        bool is_valid = false;
        string log_prefix = "[<NotSet>]";

    public:
        static void Log(const string& in) noexcept {
            
        }

        void Invalidate(bool new_is_valid = false) { is_valid = new_is_valid; }

        bool IsValid() const noexcept { return is_valid; }

        operator bool() const noexcept { return is_valid; }
};