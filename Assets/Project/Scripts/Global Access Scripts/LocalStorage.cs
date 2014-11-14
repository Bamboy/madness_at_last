using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// By Nick E. & vozochris
public class LocalStorage
{
	public static LocalStorage instance;
	public static GlobalStats currentStats;
	public static GlobalStats savedStats;

	private static string path = Application.persistentDataPath + "/info.dat";

	public LocalStorage() {
		if (instance != null)
			Debug.LogError("Only one instance of LocalStorage should be in scene.");

		instance = this;

		GlobalStats.current = currentStats = new GlobalStats();
		Load();
	}
	public void Save(){
		SaveVariables();
		BinaryFormatter bf = new BinaryFormatter();
		try
		{
			FileStream file = File.Exists(path) ? File.OpenWrite(path) : File.Create(path);
			bf.Serialize(file, currentStats);
			file.Close();
		}
		catch(Exception e)
		{
			File.Delete(path);
		}
	}
	public GlobalStats Load(){
		if (savedStats != null)
			return savedStats;

		if(File.Exists(path)){
			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				FileStream file = File.Open(path, FileMode.Open);
				GlobalStats.saved = savedStats = (GlobalStats)bf.Deserialize(file);
				file.Close();
			}
			catch(Exception e)
			{
				GlobalStats.saved = savedStats = new GlobalStats();
			}
		}
		else
			GlobalStats.saved = savedStats = new GlobalStats();
		return savedStats;
	}
	private void SaveVariables()
	{
		currentStats.kills += savedStats.kills;
		currentStats.seconds += savedStats.seconds;
		currentStats.effectVolume += savedStats.effectVolume;
		currentStats.voiceVolume += savedStats.voiceVolume;
		currentStats.musicVolume += savedStats.musicVolume;
		currentStats.masterVolume += savedStats.masterVolume;
	}
}