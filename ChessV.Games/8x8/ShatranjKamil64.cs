
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
	[Game("Shatranj Kamil (64)", typeof(Geometry.Rectangular), 8, 8,
		  Invented = "2005",
		  InventedBy = "David Paulowich",
		  Tags = "Chess Variant")]
	[Appearance(ColorScheme = "Lemon Cappuccino")]
	public class ShatranjKamil64: Shatranj
	{
		// *** PIECE TYPES *** //

		public PieceType SilverGeneral;


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			NumberOfSquareColors = 2;
			Array = "rnseksnr/pp1ge1pp/2pppp2/8/8/2PPPP2/PP1GE1PP/RNSEKSNR";
			PromotionTypes = "S";
			StalemateResult.Value = "Loss";
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			//	Add in all types from Shatranj
			base.AddPieceTypes();
			//	Upgrade the Elephant with non-capturing Dababbah move
			Elephant.StepMoveOnly( new Direction(  2,  0 ) );
			Elephant.StepMoveOnly( new Direction( -2, 0 ) );
			Elephant.StepMoveOnly( new Direction( 0, 2 ) );
			Elephant.StepMoveOnly( new Direction( 0, -2 ) );
			Elephant.MidgameValue = 165;
			Elephant.EndgameValue = 165;
			//	Add the Silver General
			AddPieceType( SilverGeneral = new SilverGeneral( "Silver General", "S", 285, 285 ) );
			//	Change value of Rook
			Rook.MidgameValue = 500;
			Rook.EndgameValue = 550;
		}
		#endregion
	}
}
