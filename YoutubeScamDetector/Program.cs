using YoutubeExplode;
using YoutubeExplode.Channels;
using YoutubeExplode.Common;

namespace YoutubeScamDetector {
    internal class Program {
        static async Task Main(string[] args) {
            while (true) {
                var youtube = new YoutubeClient();
                Console.WriteLine("YouTube video scam checker v1.0");
                Console.Write("Channel URL: ");
                var channelId = ChannelId.TryParse(Console.ReadLine());
                if (channelId == null) {
                    Console.WriteLine("Invalid channel!");
                    return;
                }
                var channel = await youtube.Channels.GetAsync((ChannelId)channelId);
                if (channel.Id == "UC7JMha1kjOS7gsJXwNtosNw") {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Hey! This is the developer's channel, subscribe :)");
                    Console.ResetColor();
                }
                Console.WriteLine("Checking uploads...");
                var uploads = await youtube.Channels
                    .GetUploadsAsync((ChannelId)channelId)
                    .CollectAsync(1000);
                Console.WriteLine("Got " + uploads.Count + " uploads (capped at 1000)");

                Dictionary<string, int> occurrencesTitle = new Dictionary<string, int>();
                foreach (var upload in uploads) {
                    if (occurrencesTitle.ContainsKey(upload.Title)) {
                        int amt = occurrencesTitle[upload.Title];
                        occurrencesTitle[upload.Title] = amt + 1;
                    }
                    else {
                        occurrencesTitle.Add(upload.Title, 1);
                    }
                }

                Console.WriteLine("Title checking results: ");

                var percentage = occurrencesTitle.Values.Count / (float)uploads.Count;
                percentage *= 100;
                if (percentage < 25) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tTitles: " + percentage + "% legit");
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    Console.WriteLine("$$$PLEASE REPORT THIS TO GOOGLE$$$$");
                    Console.WriteLine("$$$$$THIS CHANNEL IS NOT LEGIT$$$$$");
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    Console.ResetColor();
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\tTitles: " + percentage + "% legit");
                    Console.WriteLine("This channel is OK!");
                    Console.ResetColor();
                }
                Console.Write("r=restart q=quit");
                var cmd = Console.ReadLine();
                if (cmd == "q") {
                    break;
                }
            }
        }
    }
}