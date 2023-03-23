
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
	#region Lion
	[PieceType("Lion", "Chess with Different Armies")]
	public class Lion: PieceType
	{
		public Lion( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Lion", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Dabbabah.AddMoves( type );
			Tribbabah.AddMoves( type );
		}
	}
	#endregion

	#region War Elephant
	[PieceType("War Elephant", "Chess with Different Armies")]
	public class WarElephant: PieceType
	{
		public WarElephant( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "War Elephant", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			FallbackImage = "Elephant Ferz Dabbabah";
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Elephant.AddMoves( type );
			Dabbabah.AddMoves( type );
		}
	}
	#endregion

	#region Phoenix
	[PieceType("Phoenix", "Chess with Different Armies")]
	public class Phoenix: PieceType
	{
		public Phoenix( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Phoenix", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			FallbackImage = "Elephant Wazir";
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Wazir.AddMoves( type );
			Elephant.AddMoves( type );
		}
	}
	#endregion

	#region Cleric
	[PieceType("Cleric", "Chess with Different Armies")]
	public class Cleric: PieceType
	{
		public Cleric( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Cleric", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			FallbackImage = "Bishop Debbabah";
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Bishop.AddMoves( type );
			Dabbabah.AddMoves( type );
		}
	}
	#endregion

	#region Short Rook
	[PieceType("Short Rook", "Chess with Different Armies")]
	public class ShortRook: PieceType
	{
		public ShortRook( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Short Rook", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			#region Customize piece-square-tables for the Charging Rook
			PSTMidgameInSmallCenter = 0;
			PSTMidgameInLargeCenter = 0;
			PSTMidgameSmallCenterAttacks = 2;
			PSTMidgameLargeCenterAttacks = 2;
			PSTMidgameForwardness = -1;
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
			type.Slide( new Direction( 0, 1 ), 4 );
			type.Slide( new Direction( 0, -1 ), 4 );
			type.Slide( new Direction( 1, 0 ), 4 );
			type.Slide( new Direction( -1, 0 ), 4 );
		}
	}
	#endregion

	#region Tower
	[PieceType("Tower", "Chess with Different Armies")]
	public class Tower: PieceType
	{
		public Tower( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Tower", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			FallbackImage = "Wazir Dabbabah";
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Wazir.AddMoves( type );
			Dabbabah.AddMoves( type );
		}
	}
	#endregion

	#region Narrow Knight
	[PieceType("Narrow Knight", "Chess with Different Armies")]
	public class NarrowKnight: PieceType
	{
		public NarrowKnight( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Narrow Knight", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			//	Customize piece-square-tables for the Narrow Knight
			PSTMidgameInSmallCenter = 12;
			PSTMidgameInLargeCenter = 8;
			PSTMidgameForwardness = 2;
			PSTMidgameLargeCenterAttacks = 4;
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			type.Step( new Direction( 2, 1 ) );
			type.Step( new Direction( 2, -1 ) );
			type.Step( new Direction( -2, -1 ) );
			type.Step( new Direction( -2, 1 ) );
		}
	}
	#endregion

	#region Charging Rook
	[PieceType("Charging Rook", "Chess with Different Armies")]
	public class ChargingRook: PieceType
	{
		public ChargingRook( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Charging Rook", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			PSTMidgameForwardness = -8;
			PSTEndgameForwardness = -8;
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 0, 1 ) );
			type.Slide( new Direction( 0, -1 ) );
			type.Slide( new Direction( 1, 0 ) );
			type.Step( new Direction( -1, 0 ) );
			type.Step( new Direction( -1, 1 ) );
			type.Step( new Direction( -1, -1 ) );
		}
	}
	#endregion

	#region Charging Knight
	[PieceType("Charging Knight", "Chess with Different Armies")]
	public class ChargingKnight: PieceType
	{
		public ChargingKnight( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "ChargingKnight", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			//	Customize piece-square-tables for the Charging Knight
			PSTMidgameInSmallCenter = 12;
			PSTMidgameInLargeCenter = 8;
			PSTMidgameForwardness = 1;
			PSTMidgameLargeCenterAttacks = 4;
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 2 ) );
			type.Step( new Direction( 2, 1 ) );
			type.Step( new Direction( 2, -1 ) );
			type.Step( new Direction( 1, -2 ) );
			type.Step( new Direction( -1, 1 ) );
			type.Step( new Direction( -1, 0 ) );
			type.Step( new Direction( -1, -1 ) );
			type.Step( new Direction( 0, 1 ) );
			type.Step( new Direction( 0, -1 ) );
		}
	}
	#endregion

	#region Colonel
	[PieceType("Colonel", "Chess with Different Armies")]
	public class Colonel: PieceType
	{
		public Colonel( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Colonel", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );

			PSTMidgameForwardness = -6;
			PSTEndgameForwardness = -6;
		}

		public static new void AddMoves( PieceType type )
		{
			type.Step( new Direction( 1, 2 ) );
			type.Step( new Direction( 2, 1 ) );
			type.Step( new Direction( 2, -1 ) );
			type.Step( new Direction( 1, -2 ) );
			type.Step( new Direction( 1, 1 ) );
			type.Step( new Direction( -1, 1 ) );
			type.Step( new Direction( 1, -1 ) );
			type.Step( new Direction( -1, 0 ) );
			type.Step( new Direction( -1, -1 ) );
			type.Slide( new Direction( 1, 0 ) );
			type.Slide( new Direction( 0, 1 ) );
			type.Slide( new Direction( 0, -1 ) );
		}
	}
	#endregion
}
