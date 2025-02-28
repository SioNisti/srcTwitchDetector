using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace srcTwitchDetector
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        [STAThread]
        private static async Task Main(string[] args)
        {
            Console.Write("Enter Game Name: ");
            string gameName = Console.ReadLine();

            // Fetch the game ID using the game name
            string gameId = await GetGameIdByName(gameName);
            if (string.IsNullOrEmpty(gameId))
            {
                Console.WriteLine("Game not found.");
                return;
            }

            int offset = 0;
            bool hasData;
            var runsList = new List<RunInfo>();

            do
            {
                string apiUrl = $"https://www.speedrun.com/api/v1/runs?game={gameId}&orderby=submitted&direction=desc&embed=players&max=200&offset={offset}";
                string jsonResponse = await client.GetStringAsync(apiUrl);
                JObject json = JObject.Parse(jsonResponse);

                var runs = json["data"];
                hasData = runs.Any();

                foreach (var run in runs)
                {
                    try
                    {
                        string playerName = run["players"]["data"][0]["rel"].ToString() == "guest" ?
                            run["players"]["data"][0]["name"].ToString() :
                            run["players"]["data"][0]["names"]["international"].ToString();

                        var videoEntries = run["videos"]?.Children().ToList().ElementAtOrDefault(0)?.Children().ToList();
                        if (videoEntries != null)
                        {
                            foreach (var video in videoEntries)
                            {
                                foreach (var uri in video.Children())
                                {
                                    if (uri["uri"] != null && uri["uri"].ToString().Contains("twitch"))
                                    {
                                        var runInfo = new RunInfo
                                        {
                                            PlayerName = playerName,
                                            RunDate = run["date"]?.ToString(), // Use the "date" field
                                            WebLink = run["weblink"].ToString(),
                                            TwitchUri = uri["uri"].ToString(),
                                            RunStatus = run["status"]["status"].ToString()
                                        };
                                        runsList.Add(runInfo);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());
                    }
                }

                offset += 200;
            } while (hasData);

            // Output the data as JSON
            string jsonOutput = JsonConvert.SerializeObject(runsList, Formatting.Indented);
            Console.WriteLine(jsonOutput);
            File.WriteAllText($"{gameName}-{gameId}.json",jsonOutput);

            Console.WriteLine($"Done!\nThe results have been saved as \"{gameName}-{gameId}.json\"");
            Console.ReadKey();
        }

        private static async Task<string> GetGameIdByName(string gameName)
        {
            string apiUrl = $"https://www.speedrun.com/api/v1/games?name={Uri.EscapeDataString(gameName)}";
            string jsonResponse = await client.GetStringAsync(apiUrl);
            JObject json = JObject.Parse(jsonResponse);

            var games = json["data"];
            if (games.Any())
            {
                return games[0]["id"].ToString();
            }

            return null;
        }

        public class RunInfo
        {
            public string PlayerName { get; set; }
            public string RunDate { get; set; } // Date when the run was performed
            public string WebLink { get; set; }
            public string TwitchUri { get; set; }
            public string RunStatus { get; set; }
        }
    }
}