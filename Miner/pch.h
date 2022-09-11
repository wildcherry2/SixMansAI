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
using std::filesystem::path;
using std::format;
using std::ifstream;
using std::istringstream;
using std::list;
using std::make_shared;
using std::map;
using std::ofstream;
using std::ostringstream;
using std::regex;
using std::shared_ptr;
using std::stoull;
using std::string;
using std::stringstream;
using std::tm;
using std::tuple;
using std::vector;
