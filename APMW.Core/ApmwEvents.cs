
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
    public List<Action<Object>> StartedEventHandlers = new List<Action<Object>>();
    /** ChessV.Base.PieceType */
    public List<Func<Dictionary<KeyValuePair<int, int>, Object>>> PlayerPieceSetProvider = new List<Func<Dictionary<KeyValuePair<int, int>, Object>>>();
    /** Provides 0 if the player has found PlayAsWhite item, otherwise provides 1 */
    public List<Func<int>> GeriProvider = new List<Func<int>>();
  }
}
