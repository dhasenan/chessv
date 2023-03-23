
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

namespace ChessV.Games.Rules.KingsCourt
{
	public class KingsFlightRule: Rule
	{
		public KingsFlightRule( PieceType kingType, PieceType chasingType )
		{
			this.kingType = kingType;
			this.chasingType = chasingType;
		}


		public override void GenerateSpecialMoves( MoveList list, bool capturesOnly, int ply )
		{
			int kingSquare = Board.GetPieceTypeBitboard( Game.CurrentSide, kingType.TypeNumber ).LSB;
			BitBoard potentialAttackers = Board.GetPieceTypeBitboard( Game.CurrentSide ^ 1, chasingType.TypeNumber );
			while( potentialAttackers )
			{
				int sq = potentialAttackers.ExtractLSB();
				int direction = Board.DirectionLookup( sq, kingSquare );
				if( direction >= 0 && chasingType.AttackRangePerDirection[direction] >= Board.GetDistance( sq, kingSquare ) )
				{
					//	King is attacked by appropriate chasing type.
					//	Allow him to flee two squares in any direction.
					for( int nDir = 0; nDir < 8; nDir++ )
					{
						int nextSquare = Board.NextSquare( nDir, kingSquare );
						if( nextSquare >= 0 && Board[nextSquare] == null )
						{
							nextSquare = Board.NextSquare( nDir, nextSquare );
							if( nextSquare >= 0 )
							{
								Piece pieceOnSquare = Board[nextSquare];
								if( pieceOnSquare == null )
								{
									list.BeginMoveAdd( MoveType.StandardMove, kingSquare, nextSquare );
									Piece king = list.AddPickup( kingSquare );
									list.AddDrop( king, nextSquare );
									list.EndMoveAdd( 150 );
								}
								else if( pieceOnSquare.Player != Game.CurrentSide )
								{
									list.BeginMoveAdd( MoveType.StandardCapture, kingSquare, nextSquare );
									Piece king = list.AddPickup( kingSquare );
									list.AddPickup( nextSquare );
									list.AddDrop( king, nextSquare );
									list.EndMoveAdd( 3000 + pieceOnSquare.PieceType.MidgameValue );
								}
							}
						}
					}
				}
			}
		}

		protected PieceType kingType;
		protected PieceType chasingType;
	}
}
