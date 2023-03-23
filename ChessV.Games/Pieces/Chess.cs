
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
	#region Rook
	[PieceType("Rook", "Chess")]
	public class Rook: PieceType
	{
		public Rook( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Rook", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			#region Customize piece-square-tables for the Rook
			PSTMidgameInSmallCenter = 0;
			PSTMidgameInLargeCenter = 0;
			PSTMidgameSmallCenterAttacks = 2;
			PSTMidgameLargeCenterAttacks = 2;
			PSTMidgameForwardness = 0;
			PSTMidgameGlobalOffset = 0;
			PSTEndgameInSmallCenter = 0;
			PSTEndgameInLargeCenter = 0;
			PSTEndgameSmallCenterAttacks = 0;
			PSTEndgameLargeCenterAttacks = 0;
			PSTEndgameForwardness = 0;
			PSTEndgameGlobalOffset = 0;
			#endregion
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 0, 1 ) );
			type.Slide( new Direction( 0, -1 ) );
			type.Slide( new Direction( 1, 0 ) );
			type.Slide( new Direction( -1, 0 ) );
		}
	}
	#endregion

	#region Bishop
	[PieceType("Bishop", "Chess")]
	public class Bishop: PieceType
	{
		public Bishop( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Bishop", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 1, 1 ) );
			type.Slide( new Direction( 1, -1 ) );
			type.Slide( new Direction( -1, 1 ) );
			type.Slide( new Direction( -1, -1 ) );
		}
	}
	#endregion

	#region Queen
	[PieceType("Queen", "Chess")]
	public class Queen: PieceType
	{
		public Queen( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Queen", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Rook.AddMoves( type );
			Bishop.AddMoves( type );
		}
	}
	#endregion

	#region Knight
	[PieceType("Knight", "Chess")]
	public class Knight: PieceType
	{
		public Knight( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Knight", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			//	Customize piece-square-tables for the Knight
			PSTMidgameInSmallCenter = 8;
			PSTMidgameInLargeCenter = 5;
			PSTMidgameForwardness = 2;
			PSTMidgameLargeCenterAttacks = 4;
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 2 ) );
			type.Step( new Direction( 2, 1 ) );
			type.Step( new Direction( 2, -1 ) );
			type.Step( new Direction( 1, -2 ) );
			type.Step( new Direction( -1, -2 ) );
			type.Step( new Direction( -2, -1 ) );
			type.Step( new Direction( -2, 1 ) );
			type.Step( new Direction( -1, 2 ) );
		}
	}
	#endregion

	#region King
	[PieceType("King", "Chess")]
	public class King: PieceType
	{
		public King( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "King", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			#region customize piece-square-tables for the King
			//	midgame tables
			PSTMidgameInSmallCenter = 0;
			PSTMidgameInLargeCenter = 0;
			PSTMidgameSmallCenterAttacks = 0;
			PSTMidgameLargeCenterAttacks = 0;
			PSTMidgameForwardness = -15;
			//	endgame tables
			PSTEndgameForwardness = 4;
			PSTEndgameInLargeCenter = 12;
			#endregion
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Wazir.AddMoves( type );
		}
	}
	#endregion

	#region Pawn
	[PieceType("Pawn", "Chess")]
	public class Pawn: PieceType
	{
		public Pawn( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Pawn", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			IsPawn = true;
			IsSliced = false;
			AddMoves( this );

			//	Customize the piece-square-tables for the Pawn
			PSTMidgameForwardness = 7;
			PSTEndgameForwardness = 10;
			PSTMidgameInSmallCenter = 6;
		}

		public static new void AddMoves( PieceType type )
		{
			type.StepMoveOnly( new Direction( 1, 0 ) );
			type.StepCaptureOnly( new Direction( 1, 1 ) );
			type.StepCaptureOnly( new Direction( 1, -1 ) );
		}

		public override void Initialize( Game game )
		{
			base.Initialize( game );

			//	Set the pawn hash keys, used for the pawn structure hash table.
			//	Every other type has zeros here (assigned by the base class 
			//	implementation of this function.)  This override sets the 
			//	values to non-zero values for the pawn piece type only.
			for( int player = 0; player < game.NumPlayers; player++ )
				pawnHashKeyIndex[player] = 256 * (player + 1);
		}
	}
	#endregion
}
