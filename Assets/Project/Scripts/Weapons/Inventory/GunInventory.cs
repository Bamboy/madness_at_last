using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.WeaponSystem.Weapons;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/20/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem
{
	public class GunInventory : MonoBehaviour 
	{
		public GunBase[] guns; //This contains one of each weapon we will ever be able to use. This also contains information about our ammo.
		public bool[] hasWeapon; //1 means we have the weapon in the slot, 0 means we don't.
		public int activeWeapon = 0;
		private int weaponCount;
		private int MAX_LOOP_COUNT = 10; 

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

		//Returns the slot that the weapon is in. If we do not have access to the weapon, returns -1.
		public int GetWeapon( System.Type weapon )
		{
			for( int i = 0; i < guns.Length; i++ )
			{
				if(Object.ReferenceEquals( weapon, guns[i].GetType() )) //Returns true if the class types are the same.
				{
					return i;
				}
			}
			return -1;
		}

		private void HideGuns() //This is called when the 'ActiveWeapon' variable changes.
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
			CountWeapons();
			HideGuns();
			#endregion
		}
		int CountWeapons()
		{
			weaponCount = 0;
			for( int i = 0; i < guns.Length; i++ )
			{ 
				if( hasWeapon[i] == false || (guns[i].Ammo <= 0 && guns[i].ClipAmmo <= 0) )
				{
					continue; //Continue stops this current loop cycle and goes to the next one.
				}

				//Debug.Log ("We do have this weapon");
				weaponCount++;
			}
			return weaponCount;
		}

		#region Inputs
		public bool InputFire   {set{
				if( activeWeapon > -1 ) {guns[activeWeapon].InputFire = value;} 
		}}
		public bool InputReload {set{ 
				if( activeWeapon > -1 ) {guns[activeWeapon].InputReload = value;} 
		}}

		#region Weapon Selection
		public void NextWeapon()
		{
			if( weaponCount <= 1 )
				return;

			int newWep = ActiveWeapon;
			newWep++;
			if( newWep == guns.Length ) 
				newWep = 0; //We would get an Index Out Of Range error if we didn't change it!
			Debug.Log( newWep );
			int breakCounter = 0;
			while((guns[newWep].Ammo <= 0 && guns[newWep].ClipAmmo <= 0) || hasWeapon[newWep] == false ) //Don't stop on a weapon unless we have it!
			{
				newWep++;
				if( newWep == guns.Length ) 
					newWep = 0; //We would get an Index Out Of Range error if we didn't change it!

				breakCounter++;
				if( breakCounter > MAX_LOOP_COUNT )
				{
					Debug.LogWarning("Infinite loop detected!", this);
					break;
				}

			}
			ActiveWeapon = newWep; //Finalize our weapon switch.
		}
		public void PreviousWeapon()
		{
			int newWep = ActiveWeapon;
			newWep--;
			if( newWep == -1 ) 
				newWep = guns.Length - 1; //We would get an Index Out Of Range error if we didn't change it!
			
			int breakCounter = 0;
			while((guns[newWep].Ammo <= 0 && guns[newWep].ClipAmmo <= 0) || hasWeapon[newWep] == false ) //Don't stop on a weapon unless we have it!
			{
				newWep--;
				if( newWep == -1 ) 
					newWep = guns.Length - 1; //We would get an Index Out Of Range error if we didn't change it!
				
				breakCounter++;
				if( breakCounter > MAX_LOOP_COUNT )
				{
					Debug.LogWarning("Infinite loop detected!", this);
					break;
				}
				
			}
			ActiveWeapon = newWep; //Finalize our weapon switch.
		}
		#endregion

		#endregion







		void LateUpdate()
		{
			if( ActiveWeapon > -1 && ActiveWeapon < guns.Length && guns != null )
			{
				if( guns[ ActiveWeapon ].Ammo <= 0 && guns[ ActiveWeapon ].ClipAmmo <= 0 )
				{
					hasWeapon[ActiveWeapon] = false;

					if( weaponCount > 1 )
						NextWeapon();
					else
						ActiveWeapon = -1;
				}

				CountWeapons();
				
				if( weaponCount == 0 )
				{
					ActiveWeapon = -1;
				}
			}
			else
			{

				if( CountWeapons() > 0 )
				{
					NextWeapon();
				}
			}


		}


	}













}