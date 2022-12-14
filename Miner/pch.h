#pragma once
#include <algorithm>
#include <chrono>
#include <concepts>
#include <filesystem>
#include <format>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <list>
#include <map>
#include <nlohmann/json.hpp>
#include <ranges>
#include <regex>
#include <sstream>
#include <string>
#include <tuple>
#include <type_traits>
#include <vector>
#include "EMessageType.h"
#include "ESeason.h"
#include "ETeamTag.h"

using nlohmann::json;
using std::chrono::hh_mm_ss;
using std::chrono::milliseconds;
using std::chrono::parse;
using std::chrono::sys_time;
using std::chrono::system_clock;
using std::chrono::time_point;
using std::cout;
using std::exception;
using std::filesystem::path;
using std::format;
using std::ifstream;
using std::istringstream;
using std::list;
using std::make_format_args;
using std::make_shared;
using std::map;
using std::ofstream;
using std::ostringstream;
using std::ranges::none_of;
using std::regex;
using std::regex_search;
using std::shared_ptr;
using std::smatch;
using std::stoull;
using std::string;
using std::stringstream;
using std::string_view;
using std::tm;
using std::to_string;
using std::tuple;
using std::vector;
using std::vformat;
