using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Nixill.CalcLib.Modules;
using Nixill.DiceLib;

namespace Nixill.Discord.ShadowRoller
{
  public class ShadowRollerMain
  {
    internal static DiscordClient Discord;
    internal static DiscordSlashClient Commands;

    static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
      // Let's load CalcLib modules
      MainModule.Load();
      DiceModule.Load();

      // Let's get the bot set up
#if DEBUG
      string botToken = File.ReadAllLines("debug_token")[0];
#else
      string botToken = File.ReadAllLines("token")[0];
#endif

      Discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = botToken,
        TokenType = TokenType.Bot
      });

      Commands = new DiscordSlashClient(new DiscordSlashConfiguration()
      {
        Token = botToken,
        Client = Discord,
        Logger = Discord.Logger,
      });

      Discord.InteractionCreated += Commands.HandleGatewayEvent;

      await Discord.ConnectAsync();

      Commands.RegisterCommands(Assembly.GetExecutingAssembly());

      await Commands.StartAsync();
      await Task.Delay(-1);
    }
  }
}