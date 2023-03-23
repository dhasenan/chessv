
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

using System.Collections.Generic;

namespace ChessV.Games
{
	[Game("Chess on a 12 by 12 Board", typeof(Geometry.Rectangular), 12, 12,
		  Invented = "2000",
		  InventedBy = "Doug Vogel",
		  Tags = "Chess Variant")]
	[Appearance(ColorScheme = "Baby Blues")]
	public class ChessOnA12x12Board: Abstract.Generic12x12
	{
		// *** CONSTRUCTION *** //

		public ChessOnA12x12Board():
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}

		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "12/12/2rnbqkbnr2/2pppppppp2/12/12/12/12/2PPPPPPPP2/2RNBQKBNR2/12/12";
			Castling.AddChoice( "ChessOnA12x12Board", "Kings on g3 and g10 slide two squares in either direction to castle with the pieces on the c and j files" );
			Castling.Value = "ChessOnA12x12Board";
			PromotionRule.AddChoice( "ChessOnA12x12Board", "Standard promotion except that promote on the 10th rank" );
			PromotionRule.Value = "Custom";
			PromotionTypes = "QRBN";
			PawnMultipleMove.Value = "@4(2)";
			EnPassant = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();

			//	add custom pawn promotion rule
			if( PromotionRule.Value == "ChessOnA12x12Board" )
			{
				List<PieceType> availablePromotionTypes = ParseTypeListFromString( PromotionTypes );
				AddBasicPromotionRule( Pawn, availablePromotionTypes, loc => loc.Rank == 9 );
			}

			//	add custom castling rule
			if( Castling.Value == "ChessOnA12x12Board" )
			{
				AddCastlingRule();
				CastlingMove( 0, "g3", "i3", "j3", "h3", 'K' );
				CastlingMove( 0, "g3", "e3", "c3", "f3", 'Q' );
				CastlingMove( 1, "g10", "i10", "j10", "h10", 'k' );
				CastlingMove( 1, "g10", "e10", "c10", "f10", 'q' );
			}
		}
		#endregion
	}
}
