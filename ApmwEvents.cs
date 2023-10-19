using System;

namespace APMW.Core
{
	public class Starter
	{
		public static _instance;

		public static Starter getInstance()
		{
			if (_instance == null)
			{
				lock (typeof(Starter))
        {
          if (_instance == null)
					{
						_instance = Starter
					}
        }
			}
			return _instance;
		}

		public List<delegate> StartedEventHandlers = new ArrayList();
  }
}
