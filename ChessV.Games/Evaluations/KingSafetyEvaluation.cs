
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
using System.Collections.Generic;
using ChessV.Games.Abstract;

namespace ChessV.Evaluations
{
	public class KingSafetyEvaluation: Evaluation
	{
		// *** CONSTRUCTION *** //

		public KingSafetyEvaluation( PieceType kingType, PieceType pawnType )
		{
			this.kingType = kingType;
			kingTypeNumber = kingType.TypeNumber;
			this.pawnType = pawnType;
			pawnTypeNumber = pawnType.TypeNumber;
			tropismTypes = new List<int>();
		}


		// *** INITIALIZATION *** //

		public override void Initialize( Game game )
		{
			base.Initialize( game );
			if( !(game is GenericChess) )
				throw new Exception( "Fatal Error: PawnStructureEvaluation - game must derive from GenericChess" );
			chessGame = (GenericChess) game;
		}

		public void AddTropism( PieceType type )
		{
			tropismTypes.Add( type.TypeNumber );
		}


		// *** OVERRIDES *** //

		public override void AdjustEvaluation( ref int midgameEval, ref int endgameEval )
		{
			int penalty;
			int kingsq;
			int kingrank;
			int kingfile;
			int pawnguards;
			int sq;
			Piece p;

			PawnHashEntry pawnhash = chessGame.PawnStructureInfo;

			// *** PLAYER 0 *** //

			penalty = 0;
			kingsq = board.GetPieceTypeBitboard( 0, kingTypeNumber ).LSB;
			kingrank = board.GetRank( kingsq );
			kingfile = board.GetFile( kingsq );

			// count pawn guards
			pawnguards = 0;
			if( kingfile == 0 )
				pawnguards++;
			else
			{
				sq = board.NextSquare( PredefinedDirections.NW, kingsq );
				if( sq >= 0 )
				{
					p = board[sq];
					if( p != null && p.Player == 0 && p.TypeNumber == pawnTypeNumber )
						pawnguards++;
				}
			}
			if( kingfile == board.NumFiles - 1 )
				pawnguards++;
			else
			{
				sq = board.NextSquare( PredefinedDirections.NE, kingsq );
				if( sq >= 0 )
				{
					p = board[sq];
					if( p != null && p.Player == 0 && p.TypeNumber == pawnTypeNumber )
						pawnguards++;
				}
			}
			sq = board.NextSquare( PredefinedDirections.N, kingsq );
			if( sq >= 0 )
			{
				p = board[sq];
				if( p != null && p.Player == 0 && p.TypeNumber == pawnTypeNumber )
					pawnguards++;
			}
			//	determine penalty for bad pawn protection
			penalty = 3 - pawnguards;
			if( kingrank != 0 )
			{
				penalty += 3;
				if( kingfile == 0 )
					penalty--;
				else
				{
					sq = board.NextSquare( PredefinedDirections.W, kingsq );
					p = board[sq];
					if( p != null && p.Player == 0 && p.TypeNumber == pawnTypeNumber )
						penalty--;
				}
				if( kingfile == board.NumFiles - 1 )
					penalty--;
				else
				{
					sq = board.NextSquare( PredefinedDirections.E, kingsq );
					p = board[sq];
					if( p != null && p.Player == 0 && p.TypeNumber == pawnTypeNumber )
						penalty--;
				}
			}
			//	penalty for being on or adjacent to open file or in front of pawn line
			int a = pawnhash.GetBackPawnRank( 0, kingfile );
			if( pawnhash.GetBackPawnRank( 0, kingfile ) == 15 ||
				pawnhash.GetBackPawnRank( 0, kingfile ) < board.GetRank( kingsq ) )
				penalty += 2;
			else if( kingfile > 0 &&
				(pawnhash.GetBackPawnRank( 0, kingfile - 1 ) == 15 ||
				 pawnhash.GetBackPawnRank( 0, kingfile - 1 ) < board.GetRank( kingsq )) )
				penalty += 2;
			else if( kingfile < board.NumFiles - 1 &&
				(pawnhash.GetBackPawnRank( 0, kingfile + 1 ) == 15 ||
				 pawnhash.GetBackPawnRank( 0, kingfile + 1 ) < board.GetRank( kingsq )) )
				penalty += 2;
			//	penalty for being close to enemy pieces of tropism types
			foreach( int type in tropismTypes )
			{
				BitBoard bb = board.GetPieceTypeBitboard( 1, type );
				while( bb )
				{
					int psq = bb.ExtractLSB();
					int distance = board.GetDistance( kingsq, psq );
					if( distance < 3 )
					{
						penalty++;
						if( distance == 1 )
							penalty += 2;
					}
				}
			}
			//	assess penalty
			midgameEval -= penalty * 4;
			if( penalty >= 2 )
				midgameEval -= (penalty - 1) * 3;
			if( penalty >= 4 )
				midgameEval -= (penalty - 3) * 4;


			// *** PLAYER 1 *** //

			penalty = 0;
			kingsq = board.GetPieceTypeBitboard( 1, kingTypeNumber ).LSB;
			kingrank = board.GetRank( kingsq );
			kingfile = board.GetFile( kingsq );

			// count pawn guards
			pawnguards = 0;
			if( kingfile == 0 )
				pawnguards++;
			else
			{
				sq = board.NextSquare( PredefinedDirections.SW, kingsq );
				if( sq >= 0 )
				{
					p = board[sq];
					if( p != null && p.Player == 1 && p.TypeNumber == pawnTypeNumber )
						pawnguards++;
				}
			}
			if( kingfile == board.NumFiles - 1 )
				pawnguards++;
			else
			{
				sq = board.NextSquare( PredefinedDirections.SE, kingsq );
				if( sq >= 0 )
				{
					p = board[sq];
					if( p != null && p.Player == 1 && p.TypeNumber == pawnTypeNumber )
						pawnguards++;
				}
			}
			sq = board.NextSquare( PredefinedDirections.S, kingsq );
			if( sq >= 0 )
			{
				p = board[sq];
				if( p != null && p.Player == 1 && p.TypeNumber == pawnTypeNumber )
					pawnguards++;
			}
			//	determine penalty for bad pawn protection
			penalty = 3 - pawnguards;
			if( kingrank != board.NumRanks - 1 )
			{
				penalty += 3;
				if( kingfile == 0 )
					penalty--;
				else
				{
					sq = board.NextSquare( PredefinedDirections.W, kingsq );
					p = board[sq];
					if( p != null && p.Player == 1 && p.TypeNumber == pawnTypeNumber )
						penalty--;
				}
				if( kingfile == board.NumFiles - 1 )
					penalty--;
				else
				{
					sq = board.NextSquare( PredefinedDirections.E, kingsq );
					p = board[sq];
					if( p != null && p.Player == 1 && p.TypeNumber == pawnTypeNumber )
						penalty--;
				}
			}
			//	penalty for being on or adjacent to open file or in front of pawn line
			if( pawnhash.GetBackPawnRank( 0, kingfile ) == 0 ||
				pawnhash.GetBackPawnRank( 0, kingfile ) > board.GetRank( kingsq ) )
				penalty += 2;
			else if( kingfile > 0 &&
				(pawnhash.GetBackPawnRank( 0, kingfile - 1 ) == 0 ||
				 pawnhash.GetBackPawnRank( 0, kingfile - 1 ) > board.GetRank( kingsq )) )
				penalty += 2;
			else if( kingfile < board.NumFiles - 1 &&
				(pawnhash.GetBackPawnRank( 0, kingfile + 1 ) == 0 ||
				 pawnhash.GetBackPawnRank( 0, kingfile + 1 ) > board.GetRank( kingsq )) )
				penalty += 2;
			//	penalty for being close to enemy pieces of tropism types
			foreach( int type in tropismTypes )
			{
				BitBoard bb = board.GetPieceTypeBitboard( 0, type );
				while( bb )
				{
					int psq = bb.ExtractLSB();
					int distance = board.GetDistance( kingsq, psq );
					if( distance < 3 )
					{
						penalty++;
						if( distance == 1 )
							penalty += 2;
					}
				}
			}
			//	assess penalty
			midgameEval += penalty * 4;
			if( penalty >= 2 )
				midgameEval += (penalty - 1) * 3;
			if( penalty >= 4 )
				midgameEval += (penalty - 3) * 4;
		}


		// *** PROTECTED DATA *** //

		protected PieceType kingType;
		protected int kingTypeNumber;
		protected PieceType pawnType;
		protected int pawnTypeNumber;
		protected List<int> tropismTypes;
		protected GenericChess chessGame;
	}
}
