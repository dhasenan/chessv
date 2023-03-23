
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
	[Game("Chancellor Chess", typeof(Geometry.Rectangular), 9, 9,
		  XBoardName = "chancellor",
		  InventedBy = "Ben Foster",
		  Invented = "1889",
		  Tags = "Chess Variant,Historic")]
	public class ChancellorChess: Abstract.Generic9x9
	{
		// *** PIECE TYPES *** //

		public PieceType Chancellor;


		// *** CONSTRUCTION *** //

		public ChancellorChess(): 
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rnbqkcnbr/ppppppppp/9/9/9/9/9/PPPPPPPPP/RNBQKCNBR";
			PawnDoubleMove = true;
			EnPassant = true;
			Castling.Value = "Standard";
			PromotionRule.Value = "Standard";
			PromotionTypes = "QCRNB";
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();

			AddPieceType( Chancellor = new Chancellor( "Chancellor", "C", 950, 900 ) );
		}
		#endregion

		#region AddEvaluations
		public override void AddEvaluations()
		{
			base.AddEvaluations();
			if( Chancellor != null && Chancellor.Enabled )
				RookTypeEval.AddRookOn7thBonus( Chancellor, King, 2, 8 );
		}
		#endregion
	}
}
