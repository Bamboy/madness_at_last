using UnityEngine;
using Excelsion.WeaponSystem;
using Excelsion.WeaponSystem.Weapons;
using System;

/// By: Cristian "vozochris" Vozoca
/// Modified by Stephan "Bamboy" Ennen, 11/18/14
namespace Excelsion.Objects.Pickups
{
	public class WeaponPickup : Pickup
	{
		public bool giveWeaponToo;
		public Weapon weapon;
		public int ammo;

		private Type wepType;

		void Start()
		{
			wepType = GunHelper.GetWeaponClass( weapon );
			if( wepType == null )
			{
				Debug.LogError("The associated weapon type was not found! ( "+ weapon.ToString() + " )", this);
				Debug.Break();
			}
		}

		protected override void OnPlayerEnter(GameObject obj)
		{
			GunInventory inventory = obj.GetComponentInChildren<GunInventory>();

			int slot = inventory.GetWeapon( wepType );
			if( slot != -1 )
			{
				if( giveWeaponToo && inventory.hasWeapon[ slot ] == false )
				{
					inventory.hasWeapon[ slot ] = true;
					GunBase newGun = inventory.guns[ slot ];

					//Choose a random amount of ammo to put in the clip...
					//Choose a random range between 0 and the size of clip.. but DO NOT exceed 'ammo'.
					int ammoForClip = Mathf.Min( UnityEngine.Random.Range(0, newGun.ClipSize), ammo );
					//Take our clip ammo out of total, DO NOT exceed the total ammo allowed to be carried.
					ammo = Mathf.Min( ammo - ammoForClip, newGun.MaxAmmo );
					//Finalize the values.
					newGun.ClipAmmo = ammoForClip;
					newGun.Ammo = ammo;

					base.OnPlayerEnter( obj ); //This will destroy this gameobject and its children.
				}
				else
				{
					if( inventory.hasWeapon[ slot ] == true )
					{
						GunBase gun = inventory.guns[ slot ];

						if( gun.Ammo < gun.MaxAmmo ) //Only give ammo if we aren't at our max ammo already.
						{
							gun.Ammo += ammo; //Note that this is automatically clamped. (So there is no point in clamping twice)
							base.OnPlayerEnter( obj ); //This will destroy this gameobject and its children.
						}
					}
				}
			}


		}
	}
}