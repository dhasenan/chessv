using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessV.Games.Rules.Apmw
{
	public class ApmwStalemateRule: CheckmateRule
	{
		public ApmwStalemateRule( PieceType royalPieceType ) : base( royalPieceType ) { }

    public override MoveEventResponse MoveBeingMade( MoveInfo move, int ply )
    {
      if (Game == null || Game.Match == null)
        return base.IllegalCheckMoves(move);
      Player player = Game.Match.GetPlayer(move.Player);
      if (player is HumanPlayer)
        return MoveEventResponse.NotHandled;
			return base.IllegalCheckMoves(move);
    }

    public override MoveEventResponse NoMovesResult(int currentPlayer, int ply)
    {
      MoveEventResponse response = base.NoMovesResult(currentPlayer, ply);
      if (response != StalemateResult)
        return response;
      Player player = Game.Match.GetPlayer(currentPlayer);
      if (player is HumanPlayer)
        return MoveEventResponse.GameLost;
      return MoveEventResponse.GameWon;
    }

    public override int PositionalSearchExtension(int currentPlayer, int ply)
    {
      if (ply < 20)
        return base.PositionalSearchExtension(currentPlayer, ply);
      return 0;
    }
  }
}
