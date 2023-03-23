
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
	[Game("King's Court", typeof(Geometry.Rectangular), 12, 8,
		  Invented = "1997",
		  InventedBy = "Sidney LeVasseur",
		  Tags = "Chess Variant")]
	[Appearance(ColorScheme = "Golden Goose Egg")]
	public class KingsCourt: Abstract.Generic12x8
	{
		// *** PIECE TYPES *** //

		PieceType Chancellor;
		PieceType Jester;


		// *** CONSTRUCTION *** //

		public KingsCourt():
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rjcnbqkbncjr/pppppppppppp/12/12/12/12/PPPPPPPPPPPP/RJCNBQKBNCJR";
			Castling.Value = "Flexible";
			PawnDoubleMove = true;
			EnPassant = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			AddPieceType( Jester = new FreePadwar( "Jester", "J", 400, 400, "ElephantFerz2" ) );
			AddPieceType( Chancellor = new Amazon( "Chancellor", "C", 700, 700, "Duke" ) );
			//	limit the range of the slides to two spaces
			MoveCapability[] moves;
			int nMoves = Chancellor.GetMoveCapabilities( out moves );
			for( int x = 0; x < nMoves; x++ )
				if( moves[x].MaxSteps > 1 )
					moves[x].MaxSteps = 2;
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();
			AddRule( new Games.Rules.KingsCourt.KingsFlightRule( King, Chancellor ) );
		}
		#endregion
	}
}
