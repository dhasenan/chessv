
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChessV.Base
{
  public class Starter
	{
		public static Starter _instance;

		public static Starter getInstance()
		{
			if (_instance == null)
			{
				lock (typeof(Starter))
        {
          if (_instance == null)
					{
						_instance = new Starter();
					}
        }
			}
			return _instance;
    }

    public int pocketSeed = -1;

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

    /** ChessV.Base.Match */
    public List<Action<object>> StartedEventHandlers = new List<Action<object>>();
    /** ChessV.Base.PieceType */
    public List<Func<Dictionary<KeyValuePair<int, int>, PieceType>>> PlayerPieceSetProvider = new List<Func<Dictionary<KeyValuePair<int, int>, PieceType>>>();
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Func<int>> GeriProvider = new List<Func<int>>();
  }
}
