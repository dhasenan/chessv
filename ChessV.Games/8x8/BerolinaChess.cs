
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

namespace ChessV.Games
{
	//**********************************************************************
	//
	//                          BerolinaChess
	//
	//    This class implements the game of Berolina Chess.  It is like 
	//    orthodox Chess except that the pawns move diagonally and capture 
	//    forwards (the opposite of Chess.)  Since pawns are so fundamental 
	//    to Chess, however, this requires some care.

	[Game("Berolina Chess", typeof(Geometry.Rectangular), 8, 8,
		  XBoardName = "berolina",
		  InventedBy = "Edmund Hebermann",
		  Invented = "1926",
		  Tags = "Chess Variant,Historic,Popular",
		  GameDescription1 = "Standard chess but with the pawn moves switched",
		  GameDescription2 = "Pawns capture directly forward and move without capturing diagonally forward")]
	public class BerolinaChess: Abstract.Generic8x8
	{
		// *** PIECE TYPES *** //

		public PieceType BerolinaPawn;


		// *** CONSTRUCTION *** //

		public BerolinaChess(): 
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
			PawnDoubleMove = true;
			EnPassant = true;
			Castling.Value = "Standard";
			PromotionRule.Value = "Standard";
			PromotionTypes = "QRNB";
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			replacePieceType( Pawn, BerolinaPawn = new Pieces.Berolina.BerolinaPawn( "Pawn", "P", 100, 125 ) );
			PromotingType = BerolinaPawn;
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();

			// *** PAWN DOUBLE MOVE *** //
			if( PawnDoubleMove && BerolinaPawn.Enabled )
			{
				MoveCapability doubleMoveNE = new MoveCapability();
				doubleMoveNE.MinSteps = 2;
				doubleMoveNE.MaxSteps = 2;
				doubleMoveNE.MustCapture = false;
				doubleMoveNE.CanCapture = false;
				doubleMoveNE.Direction = new Direction( 1, 1 );
				doubleMoveNE.Condition = location => location.Rank == 1;
				BerolinaPawn.AddMoveCapability( doubleMoveNE );

				MoveCapability doubleMoveNW = new MoveCapability();
				doubleMoveNW.MinSteps = 2;
				doubleMoveNW.MaxSteps = 2;
				doubleMoveNW.MustCapture = false;
				doubleMoveNW.CanCapture = false;
				doubleMoveNW.Direction = new Direction( 1, -1 );
				doubleMoveNW.Condition = location => location.Rank == 1;
				BerolinaPawn.AddMoveCapability( doubleMoveNW );
			}

			// *** EN-PASSANT *** //
			if( EnPassant && BerolinaPawn.Enabled )
				AddRule( new Rules.Berolina.BerolinaEnPassantRule( BerolinaPawn ) );
		}
		#endregion
	}
}
