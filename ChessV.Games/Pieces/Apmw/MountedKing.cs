using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV.Games.Pieces.Apmw
{
  internal class MountedKing : PieceType
  {
    public MountedKing(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null)
      : base("Mounted King", name, notation, midgameValue, endgameValue, preferredImageName)

    {
      AddMoves(this);

      //	midgame tables
      PSTMidgameInSmallCenter = 8;
      PSTMidgameInLargeCenter = 5;
      PSTMidgameLargeCenterAttacks = 4;
      PSTMidgameForwardness = -13;
      //	endgame tables
      PSTEndgameForwardness = 6;
      PSTEndgameInLargeCenter = 14;
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
      Knight.AddMoves(type);
    }
  }

  internal class HyperKing : PieceType
  {
    public HyperKing(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null)
      : base("Hyper King", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);

      //	midgame tables
      PSTMidgameInSmallCenter = 10;
      PSTMidgameInLargeCenter = 6;
      PSTMidgameLargeCenterAttacks = 8;
      PSTMidgameForwardness = -10;
      //	endgame tables
      PSTEndgameForwardness = 8;
      PSTEndgameInLargeCenter = 16;
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
      Nightrider.AddMoves(type);
      Elephant.AddMoves(type);
    }
  }
}
