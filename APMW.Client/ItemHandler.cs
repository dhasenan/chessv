using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using static Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper;

namespace Archipelago.APChessV
{
  internal class ItemHandler
  {

    public ItemHandler(ArchipelagoSession session)
    {
      ReceivedItemsHelper ReceivedItemsHelper = session.Items;

      this.Hook(session.Items);
      ItemReceivedHandler irHandler = (helper) => this.Process(helper);
      ReceivedItemsHelper.ItemReceived += irHandler;
    }

    public void Hook(ReceivedItemsHelper ReceivedItemsHelper)
    {
      var items = ReceivedItemsHelper.AllItemsReceived;

      ApmwCore.getInstance().foundPockets = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket");
      ApmwCore.getInstance().GeriProvider = () => items.Any(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Play as White") ? 0 : 1;
      ApmwCore.getInstance().EngineWeakeningProvider = () => items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Engine ELO Lobotomy");
    }

    public void Process(ReceivedItemsHelper ReceivedItemsHelper)
    {
      var items = ReceivedItemsHelper.AllItemsReceived;

      ApmwCore.getInstance().foundPockets = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket");
      ApmwCore.getInstance().foundPawns = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pawn");
      ApmwCore.getInstance().foundPieces = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Minor Piece");
      ApmwCore.getInstance().foundMajors = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Major Piece");
      ApmwCore.getInstance().foundQueens = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Major to Queen");
    }
  }
}