
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
	[Game("Cagliostro's Chess", typeof(Geometry.Rectangular), 12, 8,
		  Invented = "1970s",
		  InventedBy = "Savio Cagliostro",
		  Tags = "Chess Variant")]
	public class CagliostrosChess: Abstract.Generic12x8
	{
		// *** PIECE TYPES *** //

		public PieceType Archbishop;
		public PieceType Chancellor;
		public PieceType Amazon;


		// *** CONSTRUCTION *** //

		public CagliostrosChess():
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}

		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rnbacqkgabnr/pppppppppppp/12/12/12/12/PPPPPPPPPPPP/RNBACQKGABNR";
			PromotionRule.Value = "Standard";
			PromotionTypes = "QCAGRNB";
			Castling.Value = "4-4";
			PawnDoubleMove = true;
			EnPassant = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			AddPieceType( Archbishop = new Archbishop( "Archbishop", "A", 900, 900 ) );
			AddPieceType( Chancellor = new Chancellor( "Chancellor", "C", 950, 975 ) );
			AddPieceType( Amazon = new Amazon( "General", "G", 1250, 1250 ) );
		}
		#endregion
	}
}
