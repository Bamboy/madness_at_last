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
		drifter = GetComponent< FirstPersonDrifter >();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( sendInputs )
		{
			drifter.SetInputs(
				Input.GetKey(Utils.KeyManager.Get("left")) ? -1.0f : Input.GetKey(Utils.KeyManager.Get("right")) ? 1.0f : 0.0f,
				Input.GetKey(Utils.KeyManager.Get("forward")) ? 1.0f : Input.GetKey(Utils.KeyManager.Get("back")) ? -1.0f : 0.0f,
				Input.GetKey(Utils.KeyManager.Get("run")),
				Input.GetKey(Utils.KeyManager.Get("jump")),
				Input.GetKey(Utils.KeyManager.Get("crouch"))
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
