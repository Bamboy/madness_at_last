using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

namespace Utils.Loaders
{
	public class JSONLoader
	{
		public const string PATH_PREFIX = "Data/JSON/";

		private static Dictionary<string, JSONNode> CachedJSONNodes = new Dictionary<string, JSONNode>();

		public static JSONNode Load(string JSONFilePath, bool cache = true)
		{
			if (CachedJSONNodes.ContainsKey(JSONFilePath) && cache)
				return CachedJSONNodes[JSONFilePath];
			else if (cache)
				return CachedJSONNodes[JSONFilePath] = JSON.Parse(Resources.Load<TextAsset>(PATH_PREFIX + JSONFilePath).text);
			else
				return JSON.Parse(Resources.Load<TextAsset>(PATH_PREFIX + JSONFilePath).text);
		}

		public static void Clear()
		{
			CachedJSONNodes.Clear();
		}
	}
}
