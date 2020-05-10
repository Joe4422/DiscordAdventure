using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordZMachine
{
    class Program
    {
        private DiscordSocketClient _client;
        private GameInstanceManager gameInstanceManager;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(TokenType.Bot, System.IO.File.ReadAllText(@"token.txt"));
            await _client.StartAsync();

            gameInstanceManager = new GameInstanceManager();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith('&') && message.Author.IsBot == false)
            {
                if (message.Content.StartsWith("&!"))
                {
                    string command = message.Content.Split(" ")[0][2..];
                    switch (command)
                    {
                        case "games":
                            ListGames(message.Channel);
                            break;
                        case "start":
                            StartGame(message);
                            break;
                        case "up":
                            IsUp(message.Channel);
                            break;
                    }
                }
                else
                {
                    string input = message.Content[1..];
                    gameInstanceManager.EnqueueIncomingMessage(message.Channel, input);
                }
            }
            
            return Task.CompletedTask;
        }

        private void ListGames(ISocketMessageChannel channel)
        {
            string output = "Available games are:\n";
            foreach (string s in Directory.GetFiles(Directory.GetCurrentDirectory()).Where((x) => x.ToLower().EndsWith(".dat") || x.ToLower().Contains(".z")))
            {
                output += " • " + Path.GetFileName(s) + "\n";
            }
            channel.SendMessageAsync(output);
        }

        private void StartGame(SocketMessage message)
        {

            string[] words = message.Content.Split(" ");
            if (words.Length == 1)
            {
                message.Channel.SendMessageAsync("Please specify a game!");
            }
            else
            {
                string gameFile = words[1];
                if (File.Exists(gameFile))
                {
                    message.Channel.SendMessageAsync("Starting instance...");
                    GameInstanceManager.CreateInstanceResult result = gameInstanceManager.CreateInstance(message.Channel, File.OpenRead(gameFile));

                    message.Channel.SendMessageAsync(result switch
                    {
                        GameInstanceManager.CreateInstanceResult.Fail_Exists => "An instance is already running in this channel!",
                        GameInstanceManager.CreateInstanceResult.Fail_InstanceLimit => "There are too many instances already running! Try again later.",
                        GameInstanceManager.CreateInstanceResult.Fail_IOError => "Error opening file! Please try a different one.",
                        GameInstanceManager.CreateInstanceResult.Success => null,
                        _ => throw new InvalidEnumArgumentException("Invalid enum member passed.")
                    });
                }
                else
                {
                    message.Channel.SendMessageAsync("That game does not exist!");
                }
            }
        }

        private void IsUp(ISocketMessageChannel messageChannel)
        {
            messageChannel.SendMessageAsync(gameInstanceManager.InstanceExists(messageChannel) switch {
                true => "An instance is currently running in this channel.",
                false => "No instance is currently running in this channel."
            });
        }
    }
}
