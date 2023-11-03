using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using ChessV;
using ChessV.Base;
using ChessV.Games;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;
using static Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper;

namespace Archipelago.APChessV
{
  internal class ItemHandler
  {

    public ItemHandler(ReceivedItemsHelper receivedItemsHelper)
    {
      ReceivedItemsHelper = receivedItemsHelper;

      this.Hook();
      irHandler = (helper) => this.Hook();
      ReceivedItemsHelper.ItemReceived += irHandler;

      // overwrite global state
      ApmwCore.getInstance().PlayerPieceSetProvider = () => generatePlayerPieceSet();
      ApmwCore.getInstance().PlayerPocketPiecesProvider = () => generatePocketItems();
    }

    private ReceivedItemsHelper ReceivedItemsHelper;
    private ItemReceivedHandler irHandler;

    public void Hook()
    {
      var items = ReceivedItemsHelper.AllItemsReceived;
      ReceivedItemsHelper.ItemReceived += irHandler;

      ApmwCore.getInstance().foundPockets = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket");
      ApmwCore.getInstance().foundPocketRange = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket Range");
      ApmwCore.getInstance().foundPocketGems = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket Gems");
      ApmwCore.getInstance().GeriProvider = () => items.Any(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Play as White") ? 0 : 1;
      ApmwCore.getInstance().EngineWeakeningProvider = () => items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Engine ELO Lobotomy");
      ApmwCore.getInstance().foundPockets = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pocket");
      ApmwCore.getInstance().foundPawns = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pawn");
      ApmwCore.getInstance().foundMinors = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Minor Piece");
      ApmwCore.getInstance().foundMajors = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Major Piece");
      ApmwCore.getInstance().foundQueens = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Major To Queen");
      ApmwCore.getInstance().foundPawnForwardness = items.Count(
        (item) => ReceivedItemsHelper.GetItemName(item.Item) == "Progressive Pawn Forwardness");
    }

    public void Unhook()
    {
      ReceivedItemsHelper.ItemReceived -= irHandler;
    }

    public (Dictionary<KeyValuePair<int, int>, PieceType>, string) generatePlayerPieceSet()
    {
      List<string> promotions = new List<string>();
      List<int> order;
      // indices 0..7 are the back rank - 8..9 can be pieces if the first rank fills up
      List<PieceType> withMajors = GenerateMajors(out order, promotions);
      // replace some or all majors with queens
      List<PieceType> withQueens = SubstituteQueens(withMajors, order, promotions);
      // then add minor pieces until out of space
      List<PieceType> withMinors = GenerateMinors(withQueens, promotions);
      List<PieceType> withPawns = GeneratePawns(withMinors);

      Dictionary<KeyValuePair<int, int>, PieceType> pieces = new Dictionary<KeyValuePair<int, int>, PieceType>();
      for (int i = 0; i < 3; i++)
        for (int j = 0; j < 8; j++)
          if (withPawns[i * 8 + j] != null)
            pieces.Add(new KeyValuePair<int, int>(2-i, j), withPawns[i*8 + j]);

      return (pieces, String.Join("", promotions));
    }

    public List<PieceType> GeneratePawns(List<PieceType> minors)
    {
      List<PieceType> pawns = ApmwCore.getInstance().pawns.ToList();
      List<PieceType> thirdRank = new List<PieceType>() { null, null, null, null, null, null, null, null };
      List<PieceType> pawnRank = minors.Skip(8).ToList();

      Random random = new Random(ApmwConfig.getInstance().pawnSeed);

      int startingPieces = pawnRank.Count((item) => item != null);
      int totalChessmen = ApmwCore.getInstance().foundPawns + startingPieces;

      for (int i = startingPieces; i < Math.Min(8, totalChessmen); i++)
      {
        var piece = pawns[random.Next(pawns.Count)];
        chooseIndexAndPlace(pawnRank, random, piece);
      }
      for (int i = 8; i < Math.Min(16, totalChessmen); i++)
      {
        var piece = pawns[random.Next(pawns.Count)];
        chooseIndexAndPlace(thirdRank, random, piece);
      }

      int remainingForwardness = ApmwCore.getInstance().foundPawnForwardness;
      List<int> possibleForwardPawnPositions = new List<int>();
      for (int i = 0; i < thirdRank.Count; i++)
        if (thirdRank[i] == null && pawnRank[i] != null && pawnRank[i].Name.Contains("Pawn"))
          possibleForwardPawnPositions.Add(i);
      for (
        int i = random.Next(possibleForwardPawnPositions.Count);
        remainingForwardness-- > 0 && possibleForwardPawnPositions.Count > 0;
        i = random.Next(possibleForwardPawnPositions.Count))
      {
        // swap backward with forward
        thirdRank[possibleForwardPawnPositions[i]] = pawnRank[possibleForwardPawnPositions[i]];
        pawnRank[possibleForwardPawnPositions[i]] = null;
        // setup for next iteration (TODO(chesslogic): could move this into the for loop syntax)
        possibleForwardPawnPositions.RemoveAt(i);
      }

      List<PieceType> output = new List<PieceType>();
      output.AddRange(minors.Take(8));
      output.AddRange(pawnRank);
      output.AddRange(thirdRank);

      return output;
    }

    public List<PieceType> GenerateMinors(List<PieceType> queens, List<string> promotions)
    {
      HashSet<string> promoPieces = new HashSet<string>();
      List<PieceType> minors = ApmwCore.getInstance().minors.ToList();
      List<PieceType> outer = queens.Skip(8).Take(2).ToList();
      List<PieceType> left = queens.Take(4).ToList();
      List<PieceType> right = queens.Skip(5).Take(3).ToList();
      List<PieceType> temp = new List<PieceType>() { null, null, null, };
      temp.AddRange(outer);
      temp.AddRange(new List<PieceType>() { null, null, null, });
      outer = temp;

      Random random = new Random(ApmwConfig.getInstance().minorSeed);

      int startingPieces = ApmwCore.getInstance().foundMajors;
      int totalPieces = startingPieces + ApmwCore.getInstance().foundMinors;

      int player = ApmwCore.getInstance().GeriProvider();
      int parity = left.Count((piece) => piece != null) - right.Count((piece) => piece != null);
      for (int i = startingPieces; i < Math.Min(7, totalPieces); i++)
      {
        var piece = minors[random.Next(minors.Count)];
        promoPieces.Add(piece.Notation[player]);
        parity = placeInArray(new List<int>(), left, right, random, parity, i, piece);
      }
      for (int i = 7; i < Math.Min(15, totalPieces); i++)
      {
        var piece = minors[random.Next(minors.Count)];
        promoPieces.Add(piece.Notation[player]);
        chooseIndexAndPlace(outer, random, piece);
      }

      List<PieceType> output = new List<PieceType>();
      output.AddRange(left);
      output.Add(ApmwCore.getInstance().king);
      output.AddRange(right);
      output.AddRange(outer);
      promotions.Add(string.Join("", promoPieces));

      return output;
    }

    public List<PieceType> SubstituteQueens(List<PieceType> majors, List<int> order, List<string> promotions)
    {
      /*
      IEnumerable<int> majorIndexes =
        majors.Select((major, index) => major != null ? index : -1)
            .Where(index => index != -1);
      */

      HashSet<string> promoPieces = new HashSet<string>();
      List<PieceType> queens = ApmwCore.getInstance().queens.ToList();
      int kingIndex = 4; // king is on E file

      Random random = new Random(ApmwConfig.getInstance().queenSeed);

      int player = ApmwCore.getInstance().GeriProvider();
      for (int i = 0; i < ApmwCore.getInstance().foundQueens && i < order.Count; i++)
      {
        var piece = queens[random.Next(queens.Count)];
        promoPieces.Add(piece.Notation[player]);
        if (order[i] < kingIndex)
          majors[order[i]] = piece;
        else
          majors[order[i]+1] = piece;
      }
      promotions.Add(String.Join("", promoPieces));
      return majors;
    }

    public List<PieceType> GenerateMajors(out List<int> order, List<string> promotions)
    {
      HashSet<string> promoPieces = new HashSet<string>();
      order = new List<int>();
      List<PieceType> majors = ApmwCore.getInstance().majors.ToList();
      List<PieceType> outer = new List<PieceType>() { null, null };
      List<PieceType> left = new List<PieceType>() { null, null, null, null };
      List<PieceType> right = new List<PieceType>() { null, null, null };

      Random random = new Random(ApmwConfig.getInstance().majorSeed);

      int player = ApmwCore.getInstance().GeriProvider();
      int parity = 0;
      for (int i = 0; i < Math.Min(7, ApmwCore.getInstance().foundMajors); i++)
      {
        var piece = majors[random.Next(majors.Count)];
        promoPieces.Add(piece.Notation[player]);
        parity = placeInArray(order, left, right, random, parity, i, piece);
      }
      for (int i = 7; i < ApmwCore.getInstance().foundMajors; i++)
      {
        var piece = majors[random.Next(majors.Count)];
        promoPieces.Add(piece.Notation[player]);
        order.Add(chooseIndexAndPlace(outer, random, piece) + 8);
      }

      List<PieceType> output = new List<PieceType>();
      output.AddRange(left);
      output.Add(ApmwCore.getInstance().king);
      output.AddRange(right);
      output.AddRange(outer);
      promotions.Add(String.Join("", promoPieces));

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
      int side;
      // there are 4 spaces on the left (queenside) vs 3 on right (kingside)
      if (i >= right.Count * 2 || i >= left.Count * 2)
      {
        side = right.Count - left.Count; // 3 - 4 = -1
        parity = 0;
      }
      // if we need to choose a side, it should be random
      else if (parity == 0)
      {
        parity = random.Next(2) * 2 - 1;
        side = -parity;
      }
      // we chose the other side last time, let's go somewhere new
      else
      {
        side = parity;
        parity = 0;
      }

      if (side <= 0)
      {
        order.Add(chooseIndexAndPlace(left, random, piece));
      }
      else
      {
        order.Add(chooseIndexAndPlace(right, random, piece) + left.Count); // left.Count == 4
      }

      return parity;
    }

    private static int chooseIndexAndPlace(List<PieceType> items, Random random, PieceType piece)
    {
      var index = 0;
      var skips = random.Next(items.Count(item => item == null));
      while (items[index] != null || skips > 0)
      {
        if (items[index] == null)
          skips--;
        index++;
      }
      items[index] = piece;
      return index;
    }

    public List<PieceType> generatePocketItems()
    {
      int foundPockets = ApmwCore.getInstance().foundPockets;
      var pockets = ApmwConfig.getInstance().generatePocketValues(foundPockets);
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
        Random random = new Random(ApmwConfig.getInstance().pocketChoiceSeed[i]);
        int index = random.Next(setOfPieceType.Count);
        pocketPieces.Add(setOfPieceType.ToList()[index]);
      }
      return pocketPieces;
    }
  }
}