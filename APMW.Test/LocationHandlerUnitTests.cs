
using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Archipelago.APChessV
{

  [TestClass]
  public class LocationHandlerUnitTests
  {
    LocationHandler handler;
    public LocationHandlerUnitTests()
    {
      handler = new LocationHandler(Mock.Of<ArchipelagoSession>());
    }

    [TestInitialize()]
    public void beforeEach()
    {
    }

    [TestMethod]
    public void updateMoveState_findsNewPiece()
    {

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
  }
}
