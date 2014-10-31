using UnityEngine;
using System.Collections.Generic;
//using Weapons;

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

		private WeaponInventory weaponInventory;

		protected override void Init(string statsFilePath, params string[] statsJSONPath)
		{
			base.Init(statsFilePath, statsJSONPath);
			health = GetStat("health");
			weaponInventory = GetComponent<WeaponInventory>();
			if (weaponInventory == null)
				weaponInventory = GetComponentInChildren<WeaponInventory>();
		}

		/// <summary>
		/// Damage by specified amount, type and source.
		/// </summary>
		/// <param name="amount">Amount of damage, positive amount heals the target</param>
		/// <param name="type">Damage type</param>
		/// <param name="effects">Damage effects</param>
		/// <param name="source">Source of damage.</param>
		public void Damage(float amount, DamageType type = DamageType.Physical, string[] effects = null, System.Object source = null)
		{
			if (HasStat("resistance" + type))
				health.Current += amount * (GetStat("resistance" + type).Current + GetStat("resistance").Current);
			else
				health.Current += amount;

			if (effects != null)
			{
				foreach(string effect in effects)
					AddEffect(effect, "WeaponTypes", "Stats/Effects/DamageTypes");
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
				if (bullet == null)
					continue;
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

			// Get owner Weapon data and damage the unit
			WeaponInventory weaponInventory = owner.weaponInventory;
			WeaponSlot weaponSlot = weaponInventory.slots[weaponInventory.activeSlot];
			string weapon = weaponSlot.Weapon;
			GunDefinitions gunDefs = GunDefinitions.Get();
			
			Vector2 damageRange = gunDefs.GetDamageRange(weapon);
			DamageType damageType = gunDefs.GetDamageType(weapon);
			string[] damageEffects = gunDefs.GetDamageEffects(weapon);
			
			float damage = Random.Range(damageRange.x, damageRange.y) * damageMultiplier;
			unit.Damage(-damage, damageType, damageEffects, weaponSlot);
		}
	}
}
