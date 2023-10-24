using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Archipelago.APChessV
{
  public class LocationHandler
  {

    public LocationHandler(ILocationCheckHelper locationCheckHelper)
    {
      LocationCheckHelper = locationCheckHelper;

      seHandler = (match) =>
      {
        this.match = match;
        this.Hook();
      };
      ApmwCore.getInstance().StartedEventHandlers.Add(seHandler);
    }

    public ILocationCheckHelper LocationCheckHelper { get; private set; }

    private StartedEventHandler seHandler;
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
      // TODO(chesslogic): why does this continue to increment between games?
      this.capturedPawns = 0;
      this.capturedPieces = 0;
    }

    public void Unhook()
    {
      ApmwCore.getInstance().StartedEventHandlers.Remove(seHandler);
      seHandler = null;
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
      info.PieceCaptured = match.Game.Board[move.ToSquare];
      HandleMove(info);
    }

    public void HandleMove(MoveInfo info)
    {
      long location;
      if (info == null)
        return; // probably never happens

      // CPU can't emit locations - we've updated state, so return early
      if (info.Player != humanPlayer)
      {
        UpdateMoveState(info);
        return;
      }

      Piece piece = info.PieceMoved;
      string pieceName = piece.PieceType.Name;

      //
      // BEGIN various king moves ...
      //

      // check if move is early and is directly forward one step
      if (pieceName.Equals("King"))
      {
        if (match.Game.GameTurnNumber <= 2 &&
          match.Game.Board.GetFile(info.ToSquare) == 4 &&
          (match.Game.Board.GetRank(info.ToSquare) == 1 || match.Game.Board.GetRank(info.ToSquare) == 6))
        {
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Once");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
        // check if move is to A file
        if (match.Game.Board.GetFile(info.ToSquare) == 0)
        {
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud A File");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
        // check if move is to distant rank
        if ((info.Player == 1 && match.Game.Board.GetRank(info.ToSquare) == 0) ||
          (info.Player == 0 && match.Game.Board.GetRank(info.ToSquare) == 7))
        {
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Promotion");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
        // check if move is to center
        if (match.Game.Board.InSmallCenter(info.ToSquare) == 1)
        {
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Thrice");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
      }

      //
      // END various king moves ...
      //

      // check if move is capture
      if ((info.MoveType & MoveType.CaptureProperty) != 0)
      {
        // handle king captures
        if (pieceName.Equals("King"))
        {
          location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Capture");
          LocationCheckHelper.CompleteLocationChecks(location);
        }
        // handle specific piece
        int originalSquare = info.ToSquare;
        if (currentSquaresToOriginalSquares.ContainsKey(info.ToSquare))
          originalSquare = currentSquaresToOriginalSquares[info.ToSquare];
        int originalFile = match.Game.Board.GetFile(originalSquare);
        string fileNotation = match.Game.Board.GetFileNotation(originalFile);
        fileNotation = fileNotation.ToUpper();
        bool isPiece = !info.PieceCaptured.PieceType.Name.Contains("Pawn");
        string locationName;
        if (isPiece)
          locationName = "Capture Piece " + fileNotation;
        else
          locationName = "Capture Pawn " + fileNotation;
        location = LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName);
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

      UpdateMoveState(info);
    }

    public void UpdateMoveState(MoveInfo info)
    {
      // update original positions
      if (currentSquaresToOriginalSquares.ContainsKey(info.FromSquare))
        currentSquaresToOriginalSquares[info.ToSquare] = currentSquaresToOriginalSquares[info.FromSquare];
      else
        currentSquaresToOriginalSquares[info.ToSquare] = info.FromSquare;
    }
  }
}