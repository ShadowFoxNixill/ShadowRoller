TheFoxySurprise's expression parser features the following operators:

# Operator summary
Operator|Description
-:|:-
`+`|Adds two numbers, or joins two lists.
`-`|Subtracts two numbers, or joins two lists with the right-hand list negated.
`*`|Multiplies two numbers.
`/`|Divides two numbers.
`//`|Divides two numbers and discards the remainder.
`%`|Divides two numbers and gives the remainder.
`^`|Raises one number to the power of another.
Prefix `-`|Negates a number or list.
`d`|Rolls a specific number of dice.
Prefix `d`|Rolls one die.
`u`(comparison)|Rolls dice until a comparison is satisfied.
`k`|Keeps a specific number of dice from the start of the list.
`kh`|Keeps a specific number of the highest dice from a list.
`kl`|Keeps a specific number of the lowest dice from a list.
`k`(comparison)|Keeps dice from a list that satisfy a comparison.
`dh`|Drops a specific number of the highest dice from a list.
`dl`|Drops a specific number of the lowest dice from a list.
`d`(comparison)|Drops dice from a list that satisfy a comparison.
`**`|Performs an operation multiple times.

# Operator details
|Left operand|Operator|Right operand|Description
|-:|:-:|:-|:-
|Number "l"|`+`|Number "r"|Gives the sum of "l" + "r".
|List¹ "l"|`+`|List¹ "r"|Joins the two lists.
|Number "l"|`-`|Number "r"|Gives the difference of "l" - "r".
|List¹ "l"|`-`|List¹ "r"|Negates "r", then joins the two lists.
|Number "l"|`*`|Number "r"|Gives the product of "l" * "r".
|Number "l"|`*`|List "r"|Multiplies each item in "r" by "l".
|List "l"|`*`|Number² "r"|Multiplies each item in "l" by "r".
|Number "l"|`/`|Number² "r"|Gives the (decimal) quotient of "l" / "r".
|List "l"|`/`|Number² "r"|Divides each item in "l" by "r".
|Number² "l"|`//`|Number² "r"|Gives the (integer) quotient of "l" / "r".
|Number² "l"|`%`|Number² "r"|Gives the remainder of "l" / "r".
|Number² "l"|`^`|Number² "r"|Raises "l" to the power of "r".
||`-`|Number "n"|Negates "n".
||`-`|List "l"|Negates every value in "l".
|Number² "n"|`d`|Number "s"|Rolls "n" dice that have sides from 1 to "s".
|Number² "n"|`d`|List "l"|Rolls "n" dice that have sides described in "l".
||`d`|Number "s"|Rolls one die that has sides from 1 to "s".
||`d`|List "l"|Rolls one die that has sides described in "l".
|Number "s"|`u`|Comparison "c"|Rolls dice with sides from 1 to "s" until a rolled value satisfies the comparison "c" (or 25 rolls are complete).
|List "l"|`u`|Comparison "c"|Rolls dice with sides described in "l" until a rolled value satisfies the comparison "c" (or 25 rolls are complete).
|List¹ "l"|`k`|Number² "n"|Keeps the first "n" items in "l".
|List¹ "l"|`kh`|Number² "n"|Keeps the highest "n" items in "l".
|List¹ "l"|`kl`|Number² "n"|Keeps the lowest "n" items in "l".
|List¹ "l"|`k`|Comparison "c"|Keeps the items from "l" that satisfy the comparison "c".
|List¹ "l"|`dh`|Number² "n"|Drops the highest "n" items from "l".
|List¹ "l"|`dl`|Number² "n"|Drops the lowest "n" items from "l".
|List¹ "l"|`d`|Comparison "c"|Drops the items from "l" that satisfy the comparison "c".
|Expression "o"|`**`|Number² "n"|Operates "o" "n" times and returns a list of the results.

¹ A number where a list is expected is treated as a list containing only that number.  
² A list where a number is expected is treated as the sum of that list's elements.

# Operator precedence
Within a single bracket group (`()`, `[]`, `{}`, `,`), multiple operators are parsed from high to low order. The `^` level is parsed right to left; the others left to right.

Level|RTL?|Operators
-:|:-:|:-
100||`d`, `u` (comparison), `d` (prefix)
95||`k`, `kh`, `kl`, `dh`, `dl`, `k` (comparison), `d` (comparison)
15|✔|`^`
10||`*`, `/`, `//`, `%`
0||`+`, `-`, `-` (prefix)
-10||`**`