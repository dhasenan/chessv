
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

namespace ChessV.Games.Rules
{
	public class BishopConversionRule: Rule
	{
		#region PrivFlags helper enumeration
		enum PrivFlags
		{
			none = 0,
			p0sq0can = 1,
			p0sq0must = 2,
			p0sq1can = 4,
			p0sq1must = 8,
			p1sq0can = 16,
			p1sq0must = 32,
			p1sq1can = 64,
			p1sq1must = 128,
			p0all = p0sq0can | p0sq0must | p0sq1can | p0sq1must,
			p1all = p1sq0can | p1sq0must | p1sq1can | p1sq1must
		}
		#endregion


		// *** CONSTRUCTION *** //

		#region Constructor
		public BishopConversionRule( string[] bishopSquares )
		{
			this.bishopSquares = bishopSquares;
		}
		#endregion


		// *** OVERRIDES *** //

		#region Initialize
		public override void Initialize( Game game )
		{
			base.Initialize( game );
			hashKeyIndex = game.HashKeys.TakeKeys( 256 );
			privs = new int[Game.MAX_PLY];
			for( int x = 0; x < Game.MAX_PLY; x++ )
				privs[x] = 0;
			gameHistory = new int[Game.MAX_GAME_LENGTH];
			gameHistory[0] = 0;
			p0square0 = game.NotationToSquare( bishopSquares[0] );
			p0square1 = game.NotationToSquare( bishopSquares[1] );
			p1square0 = game.NotationToSquare( bishopSquares[2] );
			p1square1 = game.NotationToSquare( bishopSquares[3] );
			allPrivString = bishopSquares[0][0].ToString().ToUpper() +
				bishopSquares[1][0].ToString().ToUpper() + bishopSquares[2][0] + bishopSquares[3][0];
			//	Hook up MoveBeingPlayed event handler
			game.MoveBeingPlayed += MoveBeingPlayedHandler;
		}
		#endregion

		#region ClearGameState
		public override void ClearGameState()
		{
			for( int x = 0; x < Game.MAX_PLY; x++ )
				privs[x] = 0;
			for( int x = 0; x < Game.MAX_GAME_LENGTH; x++ )
				gameHistory[x] = 0;
		}
		#endregion

		#region GetPositionHashCode
		public override ulong GetPositionHashCode( int ply )
		{
			int priv = ply == 1 ? gameHistory[Game.GameMoveNumber] : privs[ply - 1];
			return HashKeys.Keys[hashKeyIndex + priv];
		}
		#endregion

		#region SetDefaultsInFEN
		public override void SetDefaultsInFEN( FEN fen )
		{
			if( fen["bishop-conversion"] == "#default" )
				fen["bishop-conversion"] = allPrivString;
		}
		#endregion

		#region PositionLoaded
		public override void PositionLoaded( FEN fen )
		{
			string bc = fen["bishop-conversion"];
			if( bc == "-" )
				privs[0] = 0;
			else
			{
				int cursor = 0;
				while( cursor < bc.Length )
				{
					if( Char.IsUpper( bc[cursor] ) )
					{
						//	upper-case so this is a player 0 privilege
						if( Char.ToLower( bc[cursor] ) == Game.GetSquareNotation( p0square0 )[0] )
						{
							privs[0] |= (int) PrivFlags.p0sq0can;
							if( cursor + 1 < bc.Length && bc[cursor + 1] == '+' )
							{
								privs[0] |= (int) PrivFlags.p0sq0must;
								cursor++;
							}
						}
						else if( Char.ToLower( bc[cursor] ) == Game.GetSquareNotation( p0square1 )[0] )
						{
							privs[0] |= (int) PrivFlags.p0sq1can;
							if( cursor + 1 < bc.Length && bc[cursor + 1] == '+' )
							{
								privs[0] |= (int) PrivFlags.p0sq1must;
								cursor++;
							}
						}
						else
							throw new Exceptions.FENParseFailureException( "bishop-conversion", bc, "Unexpected character in bishop conversion notation: " + bc[cursor] );
					}
					else
					{
						//	lower-case so this is a player 1 privilege
						if( bc[cursor] == Game.GetSquareNotation( p1square0 )[0] )
						{
							privs[0] |= (int) PrivFlags.p1sq0can;
							if( cursor + 1 < bc.Length && bc[cursor + 1] == '+' )
							{
								privs[0] |= (int) PrivFlags.p1sq0must;
								cursor++;
							}
						}
						else if( bc[cursor] == Game.GetSquareNotation( p1square1 )[0] )
						{
							privs[0] |= (int) PrivFlags.p1sq1can;
							if( cursor + 1 < bc.Length && bc[cursor + 1] == '+' )
							{
								privs[0] |= (int) PrivFlags.p1sq1must;
								cursor++;
							}
						}
						else
							throw new Exceptions.FENParseFailureException( "bishop-conversion", bc, "Unexpected character in bishop conversion notation: " + bc[cursor] );
					}
					cursor++;
				}
			}
			gameHistory[Game.GameMoveNumber] = privs[0];
		}
		#endregion

		#region SavePositionToFEN
		public override void SavePositionToFEN( FEN fen )
		{
			PrivFlags conversionPrivs = (PrivFlags) gameHistory[Game.GameMoveNumber];
			string privString = "";
			//	player 0 privs
			if( (conversionPrivs & PrivFlags.p0sq0must) != PrivFlags.none )
				privString += bishopSquares[0][0].ToString().ToUpper() + "+";
			else if( (conversionPrivs & PrivFlags.p0sq1must) != PrivFlags.none )
				privString += bishopSquares[1][0].ToString().ToUpper() + "+";
			else if( (conversionPrivs & PrivFlags.p0sq0can) != PrivFlags.none )
				privString += bishopSquares[0][0].ToString().ToUpper();
			else if( (conversionPrivs & PrivFlags.p0sq1can) != PrivFlags.none )
				privString += bishopSquares[1][0].ToString().ToUpper();
			//	player 1 privs
			if( (conversionPrivs & PrivFlags.p1sq0must) != PrivFlags.none )
				privString += bishopSquares[2][0].ToString() + "+";
			else if( (conversionPrivs & PrivFlags.p1sq1must) != PrivFlags.none )
				privString += bishopSquares[3][0].ToString() + "+";
			else if( (conversionPrivs & PrivFlags.p1sq0can) != PrivFlags.none )
				privString += bishopSquares[2][0].ToString();
			else if( (conversionPrivs & PrivFlags.p1sq1can) != PrivFlags.none )
				privString += bishopSquares[3][0].ToString();
			if( privString == "" )
				privString = "-";
			fen["bishop-conversion"] = privString;
		}
		#endregion

		#region MoveBeingPlayedHandler
		public void MoveBeingPlayedHandler( MoveInfo move )
		{
			gameHistory[Game.GameMoveNumber] = privs[1];
			privs[0] = privs[1];
		}
		#endregion

		#region MoveBeingMade
		public override MoveEventResponse MoveBeingMade( MoveInfo move, int ply )
		{
			PrivFlags currentPrivs = (PrivFlags) (ply == 1 ? gameHistory[Game.GameMoveNumber] : privs[ply - 1]);
			PrivFlags newPrivs = currentPrivs;

			//	We first check to see if there are any bishop conversion privs left.
			//	This is so that when they are gone we don't waste any more computational 
			//	effort with any further checking.
			if( currentPrivs != PrivFlags.none )
			{
				if( move.FromSquare == p0square0 && move.Player == 0 )
				{
					if( (currentPrivs & PrivFlags.p0sq0must) != PrivFlags.none )
					{
						//	make sure this is a conversion move - otherwise it is illegal
						if( move.ToSquare != Board.NextSquare( PredefinedDirections.N, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.S, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.E, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	not a conversion move, so it is illegal
							return MoveEventResponse.IllegalMove;
						//	since the move is legal, this must be a conversion move so 
						//	remove remove all player 0's conversion flags
						newPrivs = newPrivs & (~PrivFlags.p0all);
					}
					else if( (currentPrivs & PrivFlags.p0sq0can) != PrivFlags.none )
					{
						//	is this a converion move?
						if( move.ToSquare == Board.NextSquare( PredefinedDirections.N, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.S, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.E, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	we are converting, so remove all player 0 privs
							newPrivs = newPrivs & (~PrivFlags.p0all);
						else
						{
							newPrivs = newPrivs & (~PrivFlags.p0sq0can);
							//	we are not converting with this piece, so if the 
							//	other square priv is still available, change it 
							//	from can convert to must convert
							if( (currentPrivs & PrivFlags.p0sq1can) != PrivFlags.none )
								newPrivs = (newPrivs & (~PrivFlags.p0all)) | PrivFlags.p0sq1must;
						}
					}
				}
				else if( move.FromSquare == p0square1 && move.Player == 0 )
				{
					if( (currentPrivs & PrivFlags.p0sq1must) != PrivFlags.none )
					{
						//	make sure this is a conversion move - otherwise it is illegal
						if( move.ToSquare != Board.NextSquare( PredefinedDirections.N, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.S, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.E, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	not a conversion move, so it is illegal
							return MoveEventResponse.IllegalMove;
						//	since the move is legal, this must be a conversion move so 
						//	remove remove all player 0's conversion flags
						newPrivs = newPrivs & (~PrivFlags.p0all);
					}
					else if( (currentPrivs & PrivFlags.p0sq1can) != PrivFlags.none )
					{
						//	is this a converion move?
						if( move.ToSquare == Board.NextSquare( PredefinedDirections.N, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.S, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.E, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	we are converting, so remove all player 0 privs
							newPrivs = newPrivs & (~PrivFlags.p0all);
						else
						{
							newPrivs = newPrivs & (~PrivFlags.p0sq1can);
							//	we are not converting with this piece, so if the 
							//	other square priv is still available, change it 
							//	from can convert to must convert
							if( (currentPrivs & PrivFlags.p0sq0can) != PrivFlags.none )
								newPrivs = (newPrivs & (~PrivFlags.p0all)) | PrivFlags.p0sq0must;
						}
					}
				}
				else if( move.FromSquare == p1square0 && move.Player == 1 )
				{
					if( (currentPrivs & PrivFlags.p1sq0must) != PrivFlags.none )
					{
						//	make sure this is a conversion move - otherwise it is illegal
						if( move.ToSquare != Board.NextSquare( PredefinedDirections.N, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.S, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.E, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	not a conversion move, so it is illegal
							return MoveEventResponse.IllegalMove;
						//	since the move is legal, this must be a conversion move so 
						//	remove remove all player 0's conversion flags
						newPrivs = newPrivs & (~PrivFlags.p1all);
					}
					else if( (currentPrivs & PrivFlags.p1sq0can) != PrivFlags.none )
					{
						//	is this a converion move?
						if( move.ToSquare == Board.NextSquare( PredefinedDirections.N, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.S, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.E, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	we are converting, so remove all player 1 privs
							newPrivs = newPrivs & (~PrivFlags.p1all);
						else
						{
							newPrivs = newPrivs & (~PrivFlags.p1sq0can);
							//	we are not converting with this piece, so if the 
							//	other square priv is still available, change it 
							//	from can convert to must convert
							if( (currentPrivs & PrivFlags.p1sq1can) != PrivFlags.none )
								newPrivs = (newPrivs & (~PrivFlags.p1all)) | PrivFlags.p1sq1must;
						}
					}
				}
				else if( move.FromSquare == p1square1 && move.Player == 1 )
				{
					if( (currentPrivs & PrivFlags.p1sq1must) != PrivFlags.none )
					{
						//	make sure this is a conversion move - otherwise it is illegal
						if( move.ToSquare != Board.NextSquare( PredefinedDirections.N, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.S, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.E, move.FromSquare ) &&
							move.ToSquare != Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	not a conversion move, so it is illegal
							return MoveEventResponse.IllegalMove;
						//	since the move is legal, this must be a conversion move so 
						//	remove remove all player 0's conversion flags
						newPrivs = newPrivs & (~PrivFlags.p1all);
					}
					else if( (currentPrivs & PrivFlags.p1sq1can) != PrivFlags.none )
					{
						//	is this a converion move?
						if( move.ToSquare == Board.NextSquare( PredefinedDirections.N, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.S, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.E, move.FromSquare ) ||
							move.ToSquare == Board.NextSquare( PredefinedDirections.W, move.FromSquare ) )
							//	we are converting, so remove all player 1 privs
							newPrivs = newPrivs & (~PrivFlags.p1all);
						else
						{
							newPrivs = newPrivs & (~PrivFlags.p1sq1can);
							//	we are not converting with this piece, so if the 
							//	other square priv is still available, change it 
							//	from can convert to must convert
							if( (currentPrivs & PrivFlags.p1sq0can) != PrivFlags.none )
								newPrivs = (newPrivs & (~PrivFlags.p1all)) | PrivFlags.p1sq0must;
						}
					}
				}
				if( move.ToSquare == p0square0 && move.Player == 1 )
				{
					//	can the player convert the piece on this square?
					//	if so, he just lost that piece (and that privilege)
					if( (currentPrivs & PrivFlags.p0sq0can) != PrivFlags.none )
					{
						if( (currentPrivs & PrivFlags.p0sq1can) != PrivFlags.none )
							newPrivs = (newPrivs & (~PrivFlags.p0all)) | PrivFlags.p0sq1can;
						else
							newPrivs = newPrivs & (~PrivFlags.p0all);
					}
					//	was the player required to convert the piece on this square?
					else if( (currentPrivs & PrivFlags.p0sq0must) != PrivFlags.none )
						//	strip all the player's privs
						newPrivs = newPrivs & (~PrivFlags.p0all);
				}
				else if( move.ToSquare == p0square1 && move.Player == 1 )
				{
					//	can the player convert the piece on this square?
					//	if so, he just lost that piece (and that privilege)
					if( (currentPrivs & PrivFlags.p0sq1can) != PrivFlags.none )
					{
						if( (currentPrivs & PrivFlags.p0sq0can) != PrivFlags.none )
							newPrivs = (newPrivs & (~PrivFlags.p0all)) | PrivFlags.p0sq0can;
						else
							newPrivs = newPrivs & (~PrivFlags.p0all);
					}
					//	was the player required to convert the piece on this square?
					else if( (currentPrivs & PrivFlags.p0sq1must) != PrivFlags.none )
						//	strip all the player's privs
						newPrivs = newPrivs & (~PrivFlags.p0all);
				}
				else if( move.ToSquare == p1square0 && move.Player == 0 )
				{
					//	can the player convert the piece on this square?
					//	if so, he just lost that piece (and that privilege)
					if( (currentPrivs & PrivFlags.p1sq0can) != PrivFlags.none )
					{
						if( (currentPrivs & PrivFlags.p1sq1can) != PrivFlags.none )
							newPrivs = (newPrivs & (~PrivFlags.p1all)) | PrivFlags.p1sq1can;
						else
							newPrivs = newPrivs & (~PrivFlags.p1all);
					}
					//	was the player required to convert the piece on this square?
					else if( (currentPrivs & PrivFlags.p1sq0must) != PrivFlags.none )
						//	strip all the player's privs
						newPrivs = newPrivs & (~PrivFlags.p1all);
				}
				else if( move.ToSquare == p1square1 && move.Player == 0 )
				{
					//	can the player convert the piece on this square?
					//	if so, he just lost that piece (and that privilege)
					if( (currentPrivs & PrivFlags.p1sq1can) != PrivFlags.none )
					{
						if( (currentPrivs & PrivFlags.p1sq0can) != PrivFlags.none )
							newPrivs = (newPrivs & (~PrivFlags.p1all)) | PrivFlags.p1sq0can;
						else
							newPrivs = newPrivs & (~PrivFlags.p1all);
					}
					//	was the player required to convert the piece on this square?
					else if( (currentPrivs & PrivFlags.p1sq1must) != PrivFlags.none )
						//	strip all the player's privs
						newPrivs = newPrivs & (~PrivFlags.p1all);
				}
			}

			privs[ply] = (int) newPrivs;
			if( ply == 1 )
				gameHistory[Game.GameMoveNumber + 1] = privs[1];
			return MoveEventResponse.MoveOk;
		}
		#endregion

		#region GenerateSpecialMoves
		public override void GenerateSpecialMoves( MoveList list, bool capturesOnly, int ply )
		{
			PrivFlags priv = (PrivFlags) (ply == 1 ? gameHistory[Game.GameMoveNumber] : privs[ply - 1]);
			if( Game.CurrentSide == 0 )
			{
				if( (priv & PrivFlags.p0sq0can) != PrivFlags.none ||
					(priv & PrivFlags.p0sq0must) != PrivFlags.none )
				{
					addMove( list, p0square0, Board.NextSquare( PredefinedDirections.N, p0square0 ), capturesOnly );
					addMove( list, p0square0, Board.NextSquare( PredefinedDirections.S, p0square0 ), capturesOnly );
					addMove( list, p0square0, Board.NextSquare( PredefinedDirections.E, p0square0 ), capturesOnly );
					addMove( list, p0square0, Board.NextSquare( PredefinedDirections.W, p0square0 ), capturesOnly );
				}
				if( (priv & PrivFlags.p0sq1can) != PrivFlags.none ||
					(priv & PrivFlags.p0sq1must) != PrivFlags.none )
				{
					addMove( list, p0square1, Board.NextSquare( PredefinedDirections.N, p0square1 ), capturesOnly );
					addMove( list, p0square1, Board.NextSquare( PredefinedDirections.S, p0square1 ), capturesOnly );
					addMove( list, p0square1, Board.NextSquare( PredefinedDirections.E, p0square1 ), capturesOnly );
					addMove( list, p0square1, Board.NextSquare( PredefinedDirections.W, p0square1 ), capturesOnly );
				}
			}
			else
			{
				if( (priv & PrivFlags.p1sq0can) != PrivFlags.none ||
					(priv & PrivFlags.p1sq0must) != PrivFlags.none )
				{
					addMove( list, p1square0, Board.NextSquare( PredefinedDirections.N, p1square0 ), capturesOnly );
					addMove( list, p1square0, Board.NextSquare( PredefinedDirections.S, p1square0 ), capturesOnly );
					addMove( list, p1square0, Board.NextSquare( PredefinedDirections.E, p1square0 ), capturesOnly );
					addMove( list, p1square0, Board.NextSquare( PredefinedDirections.W, p1square0 ), capturesOnly );
				}
				if( (priv & PrivFlags.p1sq1can) != PrivFlags.none ||
					(priv & PrivFlags.p1sq1must) != PrivFlags.none )
				{
					addMove( list, p1square1, Board.NextSquare( PredefinedDirections.N, p1square1 ), capturesOnly );
					addMove( list, p1square1, Board.NextSquare( PredefinedDirections.S, p1square1 ), capturesOnly );
					addMove( list, p1square1, Board.NextSquare( PredefinedDirections.E, p1square1 ), capturesOnly );
					addMove( list, p1square1, Board.NextSquare( PredefinedDirections.W, p1square1 ), capturesOnly );
				}
			}
		}
		#endregion

		#region AdjustEvaluation
		public override void AdjustEvaluation( int ply, ref int midgameEval, ref int endgameEval )
		{
			//	Compensate for the two-bishops evaluation bonus.  The ColorboundEvaluation will give 
			//	a bonus only if two bishops are on different colors.  But before either bishop has 
			//	converted they are on the same color, however we want to give the bonus anyway.
			//	Otherwise, the computer will have a very, very strong desire to convert one of the 
			//	bishops and will do it almost immedately (which is not smart.)

			PrivFlags priv = (PrivFlags) (ply == 1 ? gameHistory[Game.GameMoveNumber] : privs[ply - 1]);

			//	Fast bail-out if the conversion privs are gone
			if( priv == PrivFlags.none )
				return;

			//	Bonus for White if he still has two bishops and one will convert
			if( ((priv & PrivFlags.p0sq0can) != PrivFlags.none &&
				 (priv & PrivFlags.p0sq1can) != PrivFlags.none) )
			{
				midgameEval += 62;
				endgameEval += 62;
			}
			else if( ((priv & PrivFlags.p0sq0must) != PrivFlags.none && 
				 Board.GetPieceTypeBitboard( 0, Board[p0square0].TypeNumber ).BitCount > 1) ||
				((priv & PrivFlags.p0sq1must) != PrivFlags.none &&
				 Board.GetPieceTypeBitboard( 0, Board[p0square1].TypeNumber ).BitCount > 1))
			{
				midgameEval += 42;
				endgameEval += 42;
			}

			//	Bonus for Black if he still has two bishops and one will convert
			if( ((priv & PrivFlags.p1sq0can) != PrivFlags.none &&
				 (priv & PrivFlags.p1sq1can) != PrivFlags.none) )
			{
				midgameEval -= 62;
				endgameEval -= 62;
			}
			else if( ((priv & PrivFlags.p1sq0must) != PrivFlags.none &&
				 Board.GetPieceTypeBitboard( 1, Board[p1square0].TypeNumber ).BitCount > 1) ||
				((priv & PrivFlags.p1sq1must) != PrivFlags.none &&
				 Board.GetPieceTypeBitboard( 1, Board[p1square1].TypeNumber ).BitCount > 1) )
			{
				midgameEval -= 42;
				endgameEval -= 42;
			}
		}
		#endregion

		#region GetNotesForPieceType
		public override void GetNotesForPieceType( PieceType type, List<string> notes )
		{
			if( Board[p0square0] != null && Board[p0square0].PieceType == type )
				notes.Add( "bishop conversion rule" );
		}
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region addMove
		protected void addMove( MoveList list, int fromSquare, int toSquare, bool capturesOnly )
		{
			if( toSquare >= 0 && toSquare < Board.NumSquares )
			{
				Piece pieceOnDestinationSquare = Board[toSquare];
				if( pieceOnDestinationSquare == null )
				{
					if( !capturesOnly )
						list.AddMove( fromSquare, toSquare );
				}
				else if( pieceOnDestinationSquare.Player != Game.CurrentSide )
					list.AddCapture( fromSquare, toSquare );
			}
		}
		#endregion


		// *** PROTECTED DATA MEMBERS *** //

		protected int[] privs;
		protected int[] gameHistory;
		protected string[] bishopSquares;
		protected int p0square0;
		protected int p0square1;
		protected int p1square0;
		protected int p1square1;
		protected char file0;
		protected char file1;
		protected int hashKeyIndex;
		protected string allPrivString;
	}
}
