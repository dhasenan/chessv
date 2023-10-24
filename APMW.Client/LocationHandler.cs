﻿using Archipelago.MultiClient.Net;
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
      List<long> locations = new List<long>();
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
      
      // TODO(chesslogic): refactor these, extract into individual methods, probably reduce them to 7 lines

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
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Once"));
        }
        // check if move is to A file
        if (match.Game.Board.GetFile(info.ToSquare) == 0)
        {
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud A File"));
        }
        // check if move is to distant rank
        if ((info.Player == 1 && match.Game.Board.GetRank(info.ToSquare) == 0) ||
          (info.Player == 0 && match.Game.Board.GetRank(info.ToSquare) == 7))
        {
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Promotion"));
        }
        // check if move is to center
        if (match.Game.Board.InSmallCenter(info.ToSquare) == 1)
        {
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Thrice"));
        }
      }

      //
      // END various king moves ...
      //

      //
      // START captures ...
      //

      // check if move is capture
      if ((info.MoveType & MoveType.CaptureProperty) != 0)
      {
        // handle king captures
        if (pieceName.Equals("King"))
        {
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Bongcloud Capture"));
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
        locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName));
        // handle piece sequence
        int captures;
        if (isPiece)
          captures = ++capturedPieces;
        else
          captures = ++capturedPawns;
        if (captures > 1)
        {
          if (isPiece)
          {
            locationName = "Capture " + captures + " Pieces";
            if (capturedPawns >= captures)
            {
              locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName));
              locationName = "Capture " + captures + " Of Each";
            }
          }
          else
          {
            locationName = "Capture " + captures + " Pawns";
            if (capturedPieces >= captures)
            {
              locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName));
              locationName = "Capture " + captures + " Of Each";
            }
          }
          locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", locationName));
          if (capturedPieces >= 7 && capturedPawns >= 8)
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Capture Everything"));
          }
        }
      }

      //
      // END captures ...
      //

      //
      // BEGIN threats ...
      //

      for (int square = 0; square < match.Game.Board.NumSquares; square++)
      {
        if (match.Game.IsSquareAttacked(square, humanPlayer))
        {
          var loc = match.Game.Board.SquareToLocation(square);
          Piece attackedPiece = match.Game.Board[square];

          if (attackedPiece == null) { continue; }
          if (ApmwCore.getInstance().pawns.Contains(attackedPiece.PieceType))
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Pawn Threat"));
          }
          if (ApmwCore.getInstance().minors.Contains(attackedPiece.PieceType))
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Minor Threat"));
          }
          if (ApmwCore.getInstance().majors.Contains(attackedPiece.PieceType))
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Major Threat"));
          }
          if (ApmwCore.getInstance().queens.Contains(attackedPiece.PieceType))
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "Queen Threat"));
          }
          if (ApmwCore.getInstance().king == attackedPiece.PieceType)
          {
            locations.Add(LocationCheckHelper.GetLocationIdFromName("ChecksMate", "King Threat"));
          }
        }
      }

      //
      // END threats ...
      //

      LocationCheckHelper.CompleteLocationChecks(locations.ToArray());

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