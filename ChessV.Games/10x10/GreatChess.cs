
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
	[Game("Great Chess", typeof(Geometry.Rectangular), 10, 10,
		  Invented = "Unkown",
		  InventedBy = "1700s",
		  Tags = "Chess Variant,Historic")]
	public class GreatChess: Abstract.Generic10x10
	{
		// *** PIECE TYPES *** //

		public PieceType General;
		public PieceType Archbishop;
		public PieceType Chancellor;


		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable Variant { get; set; }


		// *** CONSTRUCTION *** //

		public GreatChess():
			base
				 ( /* symmetry = */ new MirrorSymmetry() )
		{
		}

		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rnbqkgabnr/ppppccpppp/4pp4/10/10/10/10/4PP4/PPPPCCPPPP/RNBQKGABNR";
			Variant = new ChoiceVariable( new string[] { "Classic", "Faster Pawns", "Faster Pawns and Castling" }, "Classic" );
			PromotionRule.Value = "Standard";
			PromotionTypes = "Q";
		}
		#endregion

		#region SetOtherVariables
		public override void SetOtherVariables()
		{
			base.SetOtherVariables();
			if( Variant.Value == "Classic" )
			{
				PawnMultipleMove.Value = "None";
				EnPassant = false;
				Castling.Value = "None";
			}
			else if( Variant.Value == "Faster Pawns" )
			{
				PawnMultipleMove.Value = "Great";
				EnPassant = true;
				Castling.Value = "None";
			}
			else if( Variant.Value == "Faster Pawns and Castling" )
			{
				PawnMultipleMove.Value = "Great";
				EnPassant = true;
				Castling.Value = "Standard";
			}
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			AddPieceType( Archbishop = new Archbishop( "Archbishop", "A", 750, 800 ) );
			AddPieceType( Chancellor = new Chancellor( "Chancellor", "C", 925, 1000 ) );
			AddPieceType( General = new Amazon( "General", "G", 1400, 1500 ) );
		}
		#endregion
	}
}
