
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

namespace ChessV.Games.Rules.YangQi
{
  public class YangQiKingSwapRule : Rule
  {
    public YangQiKingSwapRule(PieceType kingType, PieceType pawnType)
    {
      this.kingType = kingType;
      this.pawnType = pawnType;
    }

    public override void GenerateSpecialMoves(MoveList list, bool capturesOnly, int ply)
    {
      if (!capturesOnly)
      {
        int king = Board.GetPieceTypeBitboard(Game.CurrentSide, kingType.TypeNumber).LSB;
        if (!IsSquareAttacked(king, Game.CurrentSide ^ 1))
        {
          for (int dir = 0; dir < 8; dir++)
          {
            int nextSquare = Board.NextSquare(dir, king);
            if (nextSquare >= 0 && Board[nextSquare] != null &&
              Board[nextSquare].Player == Game.CurrentSide &&
              Board[nextSquare].PieceType != pawnType)
            {
              //	The king can swap with this piece.  We won't bother 
              //	to see if the target square is attacked because the 
              //	CheckmateRule is going to verify that anyway and we 
              //	don't want to perform that operation twice.
              list.BeginMoveAdd(MoveType.Swap, king, nextSquare);
              Piece kingpiece = list.AddPickup(king);
              Piece otherpiece = list.AddPickup(nextSquare);
              list.AddDrop(kingpiece, nextSquare);
              list.AddDrop(otherpiece, king);
              list.EndMoveAdd(0);
            }
          }
        }
      }
    }

    public override void GetNotesForPieceType(PieceType type, List<string> notes)
    {
      if (type == kingType)
        notes.Add("swap ability");
    }

    protected PieceType kingType;
    protected PieceType pawnType;
  }
}
