# Wrestler Match Editor
This mod lets you to set the wrestler match settings in a text file and load them in the game. Works similar to my other mod Booker Card Editor

In the mod directory open the `CustomMatch.txt` in a text editor and put your custom match settings there. Once done, save the file and in booker mode calendar press `+` (rebindable in the config file) to load them in game.

List of match settings and templates/examples are provided below and in the text files.

Match settings:
- `show_type:` (optional) sets the show type, supported: 0 - no show, 1 - TV taping, 2 - Pay-per-view, 4 - interpromotional, 5 - charity, 6 - memorial. Example: `card_type: 2`.
- `match_size` (optional) will set the number of contestants in the match. Example: `match_size: 3`
- `gimmick` (optional) gimmick will set the match gimmick (first line on the match description in the calendar screen). Example: `gimmick: 29`
- `match_type` (optional) will set the match type (second line on the match description in the calendar screen). Example: `match_type: 18`
- `opponent` (optional) opponent will set a specific opponent. Note that picking someone from a different promotion will set the show type to interpromotional contest. Example: `opponent: 106`.

Gimmicks & Matches:
```
Gimmicks:
1: Hardcore
2: Furniture Smash (Tables?)
3: Steel Cage
4: Empty Arena
5: Hell in a Cell
6: Loser Leaves
7: Hair Vs Hair
8: Race Against Time
9: Multiple Referee
10: Guest Referee
11: Backstage (?)
12: Handicap
13: Mask Vs Mask
14: No Ring
15: Tables & Chairs
16: Tag Team
17: Team
18: Shoot Fight
19: Submission
20: Street Fight
21: First Blood
22: Torture Chamber
23: Non Title
24: Open Challenge
25: Guest Partner
26: Cage Fight
27: Barbed Wire
28: Exploding
29: Exploding Barbed Wire
30: Red Light
31: Blackout
32: Disco
33: Buried Alive
34: Backstage  (Hollywood?)
35: Christmas Chaos
36: Exposed Wood
37: Exposed Metal
51: Ropeless (square ring)
52: Ropeless (hexagon ring)
61: Steel Cage (square ring)
62: Steel Cage (hexagon ring)
71: Ropeless Cage (square ring)
72: Ropeless Cage (hexagon ring)
101: random furniture
102: random weapon
103: random furniture + random weapon

Matches:
1: Confrontation
2: Singles
3: Best Of Three
4: Ironman
5: Last Laugh
6: Submission
7: Last Man Standing
8: Street Fight
9: First Blood
10: Sumo Contest
11: Shoot Fight
12: Triple Threat
13: Handicap
14: Tag Team
15: Tag Elimination
16: Team
17: War
18: Elimination
19: Battle Royal
20: Countdown Battle Royal
21: Countdown Elimination
22: Gauntlet
23: Escape To Victory
24: Furniture Smash
25: Hardcore
```
Tutorial Template:
```
# show_type will set a type, supported ones are: 0 - no show, 1 - TV taping, 2 - Pay-per-view,
# 4 - interpromotional contest, 5 - charity, 6 - memorial
# NOTE: 3 - tournament, however I wouldn't recommend you to use it,
# it might be an incomplete/scrapped feature considering it never appears in game.

show_type: 2

# match_size will set the number of wrestlers. 

match_size: 10

# gimmick will set the match gimmick (first line on the match description in the calendar screen)

gimmick: 29

# match_type will set the match type (second line on the match description in the calendar screen)

match_type: 25

# opponent will set a specific opponent. Note that picking someone from a different promotion
# will set the show type to interpromotional contest

opponent: 106

# Once you are done, go into wrestler mode and in the calendar screen, press the + button on numpad
# (rebindable in the separate config file) and you should see the changes.
# Note that some gimmick/match combinations may or may not work and/or switch to something else by the game.
```