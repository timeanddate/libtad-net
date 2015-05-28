using System;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace TimeAndDate.Services
{
	public static class InMemStore
	{
		private static Dictionary<string, object> _db = new Dictionary<string, object>();
		
		public static object Get(string key)
		{
			return _db[key];
		}
		
		public static void Store (string key, object obj)
		{
			if (_db.ContainsKey (key))
				_db [key] = obj;
			else
				_db.Add (key, obj);
		}
	}
}

