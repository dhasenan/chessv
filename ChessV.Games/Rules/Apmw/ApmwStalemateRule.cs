using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV.Games.Rules.Apmw
{
	public class ApmwStalemateRule: CheckmateRule
	{
		public ApmwStalemateRule( PieceType royalPieceType ) : base( royalPieceType ) { }

    public override MoveEventResponse MoveBeingMade( MoveInfo move, int ply )
    {
      Player player = Game.Match.GetPlayer(move.Player);
      if (player is HumanPlayer)
        return MoveEventResponse.NotHandled;
			return base.MoveBeingMade(move, ply);
    }
	}
}
