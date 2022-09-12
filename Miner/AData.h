#pragma once
#include "pch.h"

namespace Components {
    struct AData {
        protected:
            bool is_valid = true;
            string log_prefix = "[<NotSet>]";

        public:
            virtual ~AData() = default;

            template<typename ... T>
            void Log(const string& fmt_str, T&& ... in) const {
                try {
                    cout << log_prefix << " " << vformat(string_view(fmt_str), make_format_args(in...)) << "\n";
                }
                catch(const exception& ex) {
                    cout << log_prefix << "[FORMATTING ERROR] " << fmt_str << " " << "(Exception: " << ex.what() << ")";
                }
            }

            [[nodiscard]] bool IsValid() const noexcept { return is_valid; }
            operator bool() const noexcept { return is_valid; }
            void Invalidate(bool new_is_valid = false) { is_valid = new_is_valid; }

        protected:
            explicit AData(const string& logPrefix) : log_prefix(logPrefix) {}
    };
}