using DSharpPlus;
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using SomeDiscordBotThing.commands;
using Microsoft.Extensions.Logging;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.EventArgs;
using DSharpPlus.VoiceNext;
namespace SomeDiscordBotThing
{   

    class Bot
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.MainAsync().GetAwaiter().GetResult();
        }
    }
    public class Program
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        


        public async Task MainAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = Resource1.bottoken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);
            Client.UseVoiceNext();

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                AckPaginationButtons = true,
                Timeout = TimeSpan.FromSeconds(60)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { "-" },
                EnableDms = false,
                EnableDefaultHelp = false,
                DmHelp = true
            };
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<ExampleVoiceCommands>();
            Commands.RegisterCommands<commandthingy>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
