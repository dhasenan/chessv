
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

namespace ChessV.Games
{
  [Game("Ministers Chess", typeof(Geometry.Rectangular), 9, 9,
      XBoardName = "ministers",
      InventedBy = "Michael Corinthios",
      Invented = "1975",
      Tags = "Chess Variant")]
  public class MinistersChess : Abstract.Generic9x9
  {
    // *** CONSTRUCTION *** //

    public MinistersChess() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "rnbmkmbnr/ppppppppp/9/9/9/9/9/PPPPPPPPP/RNBMKMBNR";
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.Value = "Long";
      PromotionRule.Value = "Standard";
      PromotionTypes = "MRNB";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();

      //	Rename the Queen to the Minister
      Queen.Name = "Minister";
      Queen.SetNotation("M");
    }
    #endregion
  }
}
