#pragma once
#include <nlohmann/json.hpp>
#include <string>
#include <algorithm>
#include <vector>
#include <iomanip>
#include <sstream>
#include <chrono>
#include <map>
#include <tuple>
#include <iostream>
#include <fstream>
#include <format>
#include <type_traits>
#include <list>
#include <regex>
#include "EMessageType.h"
#include "ESeason.h"

using nlohmann::json;
using std::string;
using std::vector;
using std::tm;
using std::stringstream;
using std::chrono::hh_mm_ss;
using std::chrono::time_point;
using std::chrono::sys_time;
using std::chrono::milliseconds;
using std::map;
using std::shared_ptr;
using std::make_shared;
using std::tuple;
using std::cout;
using std::ifstream;
using std::ofstream;
using std::format;
using std::list;
using std::regex;

class APlayer;
enum class ETeamTag;
using Season = tuple<EMonth, EYear>;
using Team = tuple<APlayer, APlayer, APlayer>;
using Lobby = tuple<Team, Team>;
using ScoreReport = tuple<int, Lobby, ETeamTag>;