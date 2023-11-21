
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

namespace ChessV.Games
{
  [Game("Duplex Chess", typeof(Geometry.Rectangular), 8, 8,
      Invented = "2018",
      InventedBy = "Greg Strong",
      GameDescription1 = "A modest double-move variant with only short range pieces",
      GameDescription2 = "and three different victory conditions",
      Tags = "Chess Variant,Multi-Move")]
  public class DuplexChess : Abstract.Generic8x8
  {
    // *** PIECE TYPES *** //

    public PieceType Tower;
    public PieceType Elephant;
    public PieceType GoldGeneral;
    public PieceType SilverGeneral;
    public PieceType JumpingGeneral;


    // *** CONSTRUCTION *** //

    public DuplexChess() :
      base
         ( /* symmetry = */ new MirrorSymmetry())
    {
    }

    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      FENStart = "#{Array} ws #default #default 0 1";
      Array = "tnejkent/pppppppp/8/8/8/8/PPPPPPPP/TNEJKENT";
      PromotionRule.Value = "Custom";
      PromotionTypes = "S";
      PawnDoubleMove = false;
      EnPassant = false;
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddPieceType(Tower = new Tower("Tower", "T", 325, 325));
      AddPieceType(Knight = new Knight("Knight", "N", 325, 325));
      AddPieceType(Elephant = new ElephantFerz("Elephant", "E", 275, 275));
      AddPieceType(GoldGeneral = new GoldGeneral("Gold General", "G", 225, 225));
      AddPieceType(SilverGeneral = new SilverGeneral("Silver General", "S", 200, 200));
      AddPieceType(JumpingGeneral = new JumpingGeneral("Jumping General", "J", 700, 700));
    }
    #endregion

    #region AddRules
    public override void AddRules()
    {
      base.AddRules();


      // *** VICTORY CONDITIONS *** //

      //	Remove the Checkmate rule
      RemoveRule(typeof(Rules.CheckmateRule));

      //	Player can win by capturing king or last pawn.
      //	This is handled by the ExtinctionRule
      AddRule(new Rules.Extinction.ExtinctionRule("KP"));

      //	Game is won if a King reaches the last rank
      AddRule(new Rules.LocationVictoryConditionRule(King, loc => loc.Rank == 7));


      //	Create the four generals that begin "in hand"
      Piece[] generals = new Piece[4];
      AddPiece(generals[0] = new Piece(this, 0, SilverGeneral, -1));
      AddPiece(generals[1] = new Piece(this, 1, SilverGeneral, -1));
      AddPiece(generals[2] = new Piece(this, 1, GoldGeneral, -1));
      AddPiece(generals[3] = new Piece(this, 0, GoldGeneral, -1));

      //	Add KalavianChessMoveCompletionRule (which will automatically 
      //	replace MoveCompletionDefaultRule since there can be 
      //	only one MoveCompletionRule.)  This handles the double-moves with 
      //	all appropriate restrictions for this game.
      AddRule(new Rules.MultiMove.DuplexChessMoveCompletionRule(King, Pawn, generals));


      //	Handle Kalavian Chess's custom promotion
      if (PromotionRule.Value == "Custom")
      {
        //	Automatic promotion upon reaching the 6th rank to any type from the 
        //	promotion types (which, by default, is only the Silver General)
        List<PieceType> availablePromotionTypes = ParseTypeListFromString(PromotionTypes);
        AddBasicPromotionRule(Pawn, availablePromotionTypes, loc => loc.Rank == 5);
      }

      //	Handle Silver General option promotion to Gold General
      Rules.ComplexPromotionRule silverGeneralPromotionRule = new Rules.ComplexPromotionRule();
      silverGeneralPromotionRule.AddPromotionCapability(SilverGeneral, new List<PieceType> { GoldGeneral }, null,
        (fromLoc, toLoc) => fromLoc.Rank == 7 || toLoc.Rank == 7 ? Rules.PromotionOption.CanPromote : Rules.PromotionOption.CannotPromote);
      AddRule(silverGeneralPromotionRule);
    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
    {
      base.AddEvaluations();

      OutpostEval.AddOutpostBonus(Elephant);
      OutpostEval.AddOutpostBonus(Tower, 10, 2, 5, 5);
    }
    #endregion
  }
}
