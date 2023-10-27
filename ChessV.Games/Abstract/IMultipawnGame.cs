using System.Collections.Generic;

namespace ChessV.Games
{
  internal interface IMultipawnGame
  {
    HashSet<PieceType> Pawns { get; }
  }
}