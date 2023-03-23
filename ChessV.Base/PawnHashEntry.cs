
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

using System;

namespace ChessV
{
	public struct PawnHashEntry
	{
		public UInt64 HashCode;
		public Int32 MidgameAdjustment;
		public Int32 EndgameAdjustment;
		private UInt64 backPawnRank0;
		private UInt64 backPawnRank1;
		public BitBoard PassedPawns;

		public int GetBackPawnRank( int player, int file )
		{
			UInt64 backPawnRank = (player == 0 ? backPawnRank0 : backPawnRank1);
			return (int) ((backPawnRank >> (file * 4)) & 0x0000000FUL);
		}

		public void SetBackPawnRank( int player, int file, int backRank )
		{
			UInt64 clear = 0xFFFFFFFFFFFFFFFFUL ^ (0x0FUL << (file * 4));
			UInt64 set = (UInt64) backRank << (file * 4);
			if( player == 0 )
				backPawnRank0 = (backPawnRank0 & clear) | set;
			else
				backPawnRank1 = (backPawnRank1 & clear) | set;
		}
	}
}
