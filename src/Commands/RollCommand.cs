using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.Entities;
using Nixill.CalcLib.Objects;
using Nixill.CalcLib.Parsing;
using Nixill.CalcLib.Varaibles;
using Nixill.DiceLib;

namespace Nixill.Discord.ShadowRoller.Commands
{
  public class RollCommand : BaseSlashCommandModule
  {
    public RollCommand(IServiceProvider p) : base(p) { }

#if DEBUG
    [SlashCommand("roll", 608847976554692611)]
#else
    [SlashCommand("roll")]
#endif
    [Description("Rolls dice with a random seed!")]
    public async Task RollCommandAsync(InteractionContext ctx,
      [Description("The expression to roll.")] string roll_text)
      => await RollMethod(ctx, roll_text, (int)ctx.Interaction.Id, false);

#if DEBUG
    [SlashCommand("roll_seeded", 608847976554692611)]
#else
    [SlashCommand("roll_seeded")]
#endif
    [Description("Rolls dice with a specific seed!")]
    public async Task SeededRollCommandAsync(InteractionContext ctx,
      [Description("The expression to roll.")] string roll_text,
      [Description("The seed to use.")] int seed)
      => await RollMethod(ctx, roll_text, seed, false);

#if DEBUG
    [SlashCommand("roll_detailed", 608847976554692611)]
#else
    [SlashCommand("roll_detailed")]
#endif
    [Description("Rolls dice with a random seed, and provides detailed output.")]
    public async Task DetailedRollCommandAsync(InteractionContext ctx,
      [Description("The expression to roll.")] string roll_text)
      => await RollMethod(ctx, roll_text, (int)ctx.Interaction.Id, true);

#if DEBUG
    [SlashCommand("roll_seeded_detailed", 608847976554692611)]
#else
    [SlashCommand("roll_seeded_detailed")]
#endif
    [Description("Rolls dice with a specific seed, and provides detailed output.")]
    public async Task SeededDetailedRollCommandAsync(InteractionContext ctx,
      [Description("The expression to roll.")] string roll_text,
      [Description("The seed to use.")] int seed)
      => await RollMethod(ctx, roll_text, seed, true);

    public async Task RollMethod(InteractionContext ctx, string roll_text, int seed, bool detailed)
    {
      if (roll_text.Contains('"'))
      {
        await ctx.ReplyAsync("Shadow Roller doesn't support strings.");
        return;
      }

      CalcObject obj = null;

      try
      {
        // Get the expression and some intermediate objects ready
        obj = CLInterpreter.Interpret(roll_text);
        CalcValue res;
        CalcNumber num;

        // Get the context and vars ready
        CLContextProvider context = new CLContextProvider();

        context.Add(new DiceContext
        {
          PerFunctionLimit = 100,
          PerRollLimit = 25
        });

        context.Add(new Random(seed));

        List<(string, CalcList)> history = new List<(string, CalcList)>();
        context.Add(history);

        CLLocalStore vars = new CLLocalStore();

        // Get the outputs ready too!
        string input = null;
        string list = null;
        string result = null;

        // And start parsing things.
        if (obj is CalcExpression exp)
        {
          input = exp.ToCode();
        }
        res = obj.GetValue(vars, context);

        if (res is CalcList lst)
        {
          num = lst.Sum();
          list = lst.ToString(2);
        }
        else
        {
          num = (CalcNumber)res;
        }

        result = num.ToString();

        // Now build the output.
        if (!detailed)
        {
          StringBuilder ret = new StringBuilder();

          if (input != null)
          {
            ret.Append($"Input: `{input}` // Result: ");
          }

          ret.Append($"**{result}**");

          if (list != null)
          {
            ret.Append($" {list}");
          }

          if (history.Count > 0)
          {
            ret.Append($" *(Seed: {seed})*");
          }

          await ctx.ReplyAsync(ret.ToString());
        }
        else
        {
          DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

          if (input != null) builder.AddField("Input interpretation", $"`{input}`", true);
          builder.AddField("Output value", $"**{result}**", true);
          if (list != null) builder.AddField("Output list", list, true);
          builder.AddField("Random seed", seed.ToString(), true);

          StringBuilder historyOutput = new StringBuilder();
          foreach (var item in history)
          {
            historyOutput.AppendLine($"`{item.Item1}`: {item.Item2.ToString(1)}");
          }

          builder.AddField("Rolls", historyOutput.ToString(), false);

          await ctx.ReplyAsync("", new DiscordEmbed[] { builder.Build() });
        }
      }
      catch (Exception ex)
      {
        if (obj != null) await ctx.ReplyAsync($"Input interpretation: `{obj.ToCode()}`\n\nException thrown: {ex}");
        else await ctx.ReplyAsync($"Exception thrown: {ex}");
      }
    }
  }
}