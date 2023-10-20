
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2017 BY GREG STRONG

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

using System;
using System.Collections.Generic;

namespace ChessV.Boards
{
	public class BoardWithCards: Board
	{
		public int HandSize;

		public BoardWithCards( int nFiles, int nRanks, int handSize, int numPlayers = 2) : base( nFiles, nRanks, nRanks * nFiles + handSize * numPlayers )
		{
			HandSize = handSize;
			for( int player = 0; player < numPlayers; player++ )
			{
				for (int handIndex = 0; handIndex < handSize; handIndex++)
				{
					int square = NumSquares + handIndex * (1 + player);
					rankBySquare[square] = player;
          fileBySquare[square] = -handIndex;
        }
			}
		}

		public override Location SquareToLocation( int square )
		{
			if (square < NumSquares)
				return new Location(rankBySquare[square], fileBySquare[square]);
			else {
				int offset = square - NumSquares;
				return new Location(offset % HandSize, -offset / HandSize) ;
			}
		}

		public override int LocationToSquare( Location location )
		{
			if( location.File >= 0 )
				return location.File * NumRanks + location.Rank;
			else
				return NumSquares + location.Rank;
		}
	}
}
