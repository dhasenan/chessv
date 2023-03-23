
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

using System.Collections.Generic;

namespace ChessV.Games
{
	#region Falcon
	[PieceType("Falcon", "Multi-Path")]
	public class Falcon: PieceType
	{
		public Falcon( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Falcon", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			FallbackImage = "Bird";
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 3, 1 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 0 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 0 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 0 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 1 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 0, 1 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 1 ), new Direction( 0, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 0, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 0, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 0 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, -1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 0 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, -1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 0 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 1, -1 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 0, -1 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, -1 ), new Direction( 0, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 0, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 1, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 0, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 0 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 1 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 0 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 1 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 0 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -1, 1 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 0, 1 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 1 ), new Direction( 0, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 0, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -1, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 0, 1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 0 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, -1 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 0 ), new Direction( -1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, -1 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 0 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, -1 ), new Direction( -1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -1, -1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 0, -1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, -1 ), new Direction( 0, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 0, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -1, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 0, -1 ), new Direction( -1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region FreePadwar
	[PieceType("Free Padwar", "Multi-Path")]
	public class FreePadwar: PieceType
	{
		public FreePadwar( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Free Padwar", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
			FallbackImage = "ElephantFerz";
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction( 1, 1 ), 2 );
			type.Slide( new Direction( 1, -1 ), 2 );
			type.Slide( new Direction( -1, 1 ), 2 );
			type.Slide( new Direction( -1, -1 ), 2 );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 2, 0 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region ChainedPadwar
	[PieceType("Chained Padwar", "Multi-Path")]
	public class ChainedPadwar: PieceType
	{
		public ChainedPadwar( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Chained Padwar", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
			FallbackImage = "ElephantFerz";
		}

		public static new void AddMoves( PieceType type )
		{
			MoveCapability move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( 1, 1 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( 1, -1 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( -1, 1 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( -1, -1 );
			type.AddMoveCapability( move );

			//	add the multi-path moves
			move = MoveCapability.Step( new Direction( 2, 0 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
			move = new MoveCapability();

			move = MoveCapability.Step( new Direction( -2, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region FreeWarrior
	[PieceType("Free Warrior", "Multi-Path")]
	public class FreeWarrior: PieceType
	{
		public FreeWarrior( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Free Warrior", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
			FallbackImage = "WazirDabbabah";
		}

		public static new void AddMoves( PieceType type )
		{
			type.Slide( new Direction(  0,  1 ), 2 );
			type.Slide( new Direction(  0, -1 ), 2 );
			type.Slide( new Direction(  1,  0 ), 2 );
			type.Slide( new Direction( -1,  0 ), 2 );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 1, 1 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1,  0 ), new Direction(  0,  1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction(  0,  1 ), new Direction( -1,  0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction(  1, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction(  1,  0 ), new Direction(  0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction(  0, -1 ), new Direction(  1,  0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1,  0 ), new Direction(  0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction(  0, -1 ), new Direction( -1,  0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region ChainedWarrior
	[PieceType( "Chained Warrior", "Multi-Path" )]
	public class ChainedWarrior: PieceType
	{
		public ChainedWarrior( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Chained Warrior", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
			FallbackImage = "WazirDabbabah";
		}

		public static new void AddMoves( PieceType type )
		{
			MoveCapability move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( 2, 0 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( 0, 2 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( 0, -2 );
			type.AddMoveCapability( move );

			move = new MoveCapability();
			move.MinSteps = 2;
			move.MaxSteps = 2;
			move.CanCapture = true;
			move.Direction = new Direction( -2, 0 );
			type.AddMoveCapability( move );

			//	add the multi-path moves
			move = MoveCapability.Step( new Direction( 1, 1 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Oliphant
	[PieceType("Oliphant", "Multi-Path")]
	public class Oliphant: PieceType
	{
		public Oliphant( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Oliphant", name, notation, midgameValue, endgameValue, preferredImageName == null ? "Ferz Elephantrider" : preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Elephant.AddMoves( type );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 3, 3 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 2 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, -2 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 2 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, -2 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 4, 4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 2 ), new Direction( 2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 4, -4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, -2 ), new Direction( 2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -4, 4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 2 ), new Direction( -2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -4, -4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, -2 ), new Direction( -2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Lightning Warmachine
	[PieceType("Lightning Warmachine", "Multi-Path")]
	public class LightningWarmachine: PieceType
	{
		public LightningWarmachine( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Lightning Warmachine", name, notation, midgameValue, endgameValue, preferredImageName == null ? "Wazir Dabbabahrider" : preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Wazir.AddMoves( type );
			Dabbabah.AddMoves( type );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 3, 0 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 0 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 0 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 2 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 0, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -2 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 0, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 4, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 0 ), new Direction( 2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -4, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 0 ), new Direction( -2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 2 ), new Direction( 0, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -4 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -2 ), new Direction( 0, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Bent Hero
	[PieceType("Bent Hero", "Multi-Path")]
	public class BentHero: PieceType
	{
		public BentHero( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Bent Hero", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Wazir.AddMoves( type );
			Dabbabah.AddMoves( type );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 3, 0 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 0 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 0 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 2 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 0, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -2 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 0, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 0 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 0 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 0, 2 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 2 ), new Direction( 1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( 0, 2 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 2 ), new Direction( -1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 0 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 0 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 2, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( 0, -2 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -2 ), new Direction( -1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 0, -2 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -2 ), new Direction( 1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Bent Shaman
	[PieceType("Bent Shaman", "Multi-Path")]
	public class BentShaman: PieceType
	{
		public BentShaman( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Bent Shaman", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Elephant.AddMoves( type );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 3, 3 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 2 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 2 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, -2 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, -2 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 2 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 2 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, 2 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, 2 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( -2, 2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -3, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, -2 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 3, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, -2 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -2, -2 ), new Direction( 1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( -2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, -3 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 2, -2 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 2, -2 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Sliding General
	[PieceType("Sliding General", "Multi-Path")]
	public class SlidingGeneral: PieceType
	{
		public SlidingGeneral( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ) :
			base( "Sliding General", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			Ferz.AddMoves( type );
			Wazir.AddMoves( type );

			//	add the multi-path moves
			MoveCapability move = MoveCapability.Step( new Direction( 2, 0 ) );
			MovePathInfo movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 1 ), new Direction( -1, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( 0, 1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, 0 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, 1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, 0 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -2, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( -1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( -1, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 0, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( -1, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( -1, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 1, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 0, -1 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, -2 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );

			move = MoveCapability.Step( new Direction( 2, -1 ) );
			movePath = new MovePathInfo();
			movePath.AddPath( new List<Direction>() { new Direction( 1, -1 ), new Direction( 1, 0 ) } );
			movePath.AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, -1 ) } );
			move.PathInfo = movePath;
			type.AddMoveCapability( move );
		}
	}
	#endregion

	#region Mao
	[PieceType("Mao", "Multi-Path")]
	public class Mao: PieceType
	{
		public Mao( string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null ):
			base( "Mao", name, notation, midgameValue, endgameValue, preferredImageName )
		{
			AddMoves( this );
		}

		public static new void AddMoves( PieceType type )
		{
			type.AddMoveCapability( MoveCapability.Step( new Direction( 2, 1 ) ).AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, 1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( 1, 2 ) ).AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( 1, 1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( -1, 2 ) ).AddPath( new List<Direction>() { new Direction( 0, 1 ), new Direction( -1, 1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( -2, 1 ) ).AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, 1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( -2, -1 ) ).AddPath( new List<Direction>() { new Direction( -1, 0 ), new Direction( -1, -1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( -1, -2 ) ).AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( -1, -1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( 1, -2 ) ).AddPath( new List<Direction>() { new Direction( 0, -1 ), new Direction( 1, -1 ) } ) );
			type.AddMoveCapability( MoveCapability.Step( new Direction( 2, -1 ) ).AddPath( new List<Direction>() { new Direction( 1, 0 ), new Direction( 1, -1 ) } ) );
		}
	}
	#endregion
}
