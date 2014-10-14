using UnityEngine;
using System.Collections.Generic;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public enum DamageType
	{
		Physical, Heal, Fire, Frost
	}
	[System.Serializable]
	public class Unit : StatObject
	{
		[HideInInspector]
		public Stat health;
		public Resistance[] resistances;

		WeaponInventory weaponInventory;

		protected virtual void Awake()
		{
			Init();

			health = GetStat("health");

			weaponInventory = GetComponent<WeaponInventory>();
		}

		/// <summary>
		/// Damage by specified amount, type and source.
		/// </summary>
		/// <param name="amount">Amount of damage, positive amount heals the target</param>
		/// <param name="type">Damage type</param>
		/// <param name="effects">Damage effects</param>
		/// <param name="source">Source of damage.</param>
		public void Damage(float amount, DamageType type = DamageType.Physical, GameObject[] effects = null, System.Object source = null)
		{
			Resistance resistance = null;
			foreach(Resistance res in resistances)
			{
				if (res.type == type)
				{
					resistance = res;
					break;
				}
			}
			if (resistance != null)
				health.Current += amount * resistance.value;
			else
				health.Current += amount;

			if (effects != null)
			{
				foreach(GameObject effect in effects)
					AddEffect(effect);
			}

			if (health.Current <= 0)
				Die(source);
			else if (health.Current > health.Max)
				health.Current = health.Max;
		}

		public virtual void Die(System.Object source = null)
		{
			Destroy(gameObject);
		}

		public void OnShotFired(BulletInfo[] bullets)
		{
			foreach(BulletInfo bullet in bullets)
			{
				RaycastHit ray = bullet.data;
				if (ray.collider == null)
					continue;
				WeaponDamage(ray.collider.gameObject.GetComponentInChildren<Unit>(), this);
			}
		}

		public static void WeaponDamage(Unit unit, Unit owner, float damageMultiplier = 1)
		{
			if (unit == null)
				return;
			if (owner == null)
				return;

			WeaponInventory weaponInventory = owner.weaponInventory;
			WeaponSlot weaponSlot = weaponInventory.slots[weaponInventory.activeSlot];
			string weapon = weaponSlot.Weapon;
			GunDefinitions gunDefs = GunDefinitions.Get();
			
			Vector2 damageRange = gunDefs.GetDamageRange(weapon);
			DamageType damageType = gunDefs.GetDamageType(weapon);
			GameObject[] damageEffects = gunDefs.GetDamageEffects(weapon);

			float damage = Random.Range(damageRange.x, damageRange.y) * damageMultiplier;
			unit.Damage(-damage, damageType, damageEffects, weaponSlot);
		}
	}
}
