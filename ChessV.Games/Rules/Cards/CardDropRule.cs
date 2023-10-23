
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
using System.Linq;

namespace ChessV.Games.Rules.Cards
{
	public class CardDropRule: Rule
  {
    protected int handSize;
    protected int[] pocketSquares;

		protected static Game game;

		public CardDropRule()
		{
		}

		public override void Initialize( Game game )
		{
			base.Initialize( game );
      handSize = game.Board.NumSquares - game.Board.NumRanks * game.Board.NumFiles / 2;
      pocketSquares = new int[handSize * 2];
      for ( int player = 0; player < game.NumPlayers; player++ )
      {
				for (int card = 0; card < handSize; card++)
				{
					pocketSquares[player] = Board.LocationToSquare(new Location(player, -card));
				}
      }
		}

		public override void PostInitialize()
		{
			
		}

		public override void PositionLoaded( FEN fen )
		{
			// TODO(chesslogic): load pockets from apmwcore, ignore FEN

			/*
			int[] files = { 1, 1 };
			foreach( char c in fen["pieces in hand"].ToCharArray() )
			{
				if( c != '-' && c != '@' )
				{
					PieceType type = Game.GetTypeByNotation( c.ToString() );
					int player = Char.IsUpper( c ) ? 0 : 1;
					Location loc = new Location( player, -files[player]++ ); // TODO(chesslogic): order of operations?
          Piece piece = new Piece( Game, player, type, loc );
					Board.Game.AddPiece( piece );
				}
			}
			*/
		}

    public override MoveEventResponse MoveBeingUnmade(MoveInfo info, int ply)
    {
      Player movingPlayer = Game.CurrentPlayer;
      if (movingPlayer != null)
      {
        if (info.FromSquare >= 0)
          return MoveEventResponse.NotHandled;
        if (movingPlayer.Side != info.Player)
          movingPlayer = movingPlayer.Opponent;
				movingPlayer.GemsSpent += info.PieceMoved.MidgameValue / 100;
				return MoveEventResponse.Handled;
			}
			// TODO(chesslogic): try NotHandled
			return MoveEventResponse.Handled;
    }

    public override MoveEventResponse MoveBeingMade(MoveInfo info, int something)
    {
      Player movingPlayer = Game.CurrentPlayer;
      if (movingPlayer != null) {
				if (info.FromSquare >= 0)
          return MoveEventResponse.NotHandled;
        if (movingPlayer.Side != info.Player)
					movingPlayer = movingPlayer.Opponent;
        if (movingPlayer.Gems < info.PieceMoved.MidgameValue / 100)
          return MoveEventResponse.IllegalMove;
        movingPlayer.GemsSpent += info.PieceMoved.MidgameValue / 100;
        return MoveEventResponse.Handled;
      }
			return MoveEventResponse.Handled;
    }

		public override void GenerateSpecialMoves( MoveList list, bool capturesOnly, int ply )
		{
			if( !capturesOnly )
			{
				int pocketSquare = pocketSquares[Game.CurrentSide];
				Piece pieceInPocket = Board[pocketSquare];
				if( pieceInPocket != null )
        {
          Player movingPlayer = Game.CurrentPlayer;
          if (Game.CurrentPlayer.Side != list.CurrentMove.Player)
          {
            movingPlayer = movingPlayer.Opponent;
          }
					if (movingPlayer.Gems < pieceInPocket.MidgameValue / 100)
					{
						return;
					}

          // TODO(chesslogic): square bounding based on apmw Pocket Forwardness
          for ( int square = 0; square < Board.NumSquares; square++ )
					{
						if( Board[square] == null )
						{
							list.BeginMoveAdd( MoveType.Drop, pocketSquare, square );
							Piece piece = list.AddPickup( pocketSquare );
							list.AddDrop( piece, square, pieceInPocket.PieceType );
              list.EndMoveAdd( piece.PieceType.GetMidgamePST( square ) - 10 );
						}
					}
				}
			}
		}
	}
}
