using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/13/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem
{
	public class InvInput : MonoBehaviour 
	{
		public bool inputEnabled = true;

		private GunInventory inv;
		void Start () {
			inv = GetComponent< GunInventory >();
		}

		void Update () 
		{
			if( inputEnabled )
			{
				inv.InputFire = Input.GetMouseButton(0);
				inv.InputReload = Input.GetKey( KeyCode.R );

				if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) // back
				{
					inv.PreviousWeapon();
				}
				else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) // forward
				{
					inv.NextWeapon();
				}
			}
		}
	}
}
