using UnityEngine;

public class Main : MonoBehaviour
{
	void Awake()
	{
		new LocalStorage();
	}

	void OnApplicationQuit()
	{
		LocalStorage.instance.Save();
	}
}
