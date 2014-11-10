using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.WeaponSystem.Weapons;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem
{
	public class GunInventory : MonoBehaviour 
	{
		public GunBase[] guns; //This contains one of each weapon we will ever be able to use. This also contains information about our ammo.
		public bool[] hasWeapon; //1 means we have the weapon in the slot, 0 means we don't.
		public int activeWeapon = 0;

		#region Accessors
		public int ActiveWeapon {
			get{ return activeWeapon; }
			set{
				if( value < 0 || value >= hasWeapon.Length || hasWeapon[ value ] == false )
				{
					activeWeapon = -1;
					HideGuns();
				}
				else
				{
					activeWeapon = value;
					HideGuns();
				}
			}
		}
		#endregion

		private void HideGuns() //This is called when the 'activeWeapon' variable changes.
		{
			for( int i = 0; i < guns.Length; i++ )
			{
				if( activeWeapon == i && hasWeapon[i] )
					guns[i].ShowWeapon( true );
				else
					guns[i].ShowWeapon( false );
			}
		}

		void Start () 
		{
			#region Startup error handling
			if( guns != null )
			{
				if( guns.Length == 0 )
				{
					Debug.LogError("No weapons were assigned to this GunInventory. You need to do this even if you don't want the player starting with weapons.", this);
					Debug.Break();
				}
			}
			else
			{
				Debug.LogError("No weapons were assigned to this GunInventory. You need to do this even if you don't want the player starting with weapons.", this);
				Debug.Break();
			}

			if( hasWeapon == null || hasWeapon.Length != guns.Length )
			{
				Debug.LogWarning("HasWeapon array length was not the same as the Guns array length. Starting with no weapons because of this.", this);
				hasWeapon = new bool[guns.Length];
				activeWeapon = -1;
			}
			#endregion
		}

		#region Inputs
		public bool InputFire{set{ guns[activeWeapon].InputFire = value; }}
		public bool InputReload {set{ guns[activeWeapon].InputReload = value; }}




		#endregion







		//void 


	}













}