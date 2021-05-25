using System;
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
      [Description("The expression to roll.")] string roll_text,
      [Description("Whether or not to use detailed output.")] bool detailed = false) =>
    await SeededRollCommandAsync(ctx, roll_text, new Random().Next(), detailed);

#if DEBUG
    [SlashCommand("roll_seeded", 608847976554692611)]
#else
    [SlashCommand("roll_seeded")]
#endif
    [Description("Rolls dice with a specific seed!")]
    public async Task SeededRollCommandAsync(InteractionContext ctx,
      [Description("The expression to roll.")] string roll_text,
      [Description("The seed to use.")] int seed,
      [Description("Whether or not to use detailed output.")] bool detailed = false)
    {
      if (roll_text.Contains('"'))
      {
        await ctx.ReplyAsync("Shadow Roller doesn't support strings.");
        return;
      }

      try
      {
        // Get the expression and some intermediate objects ready
        CalcObject obj = CLInterpreter.Interpret(roll_text);
        CalcValue res;
        CalcNumber num;

        // Get the context and vars ready
        CLContextProvider context = new CLContextProvider();

        context.Add(new DiceContext
        {
          PerFunctionLimit = 50,
          PerRollLimit = 50
        });

        context.Add(new Random(seed));

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

          ret.Append($" *(Seed: {seed})*");
          await ctx.ReplyAsync(ret.ToString());
        }
        else
        {
          DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

          if (input != null) builder.AddField("Input interpretation", $"`{input}`", true);
          builder.AddField("Output value", $"**{result}**", true);
          if (list != null) builder.AddField("Output list", list, true);
          builder.AddField("Random seed", seed.ToString(), true);
          builder.AddField("Note", "In a future version, this will also list all the numbers that were rolled, even if discarded.");

          await ctx.ReplyAsync("", new DiscordEmbed[] { builder.Build() });
        }
      }
      catch (Exception ex)
      {
        await ctx.ReplyAsync($"Exception thrown: {ex}");
      }
    }
  }
}