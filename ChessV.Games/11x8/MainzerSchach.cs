
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
  [Game("Mainzer Schach", typeof(Geometry.Rectangular), 11, 8,
      Invented = "2004",
      InventedBy = "Jörg Knappen",
      Tags = "Chess Variant")]
  public class MainzerSchach : Abstract.Generic11x8
  {
    // *** PIECE TYPES *** //

    public PieceType Archbishop;
    public PieceType Chancellor;
    public PieceType Amazon;


    // *** CONSTRUCTION *** //

    public MainzerSchach() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "rjbbqkmnnjr/ppppppppppp/11/11/11/11/PPPPPPPPPPP/RJBBQKMNNJR";
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.Value = "Long";
      PromotionRule.Value = "Standard";
      PromotionTypes = "AQMJRNB";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();
      AddPieceType(Archbishop = new Archbishop("Janus", "J", 900, 900));
      AddPieceType(Chancellor = new Chancellor("Marshall", "M", 950, 950));
      AddPieceType(Amazon = new Amazon("Amazon", "A", 1500, 1600));
    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
    {
      base.AddEvaluations();

      if (Chancellor != null && Chancellor.Enabled)
        RookTypeEval.AddRookOn7thBonus(Chancellor, King, 2, 8);
      if (Amazon != null && Amazon.Enabled)
        RookTypeEval.AddRookOn7thBonus(Amazon, King);
    }
    #endregion
  }
}
