
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

namespace ChessV.Games.Rules.MultiMove
{
  public class DuplexChessMoveCompletionRule : MoveCompletionRule
  {
    // *** CONSTRUCTION *** //

    public DuplexChessMoveCompletionRule(PieceType kingType, PieceType pawnType, Piece[] generals)
    {
      this.kingType = kingType;
      this.pawnType = pawnType;
      this.generals = generals;
      currentState = 1;
    }

    public override void Initialize(Game game)
    {
      base.Initialize(game);
      hashKeyIndex = game.HashKeys.TakeKeys(stateNotations.Length);
      pieceLastMoved = new Piece[Game.MAX_GAME_LENGTH + Game.MAX_PLY];
      searchStateHistory = new int[Game.MAX_GAME_LENGTH + Game.MAX_PLY];
      searchStateHistoryIndex = 0;
    }


    // *** OVERRIDES *** //

    public override int TurnNumber
    {
      get { return turnNumber; }
    }

    public override void PositionLoaded(FEN fen)
    {
      base.PositionLoaded(fen);
      //	read the turn number from the FEN
      if (!Int32.TryParse(fen["turn number"], out turnNumber))
        throw new Exception("FEN parse error - invalid turn number specified: '" + fen["turn number"] + "'");
      //	read the current player from the FEN
      string currentPlayerNotation = fen["current player"];
      bool notationFound = false;
      for (int x = 0; x < stateNotations.Length; x++)
        if (currentPlayerNotation == stateNotations[x])
        {
          currentState = x;
          Game.CurrentSide = sidePerState[currentState];
          notationFound = true;
        }
      searchStateHistory[searchStateHistoryIndex++] = currentState;
      if (!notationFound)
        throw new Exception("FEN parse error - invalid current player specified: '" + currentPlayerNotation + "'");
    }

    public override void SavePositionToFEN(FEN fen)
    {
      fen["turn number"] = turnNumber.ToString();
      fen["current player"] = stateNotations[currentState];
    }

    public override ulong GetPositionHashCode(int ply)
    {
      return HashKeys.Keys[hashKeyIndex + currentState];
    }

    public override int GetNextSide()
    {
      if (currentState >= 4)
        return (7 - currentState) / 2;
      return ((currentState + 1) % 4) / 2;
    }

    public override void CompleteMove(MoveInfo move, int ply)
    {
      pieceLastMoved[searchStateHistoryIndex] = move.PieceMoved;
      searchStateHistory[searchStateHistoryIndex++] = currentState;
      if (currentState >= 4)
        currentState = currentState == 7 ? 1 : currentState + 1;
      else
        currentState = (currentState + 1) % 4;
      if (currentState == 0 || currentState == 7)
        turnNumber++;
      Game.CurrentSide = sidePerState[currentState];
    }

    public override void UndoingMove()
    {
      currentState = searchStateHistory[--searchStateHistoryIndex];
      if (currentState == 3 || currentState == 6)
        turnNumber--;
      Game.CurrentSide = sidePerState[currentState];
    }

    public override MoveEventResponse MoveBeingGenerated(MoveList moves, int from, int to, MoveType type)
    {
      if (currentState >= 4)
      {
        //	Handle the automatic drops of the silver and gold generals at the beginning of the game
        if (Board[from].PieceType != pawnType || Game.StartingPieceSquares[Board[from].Player, from] == 0)
          return MoveEventResponse.IllegalMove;
        //PieceType dropType = currentState < 6 ? silverGeneralType : goldGeneralType;
        moves.BeginMoveAdd(MoveType.StandardMove, from, to);
        Piece pawn = moves.AddPickup(from);
        moves.AddDrop(pawn, to);
        moves.AddDrop(generals[currentState - 4], from);
        moves.EndMoveAdd(pawnType.GetMidgamePST(Board.PlayerSquare(Board[from].Player, to)));
        return MoveEventResponse.Handled;
      }
      else
      {
        //	Don't allow to move the same piece twice in a row unless it is a king
        if (Board[from] == pieceLastMoved[searchStateHistoryIndex - 1] && Board[from].PieceType != kingType)
          return MoveEventResponse.IllegalMove;
      }
      return MoveEventResponse.NotHandled;
    }


    // *** PROTECTED DATA MEMBERS *** //

    protected PieceType kingType;
    protected PieceType pawnType;
    protected Piece[] generals;
    protected int currentState;
    protected int hashKeyIndex;
    protected Piece[] pieceLastMoved;
    protected int[] searchStateHistory;
    protected int searchStateHistoryIndex;
    protected int turnNumber;

    protected string[] stateNotations = new string[] { "w2", "w", "b2", "b", "ws", "bs", "bg", "wg" };
    protected int[] sidePerState = new int[] { 0, 0, 1, 1, 0, 1, 1, 0 };
  }
}
