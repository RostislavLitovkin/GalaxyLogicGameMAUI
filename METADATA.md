# METADATA SAVING

This document explains, how the data of the game is saved.

It is useful not only for saving the game, but also for checking the validity of all moves and will be used in the smartcontract for the competitions.

### How it works

- First 6 numbers is the seed
- 1 number for the gamemode

Then follows these variations in any order:

- 1 number (0 - 14 for the index of placing it)
- Character for the powerup, followed by a number (if it needs it)

Every value is separated by a space