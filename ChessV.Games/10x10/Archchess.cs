
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
  [Game("Archchess", typeof(Geometry.Rectangular), 10, 10,
      Invented = "1683",
      InventedBy = "Francesco Piacenza",
      Tags = "Chess Variant,Historic",
      GameDescription1 = "A large historic variant dating from 17th century Italy",
      GameDescription2 = "Created by Francesco Piacenza in 1683")]
  [Appearance(ColorScheme = "Grayscale")]
  public class Archchess : Abstract.Generic10x10
  {
    // *** PIECE TYPES *** //

    public PieceType Decurion;
    public PieceType Centurion;


    // *** GAME VARIABLES *** //

    [GameVariable] public bool KingsLeap { get; set; }


    // *** CONSTRUCTION *** //

    public Archchess() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      FENFormat = "{array} {current player} {castling} {kings-leap} {en-passant} {half-move clock} {turn number}";
      FENStart = "#{Array} w #default #default - 0 1";
      Array = "rnbckqdbnr/pppppppppp/10/10/10/10/10/10/PPPPPPPPPP/RNBCKQDBNR";
      KingsLeap = true;
      EnPassant = true;
      PromotionRule.Value = "Standard";
      PromotionTypes = "Q";
      PawnMultipleMove.Value = "Double";
      Castling.Value = "Standard";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();
      AddPieceType(Decurion = new Ferz("Decurion", "D", 140, 140));
      AddPieceType(Centurion = new Squirrel("Centurion", "C", 550, 550, "Guard"));
    }
    #endregion

    #region AddRules
    public override void AddRules()
    {
      base.AddRules();

      // *** KINGS LEAP *** //
      if (KingsLeap)
        AddRule(new Rules.KingsLeapRule(40, 49));
    }
    #endregion
  }
}
