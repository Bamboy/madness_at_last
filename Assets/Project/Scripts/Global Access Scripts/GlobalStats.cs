using UnityEngine;

[System.Serializable]
public class GlobalStats
{
	public static GlobalStats current;
	public static GlobalStats saved;

	public int seconds = 0;
	public int kills = 0;
	public float masterVolume;
	public float musicVolume;
	public float voiceVolume;
	public float effectVolume;
}
