using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace srcTwitchDetector
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string jsonfile = Console.ReadLine();
            //string jsonfile = @"P:\ae3 runs\ae26.json";
            JObject json;
            string copy = "";

            Console.WriteLine(jsonfile);
            using (StreamReader files = File.OpenText(jsonfile))
            using (JsonTextReader reader = new JsonTextReader(files))
            {
                json = (JObject)JToken.ReadFrom(reader);
            }

            for (int i = 0; i < 200; i++)
            {
                try
                {
                    //Console.WriteLine(json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]);
                    if (json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"].ToString().Contains("twitch"))
                    {
                        /*
                        Console.WriteLine($"{json["data"][i]["players"]["data"][0]["names"]["international"]} - " +
                            $"{json["data"][i]["weblink"]} - " +
                            $"{json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]}");*/
                        if (json["data"][i]["players"]["data"][0]["rel"].ToString() == "guest")
                        {
                            Console.WriteLine($"{json["data"][i]["players"]["data"][0]["name"]} - " +
                            $"{json["data"][i]["weblink"]} - " +
                            $"{json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]}");
                        } else
                        {
                            Console.WriteLine($"{json["data"][i]["players"]["data"][0]["names"]["international"]} - " +
                            $"{json["data"][i]["weblink"]} - " +
                            $"{json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]}");
                        }
                        /*Console.WriteLine($"{json["data"][i]["players"]["data"][0]["names"]["international"]} - ");
                        Console.WriteLine($"{json["data"][i]["weblink"]} - ");
                        Console.WriteLine($"{json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]}");*/

                        //Console.WriteLine($"{json["data"][i]["players"]["data"][0]["names"]["international"]}");
                        //copy += $"{json["data"][i]["weblink"]} {json["data"][i]["videos"].Children().ToList().ElementAt(0).Children().ToList()[0][0]["uri"]}";
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}