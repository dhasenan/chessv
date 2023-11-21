
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

using ChessV.Evaluations;

namespace ChessV.Games
{
  //**********************************************************************
  //
  //                           GrandChess
  //
  //    This class implements Chritian Freeling's Grand Chess, which 
  //    has the two missing compounds from Chess (similar to Capablanca 
  //    Chess) but on a 10x10 board.  The game is notable for the pawn 
  //    promotion rule (optional promotion at the 8th and 9th rank, 
  //    mandatory at the 10th, but promotion only to a captured piece 
  //    with which it is replaced,) and the lack of castling (argued 
  //    unnecessary because the rooks are already connected.)

  [Game("Grand Chess", typeof(Geometry.Rectangular), 10, 10,
      XBoardName = "grand",
      Invented = "1984",
      InventedBy = "Christian Freeling",
      Tags = "Chess Variant,Popular",
      GameDescription1 = "Christian Freeling's popular 10 x 10 chess variant",
      GameDescription2 = "with the missing compound pieces added as in Capablanca Chess")]
  [Appearance(ColorScheme = "Marmoor Quadraut")]
  public class GrandChess : Abstract.Generic10x10
  {
    // *** PIECE TYPES *** //

    public PieceType Cardinal;
    public PieceType Marshall;


    // *** CONSTRUCTION *** //

    public GrandChess() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "r8r/1nbqkmcbn1/pppppppppp/10/10/10/10/PPPPPPPPPP/1NBQKMCBN1/R8R";
      PawnMultipleMove.Value = "Grand";
      PromotionRule.Value = "Grand";
      EnPassant = true;
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();
      Knight.MidgameValue = Knight.EndgameValue = 300;
      AddPieceType(Cardinal = new Archbishop("Cardinal", "C", 750, 800));
      AddPieceType(Marshall = new Chancellor("Marshall", "M", 925, 975));
      King.PSTMidgameForwardness = 0;
    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
    {
      base.AddEvaluations();

      //	Replace the development evaluation function with an updated one that 
      //	understands that there is no castling and the rooks are already connected
      Evaluations.Grand.GrandChessDevelopmentEvaluation newDevelopentEval = new Evaluations.Grand.GrandChessDevelopmentEvaluation();
      ReplaceEvaluation(FindEvaluation(typeof(DevelopmentEvaluation)), newDevelopentEval);

      //	We also need to update the pawn structure evaluation to inform it 
      //	that pawns promote on the 5th rank.  This is important for 
      //	proper evaluation of passed pawns.
      PawnStructureEvaluation eval = (PawnStructureEvaluation)FindEvaluation(typeof(PawnStructureEvaluation));
      eval.PassedPawnEvaluation = true;
      eval.PawnPromotionRank = 5;

      if (Marshall != null && Marshall.Enabled)
        RookTypeEval.AddRookOn7thBonus(Marshall, King, 2, 8);
    }
    #endregion
  }
}
