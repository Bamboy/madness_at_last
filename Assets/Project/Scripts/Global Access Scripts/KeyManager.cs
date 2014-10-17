using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
//Created by Nick and Zac
namespace Utils{
	public class KeyManager{
		public static Dictionary<string, KeyCode> keyCodes = new Dictionary<string, KeyCode>();
		static string in_SavePath = Application.persistentDataPath + "/KeyMapping.dat";
		static string in_LoadPath = Application.persistentDataPath + "/KeyMapping.dat";

		public static void AddKeyCode(string name, KeyCode key) {
			keyCodes.Add(name, key);
		}
		public static KeyCode GetKeyCode(string name) {
			KeyCode k;
			keyCodes.TryGetValue(name, out k);
			return k;
		}
		public static void EditKeyCode(string name, KeyCode keyCode) {
			keyCodes[name] = keyCode;
		}
		public static void SetDefaultKeys(){
			AddKeyCode("forward", KeyCode.W);
			AddKeyCode("back", KeyCode.S);
			AddKeyCode("left", KeyCode.A);
			AddKeyCode("right", KeyCode.D);
			AddKeyCode("jump", KeyCode.Space);
			AddKeyCode("run", KeyCode.LeftShift);
			AddKeyCode("crouch", KeyCode.LeftControl);
		}
		public static void ResetDefaultKeys(){
			EditKeyCode("forward", KeyCode.W);
			EditKeyCode("back", KeyCode.S);
			EditKeyCode("left", KeyCode.A);
			EditKeyCode("right", KeyCode.D);
			EditKeyCode("jump", KeyCode.Space);
			EditKeyCode("run", KeyCode.LeftShift);
			EditKeyCode("crouch", KeyCode.LeftControl);
		}
		public static bool Save(){
			if(keyCodes != null && keyCodes.Count != 0){
				using(BinaryWriter BW = new BinaryWriter(File.Open(in_SavePath, FileMode.Create))){
					try{
						//Write Number of Pairs
						BW.Write(keyCodes.Count);
						
						foreach(KeyValuePair<string, KeyCode> KeyMapping in keyCodes){
							BW.Write(KeyMapping.Key);
							BW.Write((Int32)KeyMapping.Value);
						}
						//binarywriter could create the file & all values successfully written 
						return true;
					}
					catch(Exception Ex){
						//Failed to write values? Failed to save.
						return false;
					}
				}
				//If it jumped to here before completing, then the BinaryWriter failed to create the file.
				return false;
			} else {
				//Not sure if you want to count "No KeyMappings" as a successful save or a failure. I said failure.
				return false; 
			}
		}
		
		public static bool Load(){
			if(keyCodes == null){
				keyCodes = new Dictionary<string, KeyCode>();
			}
			using(BinaryReader BR = new BinaryReader(File.Open (in_LoadPath, FileMode.Open))){
				try{
					int NumberOfPairings = BR.ReadInt32();
					for(int I = 0; I < NumberOfPairings; I++){
						keyCodes.Add( BR.ReadString(), (KeyCode)BR.ReadInt32() );
					}
					//BinaryReader opened file successfully and loaded all values.
					return true;
				}
				catch(Exception Ex){
					return false;
				}
			}
			//If it jumped to here before completing, then the BinaryReader failed to open the file.
			return false;
		}
	}
}