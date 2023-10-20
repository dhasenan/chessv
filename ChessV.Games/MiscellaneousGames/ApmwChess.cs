
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

using APMW.Core;
using ChessV.Games.Pieces.Berolina;
using ChessV.Games.Rules.Alice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata;

namespace ChessV.Games
{
	//**********************************************************************
	//
	//                           AliceChess
	//
	//    This class implements Alice Chess.  This game is Chess with an 
	//    extra board that starts off empty.  With each move, a piece is 
	//    transferred to the other board.  A move must be legal on the 
	//    board on which it is made and the corresponding destination 
	//    square on the other board must be empty.  
	//
	//    This class, along with the AliceRule class (which handles the 
	//    movement between boards with minimal fuss), and the TwoBoards
	//    class (which handles the initialization and display of a pair 
	//    of boards) demonstrate a number of techniques for extending 
	//    ChessV for novel games.

	[Game("Archipelago Multiworld", typeof(Geometry.Rectangular), 8, 8, 2,
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

    public ApmwChessGame()
		{
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
      AddPieceType(BerolinaPawn = new BerolinaPawn("Berolina Pawn", "Ƥ / ƥ", 100, 125, preferredImageName: "Ferz"));
      // Cwda
      AddPieceType(Queen = new Queen("Queen", "Q", 950, 1000));
      AddPieceType(Rook = new Rook("Rook", "R", 500, 550));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 325, 350));
      AddPieceType(Knight = new Knight("Knight", "N", 325, 325));
      AddPieceType(Archbishop = new Archbishop("Archbishop", "A", 875, 875));
      AddPieceType(WarElephant = new WarElephant("War Elephant", "E", 475, 475));
      AddPieceType(Phoenix = new Phoenix("Phoenix", "X", 315, 315));
      AddPieceType(Cleric = new Cleric("Cleric", "C", 450, 500));
      AddPieceType(Chancellor = new Chancellor("Chancellor", "C", 950, 950));
      AddPieceType(ShortRook = new ShortRook("Short Rook", "S", 400, 425));
      AddPieceType(Tower = new Tower("Tower", "T", 325, 325));
      AddPieceType(Lion = new Lion("Lion", "L", 500, 500));
      AddPieceType(ChargingRook = new ChargingRook("Charging Rook", "R", 495, 530));
      AddPieceType(NarrowKnight = new NarrowKnight("Lancer", "L", 325, 325));
      AddPieceType(ChargingKnight = new ChargingKnight("Charging Knight", "N", 365, 365));
      AddPieceType(Colonel = new Colonel("Colonel", "C", 950, 950));
      // Eurasian
      AddPieceType(Cannon = new Cannon("Cannon", "O", 400, 275));
      AddPieceType(Vao = new Vao("Vao", "V", 300, 175));

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

      Starter starter = Starter.getInstance();
      Dictionary<KeyValuePair<int, int>, PieceType> pieces = (Dictionary<KeyValuePair<int, int>, PieceType>) starter.PlayerPieceSetProvider.FirstOrDefault().DynamicInvoke();

      String humanPrefix = "Black";
      String cpuPrefix = "White";
      int humanPlayer = starter.GeriProvider.First().Invoke();
      if (humanPlayer == 0)
      {
        (humanPrefix, cpuPrefix) = (cpuPrefix, humanPrefix);

        // TODO: CPU gets 1 piece per checkmate, Goal is to checkmate a "full" CPU army
        // TODO: CPU different armies
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

      //	determine player's second-row pawns
      Dictionary<int, string> notations = new Dictionary<int, string>();

      for (int rank = 0; rank < this.NumRanks; rank++)
      {
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

      SetCustomProperty(humanPrefix + "Outer", notations[0]);
      SetCustomProperty(humanPrefix + "Pawns", notations[1]);
      SetCustomProperty(humanPrefix + "Pieces", notations[2]);
    }
    #endregion
  }
}
