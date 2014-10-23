using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 08/29/14     ////
///////////////////////////////////

[RequireComponent( typeof( GunShooting ) )]
public class WeaponInput : MonoBehaviour 
{
	public bool enableInput = true;

	private GunShooting gunScript;
	void Start()
	{
		gunScript = GetComponent< GunShooting >();
	}

	void Update () 
	{

		if( enableInput )
		{
			gunScript.FireWeapon = Input.GetButton("Fire1");
			gunScript.ReloadWeapon = Input.GetButton("Reload");
		}
		else
		{
			gunScript.FireWeapon = false;
			gunScript.ReloadWeapon = false;
		}

	}
}
