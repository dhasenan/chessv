
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG

This file is part of ChessV.  ChessV is free software; you can redistribute
it and/or modify it under the terms of the GNU General Public License as 
published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

ChessV is distributed in the hope that it will be useful, but WITHOUT ANY 
WARRANTY; without even the implied warranty of MERCHANTABILITY or 
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for 
more details; the file 'COPYING' contains the License text, but if for
some reason you need a copy, please visit <http://www.gnu.org/licenses/>.

****************************************************************************/

using ChessV.Base;
using ChessV.Games.Pieces.Berolina;
using ChessV.Games.Rules.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace ChessV.Games
{
	//**********************************************************************
	//
	//                        Archipelago ChecksMate
	//
	//    Play against an AI with your friends! Get new pieces! Be confused!
  //

	[Game("Archipelago Multiworld", typeof(Geometry.Rectangular), 8, 8, 3,
		  Invented = "2019",
		  InventedBy = "Berserker", 
		  Tags = "Chess Variant,Multiple Boards,Popular,Different Armies")]
	[Appearance(ColorScheme = "Sublimation")]
	public class ApmwChessGame: Chess
  {
    // *** PIECE TYPES *** //

    //  Berolina
    public PieceType BerolinaPawn;

    //	Colorbound Clobberers
    public PieceType Archbishop;
    public PieceType WarElephant;
    public PieceType Phoenix;
    public PieceType Cleric;

    //	Remarkable Rookies
    public PieceType ShortRook;
    public PieceType Tower;
    public PieceType Lion;
    public PieceType Chancellor;

    //	Nutty Knights
    public PieceType ChargingRook;
    public PieceType NarrowKnight;
    public PieceType ChargingKnight;
    public PieceType Colonel;

    //  Eurasian
    public PieceType Cannon;
    public PieceType Vao;

    public HashSet<PieceType> pawns;
    public HashSet<PieceType> minors;
    public HashSet<PieceType> majors;
    public HashSet<PieceType> queens;
    public List<HashSet<PieceType>> pocketSets;

    public ApmwChessGame()
    {
      pawns = new HashSet<PieceType>();
      minors = new HashSet<PieceType>();
      majors = new HashSet<PieceType>();
      queens = new HashSet<PieceType>();
      pocketSets = new List<HashSet<PieceType>>() { pawns, minors, majors, queens };
    }


		// *** INITIALIZATION *** //

		#region CreateBoard
		//	We override the CreateBoard function so the game uses a board of 
		//	type BoardWithCards instead of Board.  This is enough to trigger the 
		//	board with cards architecture and proper rendering to the display.
		public override Board CreateBoard( int nPlayers, int nFiles, int nRanks, Symmetry symmetry )
		{ return new Boards.BoardWithCards( nFiles, nRanks, 3 ); }
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();
      AddRule(new CardDropRule());
      RemoveRule(typeof(Rules.CheckmateRule));
      AddRule(new Rules.Extinction.ExtinctionRule("K"));
      //AddRule(new ApmwMoveCompletionRule());
      // TODO(chesslogic): conditionally remove en passant for one player only. (the Apmw provider probably knows whether the player can en passant at this point)

    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
		{
			//	most, if not all, evaluations probably won't do the "right" thing 
			//	in this game, so we'll override this function and do nothing
		}
    #endregion

    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      FENFormat = "{array} {current player} {pieces in hand} {castling} {en-passant} {half-move clock} {turn number}";
      FENStart = "#{Array} w #{PocketPieces} KQkq - 0 1";
      Array = "#{BlackPieces}/#{BlackPawns}/#{BlackOuter}/8/8/#{WhiteOuter}/#{WhitePawns}/#{WhitePieces}";
      Castling.RemoveChoice("Flexible");
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.AddChoice("CwDA", "Standard castling with the extra exception to prevent color-bound pieces from changing square colors");
      Castling.Value = "CwDA";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();

      // We can load all the piece types, I don't think the engine cares if some pieces are never used

      // Berolina
      AddPieceType(BerolinaPawn = new BerolinaPawn("Berolina Pawn", "Ƥ/ƥ", 100, 125, preferredImageName: "Ferz"));
      // Cwda
      //AddPieceType(Queen = new Queen("Queen", "Q", 950, 1000));
      //AddPieceType(Rook = new Rook("Rook", "R", 500, 550));
      //AddPieceType(Bishop = new Bishop("Bishop", "B", 325, 350));
      //AddPieceType(Knight = new Knight("Knight", "N", 325, 325));
      AddPieceType(Archbishop = new Archbishop("Archbishop", "A", 875, 875));
      AddPieceType(WarElephant = new WarElephant("War Elephant", "E", 475, 475));
      AddPieceType(Phoenix = new Phoenix("Phoenix", "X", 315, 315));
      AddPieceType(Cleric = new Cleric("Cleric", "Ċ/ċ", 450, 500));
      AddPieceType(Chancellor = new Chancellor("Chancellor", "C", 950, 950));
      AddPieceType(ShortRook = new ShortRook("Short Rook", "S", 400, 425));
      AddPieceType(Tower = new Tower("Tower", "T", 325, 325));
      AddPieceType(Lion = new Lion("Lion", "I", 500, 500));
      AddPieceType(ChargingRook = new ChargingRook("Charging Rook", "Ṙ/ṙ", 495, 530));
      AddPieceType(NarrowKnight = new NarrowKnight("Lancer", "L", 325, 325));
      AddPieceType(ChargingKnight = new ChargingKnight("Charging Knight", "Ṅ/ṅ", 365, 365));
      AddPieceType(Colonel = new Colonel("Colonel", "K̇/k̇", 950, 950));
      // Eurasian
      AddPieceType(Cannon = new Cannon("Cannon", "O", 400, 275));
      AddPieceType(Vao = new Vao("Vao", "V", 300, 175));

      pawns.Add(Pawn);
      pawns.Add(BerolinaPawn);

      minors.Add(Bishop);
      minors.Add(Knight);
      minors.Add(Phoenix);
      minors.Add(ShortRook); // unusually powerful
      minors.Add(Tower);
      minors.Add(NarrowKnight);
      minors.Add(ChargingKnight);
      minors.Add(Vao);
      minors.Add(Cannon); // unusually powerful

      majors.Add(Rook);
      majors.Add(WarElephant);
      majors.Add(Cleric);
      majors.Add(Lion);
      majors.Add(ChargingRook);

      queens.Add(Queen);
      queens.Add(Archbishop);
      queens.Add(Chancellor);
      queens.Add(Colonel);

      //	Army adjustment
      //if ((WhiteArmy.Value == "Fabulous FIDEs" && BlackArmy.Value == "Remarkable Rookies") ||
      //  (BlackArmy.Value == "Fabulous FIDEs" && WhiteArmy.Value == "Remarkable Rookies"))
      //{
      //  //	increase the value of the bishops since the Rookies have no piece that moves that way
      //  Bishop.MidgameValue += 35;
      //  Bishop.EndgameValue += 35;
      //}
    }
    #endregion

    #region SetOtherVariables
    public override void SetOtherVariables()
    {
      base.SetOtherVariables();

      ApmwCore starter = ApmwCore.getInstance();
      Dictionary<KeyValuePair<int, int>, PieceType> pieces =
        starter.PlayerPieceSetProvider.FirstOrDefault().Invoke();

      string humanPrefix = "Black";
      string cpuPrefix = "White";
      int humanPlayer = starter.GeriProvider.First().Invoke();
      if (humanPlayer == 0)
      {
        (humanPrefix, cpuPrefix) = (cpuPrefix, humanPrefix);

        // TODO(chesslogic): CPU gets 1 piece per checkmate (as location?), Goal is to checkmate a "full" CPU army
        // TODO(chesslogic): CPU different armies
        SetCustomProperty("BlackOuter", "8");
        SetCustomProperty("BlackPawns", "pppppppp");
        SetCustomProperty("BlackPieces", "rnbqkbnr");
      }
      else
      {
        SetCustomProperty("WhiteOuter", "8");
        SetCustomProperty("WhitePawns", "PPPPPPPP");
        SetCustomProperty("WhitePieces", "RNBQKBNR");
      }

      //	determine player's board
      // TODO(chesslogic): incorporate ApmwCore
      Dictionary<int, string> notations = new Dictionary<int, string>();

      for (int rank = 0; rank < this.NumRanks; rank++)
      {
        notations[rank] = "";
        int emptySpaceCount = 0;
        for (int file = 0; file < this.NumFiles; file++)
        {
          var place = new KeyValuePair<int, int>(rank, file);
          if (pieces.ContainsKey(place))
          {
            if (emptySpaceCount > 0)
            {
              notations[rank] += Convert.ToChar('0' + emptySpaceCount);
              emptySpaceCount = 0;
            }

            notations[rank] += pieces[place].Notation[humanPlayer];
          }
          else
          {
            emptySpaceCount++;
          }
        }
        if (emptySpaceCount > 0)
        {
          notations[rank] += Convert.ToChar('0' + emptySpaceCount);
          emptySpaceCount = 0;
        }
      }

      // determine pockets
      int foundPockets = ApmwCore.getInstance().foundPockets;
      var pockets = ApmwCore.getInstance().generatePocketValues(foundPockets);
      List<PieceType> pocketPieces = new List<PieceType>();
      List<string> pocketItems = new List<string>();
      int empty = 0;
      for (int i = 0; i < 3; i++)
      {
        if (pockets[i] == 0)
        {
          empty++;
          pocketPieces.Add(null);
          continue;
        }
        var setOfPieceType = pocketSets[pockets[i]-1];
        Random random = new Random(ApmwCore.getInstance().pocketChoiceSeed[i]);
        int index = random.Next(setOfPieceType.Count);
        pocketPieces.Add(setOfPieceType.ToList()[index]);
      }
      SetCustomProperty("PocketPieces",
        (humanPlayer == 1 ? "3" : "") +
        pocketPieces.Select((PieceType piece) => piece == null ? "1" : piece.Notation[humanPlayer])
          .Aggregate("", (string piece, string notation) => notation + piece)
          + (humanPlayer == 0 ? "3" : ""));

      SetCustomProperty(humanPrefix + "Outer", notations[0]);
      SetCustomProperty(humanPrefix + "Pawns", notations[1]);
      SetCustomProperty(humanPrefix + "Pieces", notations[2]);
    }
    #endregion
  }
}
