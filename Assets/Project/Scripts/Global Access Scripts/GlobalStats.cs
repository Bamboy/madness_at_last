using UnityEngine;

[System.Serializable]
public class GlobalStats
{
	public static GlobalStats current;
	public static GlobalStats saved;

	public int seconds = 0;
	public int kills = 0;
}
