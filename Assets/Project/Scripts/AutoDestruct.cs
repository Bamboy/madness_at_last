using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour 
{
	public float delayTime = 2.5f;

	// Use this for initialization
	void Start () 
	{
		Invoke( "SelfDestruct", delayTime );
	}
	
	void SelfDestruct()
	{
		Destroy( this.gameObject );
	}
}
