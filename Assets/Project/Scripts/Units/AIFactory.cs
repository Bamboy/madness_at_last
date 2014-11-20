using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 10/28/14     ////
///////////////////////////////////

public class AIFactory : MonoBehaviour 
{

	//TODO - Make created AI a child of another object. (Organization)
	public GameObject[] AIPrefabs;



	public static int aiCount;
	public static int aiCountMax = 17;
	public static float spawnDistanceMax = 60.0f;
	public static float spawnDistanceMin = 20.0f;

	public Transform[] spawnPoints;

	private Vector3 _lastSpawn;
	private Main main;
	#region Global Access
	private static AIFactory factory;
	public static AIFactory Get() //This makes it so we can remotely access this script from anywhere in the scene.
	{
		if( !factory ) //Check if we already have an instance of this script in the scene, if so just return it.
		{
			factory = GameObject.FindObjectOfType< AIFactory >(); //Find this script in the scene.
			if( !factory ) //Make sure we found something.
			{
				//Make a new object and attach this script to it. 
				GameObject newObj = new GameObject("=== AI Factory ===");
				factory = newObj.AddComponent< AIFactory >();
			}
		}
		return factory;
	}
	#endregion

	void Start () 
	{
		main = Main.Get();
		if( AIPrefabs == null )
			Debug.LogError("No AI Prefab was set!", this);
		else
		{
			spawnPoints = gameObject.GetComponentsInChildren< Transform >();
			InvokeRepeating("TrySpawnAI", 5.0f, 5.0f);
		}


	}

	void TrySpawnAI()
	{
		if( aiCount >= aiCountMax )
			return;

		Transform player = main.Player;
		spawnPoints = ArrayTools.Shuffle<Transform>( spawnPoints ); //Randomly changes the order of the spawnpoints. 
																	//This will give spawning a more random feel.
		//Transform[] filteredPoints = new Transform[0];
		for( int i = 0; i < spawnPoints.Length; i++ )
		{
			float distance = Vector3.Distance( spawnPoints[i].position, player.position );
			//distance less than max dist, and  greater than min dist...
			if( distance < spawnDistanceMax && distance > spawnDistanceMin )
			{
				if( _lastSpawn != spawnPoints[i].position )
				{
					_lastSpawn = spawnPoints[i].position;
					SpawnAI( spawnPoints[i].position );
					break;
				}
			}
		}
	}
	void SpawnAI( Vector3 pos )
	{
		AIPrefabs = ArrayTools.Shuffle<GameObject>( AIPrefabs );

		/*GameObject newAI = (GameObject)*/
		Instantiate( AIPrefabs[0], pos, Quaternion.identity );
		aiCount++;
		Debug.Log( "AI spawned!", this );
	}












}
