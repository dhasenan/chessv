
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
	#region Gold General
	[PieceType("Gold General", "Shogi")]
	public class GoldGeneral: PieceType
	{
		public GoldGeneral( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Gold General", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 0 ) );
			type.Step( new Direction( 1, 1 ) );
			type.Step( new Direction( 1, -1 ) );
			type.Step( new Direction( 0, 1 ) );
			type.Step( new Direction( 0, -1 ) );
			type.Step( new Direction( -1, 0 ) );
		}
	}
	#endregion

	#region Silver General
	[PieceType("Silver General", "Shogi")]
	public class SilverGeneral: PieceType
	{
		public SilverGeneral( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Silver General", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 0 ) );
			type.Step( new Direction( 1, 1 ) );
			type.Step( new Direction( 1, -1 ) );
			type.Step( new Direction( -1, 1 ) );
			type.Step( new Direction( -1, -1 ) );
		}
	}
	#endregion

	#region Copper General
	[PieceType("Copper General", "Shogi")]
	public class CopperGeneral: PieceType
	{
		public CopperGeneral( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Copper General", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 0 ) );
			type.Step( new Direction( 1, 1 ) );
			type.Step( new Direction( 1, -1 ) );
			type.Step( new Direction( -1, 0 ) );
		}
	}
	#endregion

	#region Vertical Mover
	[PieceType("Vertical Mover", "Shogi")]
	public class VerticalMover: PieceType
	{
		public VerticalMover( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Vertical Mover", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 1, 0 ) );
			type.Slide( new Direction( -1, 0 ) );
			type.Step( new Direction( 0, 1 ) );
			type.Step( new Direction( 0, -1 ) );
		}
	}
	#endregion

	#region Side Mover
	[PieceType("Side Mover", "Shogi")]
	public class SideMover: PieceType
	{
		public SideMover( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Side Mover", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 0, 1 ) );
			type.Slide( new Direction( 0, -1 ) );
			type.Step( new Direction( 1, 0 ) );
			type.Step( new Direction( -1, 0 ) );
		}
	}
	#endregion
}
