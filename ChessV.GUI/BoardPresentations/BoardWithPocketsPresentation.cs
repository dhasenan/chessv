﻿
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

using ChessV.Boards;
using ChessV.GUI.Attributes;
using System;
using System.Drawing;

namespace ChessV.GUI.BoardPresentations
{
  [PresentsBoard(typeof(BoardWithPockets))]
  [PresentsBoard(typeof(BoardWithCards))]
  public class BoardWithPocketsPresentation : BoardPresentation
  {
    public BoardWithPocketsPresentation(Board board, Theme theme, bool smallPreview = false) :
      base(board, theme, smallPreview)
    { }

    protected override LocationToPresentationMapping CreateMapping(int borderSize, int squareSize)
    { return new BoardWithPocketsLocationToPresentationMapping(Board, Theme, borderSize, squareSize); }
  }

  public class BoardWithPocketsLocationToPresentationMapping : LocationToPresentationMapping
  {
    public BoardWithPocketsLocationToPresentationMapping(Board board, Theme theme, int borderSize, int squareSize) :
      base(board, theme, borderSize, squareSize)
    {
      TotalWidth += borderSize + squareSize;
    }

    public override Rectangle MapLocation(Location location, bool rotateBoard = false)
    {
      if (location.File <= -1)
      {
        if ((location.Rank == 0 && !rotateBoard) || (location.Rank == 1 && rotateBoard))
          return new Rectangle(BorderSize * 2 + Board.NumFiles * SquareSize,
            BorderSize + (Board.NumRanks + location.File) * SquareSize, SquareSize, SquareSize);
        else if ((location.Rank == 1 && !rotateBoard) || (location.Rank == 0 && rotateBoard))
          return new Rectangle(BorderSize * 2 + Board.NumFiles * SquareSize,
            BorderSize - (1 + location.File) * SquareSize, SquareSize, SquareSize);
        else
          throw new Exception("not implemented");
      }
      if (rotateBoard)
        return new Rectangle(BorderSize + (Board.NumFiles - location.File - 1) * SquareSize,
          BorderSize + location.Rank * SquareSize,
          SquareSize, SquareSize);
      else
        return new Rectangle(BorderSize + location.File * SquareSize,
          BorderSize + (Board.NumRanks - location.Rank - 1) * SquareSize,
          SquareSize, SquareSize);
    }
  }
}
