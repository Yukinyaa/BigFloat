# BigFloat
Fixed-accurate big number in C#, Made for incremental game

WOW SO BIG NUMBER

## Implimentation
Expressed by scientific notation. (e.g. 1.23e456 or 1.23E+456) 

Exponent(1.23 part on example) is `float`,  significand(456 part on example) is `BigInteger`.

Parsing may have some loss in precision, but designed to precise up to 9~10 digits.

## Features
Basic operations like `+` `-` `/` `*` `%` `ABS` etc.

# DISCLAMER
*** : Code is fresh and not tested thoroughly. provided AS-IS***

there isn't even test code ¯\\_(ツ)_/¯ 

# How to use
Intended for UnityEngine.

To use outside unity, change m to double and Mathf to Math then there *should* be no problem 
