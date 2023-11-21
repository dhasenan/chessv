
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

namespace ChessV.Games.Rules.Berolina
{
  public class BerolinaEnPassantRule : Rule
  {
    // *** PROPERTIES *** //

    public PieceType PawnType { get; private set; }


    // *** CONSTRUCTION *** //

    public BerolinaEnPassantRule(PieceType pawnType)
    {
      PawnType = pawnType;
    }

    public override void Initialize(Game game)
    {
      base.Initialize(game);
      hashKeyIndex = game.HashKeys.TakeKeys(game.Board.NumSquares);
      epCaptureSquares = new int[Game.MAX_PLY];
      epMoverSquares = new int[Game.MAX_PLY];
      for (int x = 0; x < Game.MAX_PLY; x++)
        epCaptureSquares[x] = epMoverSquares[x] = 0;
      gameHistoryCaptureSquares = new int[Game.MAX_GAME_LENGTH];
      gameHistoryCaptureSquares[0] = 0;
      gameHistoryMoverSquares = new int[Game.MAX_GAME_LENGTH];
      gameHistoryMoverSquares[0] = 0;

      //	find the move directions
      captureDirections = new int[2];
      MoveCapability[] moveCapabilities;
      int nMoveCapabilities = PawnType.GetMoveCapabilities(out moveCapabilities);
      for (int nMoveCap = 0; nMoveCap < nMoveCapabilities; nMoveCap++)
      {
        MoveCapability move = moveCapabilities[nMoveCap];
        if (move.CanCapture)
        {
          captureDirections[0] = Game.PlayerDirection(0, move.NDirection);
          captureDirections[1] = Game.PlayerDirection(1, move.NDirection);
        }
      }

      //	Hook up MoveBeingPlayed event handler
      game.MoveBeingPlayed += MoveBeingPlayedHandler;
    }

    public override void ClearGameState()
    {
      for (int x = 0; x < Game.MAX_PLY; x++)
        epCaptureSquares[x] = epMoverSquares[x] = 0;
      for (int x = 0; x < Game.MAX_GAME_LENGTH; x++)
        gameHistoryCaptureSquares[x] = gameHistoryMoverSquares[x] = 0;
    }

    public override ulong GetPositionHashCode(int ply)
    {
      int epCaptureSquare = ply == 1 ? gameHistoryCaptureSquares[Game.GameMoveNumber] : epCaptureSquares[ply - 1];
      if (epCaptureSquare != 0)
      {
        int epMoverSquare = ply == 1 ? gameHistoryMoverSquares[Game.GameMoveNumber] : epMoverSquares[ply - 1];
        return HashKeys.Keys[hashKeyIndex + epCaptureSquare] ^ HashKeys.Keys[hashKeyIndex + epMoverSquare];
      }
      else
        return 0;
    }

    public override void PositionLoaded(FEN fen)
    {
      string ep = fen["en-passant"];
      if (ep == "-")
        epCaptureSquares[0] = epMoverSquares[0] = -1;
      else
      {
        //	We need to split the notation into the two squares.
        //	We will not assume there are two characters for each since 
        //	one could put Berolina Pawns on a board with 10 or more ranks.
        int splitpoint;
        for (splitpoint = 1; ep[splitpoint] >= '0' && ep[splitpoint] <= '9'; splitpoint++) ;
        epCaptureSquares[0] = Game.NotationToSquare(ep.Substring(0, splitpoint));
        epMoverSquares[0] = Game.NotationToSquare(ep.Substring(splitpoint));
      }
    }

    public override void SavePositionToFEN(FEN fen)
    {
      int epCaptureSquare = (Game.GameMoveNumber == 0 ? epCaptureSquares[0] : gameHistoryCaptureSquares[Game.GameMoveNumber - 1]);
      int epMoverSquare = (Game.GameMoveNumber == 0 ? epMoverSquares[0] : gameHistoryMoverSquares[Game.GameMoveNumber - 1]);
      if (epCaptureSquare > 0)
        fen["en-passant"] = Game.GetSquareNotation(epCaptureSquare) + Game.GetSquareNotation(epMoverSquare);
      else
        fen["en-passant"] = "-";
    }

    public override void SetDefaultsInFEN(FEN fen)
    {
      if (fen["en-passant"] == "#default")
        fen["en-passant"] = "-";
    }

    public void MoveBeingPlayedHandler(MoveInfo move)
    {
      gameHistoryCaptureSquares[Game.GameMoveNumber] = epCaptureSquares[1];
      gameHistoryMoverSquares[Game.GameMoveNumber] = epMoverSquares[1];
    }

    public override MoveEventResponse MoveBeingMade(MoveInfo move, int ply)
    {
      epCaptureSquares[ply] = epMoverSquares[ply] = 0;
      if (ply == 1)
        gameHistoryCaptureSquares[Game.GameMoveNumber] = gameHistoryMoverSquares[Game.GameMoveNumber] = 0;
      //	if the current side to move is also the next side to move then 
      //	we return here - en passant is not possible so we would not 
      //	want to set the ep square
      if (Game.CurrentSide == Game.NextSide)
        return MoveEventResponse.NotHandled;
      if (move.PieceMoved != null && move.PieceMoved.PieceType == PawnType)
      {
        if (Board.GetDistance(move.FromSquare, move.ToSquare) > 1)
        {
          //	check to see if there are any pawn attackers - even if a pawn makes a 
          //	multi-step move, we don't set the e.p. square unless there is a pawn 
          //	that can actually take it.  this is an important consideration because of 
          //	the hashing/matching of board positions.  we don't want an identical board 
          //	position to be considered different just because a pawn made a multi-step 
          //	move - the fact that a multi-step move was made is only significant because 
          //	of the availability of an e.p. capture
          int moveDirection = Board.DirectionLookup(move.FromSquare, move.ToSquare);
          int epCaptureSquare = Board.NextSquare(moveDirection, move.FromSquare);

          //	we loop here to accomodate large-board games where a pawn can make a move 
          //	of more than two steps and still be captured e.p. on any square passed over
          while (Board[epCaptureSquare] == null)
          {
            int nextSquare = Board.NextSquare(captureDirections[move.Player], epCaptureSquare);
            if (nextSquare >= 0)
            {
              Piece piece = Board[nextSquare];
              if (piece != null && piece.PieceType == PawnType && piece.Player != move.Player)
              {
                //	en passant capture is possible
                epCaptureSquares[ply] = epCaptureSquare;
                epMoverSquares[ply] = move.ToSquare;
                if (ply == 1)
                {
                  gameHistoryCaptureSquares[Game.GameMoveNumber] = epCaptureSquare;
                  gameHistoryMoverSquares[Game.GameMoveNumber] = move.ToSquare;
                }
                return MoveEventResponse.MoveOk;
              }
            }
            epCaptureSquare = Board.NextSquare(moveDirection, epCaptureSquare);
          }
        }
      }
      return MoveEventResponse.NotHandled;
    }

    public override void GenerateSpecialMoves(MoveList list, bool capturesOnly, int ply)
    {
      int epCaptureSquare = ply == 1
        ? (Game.GameMoveNumber == 0 ? epCaptureSquares[0] : gameHistoryCaptureSquares[Game.GameMoveNumber - 1])
        : epCaptureSquares[ply - 1];
      if (epCaptureSquare > 0)
      {
        int targetSquare = ply == 1
          ? (Game.GameMoveNumber == 0 ? epMoverSquares[0] : gameHistoryMoverSquares[Game.GameMoveNumber - 1])
          : epMoverSquares[ply - 1];
        int victimPawnMoveDirection = Board.DirectionLookup(epCaptureSquare, targetSquare);
        Piece targetPawn = Board[targetSquare];

        while (epCaptureSquare != targetSquare)
        {
          int s = Board.NextSquare(captureDirections[targetPawn.Player], epCaptureSquare);
          Piece attacker = Board[Board.NextSquare(captureDirections[targetPawn.Player], epCaptureSquare)];
          if (attacker.PieceType == PawnType)
          {
            //	this is a valid en passant attack
            list.BeginMoveAdd(MoveType.EnPassant, attacker.Square, epCaptureSquare);
            list.AddPickup(attacker.Square);
            list.AddPickup(targetPawn.Square);
            list.AddDrop(attacker, epCaptureSquare, null);
            list.EndMoveAdd(3000);
          }
          epCaptureSquare = Board.NextSquare(victimPawnMoveDirection, epCaptureSquare);
        }
      }
    }

    public override void GetNotesForPieceType(PieceType type, List<string> notes)
    {
      if (type == PawnType)
        notes.Add("en passant");
    }


    // *** PROTECTED DATA MEMBERS *** //

    protected int[] epCaptureSquares;
    protected int[] epMoverSquares;
    protected int[] gameHistoryCaptureSquares;
    protected int[] gameHistoryMoverSquares;
    protected int[] captureDirections;
    protected int hashKeyIndex;
  }
}
