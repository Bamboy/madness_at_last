using UnityEngine;
using System.Collections;
using System.IO;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 10/10/14     ////
///////////////////////////////////

[RequireComponent (typeof (FirstPersonDrifter))]
public class DrifterInput : MonoBehaviour 
{
	private FirstPersonDrifter drifter;
	public bool sendInputs = true;


	// Use this for initialization
	void Start () 
	{
		//new KeyManager();
		//KeyManager.Load();
		if(File.Exists(Application.persistentDataPath + "/KeyMapping.dat")){
			KeyManager.Load();
		} else {
			KeyManager.SetDefaultKeys();
		}
		drifter = GetComponent< FirstPersonDrifter >();


	}
	
	// Update is called once per frame
	void Update () 
	{
		if( sendInputs )
		{
			drifter.SetInputs(
				Input.GetKey(KeyManager.GetKeyCode("left")) ? -1.0f : Input.GetKey(KeyManager.GetKeyCode("right")) ? 1.0f : 0.0f,
				Input.GetKey(KeyManager.GetKeyCode("forward")) ? 1.0f : Input.GetKey(KeyManager.GetKeyCode("back")) ? -1.0f : 0.0f,
				Input.GetKey(KeyManager.GetKeyCode("run")),
				Input.GetKey(KeyManager.GetKeyCode("jump")),
				Input.GetKey(KeyManager.GetKeyCode("crouch"))
				);
		}
		else
		{
			drifter.SetInputs(
				0.0f,
				0.0f,
				false,
				false,
				false
				);
		}
	}
}
