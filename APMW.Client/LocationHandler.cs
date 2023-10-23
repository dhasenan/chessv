using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;
using System.Collections.Generic;
using System.Linq;

namespace Archipelago.APChessV
{
  internal class LocationHandler
  {

    public LocationHandler(ArchipelagoSession session)
    {
      LocationCheckHelper = session.Locations;

      StartedEventHandler seHandler = (match) =>
      {
        this.match = match;
        this.Hook();
      };
      ApmwCore.getInstance().StartedEventHandlers.Add(seHandler);
    }

    public LocationCheckHelper LocationCheckHelper { get; private set; }

    private ChessV.Match match;
    private int humanPlayer;
    private int capturedPieces;
    private int capturedPawns;

    private Dictionary<int, int> currentSquaresToOriginalSquares = new Dictionary<int, int>();

    public void Hook()
    {
      //match.Game.MovePlayed += (move) => this.HandleMove(move);
      ApmwCore.getInstance().MovePlayed.Add((move) => this.HandleMove(move));
      this.humanPlayer = this.match.GetPlayer(0).IsHuman ? 0 : 1;
      this.capturedPawns = 0;
      this.capturedPieces = 0;
    }

    public void HandleMove(Movement move)
    {
      if (move == null)
        return; // probably never happens

      MoveInfo info = new MoveInfo();
      info.Player = move.Player;
      info.MoveType = move.MoveType;
      info.FromSquare = move.FromSquare;
      info.ToSquare = move.ToSquare;
      info.PieceMoved = match.Game.Board[move.FromSquare];
      HandleMove(info);
    }

    public void HandleMove(MoveInfo info)
    {
      if (info == null)
        return; // probably never happens

      // update original positions
      if (!currentSquaresToOriginalSquares.ContainsKey(info.FromSquare))
        currentSquaresToOriginalSquares[info.FromSquare] = info.ToSquare;
      else
        currentSquaresToOriginalSquares[currentSquaresToOriginalSquares[info.FromSquare]] = info.ToSquare;
      
      if (info.Player != humanPlayer) {
        return;
      }

      Piece piece = info.PieceMoved;
      string pieceName = piece.PieceType.Name;
      // check if move is not to border
      if (match.Game.Board.GetFile(info.ToSquare) == 4 &&
        match.Game.Board.GetRank(info.ToSquare) >= 1 && match.Game.Board.GetRank(info.ToSquare) <= 6)
      {
        if (pieceName.Equals("King"))
        {
          var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Once");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }
      // check if move is to center
      if (match.Game.Board.InSmallCenter(info.ToSquare) == 1)
      {
        if (pieceName.Equals("King"))
        {
          var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Thrice");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }
      // check if move is capture
      if ((info.MoveType & MoveType.CaptureProperty) != 0)
      {
        // handle specific piece
        int originalSquare = info.ToSquare;
        if (currentSquaresToOriginalSquares.ContainsKey(info.ToSquare))
          originalSquare = currentSquaresToOriginalSquares[info.ToSquare];
        int originalFile = match.Game.Board.GetFile(originalSquare);
        string fileNotation = match.Game.Board.GetFileNotation(originalFile);
        fileNotation = fileNotation.ToUpper();
        bool isPiece = !info.PieceMoved.PieceType.Name.Contains("Pawn");
        string locationName;
        if (isPiece)
          locationName = "Capture Piece " + fileNotation;
        else
          locationName = "Capture Pawn " + fileNotation;
        var location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName);
        LocationCheckHelper.CompleteLocationChecks(location);
        // handle piece sequence
        int captures;
        if (isPiece)
          captures = ++capturedPieces;
        else
          captures = ++capturedPawns;
        if (captures > 1)
        {
          if (isPiece)
            locationName = "Capture " + captures + " Pieces";
          else
            locationName = "Capture " + captures + " Pawns";
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName);
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }
    }
  }
}