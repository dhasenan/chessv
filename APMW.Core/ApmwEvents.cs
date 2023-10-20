
using System;
using System.Collections;
using System.Collections.Generic;

namespace APMW.Core
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

    /** ChessV.Base.Match */
    public List<Action<object>> StartedEventHandlers = new List<Action<object>>();
    /** ChessV.Base.PieceType */
    public List<Func<Dictionary<KeyValuePair<int, int>, object>>> PlayerPieceSetProvider = new List<Func<Dictionary<KeyValuePair<int, int>, object>>>();
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Func<int>> GeriProvider = new List<Func<int>>();
  }
}
