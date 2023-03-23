
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

namespace ChessV.Games.Rules
{
	public class LocationVictoryConditionRule: Rule
	{
		public LocationVictoryConditionRule( PieceType pieceType, ConditionalLocationDelegate victoryLocationDelegate )
		{
			types = new List<PieceType>() { pieceType };
			victoryLocation = victoryLocationDelegate;
			lastDestinationSquare = new int[Game.MAX_PLY];
			lastDestinationSquare[0] = -1;
		}

		public LocationVictoryConditionRule( List<PieceType> pieceTypes, ConditionalLocationDelegate victoryLocationDelegate )
		{
			types = pieceTypes;
			victoryLocation = victoryLocationDelegate;
			lastDestinationSquare = new int[Game.MAX_PLY];
			lastDestinationSquare[0] = -1;
		}

		public override MoveEventResponse TestForWinLossDraw( int currentPlayer, int ply )
		{
			if( lastDestinationSquare[ply-1] >= 0 &&
				types.Contains( Board[lastDestinationSquare[ply-1]].PieceType ) &&
				victoryLocation( Board.SquareToLocation( Board.PlayerSquare( Board[lastDestinationSquare[ply-1]].Player, lastDestinationSquare[ply-1] ) ) ) )
				return currentPlayer == Board[lastDestinationSquare[ply-1]].Player ? MoveEventResponse.GameWon : MoveEventResponse.GameLost;
			return MoveEventResponse.NotHandled;
		}

		public override MoveEventResponse MoveBeingMade( MoveInfo move, int ply )
		{
			lastDestinationSquare[ply] = (move.MoveType == MoveType.Pass || move.MoveType == MoveType.NullMove) ? -1 : move.ToSquare;
			return MoveEventResponse.NotHandled;
		}

		public override void GetNotesForPieceType( PieceType type, List<string> notes )
		{
			if( types.Contains( type ) )
				notes.Add( "location victory condition" );
		}

		protected List<PieceType> types;
		protected ConditionalLocationDelegate victoryLocation;
		protected int[] lastDestinationSquare;
	}
}
