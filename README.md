## ChecksMate Client

This project implements a co-op Roguelite meta-progression layer for the best semi-3d on-rails platformer since Crash Bandicoot 2.

Your opponent begins with a set of 16 chessmen. You begin with a King and the ability to Try Again. Your objective is to checkmate the King.

In addition to the 6 ordinary chessmen, you may find fairy chess pieces. These include the Berolina Pawn, various pieces from Ralph Betza's Chess With Different Armies, as well as Xiang Qi's Cannon (and an invented Vao piece, which is to the Bishop as the Cannon is to the Rook).

As you complete the following objectives, you will gain access to additional material.

 - Capture each individual enemy piece and pawn (e.g. capture pawn E, the pawn that begins on the E file)
 - Capture multiple enemy pieces and pawns in 1 match (e.g. capture 2 pawns), including sequences of pairs (e.g. both 2 pieces and 2 pawns)
 - Attack any opposing pawn, minor piece, major piece, or queen
 - Attack multiple opposing pieces with a single piece (even if it is itself attacked): two pieces, three pieces, and the King and Queen
 - Move your King each of: forward one space; to the A file; to the center 4 squares; to the opposing home rank; and to capture a piece
 - Perform the French move

TODO locations:

 - "True Fork" where the forking piece is not attacked.
 - "Discovered Attack" where a piece which was not under attack becomes under attack but not by the piece you moved
 - "Pin" and "Skewer" where a piece would be under attack if not for another piece on the same side. If the higher value piece is attacked, it's a skewer, otheerwise it's a pin
 - Short/Long "Castle" where you castle. Unless excluded (and thus only providing filler or trap items), this move will require a firmer guarantee that a user can castle (that its major pieces have not turned into queens).
 
TODO randomizer flags:

 - Piece Limits. Under some mindsets, it can be taxing to find 6 minor pieces and no Queen. By adding certain rails to the experience, one can have a more personalized approach to a Chess randomizer, where one's army bears some semblance of a traditional game.
 - Progressive Goal. Your enemy's pieces are also scattered across the multiworld! (The current design can make progression too easy.)
 - Non-Progressive Material. Pieces will not be selected progressively from a set, but instead placed with specific names in your world. This means you would find a Bishop or Cleric rather than a Progressive Minor Piece or Progressive Major Piece. (They are unlikely to come with pre-determined locations.)
 - Non-Fairy Chess. Your major pieces will always be Rooks, your minor pieces will always be Bishops and Knights, and your queens will always slay. Also, no more dumb Berolina Pawns. Who even thought mixing those was a good idea?

## ArchipelagoMW

Archipelago provides a generic framework for developing multiworld capability for game randomizers. In all cases, presently, Archipelago is also the randomizer itself.

https://github.com/ArchipelagoMW/Archipelago

https://archipelago.gg/

## ChessV

ChessV is a free, open-source universal chess program with a graphical user interface, sophisticated AI engine, and other features of traditional Chess programs. As a "universal" chess program, it not only plays orthodox Chess, it is also capable of playing games reasonbly similar to Chess. It currently plays over 100 different chess variants, and can be programmed to play additional variants.

Features
 - Plays over 100 different Chess variants, including some that are quite exotic.
 - Has a fully-featured graphical user interface, but the engine can also be used separately under another GUI (such as WinBoard) and other compliant engines can be used with ChessV's GUI.
 - Has a scripting language to allow configuration of new variants. It supports combining existing pieces and rules, and even defining new pieces, but creation of new rules is not supported.
 - Plays with a fairly high level of skill. The engine can also be configured to weaken its skill level.
 - ChessV is a .NET application so it can be run under Linux or MacOS using Mono.

 http://www.chessv.org/
 
