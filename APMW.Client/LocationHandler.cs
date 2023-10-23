using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;

namespace Archipelago.APChessV
{
  internal class LocationHandler
  {

    public LocationHandler(ArchipelagoSession session)
    {
      LocationCheckHelper = session.Locations;

      StartedEventHandler seHandler = (match) => this.match = match;
      Starter.getInstance().StartedEventHandlers.Add(seHandler);
      this.Hook();
    }

    public LocationCheckHelper LocationCheckHelper { get; private set; }

    private ChessV.Match match;

    public void Hook()
    {
      Starter.getInstance().MoveCompletionHandler.Add((move) => this.HandleMove(move));
    }

    public void HandleMove(MoveInfo info)
    {
      if (info == null)
      {
        return;
      }

      if (info.PieceMoved.PieceType.Notation.Equals("K"))
      {
        var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Once");
        LocationCheckHelper.CompleteLocationChecks(location);
      }
      var ToSquare = info.ToSquare;
      // check if move is to center
      if (match.Game.Board.GetFile(ToSquare) >= 3 && match.Game.Board.GetFile(ToSquare) <= 4 &&
        match.Game.Board.GetRank(ToSquare) >= 3 && match.Game.Board.GetRank(ToSquare) <= 4)
      {
        if (info.PieceMoved.PieceType.Notation.Equals("K"))
        {
          var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Thrice");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }

    }
  }
}