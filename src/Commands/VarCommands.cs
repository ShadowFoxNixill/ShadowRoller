using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Nixill.CalcLib.Objects;
using Nixill.CalcLib.Parsing;
using Nixill.CalcLib.Varaibles;
using Nixill.Discord.Extensions;

namespace Nixill.Discord.ShadowRoller.Commands
{
  [SlashCommandGroup("function", "Commands for manipulating functions")]
  public class VarCommands : SlashCommandModule
  {
    private static Regex rgxName = new Regex(@"^\$?[a-z][a-z-_0-9]*[a-z0-9]$");

    [SlashCommand("save", "Saves a function.")]
    public async Task SaveFunction(InteractionContext ctx,
      [Option("name", "The name of the function.")] string name,
      [Option("text", "The text of the function.")] string text)
    {
      // Pre-execution checks
      if (text.Contains('"'))
      {
        await ctx.ReplyAsync("Shadow Roller doesn't support strings.");
        return;
      }

      name = name.ToLower();

      if (!rgxName.IsMatch(name))
      {
        await ctx.ReplyAsync("Invalid function name.");
        return;
      }

      if (name.StartsWith('$') && ctx.User.Id != ShadowRollerMain.Owner)
      {
        await ctx.ReplyAsync("Only the bot's owner may save global functions.");
        return;
      }

      await ctx.DeferAsync();

      CalcObject obj = null;

      try
      {
        // Get the expression and some intermediate objects ready
        obj = CLInterpreter.Interpret(text);

        string input = obj.ToCode();

        CLVariables.Save()
      }
      catch (Exception ex)
      {
        StringBuilder builder = new StringBuilder();
        if (obj != null) builder.Append($"Input interpretation: `{obj}`\n\n");
        builder.Append("An error occurred");
#if DEBUG
        builder.Append(".\n\nThe stack trace is as follows:\n");
        builder.Append($"{ex}");
#else
        builder.AppendLine($": {ex.GetType().Name}: {ex.Message}");
#endif

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(builder.ToString()));
      }
    }
  }
}