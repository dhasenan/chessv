
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
    public int pawnSeed = -1;
    public int minorSeed = -1;
    public int majorSeed = -1;
    public int queenSeed = -1;

    /** ChessV.Base.Match */
    public List<StartedEventHandler> StartedEventHandlers = new List<StartedEventHandler>();
    /** ChessV.Base.PieceType */
    public List<Func<Dictionary<KeyValuePair<int, int>, PieceType>>> PlayerPieceSetProvider = new List<Func<Dictionary<KeyValuePair<int, int>, PieceType>>>();
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Func<int>> GeriProvider = new List<Func<int>>();
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Action<Movement>> MoveCompletionHandler = new List<Action<Movement>>();

    public void seed(int[] seeds)
    {
      pocketSeed = seeds[0];
      pawnSeed = seeds[1];
      minorSeed = seeds[2];
      majorSeed = seeds[3];
      queenSeed = seeds[4];
    }

    /** Possibly not stable - will generate a different pocket distribution as the player progresses through different foundPockets - but it is uniform */
    public (int, int, int) generatePocketValues(int foundPockets)
    {
      if (foundPockets == 0) { return (0, 0, 0); }
      if (pocketSeed == -1) { throw new InvalidOperationException("Please set Starter.pocketSeed"); }

      Random random = new Random(pocketSeed);
      var x = random.Next(Math.Max(0, foundPockets - 8), Math.Min(foundPockets, 5));
      if (x == foundPockets)
      {
        return (x, 0, 0);
      }
      var y = random.Next(Math.Max(0, foundPockets - 4 - x), Math.Min(foundPockets, 5));
      var z = foundPockets - (y + x);
      if (z < 0)
      {
        (x, y, z) = (4 - x, 4 - y, -z);
      }

      return (x, y, z);
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
