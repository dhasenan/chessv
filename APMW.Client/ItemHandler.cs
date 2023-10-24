using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper;

namespace Archipelago.APChessV
{
  internal class ItemHandler
  {

    public ItemHandler(ReceivedItemsHelper receivedItemsHelper)
    {
      ReceivedItemsHelper = receivedItemsHelper;

      this.Hook();
      ItemReceivedHandler irHandler = (helper) => this.Hook();
      ReceivedItemsHelper.ItemReceived += irHandler;
      ApmwCore.getInstance().PlayerPieceSetProvider = () => generatePlayerPieceSet();
    }

    private ReceivedItemsHelper ReceivedItemsHelper;

    public void Hook()
    {
      var items = ReceivedItemsHelper.AllItemsReceived;

      ApmwCore.getInstance().foundPockets = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket");
      ApmwCore.getInstance().GeriProvider = () => items.Any(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Play as White") ? 0 : 1;
      ApmwCore.getInstance().EngineWeakeningProvider = () => items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Engine ELO Lobotomy");
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

    public Dictionary<KeyValuePair<int, int>, PieceType> generatePlayerPieceSet()
    {
      Dictionary<KeyValuePair<int, int>, PieceType> pieces = new Dictionary<KeyValuePair<int, int>, PieceType>();
      List<int> order;
      List<PieceType> withMajors = GenerateMajors(out order);
      List<PieceType> withQueens = SubstituteQueens(withMajors, order);
      List<PieceType> withMinors = GenerateMinors(withQueens);
      List<PieceType> withPawns = GeneratePawns(withQueens);

      return pieces;
    }

    public List<PieceType> GeneratePawns(List<PieceType> minors)
    {
      List<PieceType> outer = new List<PieceType>() { null, null, null, null, null, null, null, null };
      List<PieceType> inner = new List<PieceType>() { null, null, null, null, null, null, null, null };

    }

    public List<PieceType> GenerateMinors(List<PieceType> queens)
    {
      List<PieceType> minors = ApmwCore.getInstance().minors.ToList();
      List<PieceType> outer = queens.Skip(8).Take(2).ToList();
      List<PieceType> left = queens.Take(4).ToList();
      List<PieceType> right = queens.Skip(5).Take(3).ToList();

      Random random = new Random(ApmwCore.getInstance().minorSeed);

      int parity = 0;
      for (int i = 0; i < Math.Min(7, ApmwCore.getInstance().foundMajors); i++)
      {
        var piece = minors[random.Next(minors.Count)];
        parity = placeInArray(new List<int>(), left, right, random, parity, i, piece);
      }
      for (int i = 7; i < ApmwCore.getInstance().foundMajors; i++)
      {
        parity = random.Next(2);
        var piece = minors[random.Next(minors.Count)];
        outer[parity] = piece;
      }

      List<PieceType> output = new List<PieceType>();
      output.AddRange(left);
      output.Add(ApmwCore.getInstance().king);
      output.AddRange(right);
      output.AddRange(outer);

      return output;
    }

    public List<PieceType> SubstituteQueens(List<PieceType> majors, List<int> order)
    {
      /*
      IEnumerable<int> majorIndexes =
        majors.Select((major, index) => major != null ? index : -1)
            .Where(index => index != -1);
      */

      List<PieceType> queens = ApmwCore.getInstance().queens.ToList();

      Random random = new Random(ApmwCore.getInstance().queenSeed);

      for (int i = 0; i < ApmwCore.getInstance().foundQueens && i < order.Count; i++)
      {
        var piece = queens[random.Next(majors.Count)];
        majors[order[i]] = piece;
      }
      return majors;
    }

    public List<PieceType> GenerateMajors(out List<int> order)
    {
      order = new List<int>();
      List<PieceType> majors = ApmwCore.getInstance().majors.ToList();
      List<PieceType> outer = new List<PieceType>() { null, null };
      List<PieceType> left = new List<PieceType>() { null, null, null, null };
      List<PieceType> right = new List<PieceType>() { null, null, null };

      Random random = new Random(ApmwCore.getInstance().majorSeed);

      int parity = 0;
      for (int i = 0; i < Math.Min(7, ApmwCore.getInstance().foundMajors); i++)
      {
        var piece = majors[random.Next(majors.Count)];
        parity = placeInArray(order, left, right, random, parity, i, piece);
      }
      for (int i = 7; i < ApmwCore.getInstance().foundMajors; i++)
      {
        parity = random.Next(2);
        var piece = majors[random.Next(majors.Count)];
        outer[parity] = piece;
        order.Add(parity + 8);
      }

      List<PieceType> output = new List<PieceType>();
      output.AddRange(left);
      output.Add(ApmwCore.getInstance().king);
      output.AddRange(right);
      output.AddRange(outer);

      return output;
    }

    private static int placeInArray(
      List<int> order,
      List<PieceType> left,
      List<PieceType> right,
      Random random,
      int parity,
      int i,
      PieceType piece)
    {
      int side = 1;
      if (i == 7)
      {
        side = -1;
        parity = 0;
      }
      else if (parity == 0)
      {
        parity = random.Next(2) * 2 - 1;
        side = -parity;
      }
      else
      {
        side = parity;
        parity = 0;
      }

      if (side < 0)
      {
        var index = 0;
        var skips = random.Next(left.Count(item => item == null));
        while (left[index] != null || skips > 0)
        {
          if (left[index] != null)
            index++;
          else
            skips--;
        }
        left[index] = piece;
        order.Add(index);
      }
      else
      {
        var index = 0;
        var skips = random.Next(right.Count(item => item == null));
        while (right[index] != null || skips > 0)
        {
          if (right[index] != null)
            index++;
          else
            skips--;
          order.Add(index + 4);
        }
        right[index] = piece;
      }

      return parity;
    }

    public Dictionary<int, bool> generatePawnLocations(int pawns)
    {
      for (int file = 0; file < 8; file++)
      {
        for (int rank = 0; rank < 2; rank++)
        {

        }
      }
      return new Dictionary<int, bool>();
    }

    public void generatePocketItems()
    {
      int foundPockets = ApmwCore.getInstance().foundPockets;
      var pockets = ApmwCore.getInstance().generatePocketValues(foundPockets);
      List<PieceType> pocketPieces = new List<PieceType>();
      List<string> pocketItems = new List<string>();
      int empty = 0;
      for (int i = 0; i < 3; i++)
      {
        if (pockets[i] == 0)
        {
          empty++;
          pocketPieces.Add(null);
          continue;
        }
        var setOfPieceType = ApmwCore.getInstance().pocketSets[pockets[i] - 1];
        Random random = new Random(ApmwCore.getInstance().pocketChoiceSeed[i]);
        int index = random.Next(setOfPieceType.Count);
        pocketPieces.Add(setOfPieceType.ToList()[index]);
      }
      ApmwCore.getInstance().PlayerPocketPiecesProvider = () => pocketPieces;
    }
  }
}