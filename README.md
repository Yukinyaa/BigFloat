# BigFloat
Fixed-accurate big number in C#, Made for incremental game

WOW SO BIG NUMBER

## Features
Basic operations like `+` `-` `/` `*` `ABS` etc.
Test shows 5 digit accuracy(is being tested though)

# How to use

## Using UPM
Add `https://github.com/Yukinyaa/BigFloat.git` via your UPM package.
See [Guide](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for detailed instruction.

## Oter use
This package is designed for UnityEngine use.

To use outside unity, change m to double and Mathf to Math then there *should* be no problem.

Test cases are intended for 

# Contribution
More test cases are always welcome.

# DISCLAMER
***Code is fresh and not tested thoroughly. provided AS-IS***

## Implimentation
Expressed by scientific notation. (e.g. 1.23e456 or 1.23E+456) 

Exponent(1.23 part on example) is `float`,  significand(456 part on example) is `BigInteger`.

Parsing may have some loss in precision, but designed to precise up to 9~10 digits.

# Contact
![Discord Shield](https://discordapp.com/api/guilds/884039953611362346/widget.png?style=banner2)

