﻿
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

namespace ChessV.Games
{
  public class CombatantAttribute : PieceTypePropertyAttribute
  {
    public override void AdjustMovement(PieceType pieceType)
    {
      MoveCapability[] moves;
      int nMoves = pieceType.GetMoveCapabilities(out moves);
      for (int x = 0; x < nMoves; x++)
        moves[x].MustCapture = true;
    }
  }
}
