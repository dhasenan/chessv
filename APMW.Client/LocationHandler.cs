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
      ApmwCore.getInstance().StartedEventHandlers.Add(seHandler);
      this.Hook();
    }

    public LocationCheckHelper LocationCheckHelper { get; private set; }

    private ChessV.Match match;

    public void Hook()
    {
      ApmwCore.getInstance().MoveCompletionHandler.Add((move) => this.HandleMove(move));
    }

    public void HandleMove(Movement movement)
    {
      if (movement == null)
      {
        return;
      }

      int fromSquare = movement.FromSquare;
      int toSquare = movement.ToSquare;
      string notation = match.Game.GetSquareNotation(fromSquare);
      // check if move is not to border
      if (match.Game.Board.GetFile(toSquare) >= 1 && match.Game.Board.GetFile(toSquare) <= 6 &&
        match.Game.Board.GetRank(toSquare) >= 1 && match.Game.Board.GetRank(toSquare) <= 6)
      {
        if (notation.Equals("K"))
        {
          var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Once");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }
      // check if move is to center
      if (match.Game.Board.GetFile(toSquare) >= 3 && match.Game.Board.GetFile(toSquare) <= 4 &&
        match.Game.Board.GetRank(toSquare) >= 3 && match.Game.Board.GetRank(toSquare) <= 4)
      {
        if (notation.Equals("K"))
        {
          var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Thrice");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }

    }
  }
}