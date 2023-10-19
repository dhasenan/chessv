
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

    public List<Delegate> StartedEventHandlers = new List<Delegate>();
  }
}
