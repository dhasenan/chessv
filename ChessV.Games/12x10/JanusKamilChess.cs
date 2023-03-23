
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
	[Game("Janus Kamil Chess", typeof( Geometry.Rectangular ), 12, 10,
		  InventedBy = "Jörg Knappen",
		  Invented = "2004",
		  Tags = "Chess Variant")]
	public class JanusKamilChess: Abstract.Generic12x10
	{
		// *** PIECE TYPES *** //

		public PieceType Janus;
		public PieceType Camel;


		// *** CONSTRUCTION *** //

		public JanusKamilChess() :
			base
				 ( /* symmetry = */ new MirrorSymmetry() )
		{
		}

		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "crjnbqkbnjrc/pppppppppppp/12/12/12/12/12/12/PPPPPPPPPPPP/CRJNBQKBNJRC";
			Castling.Value = "Close-Rook 3-4";
			PawnMultipleMove.Value = "Triple";
			EnPassant = true;
			PromotionTypes = "CRJNBQ";
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			AddPieceType( Janus = new Archbishop( "Janus", "J", 900, 900 ) );
			AddPieceType( Camel = new Camel( "Camel", "C", 250, 250 ) );
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();
		}
		#endregion
	}
}
