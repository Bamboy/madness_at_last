using UnityEngine;
using System.Collections;

public class WeaponSpawner : MonoBehaviour 
{
	public GameObject[] prefabs;


	// Use this for initialization
	void Awake () 
	{
		if( prefabs != null )
		{
			for( int i = 0; i < prefabs.Length; i++ )
			{
				Instantiate( prefabs[i], transform.position, Quaternion.identity );
			}
		}
		Destroy( this );
	}
}
