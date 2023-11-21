
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

namespace ChessV
{
  public partial class Game
  {
    // *** EVAULATION *** //

    public int Evaluate()
    {
      //  basic material + piece-square-tables
      int midgameEval = Board.GetMidgameMaterialEval(0) - Board.GetMidgameMaterialEval(1);
      int endgameEval = Board.GetEndgameMaterialEval(0) - Board.GetEndgameMaterialEval(1);

      //  call all game-specific evaluation functions
      foreach (Evaluation evaluation in evaluations)
        evaluation.AdjustEvaluation(ref midgameEval, ref endgameEval);

      //	call any rule that adjusts evaluation
      foreach (Rule rule in rulesHandlingAdjustEvaluation)
        rule.AdjustEvaluation(Ply, ref midgameEval, ref endgameEval);

      //  scale result based on midgame -> endgame progress and return result
      int materialEval = Board.GetPlayerMaterial(0) + Board.GetPlayerMaterial(1);
      int phase =
        materialEval >= MidgameMaterialThreshold ? 128 :
        (materialEval <= EndgameMaterialThreshold ? 0 :
        (((materialEval - EndgameMaterialThreshold) * 128) / (MidgameMaterialThreshold - EndgameMaterialThreshold)));
      int eval = sign[CurrentSide] * ((midgameEval * phase) + (endgameEval * (128 - phase))) / 128;

      //	round the eval to the nearest 4 (essentially reducing the resolution from a 
      //	hundredth of a pawn to a quarter of a pawn.)
      eval = ((eval & 2) << 1) + (eval & ~3);
      return eval;
    }
  }
}
