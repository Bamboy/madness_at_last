using UnityEngine;

/// By: Cristian "vozochris" Vozoca
public class WeaponPickup : Pickup
{
	public string weapon;
	public int amount;

	protected override void OnPlayerEnter(GameObject player)
	{
		WeaponInventory weaponInventory = player.GetComponentInChildren<WeaponInventory>();
		if (weaponInventory.AddAmmoFor(weapon, amount))
			base.OnPlayerEnter(player);
	}
}
