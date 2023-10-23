
using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Archipelago.APChessV
{

  [TestClass]
  public class LocationHandlerUnitTests
  {
    LocationHandler handler;
    Mock<ILocationCheckHelper> locations;
    Mock<ChessV.Match> match;
    Mock<Game> game;
    Mock<Board> board;
    Mock<Player> player;

    Mock<Piece> firstPiece;
    Mock<Piece> secondPiece;
    Mock<PieceType> firstPieceType;
    Mock<PieceType> secondPieceType;


    MoveInfo info;

    public LocationHandlerUnitTests()
    {
    }

    [TestInitialize()]
    public void beforeEach()
    {
      locations = new Mock<ILocationCheckHelper>();

      player = new Mock<Player>();
      player.SetupGet(mock => mock.IsHuman).Returns(true);

      board = new Mock<Board>();
      game = new Mock<Game>();
      match = new Mock<ChessV.Match>();
      match.SetupGet(mock => mock.Game).Returns(game.Object);
      match.Setup(mock => mock.GetPlayer(0)).Returns(player.Object);
      game.SetupGet(mock => mock.Board).Returns(board.Object);

      firstPieceType = new Mock<PieceType>();
      firstPiece = new Mock<Piece>();
      firstPiece.SetupGet(mock => mock.PieceType).Returns(firstPieceType.Object);
      firstPieceType.SetupGet(mock => mock.Name).Returns("Bishop");
      secondPieceType = new Mock<PieceType>();
      secondPiece = new Mock<Piece>();
      secondPiece.SetupGet(mock => mock.PieceType).Returns(secondPieceType.Object);
      secondPieceType.SetupGet(mock => mock.Name).Returns("Pawn");

      //board.Setup(mock => mock.GetFile(1)).Returns(0);

      handler = new LocationHandler(locations.Object);

      ApmwCore._instance.StartedEventHandlers.ForEach((handler) => handler(match.Object));
    }

    [TestMethod]
    public void updateMoveState_findsNewPiece()
    {

      MoveInfo info = GetMoveInfo();
      handler.UpdateMoveState(info);
      info = GetMoveInfo();
      info.FromSquare = 2;
      info.MoveType = MoveType.StandardCapture;
      handler.HandleMove(info);

      locations.Verify(locs => locs.GetLocationIdFromName("ChecksMate", "Piece A"));
    }

    public void updateMoveState_findsMovedPiece()
    {

    }

    public void updateMoveState_findsMultiMovedPiece()
    {

    }

    public void updateMoveState_findsCrisscrossedPiece()
    {

    }

    private MoveInfo GetMoveInfo() {
      MoveInfo info = new MoveInfo();

      info.Player = 0;
      info.FromSquare = 1;
      info.ToSquare = 3;
      info.MoveType = MoveType.StandardMove;
      info.PieceMoved = firstPiece.Object;
      return info;
    }
  }
}
