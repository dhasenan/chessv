
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

namespace ChessV.Boards
{
  //**********************************************************************
  //
  //                        CylindricalBoard
  //
  //    This class derives from the Board class and implements a board 
  //    where the sides are connected (a piece may move off the left side 
  //    and onto the right.  This is used in Cylindrical Chess.

  public class CylindricalBoard : Board
  {
    public CylindricalBoard(int nFiles, int nRanks) :
      base(nFiles, nRanks)
    {
      //	We need to disable simple move generation because there are 
      //	multiple paths to the same square.  This will mess up SEE.
      DisableSimpleMoveGeneration = true;
    }

    public override void Initialize()
    {
      base.Initialize();

      //	Change the notion of "small center" and "large center".
      //	What we consider the center is pretty different.

      for (int sq1 = 0; sq1 < NumRanks * NumFiles; sq1++)
      {
        int file1 = GetFile(sq1);
        int rank1 = GetRank(sq1);
        int rankFromEdge = rank1 < NumRanks - rank1 - 1 ? rank1 : NumRanks - rank1 - 1;
        int[] maxDistanceFromEdgeByNumRanks = { 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        int maxDistanceFromEdge = maxDistanceFromEdgeByNumRanks[NumRanks];
        int[] distanceFromEdge = new int[NumSquares];
        distanceFromEdge[sq1] = rankFromEdge;
        if (NumRanks >= 9)
        {
          pstInSmallCenter[sq1] = distanceFromEdge[sq1] == maxDistanceFromEdge ? 1 : 0;
          pstInLargeCenter[sq1] = distanceFromEdge[sq1] >= maxDistanceFromEdge - 1 ? 1 : 0;
        }
        else
        {
          pstInSmallCenter[sq1] = 0;
          pstInLargeCenter[sq1] = distanceFromEdge[sq1] >= maxDistanceFromEdge ? 1 : 0;
        }

        //	The matrix of distances between squares needs to change also

        int sq2;
        for (sq2 = 0; sq2 < NumRanks * NumFiles; sq2++)
        {
          int file2 = GetFile(sq2);
          int rank2 = GetRank(sq2);
          int fileOffset = Math.Min(file2 > file1 ? file2 - file1 : file1 - file2,
            file2 > file1 ? file1 + NumFiles - file2 : file2 + NumFiles - file1);
          int rankOffset = rank2 > rank1 ? rank2 - rank1 : rank1 - rank2;
          distances[sq1, sq2] = fileOffset > rankOffset ? fileOffset : rankOffset;
        }
      }
    }

    protected override void buildNextStepMatrix()
    {
      //	This is where the important change happens.  We just 
      //	change the initialization logic of this lookup matrix 
      //	so the edges are connected.   All movement in the 
      //	actual game should lookup from this matrix.
      //	get "directions" used from the Game class
      Direction[] directions;
      int nDirections = Game.GetDirections(out directions);
      NumberOfDirections = nDirections;

      //	build "nextStep" matrix for next square number in any direction from each square
      nextStep = new int[nDirections, NumSquaresExtended];
      for (int x = 0; x < nDirections; x++)
      {
        for (int y = 0; y < NumSquaresExtended; y++)
        {
          if (y < NumSquares)
          {
            Location location = SquareToLocation(y);
            Direction direction = directions[x];
            Location nextLocation = new Location(location.Rank + direction.RankOffset,
              (location.File + direction.FileOffset + NumFiles) % NumFiles);
            if (nextLocation.Rank >= 0 && nextLocation.Rank < NumRanks)
              nextStep[x, y] = LocationToSquare(nextLocation);
            else
              nextStep[x, y] = -1;
          }
          else
            nextStep[x, y] = -1;
        }
      }
    }
  }
}
