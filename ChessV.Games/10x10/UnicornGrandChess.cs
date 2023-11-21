
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
  [Game("Unicorn Grand Chess", typeof(Geometry.Rectangular), 10, 10,
      Invented = "2006",
      InventedBy = "David Paulowich;Greg Strong",
      Tags = "Chess Variant")]
  [Appearance(ColorScheme = "Lesotho", NumberOfSquareColors = 3)]
  public class UnicornGrandChess : GrandChess
  {
    // *** PIECE TYPES *** //

    public PieceType Unicorn;
    public PieceType Lion;


    // *** CONSTRUCTION *** //

    public UnicornGrandChess()
    {
    }

    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "r8r/cnlbukblnq/pppppppppp/10/10/10/10/PPPPPPPPPP/CNLBUKBLNQ/R8R";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      Cardinal.Enabled = false;
      Marshall.Name = "Chancellor";
      Marshall.SetNotation("C");
      AddPieceType(Unicorn = new Unicorn("Unicorn", "U", 1050, 1125));
      AddPieceType(Lion = new Lion("Lion", "L", 450, 450));
    }
    #endregion

  }
}
