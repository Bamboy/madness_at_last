using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;
using Utils.Loaders;
using System.IO;
//Created by Nick and vozochris
namespace Utils{
	public class KeyManager{
		public static Dictionary<string, KeyCode> keyCodes = new Dictionary<string, KeyCode>();
		private static JSONNode data;

		public static void Init(){
			if(File.Exists(Application.persistentDataPath + "/KeyCodes"))
				data = Load();
			else
				data = JSONLoader.Load("KeyCodes");
			SetFromJSON();
		}
		public static void Save(){
			JSONNode save = JSONLoader.Load("KeyCodes");
			foreach(KeyValuePair<string, KeyCode> kvp in keyCodes){
				save[kvp.Key] = kvp.Value.ToString();
			}
			save.SaveToFile(Application.persistentDataPath + "/KeyCodes");
		}
		public static JSONNode Load(){
			return JSONNode.LoadFromFile(Application.persistentDataPath + "/KeyCodes");
		}
		public static KeyCode Get(string name) {
			KeyCode k;
			keyCodes.TryGetValue(name, out k);
			return k;
		}
		public static void Set(string name, KeyCode keyCode) {
			keyCodes[name] = keyCode;
		}
		public static void ResetDefaultKeys(){
			data = JSONLoader.Load("KeyCodes");
			SetFromJSON();
		}
		private static void SetFromJSON(){
			foreach(KeyValuePair<string, JSONNode> kvp in data.AsObject){
				Set(kvp.Key, (KeyCode)Enum.Parse(typeof(KeyCode), kvp.Value.Value));
			}
		}
	}
}