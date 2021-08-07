using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Nixill.CalcLib.Modules;
using Nixill.DiceLib;
using Nixill.Discord.ShadowRoller.Commands;

namespace Nixill.Discord.ShadowRoller
{
  public class ShadowRollerMain
  {
    internal static DiscordClient Discord;
    internal static SlashCommandsExtension Commands;

    static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
      // Let's load CalcLib modules
      MainModule.Load();
      DiceModule.Load();

      // Let's get the bot set up
#if DEBUG
      string botToken = File.ReadAllLines("cfg/debug_token.cfg")[0];
#else
      string botToken = File.ReadAllLines("cfg/token.cfg")[0];
#endif

      Discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = botToken,
        TokenType = TokenType.Bot
      });

      Commands = Discord.UseSlashCommands();

      await Discord.ConnectAsync();

#if DEBUG
      Commands.RegisterCommands<RollCommand>(608847976554692611L);
#else
      Commands.RegisterCommands<RollCommand>();
#endif

      await Task.Delay(-1);
    }
  }
}