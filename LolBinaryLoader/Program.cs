using LolBinaryLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LoLBinaryLoader
{
    class Program
    {
        public static void Main()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Task.WaitAll(new List<Task>
            {
                Task.Factory.StartNew(() => new RADS(GenerateWindowsSettings()).DownloadClients()),
                Task.Factory.StartNew(() => new RADS(GenerateMacSettings()).DownloadClients())
            }.ToArray());

            sw.Stop();
            ReportTimeTaken(sw.ElapsedMilliseconds);
        }

        private static void ReportTimeTaken(long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            Console.WriteLine("Hours\tMinutes\tSeconds\tMiliseconds");
            string answer = string.Format("{0:D2}\t{1:D2}\t{2:D2}\t{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            Console.WriteLine(answer);
        }

        private static Settings GenerateMacSettings()
        {
            return new Settings
            {
                ClientType = ClientType.Mac,
                BaseUrl = "http://l3cdn.riotgames.com/releases/live/projects/lol_game_client/releases/",
                ListFileName = "releaselisting_EUW",
                UrlPath = "/files/League of Legends.exe.compressed"
            };
        }

        private static Settings GenerateWindowsSettings()
        {
            return new Settings
            {
                ClientType = ClientType.Windows,
                BaseUrl = "http://l3cdn.riotgames.com/releases/Maclive/projects/lol_game_client/releases/",
                ListFileName = "releaselisting_EUW",
                UrlPath = "/files/LeagueofLegends.app/Contents/MacOS/LeagueofLegends.compressed"
            };
        }
    }
}