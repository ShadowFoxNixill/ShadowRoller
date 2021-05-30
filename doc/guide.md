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
Description|Roll command|Example output|Notes
:-|:-|:-|:-
To roll a d20...|`/roll d20`|**16**|
To roll three d8s...|`/roll 3d8`|**12** [5, 3, 4]|Numbers are shown in the order originally rolled.
