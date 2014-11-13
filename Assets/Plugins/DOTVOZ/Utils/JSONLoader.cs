using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

// By: Cristian "vozochris" Vozoca
namespace Utils.Loaders
{
	/// <summary>
	/// JSON loading Utility class.
	/// </summary>
	public class JSONLoader
	{
		/// <summary>
		/// The Path prefix will be added to the file path when loading.
		/// </summary>
		public const string PATH_PREFIX = "Data/JSON/";

		private static Dictionary<string, JSONNode> CachedJSONNodes = new Dictionary<string, JSONNode>();

		/// <summary>
		/// Loads JSON from Resources folder.
		/// <see cref="PATH_PREFIX"/> will be added for the given file path. default = "Data/JSON/".
		/// </summary>
		/// <param name="JSONFilePath">JSON file path.</param>
		/// <param name="cache">If set to <c>true</c> cache. Use <c>true</c> if the JSON will be loaded multiple times.</param>
		public static JSONNode Load(string JSONFilePath, bool cache = true)
		{
			if (CachedJSONNodes.ContainsKey(JSONFilePath) && cache)
				return CachedJSONNodes[JSONFilePath];
			else if (cache)
				return CachedJSONNodes[JSONFilePath] = JSON.Parse(Resources.Load<TextAsset>(PATH_PREFIX + JSONFilePath).text);
			else
				return JSON.Parse(Resources.Load<TextAsset>(PATH_PREFIX + JSONFilePath).text);
		}

		/// <summary>
		/// Clears the Cache from the Loader.
		/// </summary>
		public static void Clear()
		{
			CachedJSONNodes.Clear();
		}
	}
}
