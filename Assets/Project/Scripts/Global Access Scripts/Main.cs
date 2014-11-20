using UnityEngine;
using System.IO;

public class Main : MonoBehaviour
{
	#region Global Access
	private static Main main;
	public static Main Get() //This makes it so we can remotely access this script from anywhere in the scene.
	{
		if( !main ) //Check if we already have an instance of this script in the scene, if so just return it.
		{
			main = GameObject.FindObjectOfType< Main >(); //Find this script in the scene.
			if( !main ) //Make sure we found something.
			{
				//Make a new object and attach this script to it. 
				GameObject newObj = new GameObject("=== Main Controller ===");
				main = newObj.AddComponent< Main >();
			}
		}
		return main;
	}
	#endregion


	private static Transform player;
	public Transform Player { get{return player;} set{player = value;} }
	void FindPlayerInScene()
	{
		GameObject ply = GameObject.FindGameObjectWithTag( "Player" );
		if( ply == null )
		{
			Player = null;
			Debug.LogWarning( "Player could not be found in the scene!", this );
		}
		else
			Player = ply.transform;
	}

	
	void OnLevelWasLoaded( int id )
	{
		FindPlayerInScene();
	}



	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		new LocalStorage();
		Utils.KeyManager.Init();
		FindPlayerInScene();
	}

	void OnApplicationQuit()
	{
		LocalStorage.instance.Save();
		Utils.KeyManager.Save();
	}
}
