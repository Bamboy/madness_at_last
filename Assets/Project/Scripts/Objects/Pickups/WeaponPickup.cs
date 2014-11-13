using UnityEngine;

/// By: Cristian "vozochris" Vozoca
namespace Excelsion.Objects.Pickups
{
	public class WeaponPickup : Pickup
	{
		public bool giveWeaponToo;
		public string weapon;
		public int ammo;

		protected override void OnPlayerEnter(GameObject obj)
		{
			WeaponInventory weaponInventory = obj.GetComponentInChildren<WeaponInventory>();

			if( giveWeaponToo )
			{
				if( weaponInventory.AddNewWeapon( weapon, ammo ) )
				{
					base.OnPlayerEnter( obj );
				}
				else if( weaponInventory.AddAmmoFor( weapon, ammo ) )
				{
					base.OnPlayerEnter( obj );
				}
			}
			else
			{
				if( weaponInventory.AddAmmoFor( weapon, ammo ) )
				{
					base.OnPlayerEnter( obj );
				}
			}
		}
	}
}