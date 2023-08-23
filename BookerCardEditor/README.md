# Booker Card Editor
This mod lets you to set the booker card settings in a text file and load them in the game.

In the mod directory open the `CustomCard.txt` in a text editor and put your custom card settings there. Once done, save the file and in booker mode calendar press `+` (rebindable) to load them in game.

List of card settings and templates/examples are provided below and in the text files.

Card settings:
- `reset_card` (optional) can be used to clear and reset the card, including the attendance. Usage example: `reset_card: true`
- `card_type` (optional) sets the card type, supported: 0 - no show, 1 - TV taping, 2 - Pay-per-view, 4 - interpromotional, 5 - charity, 6 - memorial. Example: `card_type: 2`. Note: interpromotional type will reset the card regardless if `reset_card: true` is set or not.
- `rival_fed` (mandatory if used in an interpromotional type) will set the chosen promotion as rivals in an interpromotional contest. Example: `rival_fed: 3`
- `card_size` (optional) will set a card size, recommended sizes are 6 and 10. Example: `card_size: 10`
- `attendance` (optional) will set the show attendance. Example: `attendance: 2000`
- `territory` (optional) will set the show territory. Example: `territory: 15`. ID list can be found below.

Individual match settings:
- `match_id` (mandatory if you are using this feature) will set the match on the card. Example: `match_id: 1`
- `match_name` (optional) will set the match name displayed on the card. Example: `match_name: Prebooked match`
- `left_name` (optional) will set the left wrestler name displayed on the card. Example: `left_name: Person id 20`
- `left_id` (optional) will set the left wrestler. Example: `left_id: 20`
- `right_name` (optional) will set the right wrestler name displayed on the card. Example: `right_name: Person id 30`
- `right_id` (optional) will set the right wrestler. Example: `right_id: 30`
- match_end (mandatory) will finalize these settings for this match.

TutorialTemplatePart1.txt:
```
# Note: all of these settings are completely optional and can be removed if not needed
# "reset_card: true" can be used if you want to completely clear the card,
# it will also reset the attendance

reset_card: true

# card_type will set a type, supported ones are: 0 - no show, 1 - TV taping, 2 - Pay-per-view,
# 4 - interpromotional contest (look at tutorial template part 2),
# 5 - charity, 6 - memorial
# NOTE: 3 - tournament, however I wouldn't recommend you to use it,
# it might be an incomplete/scrapped feature considering it never appears in game.

card_type: 2

# card_size will set the card size. Recommended ones are 6 and 10, however technically
# any size between 0-10 seems to be working

card_size: 6

# attendance can be used to set an exact number of attendance

attendance: 3000

# territory sets your current territory. Open Territories.txt for the id list of them.
# note: you might need to open map twice to see it changed.

territory: 10

# Once you are done, go into booker mode and in the calendar screen press the + button on numpad
# (rebindable) and you should see the changes.
```
TutorialTemplatePart2.txt:
```
# Additional note about interpromotional contests
# card_type: 4 will set it to an interpromotional contest
# The card will also be reset regardless if "reset_card: true" is set or not

card_type: 4

# rival_fed is required and it will set the opponent promotion.
# You can use any promotion as rivals except for Legends and your own.

rival_fed: 9

```
TutorialTemplatePart3.txt:
```
# The next bits are individual matches
# "match_id: x" and "match_end" are required, others are optional
# Use the following template to force book any person in the game
# The only limitation is that it can only be Singles format

match_id: 1
match_name: This is a top card match example
left_name: Person id 20
left_id: 20
right_name: Person id 30
right_id: 30
match_end

# For guest matches make sure to set the guest as the right person

match_id: 2
match_name: Guest match (correct)
right_name: Guest person id 123
right_id: 123
match_end

# Otherwise they both will be unselectable

match_id: 3
match_name: Guest match (incorrect)
left_name: Guest person id 123
left_id: 123
match_end

# Additional note: Left and right side names can be set only if
# they have a wrestler id set

match_id: 4
match_name: Wrestler names note
left_name: This will be replaced by "???"
left_id: 0
right_name: This will appear in game
right_id: 147
match_end

# And for the match name itself at least one person must be set.

match_id: 5
match_name: Match name that will not be visible
match_end

```
World territory id list:
```
1 = "Alaska";
2 = "Canada";
3 = "Nunavut";
4 = "Greenland";
5 = "Iceland";
6 = "Quebec";
7 = "North East USA";
8 = "South East USA";
9 = "South West USA";
10 = "North West USA";
11 = "Caribbean";
12 = "Mexico";
13 = "Colombia";
14 = "Brazil";
15 = "Argentina";
16 = "United Kingdom";
17 = "West Europe";
18 = "East Europe";
19 = "Scandanavia";
20 = "Svalbard";
21 = "West Russia";
22 = "Russia";
23 = "Siberia";
24 = "Japan";
25 = "Korea";
26 = "China";
27 = "Mongolia";
28 = "Kazakhstan";
29 = "Afghanistan";
30 = "Iran";
31 = "Turkey";
32 = "Arabia";
33 = "Egypt";
34 = "Nigeria";
35 = "Congo";
36 = "Ethiopia";
37 = "South Africa";
38 = "Madagascar";
39 = "India";
40 = "Thailand";
41 = "Philippines";
42 = "Indonesia";
43 = "Papua New Guinea";
44 = "Australia";
45 = "New Zealand";
46 = "Antarctica";
```