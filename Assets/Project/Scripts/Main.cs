using UnityEngine;
using System.IO;

public class Main : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		new LocalStorage();
		if(File.Exists(Application.persistentDataPath + "/KeyMapping.dat")){
			Utils.KeyManager.Load();
		} else {
			Utils.KeyManager.SetDefaultKeys();
		}
	}

	void OnApplicationQuit()
	{
		LocalStorage.instance.Save();
	}
}
