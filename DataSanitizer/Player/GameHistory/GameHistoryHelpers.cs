namespace Database;

public partial class Database {
    // TODO: validate team decision messages from queueblock are being actually team decision messages
    /*
     * JSON structure:
     * {
      "id": "992642470045290547",
      "type": "Default",
      "timestamp": "2022-07-02T04:06:47.264+00:00",
      "timestampEdited": null,
      "callEndedTimestamp": null,
      "isPinned": false,
      "content": "@otis, @.cgXD✰, @Bella., @Evil, @Whale, @kimo",
      "author": {
        "id": "351735054969470976",
        "name": "6MansBot",
        "discriminator": "3462",
        "nickname": "Bot 6MansBot",
        "color": "#992D22",
        "isBot": true,
        "avatarUrl": "https://cdn.discordapp.com/avatars/351735054969470976/9d680901ca1d23aeab05594b7078f1f7.png?size=512"
      },
      "attachments": [],
      "embeds": [
        {
          "title": "Lobby #886 is ready!",
          "url": null,
          "timestamp": null,
          "description": "You may now join the team channels",
          "color": "#FBBFFD",
          "footer": {
            "text": "Powered by 6mans",
            "iconUrl": "https://images-ext-1.discordapp.net/external/PKgf95hjg8sEu03F-HUZQiSE5fIelBbKuYugDEWdo3w/%3Fv%3D1/https/cdn.discordapp.com/emojis/468949999909339146.png"
          },
          "images": [],
          "fields": [
            {
              "name": "-Team 1-",
              "value": "[Whale-](https://www.rl6mans.com/profile/Whale-), [ohtits](https://www.rl6mans.com/profile/ohtits), [kimo](https://www.rl6mans.com/profile/kimo)",
              "isInline": false
            },
            {
              "name": "-Team 2-",
              "value": "[cg](https://www.rl6mans.com/profile/cg), [Bella](https://www.rl6mans.com/profile/Bella), [Evil](https://www.rl6mans.com/profile/Evil)",
              "isInline": false
            },
            {
              "name": "Creates the lobby:",
              "value": "@Evil",
              "isInline": false
            }
          ]
        }
      ],
      "stickers": [],
      "reactions": [],
      "mentions": [
        {
          "id": "213080978111987712",
          "name": "Bella.",
          "discriminator": "9149",
          "nickname": "Bella the Elite",
          "isBot": false
        },
        {
          "id": "236485142728671232",
          "name": "Evil",
          "discriminator": "0676",
          "nickname": "ControllerEvil",
          "isBot": false
        },
        {
          "id": "430460963293233152",
          "name": "Whale",
          "discriminator": "2735",
          "nickname": "Whale",
          "isBot": false
        },
        {
          "id": "290318442882400256",
          "name": ".cgXD✰",
          "discriminator": "6044",
          "nickname": "cg",
          "isBot": false
        },
        {
          "id": "403256669792108545",
          "name": "kimo",
          "discriminator": "2593",
          "nickname": "kimo",
          "isBot": false
        },
        {
          "id": "582287073524842496",
          "name": "otis",
          "discriminator": "1111",
          "nickname": "otis",
          "isBot": false
        }
      ]
    },
     */
    private bool UpdateTeam(ref List<string> team, Queue found_queue, ref int match_id, bool t1 = true) {
        foreach (var str in team) {
            string embedded_name = GetPlayerNameFromEmbeddedLink(str);
            Player.Player? player = players.Find(p => p.HasName(ref embedded_name));
            if (player != null /*&& found_queue.Value.players_in_queue.Contains(player)*/)
                if (t1) {
                    found_queue.team_one.Add(player);
                }
                else {
                    found_queue.team_two.Add(player);
                }
            else {
                Console.WriteLine("[UpdateTeamDecisions] Could not find player {0} in queue and match id = {1}",
                                  embedded_name, match_id);
                return false;
            }
        }
        return true;
    }

    private void UpdateTeamDecisions() {
        // parse teams decided messages, insert players into teams within queues
        int decisions_updated = 0;
        try {
            foreach (var from_queue_block in queue_blocks) {
                foreach (var message in from_queue_block.teams_decided_messages) {
                    var team_one_raw = new List<string>(message.GetEmbeddedField(0).value.Split(','));
                    var team_two_raw = new List<string>(message.GetEmbeddedField(1).value.Split(','));
                    var match_id = message.GetLobbyId();

                    //Console.WriteLine(match_id);
                    if (match_id >= 0) {
                        foreach (var queue in queues) {
                            if (queue.match_id == match_id) {
                                if (UpdateTeam(ref team_one_raw, queue, ref match_id) &&
                                    UpdateTeam(ref team_two_raw, queue, ref match_id, false)) {
                                    Console.WriteLine("\t[UpdateTeamDecisions] Updated lobby {0} with team 1 = {1}, {2}, {3} and team 2 = {4}, {5}, {6}",
                                                      match_id, team_one_raw[0], team_one_raw[1], team_one_raw[2],
                                                      team_two_raw[0],
                                                      team_two_raw[1], team_two_raw[2]);
                                    decisions_updated++;
                                }
                            }
                        }
                    }
                }
            }
        }

        catch (Exception e) {
            Console.WriteLine("\t[UpdateTeamDecisions] Exception caught! Message: {0}\nStack Trace: {1}", e.Message, e.StackTrace);
        }

        Console.WriteLine("\t[UpdateTeamDecisions] Set team decisions for {0} queues, {1} queues unaccounted for!", decisions_updated, queue_blocks.Count - decisions_updated);
    }

    private void SetMatchResults() {
        Console.ForegroundColor = ConsoleColor.Cyan;
        
        /*
         *  int errors = 0
         *  for each queue in queues
         *      if queue
         *          bool found_queue = false
         *          for each report in sr_messages
         *              if lobby ids match
         *                  set queue.team_one_won
         *                  found_queue = true
         *                  break
         *          if !found_queue
         *              Log error message and increment errors
         *      else
         *          Log error message and increment errors
         *  Log set results complete message and show error count
         */
        
        Console.ForegroundColor = ConsoleColor.White;
    }
    private void SetPlayerRecords() {
    }
}