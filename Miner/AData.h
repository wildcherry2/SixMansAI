#pragma once
#include "pch.h"

namespace Components {
    struct AData {
        protected:
            bool is_valid = true;
            string log_prefix = "[<NotSet>]";

        public:
            virtual ~AData() = default;

            void    Log(const string& str) const noexcept {
                cout << log_prefix << " " << str << "\n";
            }

            [[nodiscard]] bool IsValid() const noexcept { return is_valid; }
            operator bool() const noexcept { return is_valid; }
            void Invalidate(bool new_is_valid = false) { is_valid = new_is_valid; }

        protected:
            explicit AData(const string& logPrefix) : log_prefix(logPrefix) {}
    };
}