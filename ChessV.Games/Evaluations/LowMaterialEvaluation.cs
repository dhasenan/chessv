
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2020 BY GREG STRONG

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
using System.Linq;
using ChessV.Games;

namespace ChessV.Evaluations
{
	public class LowMaterialEvaluation: Evaluation
	{
		// *** PROPERTIES *** //

		public int[] PushToCorner { get; protected set; }
		public int[] PushClose { get; protected set; }
		public int[] PushAway { get; protected set; }

		//	For some games, King + Rook cannot force win, such as 
		//	Cylindrical or circular board, or Omega Chess board
		public bool KRKIsDraw { get; set; }


		// *** INITIALIZATION *** //

		#region Initialize
		public override void Initialize( Game game )
		{
			base.Initialize( game );

			KRKIsDraw = false;
			if( game.Board is Boards.CylindricalBoard )
				KRKIsDraw = true;

			royalTypeNumbers = new HashSet<int>();
			promotingTypeNumber = -1;
			kingTypeNumber = -1;
			pawnTypeNumber = -1;
			knightTypeNumber = -1;
			bishopTypeNumber = -1;
			rookTypeNumber1 = -1;
			rookTypeNumber2 = -1;

			int nFiles = game.Board.NumFiles;
			int nRanks = game.Board.NumRanks;
			int nSquares = game.Board.NumSquares;
			int nSquaresExt = game.Board.NumSquaresExtended;
			int nDistance = Math.Max( nFiles, nRanks );

			PushClose = new int[nDistance];
			for( int x = 0; x < nDistance; x++ )
				PushClose[x] = (nDistance - x) * (100 / nDistance);

			PushAway = new int[nDistance];
			for( int x = 0; x < nDistance; x++ )
			{
				if( x < 2 )
					PushAway[x] = 5;
				else if( x < 6 )
					PushAway[x] = (x - 1) * 20;
				else
					PushAway[x] = 80 + (x * 5);
			}

			PushToCorner = new int[nSquares];
			for( int x = 0; x < nSquares; x++ )
			{
				Location loc = game.Board.SquareToLocation( x );
				int distFromFileEdges = loc.File >= nFiles / 2 ? (nFiles - loc.File - 1) : loc.File;
				int distFromRankEdges = loc.Rank >= nRanks / 2 ? (nRanks - loc.Rank - 1) : loc.Rank;
				int distFromCorner = Math.Max( distFromFileEdges, distFromRankEdges );
				int distFromEdge = Math.Min( distFromFileEdges, distFromRankEdges );
				PushToCorner[x] =
					(40 - (distFromEdge * 34 / (nDistance / 2))) +
					(50 - (distFromCorner * 40 / (nDistance / 2)));
			}
		}
		#endregion

		#region PostInitialize
		public override void PostInitialize()
		{
			base.PostInitialize();

			//	find the royal piece type
			Games.Rules.CheckmateRule rule =
				(Games.Rules.CheckmateRule) game.FindRule( typeof( Games.Rules.CheckmateRule ) );
			if( rule != null )
				royalTypeNumbers = new HashSet<int>(rule.RoyalPieceTypes.Select((type) => type.TypeNumber));
			//	find the promoting type
			if( ((Games.Abstract.GenericChess) game).PromotingType != null )
				promotingTypeNumber = ((Games.Abstract.GenericChess) game).PromotingType.TypeNumber;

			PieceType[] pieceTypes;
			int pieceTypeCount = game.GetPieceTypes( out pieceTypes );
			for( int nPieceType = 0; nPieceType < pieceTypeCount; nPieceType++ )
			{
				if( pieceTypes[nPieceType] is Pawn )
					pawnTypeNumber = pieceTypes[nPieceType].TypeNumber;
				else if( (pieceTypes[nPieceType] is King && kingTypeNumber == -1) || 
					(pieceTypes[nPieceType] is King && royalTypeNumbers.Contains(nPieceType)) )
					kingTypeNumber = pieceTypes[nPieceType].TypeNumber;
				else if( pieceTypes[nPieceType] is Knight )
					knightTypeNumber = pieceTypes[nPieceType].TypeNumber;
				else if( pieceTypes[nPieceType] is Bishop )
					bishopTypeNumber = pieceTypes[nPieceType].TypeNumber;
				else if( pieceTypes[nPieceType] is Rook ||
					pieceTypes[nPieceType] is ChargingRook ||
					pieceTypes[nPieceType] is ShortRook )
				{
					if( rookTypeNumber1 == -1 )
						rookTypeNumber1 = pieceTypes[nPieceType].TypeNumber;
					else
						rookTypeNumber2 = pieceTypes[nPieceType].TypeNumber;
				}
			}
			materialHashTable = new MaterialHashEntry[65536]; // 2^16 slots = 1 MB
		}
		#endregion


		// *** OVERRIDES *** //

		#region ReleaseMemoryAllocations
		public override void ReleaseMemoryAllocations()
		{
			base.ReleaseMemoryAllocations();
			materialHashTable = null;
			PushToCorner = null;
			PushClose = null;
			PushAway = null;
		}
		#endregion

		#region TestForWinLossDraw
		public override MoveEventResponse TestForWinLossDraw( int currentPlayer, int ply )
		{
			MaterialHashType type = lookupPosition();

			if( type == MaterialHashType.InstantDraw )
				return MoveEventResponse.GameDrawn;

			return MoveEventResponse.NotHandled;
		}
		#endregion

		#region AdjustEvaluation
		public override void AdjustEvaluation( ref int midgameEval, ref int endgameEval )
		{
			MaterialHashType type = lookupPosition();

			if( type == MaterialHashType.ZeroEvaluation || 
				type == MaterialHashType.InstantDraw )
			{
				midgameEval = 0;
				endgameEval = 0;
				return;
			}

			if( type == MaterialHashType.EvalDivideByFour )
			{
				midgameEval /= 4;
				endgameEval /= 4;
				return;
			}

			if( type == MaterialHashType.ValueFunctionKXK )
			{
				evaluateKxK( ref midgameEval, ref endgameEval );
				return;
			}

			if( type == MaterialHashType.ValueFunctionKRKP )
			{
				evaluateKRKP( ref midgameEval, ref endgameEval );
				return;
			}

			if( type == MaterialHashType.ValueFunctionKRKB )
			{
				evaluateKRKB( ref midgameEval, ref endgameEval );
				return;
			}

			if( type == MaterialHashType.ValueFunctionKRKN )
			{
				evaluateKRKN( ref midgameEval, ref endgameEval );
				return;
			}
		}
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region lookupPosition
		protected MaterialHashType lookupPosition()
		{
			game.Statistics.MaterialHashLookups++;
			int slot = (int) (game.Board.MaterialHashCode & 0x000000000000FFFFUL);
			if( materialHashTable[slot].HashCode == game.Board.MaterialHashCode )
			{
				game.Statistics.MaterialHashHits++;
				return materialHashTable[slot].Type;
			}

			//	we don't have this material balance in our table, 
			//	so we must make a new entry for it ... 

			//	first, check for automatic draw by insufficient material
			int totalPieces = board.GetPlayerPieceBitboard( 0 ).BitCount + board.GetPlayerPieceBitboard( 1 ).BitCount;

			if( totalPieces == 2 )
				//	nothing but two kings is an automatic draw
				return MaterialHashType.InstantDraw;

			if( totalPieces == 3 && knightTypeNumber >= 0 && bishopTypeNumber >= 0 )
			{
				//	Assume two kings.  If the third piece is either a knight 
				//	or a bishop, the game is an automatic draw.
				int totalKnightsAndBishops =
					board.GetPieceTypeBitboard( 0, knightTypeNumber ).BitCount + 
					board.GetPieceTypeBitboard( 0, bishopTypeNumber ).BitCount +
					board.GetPieceTypeBitboard( 1, knightTypeNumber ).BitCount + 
					board.GetPieceTypeBitboard( 1, bishopTypeNumber ).BitCount;
				if( totalKnightsAndBishops == 1 )
				{
					materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
					materialHashTable[slot].Type = MaterialHashType.InstantDraw;
					return MaterialHashType.InstantDraw;
				}

				if( KRKIsDraw )
				{
					int totalRooks = rookTypeNumber1 >= 0 ?
						board.GetPieceTypeBitboard( 0, rookTypeNumber1 ).BitCount +
						board.GetPieceTypeBitboard( 1, rookTypeNumber1 ).BitCount : 0;
					if( totalRooks == 1 )
					{
						//	game doesn't end instantly, but the evaluation is a draw
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.ZeroEvaluation;
						return MaterialHashType.ZeroEvaluation;
					}
				}
			}

			else if( totalPieces == 4 && knightTypeNumber >= 0 && bishopTypeNumber >= 0 )
			{
				int wBishops = bishopTypeNumber > 0 ? board.GetPieceTypeBitboard( 0, bishopTypeNumber ).BitCount : 0;
				int bBishops = bishopTypeNumber > 0 ? board.GetPieceTypeBitboard( 1, bishopTypeNumber ).BitCount : 0;
				int wKnights = knightTypeNumber > 0 ? board.GetPieceTypeBitboard( 0, knightTypeNumber ).BitCount : 0;
				int bKnights = knightTypeNumber > 0 ? board.GetPieceTypeBitboard( 1, knightTypeNumber ).BitCount : 0;

				//	Do we have only two bishops?
				if( wBishops + bBishops == 2 )
				{
					//	Are they on the same color?
					if( (board.GetPieceTypeBitboardSliced( 0, bishopTypeNumber, 0 ).BitCount +
						 board.GetPieceTypeBitboardSliced( 1, bishopTypeNumber, 0 ).BitCount == 2) ||
						(board.GetPieceTypeBitboardSliced( 0, bishopTypeNumber, 1 ).BitCount +
						 board.GetPieceTypeBitboardSliced( 1, bishopTypeNumber, 1 ).BitCount == 2) )
					{
						//	game ends instantly in draw
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.InstantDraw;
						return MaterialHashType.InstantDraw;
					}
					//	One bishop for each player?
					else if( wBishops == 1 && bBishops == 1 )
					{
						//	game doesn't end instantly, but the evaluation is a draw
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.ZeroEvaluation;
						return MaterialHashType.ZeroEvaluation;
					}
				}
				//	Different combination of two bishops/knights?
				else if( wBishops + bBishops + wKnights + bKnights == 2 )
				{
					if( !(wBishops == 1 && wKnights == 1) && !(bBishops == 1 && bKnights == 1) )
					{
						//	game doesn't end instantly, but the evaluation is a draw
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.ZeroEvaluation;
						return MaterialHashType.ZeroEvaluation;
					}
				}

				int wRookTypes =
					(rookTypeNumber1 >= 0 ? board.GetPieceTypeBitboard( 0, rookTypeNumber1 ).BitCount : 0) +
					(rookTypeNumber2 >= 0 ? board.GetPieceTypeBitboard( 0, rookTypeNumber2 ).BitCount : 0);
				int bRookTypes =
					(rookTypeNumber1 >= 0 ? board.GetPieceTypeBitboard( 1, rookTypeNumber1 ).BitCount : 0) +
					(rookTypeNumber2 >= 0 ? board.GetPieceTypeBitboard( 1, rookTypeNumber2 ).BitCount : 0);
				int wPawns = pawnTypeNumber >= 0 ? board.GetPieceTypeBitboard( 0, pawnTypeNumber ).BitCount : 0;
				int bPawns = pawnTypeNumber >= 0 ? board.GetPieceTypeBitboard( 1, pawnTypeNumber ).BitCount : 0;

				if( (pawnTypeNumber == promotingTypeNumber) && 
					((wRookTypes == 1 && bPawns == 1) || (bRookTypes == 1 && wPawns == 1)) )
				{
					materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
					materialHashTable[slot].Type = MaterialHashType.ValueFunctionKRKP;
					return MaterialHashType.ValueFunctionKRKP;
				}

				if( (wRookTypes == 1 && bBishops == 1) || (bRookTypes == 1 && wBishops == 1) )
				{
					materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
					materialHashTable[slot].Type = MaterialHashType.ValueFunctionKRKB;
					return MaterialHashType.ValueFunctionKRKB;
				}

				if( (wRookTypes == 1 && bKnights == 1) || (bRookTypes == 1 && wKnights == 1) )
				{
					materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
					materialHashTable[slot].Type = MaterialHashType.ValueFunctionKRKN;
					return MaterialHashType.ValueFunctionKRKB;
				}
			}

			//	if the side in the lead has no pawns and less than a minor 
			//	piece advantage, then we divide the evaluation by 4
			if( promotingTypeNumber >= 0 )
			{
				if( board.GetMidgameMaterialEval( 0 ) > board.GetMidgameMaterialEval( 1 ) )
				{
					if( board.GetPieceTypeBitboard( 0, promotingTypeNumber ).BitCount == 0 &&
						board.GetMidgameMaterialEval( 0 ) - board.GetMidgameMaterialEval( 1 ) < 300 )
					{
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.EvalDivideByFour;
						return MaterialHashType.EvalDivideByFour;
					}
				}
				else if( board.GetMidgameMaterialEval( 1 ) > board.GetMidgameMaterialEval( 0 ) )
				{
					if( board.GetPieceTypeBitboard( 1, promotingTypeNumber ).BitCount == 0 &&
						board.GetMidgameMaterialEval( 1 ) - board.GetMidgameMaterialEval( 0 ) < 300 )
					{
						materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
						materialHashTable[slot].Type = MaterialHashType.EvalDivideByFour;
						return MaterialHashType.EvalDivideByFour;
					}
				}
			}

			//	see if one side has a lone king and the other has plenty
			if( (board.GetPlayerPieceBitboard( 0 ).BitCount == 1 && board.GetPlayerEndgameMaterial( 1 ) >= 500) ||
				(board.GetPlayerPieceBitboard( 1 ).BitCount == 1 && board.GetPlayerEndgameMaterial( 0 ) >= 500) )
			{
				materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
				materialHashTable[slot].Type = MaterialHashType.ValueFunctionKXK;
				return MaterialHashType.ValueFunctionKXK;
			}

			materialHashTable[slot].HashCode = game.Board.MaterialHashCode;
			materialHashTable[slot].Type = MaterialHashType.NothingSpecial;
			return MaterialHashType.NothingSpecial;
		}
		#endregion

		#region evaluateKxK
		protected void evaluateKxK( ref int midgameEval, ref int endgameEval )
		{
			int strongSide = board.GetPlayerMaterial( 0 ) > board.GetPlayerMaterial( 1 ) ? 0 : 1;
			int weakSide = strongSide ^ 1;

			int strongKingSquare = board.GetPieceTypeBitboard( strongSide, royalTypeNumbers.First() ).LSB;
			int weakKingSquare = board.GetPieceTypeBitboard( weakSide, royalTypeNumbers.First()).LSB;

			int a = board.GetPlayerEndgameMaterial( strongSide );
			int value = board.GetPlayerEndgameMaterial( strongSide ) + PushToCorner[weakKingSquare] +
				PushClose[board.GetDistance( strongKingSquare, weakKingSquare )] + 5000;

			midgameEval = endgameEval = (strongSide == 0 ? value : -value);
		}
		#endregion

		#region evaluateKRKP
		//	Evaluate endgames with a rook-type vs. a pawn.  The rook-types include the 
		//	standard rook as well as the short rook and the charging rook.
		//	We return drawish scores when the pawn is far advanced with support of the 
		//	king while the other king is far away.  This logic comes from Stockfish.
		protected void evaluateKRKP( ref int midgameEval, ref int endgameEval )
		{
			int strongSide = board.GetPlayerMaterial( 0 ) > board.GetPlayerMaterial( 1 ) ? 0 : 1;
			int weakSide = strongSide ^ 1;
			
			int strongKingSquare = board.GetPieceTypeBitboard( strongSide, kingTypeNumber ).LSB;
			int weakKingSquare = board.GetPieceTypeBitboard( weakSide, kingTypeNumber ).LSB;
			BitBoard rookBitboard = board.GetPieceTypeBitboard( strongSide, rookTypeNumber1 );
			if( rookBitboard.IsEmpty && rookTypeNumber2 >= 0 )
				rookBitboard = board.GetPieceTypeBitboard( strongSide, rookTypeNumber2 );
			if( rookBitboard.IsEmpty )
				return;
			int rookSquare = rookBitboard.LSB;
			int pawnSquare = board.GetPieceTypeBitboard( weakSide, pawnTypeNumber ).LSB;

			//	if the stronger side's king is in front of the pawn, it's a win
			if( board.GetRank( strongKingSquare ) == board.GetRank( pawnSquare ) )
			{
				//	This is more complicated than would normally be expected because we 
				//	must check the game's symmetry.  We cannot assume that player 2's 
				//	pawns move in the opposite direction of player 1's (e.g. Viking Chess.)
				if( game.Symmetry.Translate( weakSide, board.SquareToLocation( strongKingSquare ) ).Rank >
					game.Symmetry.Translate( weakSide, board.SquareToLocation( pawnSquare ) ).Rank )
				{
					int eval = board.GetEndgameMaterialEval( strongSide ) - board.GetDistance( strongKingSquare, pawnSquare );
					midgameEval = endgameEval = (strongSide == 0 ? eval : -eval);
					return;
				}
			}
			//	If the weaker side's king is too far from the pawn and the rook, it's a win.
			if( board.GetDistance( weakKingSquare, pawnSquare ) >= 3 + (game.CurrentSide == weakSide ? 1 : 0) &&
				board.GetDistance( weakKingSquare, rookSquare ) >= 3 )
			{
				//	Unless the rook-type piece is a charging rook and is too far advanced.
				//	NOTE: This check does not account for symmetry, but charging rook is 
				//	only used in CwDA.  That said, this could be improved.
				if( !(board[rookSquare].PieceType is ChargingRook) ||
					(strongSide == 0 && board.GetRank( rookSquare ) <= board.GetRank( pawnSquare )) ||
					(strongSide == 1 && board.GetRank( rookSquare ) >= board.GetRank( pawnSquare )) )
				{
					int eval = board.GetEndgameMaterialEval( strongSide ) - board.GetDistance( strongKingSquare, pawnSquare );
					midgameEval = endgameEval = (strongSide == 0 ? eval : -eval);
					return;
				}
			}
			//	If the pawn is far advanced and supported by the defending king, the position is drawish
			if( (weakSide == 0 && board.GetRank( weakKingSquare ) <= 2 ||
				 weakSide == 1 && board.GetRank( weakKingSquare ) >= board.NumRanks - 3) &&
				board.GetDistance( weakKingSquare, pawnSquare ) == 1 &&
				(weakSide == 0 && board.GetRank( strongKingSquare ) >= 3 ||
				 weakSide == 1 && board.GetRank( strongKingSquare ) <= board.NumRanks - 4) &&
				board.GetDistance( strongKingSquare, pawnSquare ) > 2 + (game.CurrentSide == strongSide ? 1 : 0) )
			{
				int eval = 70 - 7*board.GetDistance( strongKingSquare, pawnSquare );
				midgameEval = endgameEval = (strongSide == 0 ? eval : -eval);
				return;
			}

			//	default evaluation ...
			int nextPawnSquare = board.NextSquare( game.GetDirectionNumber( game.Symmetry.Translate( weakSide, new Direction( 1, 0 ) ) ), pawnSquare );
			//	find the pawn's promotion square
			int promotionSquare = nextPawnSquare;
			while( true )
			{
				int nextStep = board.NextSquare( game.GetDirectionNumber( game.Symmetry.Translate( weakSide, new Direction( 1, 0 ) ) ), promotionSquare );
				if( nextStep == -1 )
					break;
				else
					promotionSquare = nextStep;
			}
			int value = 150 - 6 *
				(board.GetDistance( strongKingSquare, nextPawnSquare ) -
				 board.GetDistance( weakKingSquare, nextPawnSquare ) -
				 board.GetDistance( pawnSquare, promotionSquare ));
			midgameEval = endgameEval = (strongSide == 0 ? value : -value);
		}
		#endregion

		#region evaluateKRKB
		//	Evaluate king plus rook-type vs. king plus bishop.  This is very simple and 
		//	always returns drawish scores.  The score is slightly bigger when the weak 
		//	king is close to the edge.  This logic comes from Stockfish.
		protected void evaluateKRKB( ref int midgameEval, ref int endgameEval )
		{
			int strongSide = board.GetPlayerMaterial( 0 ) > board.GetPlayerMaterial( 1 ) ? 0 : 1;
			int weakSide = strongSide ^ 1;
			int weakKingSquare = board.GetPieceTypeBitboard( weakSide, kingTypeNumber ).LSB;
			int eval = PushToCorner[weakKingSquare];
			midgameEval = endgameEval = (strongSide == 0 ? eval : -eval);
		}
		#endregion

		#region evaluateKRKN
		//	Evaluate king plus rook-type vs. king plus knight.  Reward pushing weak king 
		//	into the corner, plus a bonus for the knight being farther away.  Somewhat 
		//	better winning chances than KRKB.  This logic also from Stockfish.
		protected void evaluateKRKN( ref int midgameEval, ref int endgameEval )
		{
			int strongSide = board.GetPlayerMaterial( 0 ) > board.GetPlayerMaterial( 1 ) ? 0 : 1;
			int weakSide = strongSide ^ 1;
			int weakKingSquare = board.GetPieceTypeBitboard( weakSide, kingTypeNumber ).LSB;
			int weakKnightSquare = board.GetPieceTypeBitboard( weakSide, knightTypeNumber ).LSB;
			int eval = PushToCorner[weakKingSquare] + PushAway[board.GetDistance( weakKingSquare, weakKnightSquare )];
			midgameEval = endgameEval = (strongSide == 0 ? eval : -eval);
		}
		#endregion


		// *** PROTECTED DATA MEMBERS *** //

		protected HashSet<int> royalTypeNumbers;
		protected int promotingTypeNumber;
		protected int kingTypeNumber;
		protected int pawnTypeNumber;
		protected int knightTypeNumber;
		protected int bishopTypeNumber;
		protected int rookTypeNumber1;
		protected int rookTypeNumber2;
		protected MaterialHashEntry[] materialHashTable;
	}
}
