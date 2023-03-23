
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2020 BY GREG STRONG

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
	[Game("Sac Chess", typeof( Geometry.Rectangular ), 10, 10,
		  InventedBy = "Kevin Pacey",
		  Invented = "2015",
		  Tags = "Chess Variant")]
	public class SacChess: Abstract.Generic10x10
	{
		// *** PIECE TYPES *** //

		public PieceType Amazon;
		public PieceType Chancellor;
		public PieceType Archbishop;
		public PieceType Sailor;
		public PieceType Missionary;
		public PieceType Judge;


		// *** CONSTRUCTION *** //

		public SacChess(): 
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "caszmmzsac/jrnbqkbnrj/pppppppppp/10/10/10/10/PPPPPPPPPP/JRNBQKBNRJ/CASZMMZSAC";
			PawnMultipleMove.Value = "Grand";
			EnPassant = true;
			Castling.Value = "2R Close-Rook";
			PromotionRule.Value = "Standard";
			PromotionTypes = "ZQCASMJRNB";
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			AddPieceType( Amazon = new Amazon( "Amazon", "Z", 1400, 1500 ) );
			AddPieceType( Chancellor = new Chancellor( "Chancellor", "C", 925, 1000 ) );
			AddPieceType( Archbishop = new Archbishop( "Archbishop", "A", 750, 800 ) );
			AddPieceType( Sailor = new DragonKing( "Sailor", "S", 700, 700 ) );
			AddPieceType( Missionary = new DragonHorse( "Missionary", "M", 500, 500 ) );
			AddPieceType( Judge = new Centaur( "Judge", "J", 600, 600 ) );
		}
		#endregion
	}
}
