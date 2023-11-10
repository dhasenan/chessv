using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV.Games.Rules.Extinction
{
  /** Defines a game loss state when ALL specified types are extinct, rather than ANY. */
  internal class CovenantRule : ExtinctionRule
  {
    public CovenantRule(string types) : base(types)
    {
    }

    // *** OVERRIDES *** //

    public override MoveEventResponse TestForWinLossDraw(int currentPlayer, int ply)
    {
      Dictionary<int, bool> losingGame = new Dictionary<int, bool>() { [0] = true, [1] = true };
      foreach (int typeNumber in extinctionTypeNumbers)
      {
        if (losingGame[currentPlayer] && Board.GetPieceTypeBitboard(currentPlayer, typeNumber).BitCount != 0)
          losingGame[currentPlayer] = false;
        if (losingGame[currentPlayer ^ 1] && Board.GetPieceTypeBitboard(currentPlayer ^ 1, typeNumber).BitCount != 0)
          losingGame[currentPlayer ^ 1] = false;
      }
      if (losingGame[currentPlayer]) return MoveEventResponse.GameLost;
      if (losingGame[currentPlayer ^ 1]) return MoveEventResponse.GameWon;
      return MoveEventResponse.NotHandled;
    }
  }
}
