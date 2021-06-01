Welcome to Shadow Roller, a discord bot by Nixill for rolling dice. Here are just some of the things you can do with it!

# Commands
As of right now, due to [some](https://github.com/Soyvolon/DSharpPlus.SlashCommands/issues/15) [issues](https://github.com/Soyvolon/DSharpPlus.SlashCommands/issues/14) with the library used for commands, there are four commands for rolling dice:

* `/roll` - Rolls dice with a random seed and provides a simple answer.
* `/roll_detailed` - Rolls dice with a random seed and provides a more detailed answer.
* `/roll_seeded` - Rolls dice with a fixed seed and provides a simple answer.
* `/roll_seeded_detailed` - Rolls dice with a fixed seed and provides a more detailed answer.

When the issues are fixed, these will all be turned into a single `/roll` command, with optional parameters to specify a seed, detailed output, or both.

There is also the `/help` command, which gave you the link to this document, and the `/docs` command, which goes through all the operators more directly.

# Rolling dice
To roll a specific number of dice, use `/roll [L]d[R]`. Replace `[L]` with the number of dice to roll, and `[R]` with the number of sides each die has. If you're rolling one die, you can skip the `[L]` entirely. Examples:

* `/roll 4d6` - Rolls four six-sided dice, with an output like "**11** [2, 1, 3, 5]".
* `/roll 2d8` - Rolls two eight-sided dice, with an output like "**11** [7, 4]".
* `/roll d20` - Rolls one twenty-sided die, with an output like "**12**".

To roll dice until a specific number comes up, use `!r [L]u=[R]` - replace `[L]` with the number of sides each die has, and `[R]` with the number that stops the rolling. (This number won’t be part of the roll itself.) You can also use `>`, `<`, `>=`, `<=`, `!=`, `%`, or `!%` instead of `=` to make it stop on any number that’s - respectively - greater than, less than, at least, at most, not, a multiple of, or not a multiple of, `[R]`. Examples:

* `/roll 2u=1` - Rolls two-sided dice until a 1 is rolled, e.g. "**6** [2, 2, 2]".
* `/roll 3u!=2` - Rolls three-sided dice until a number that’s not 2 is rolled, e.g. "**2** [2]".
* `/roll 6u>4` - Rolls six-sided dice until a number that’s greater than 4 is rolled, e.g. "**3** [3]".

> *Note*: If the first roll meets the conditions to stop rolling, the result will be "**0** []".
