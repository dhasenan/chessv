
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
  [Game("Warochess", typeof(Geometry.Rectangular), 9, 9,
      InventedBy = "Eric Warolus",
      Invented = "2010",
      Tags = "Chess Variant",
      GameDescription1 = "Played on a 9x9 board, adding a queen with an extra pawn in front",
      GameDescription2 = "Totally symmetrical, all standard chess rules apply but without castling")]
  [Appearance(ColorScheme = "Luna Decorabat")]
  public class Warochess : Abstract.Generic9x9
  {
    // *** CONSTRUCTION *** //

    public Warochess() :
      base
        ( /* symmetry = */ new RotationalSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "rbnqkqbnr/ppppppppp/9/9/9/9/9/PPPPPPPPP/RNBQKQNBR";
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.Value = "None";
      PromotionRule.Value = "Standard";
      PromotionTypes = "QRNB";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();
    }
    #endregion
  }
}
