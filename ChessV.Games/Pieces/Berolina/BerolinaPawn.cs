
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

namespace ChessV.Games.Pieces.Berolina
{
	[PieceType("Berolina Pawn", "Miscellaneous")]
	public class BerolinaPawn: PieceType
	{
		public BerolinaPawn( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Berolina Pawn", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			IsSliced = false;
			AddMoves( this );

			//	Customize the piece-square-tables for the Pawn
			PSTMidgameForwardness = 7;
			PSTEndgameForwardness = 10;
			PSTMidgameInSmallCenter = 8;
		}

		public static new void AddMoves( PieceType type )
		{
			type.StepMoveOnly( new Direction( 1, 1 ) );
			type.StepMoveOnly( new Direction( 1, -1 ) );
			type.StepCaptureOnly( new Direction( 1, 0 ) );
		}
	}
}
