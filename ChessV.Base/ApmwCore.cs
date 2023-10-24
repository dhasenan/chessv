
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessV.Base
{
  public class ApmwCore
  {
    public static ApmwCore _instance;

    public static ApmwCore getInstance()
    {
      if (_instance == null)
      {
        lock (typeof(ApmwCore))
        {
          if (_instance == null)
          {
            _instance = new ApmwCore();
          }
        }
      }
      return _instance;
    }

    public int pocketSeed = -1;
    public List<int> pocketChoiceSeed { get; private set; }
    public int pawnSeed = -1;
    public int minorSeed = -1;
    public int majorSeed = -1;
    public int queenSeed = -1;

    public int foundPockets = -1;
    public int foundPawns = -1;
    public int foundPieces = -1;
    public int foundMajors = -1;
    public int foundQueens = -1;

    public PieceType king;
    public HashSet<PieceType> pawns;
    public HashSet<PieceType> minors;
    public HashSet<PieceType> majors;
    public HashSet<PieceType> queens;
    public List<HashSet<PieceType>> pocketSets;


    /** ChessV.Base.Match */
    public List<StartedEventHandler> StartedEventHandlers = new List<StartedEventHandler>();
    /** ChessV.Base.PieceType */
    public Func<Dictionary<KeyValuePair<int, int>, PieceType>> PlayerPieceSetProvider;
    public Func<List<PieceType>> PlayerPocketPiecesProvider;
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public Func<int> GeriProvider = () => 1;
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Action<Movement>> MovePlayed = new List<Action<Movement>>();
    public Func<int> EngineWeakeningProvider = () => 0;

    public void seed(int[] seeds)
    {
      pocketSeed = seeds[0];
      pawnSeed = seeds[1];
      minorSeed = seeds[2];
      majorSeed = seeds[3];
      queenSeed = seeds[4];
    }

    /** Possibly not stable - will generate a different pocket distribution as the player progresses through different foundPockets - but it is uniform */
    public List<int> generatePocketValues(int foundPockets)
    {
      if (foundPockets == 0) { return new List<int>() { 0, 0, 0 }; }
      if (pocketSeed == -1) { throw new InvalidOperationException("Please set Starter.pocketSeed"); }

      // preserve choices separate from values
      Random pocketRandom = new Random(pocketSeed);
      pocketChoiceSeed = new List<int>() { pocketRandom.Next(), pocketRandom.Next(), pocketRandom.Next() };

      // probably not uniform... but it's within range so it works for now. will break FEN later
      Random random = new Random(pocketSeed);
      var x = random.Next(Math.Max(0, foundPockets - 8), Math.Min(foundPockets, 5));
      if (x == foundPockets)
      {
        return new List<int>() { x, 0, 0 };
      }
      var y = random.Next(Math.Max(0, foundPockets - 4 - x), Math.Min(foundPockets, 5));
      var z = foundPockets - (y + x);
      if (z < 0)
      {
        (x, y, z) = (4 - x, 4 - y, -z);
      }

      return new List<int>() { x, y, z };
    }

    /**
     * arg spaces should be TOTAL spaces not EMPTY spaces
     * 
     * vaguely inspired by this cacophanous suggestion:
     * https://stackoverflow.com/questions/28544808/random-distribution-of-items-in-list-with-exact-number-of-occurences
     */
    public static Dictionary<int, Item> distribute<Item>(List<Item> items, int spaces)
    {
      // Create list of items * z
      Dictionary<int, Item> allItems = new Dictionary<int, Item>();
      for (int i = 0; i < items.Count; i++)
        allItems.Add(i, items[i]);
      for (int i = items.Count; i < spaces; i++)
        allItems.Add(i, default(Item));

      Random random = new Random();
      int n = allItems.Count;
      while (n > 1)
      {
        n--;
        int k = random.Next(n + 1);
        (allItems[k], allItems[n]) = (allItems[n], allItems[k]);
      }

      return allItems;
    }
  }
}
