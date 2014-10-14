using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 08/29/14     ////
///////////////////////////////////

[RequireComponent( typeof( WeaponInventory ) )]
public class InventoryInput : MonoBehaviour 
{
	public bool enableInput = true;

	private WeaponInventory inv;
	void Start()
	{
		inv = GetComponent< WeaponInventory >();
	}

	void Update()
	{
		if( enableInput ) 
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) // back
			{
				inv.PreviousWeapon();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) // forward
			{
				inv.NextWeapon();
			}

			//TODO - add support for pressing number keys to switch to different slots, but only if there is a weapon in that slot.
		}
		else
		{

		}
	}
}
