using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Nixill.CalcLib.Objects;
using Nixill.CalcLib.Parsing;
using Nixill.CalcLib.Varaibles;
using Nixill.Discord.Extensions;
using Nixill.Discord.ShadowRoller.Variables;

namespace Nixill.Discord.ShadowRoller.Commands
{
  [SlashCommandGroup("function", "Commands for manipulating functions and variables")]
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

        CLContextProvider context = new CLContextProvider();

        context.Add(ctx.Guild);

        CLVariables.Save(name, obj, context);

        await ctx.EditAsync($"Function `{name}` saved with code `{input}`. To run it use `/roll {{name}}`.");
      }
      catch (Exception ex)
      {
        StringBuilder builder = new StringBuilder();
        if (obj != null) builder.Append($"Input interpretation: `{obj.ToCode()}`\n\n");
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

    [SlashCommand("delete", "Deletes a saved function/variable.")]
    public async Task DeleteFunction(InteractionContext ctx,
      [Option("name", "The name of the function/variable to delete.")] string name
    )
    {
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

      CLContextProvider context = new CLContextProvider();
      context.Add(ctx.Guild);

      CLVariables.Delete(name, context);

      await ctx.EditAsync($"The function `{name}` has been deleted.");
    }

    [SlashCommand("list", "Lists all saved functions/variables.")]
    public async Task ListFunctions(InteractionContext ctx,
      [Option("global", "Whether or not to list global functions.")] bool global = false
    )
    {
      await ctx.DeferAsync();

      IDictionary<string, string> vars;

      if (global) vars = VarIO.Handler.ListVariables(0);
      else vars = VarIO.Handler.ListVariables(ctx.Guild.Id);

      StringBuilder ret = new StringBuilder();
      ret.Append("The following functions are available ");

      if (global) ret.Append("globally");
      else ret.Append("in this server");

      ret.Append(":\n```\n");

      foreach (var variable in vars)
      {
        ret.Append($"{{{variable.Key}}} => {{{variable.Value}}}");
      }

      ret.Append("```");

      await ctx.EditAsync(ret.ToString());
    }
  }
}