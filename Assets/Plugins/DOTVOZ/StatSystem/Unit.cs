using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	/// <summary>
	/// Damage type.
	/// To use DamageType Resistances, create Stats with prefix "resistance" and DamageType string. Example: "resistancePhysical".
	/// To use DamageType Effects, create a "DamageTypes" JSON file inside "Resources/Data/JSON/Stats/Effects/" folder, This is the default path, you can change it.
	/// There create an Object named "DamageTypes" then write the Effects.
	/// More info about writing the JSON data in Readme.txt file, Samples and Demo.
	/// </summary>
	public enum DamageType
	{
		NoResistance, Physical, Heal, Fire, Frost
	}

	/// <summary>
	/// Unit is a StatObject with Health that can Die and has a basic Damage system implemented.
	/// </summary>
	public class Unit : StatObject
	{
		[HideInInspector]
		public Stat health;

		/// <summary>
		/// Initializes the <see cref="Unit"/>.
		/// </summary>
		/// <param name="statsFilePath">Path of the JSON file. Inside Resources folder, default JSONLoader path is prefixed with: <see cref="Data/JSON/"/></param>
		/// <param name="statsJSONPath">Path of the Stats inside the JSON file.</param>
		protected override void Init(string statsFilePath, params string[] statsJSONPath)
		{
			base.Init(statsFilePath, statsJSONPath);
			health = GetStat("health");
		}

		/// <summary>
		/// Damage by specified amount, type and source.
		/// </summary>
		/// <param name="amount">Amount of damage, positive amount heals the target.</param>
		/// <param name="type">Damage type.</param>
		/// <param name="effects">Damage effects.</param>
		/// <param name="source">Source of damage.</param>
		public void Damage(float amount, DamageType type = DamageType.Physical, string[] effects = null, System.Object source = null)
		{
			if (type == DamageType.NoResistance)
				health.Current += amount;
			else
			{
				float globalResistance = HasStat("resistance") ? GetStat("resistance").Current : 0;
				if (type == DamageType.Heal || type == DamageType.Physical)
					globalResistance = 0;// You should not have global resistance for healing and physical damage.
				float resistance = (HasStat("resistance" + type) ? GetStat("resistance" + type).Current : 0) + globalResistance;
				if (resistance > 1)
					resistance = 1;// If resistance is bigger than 1 (100%) set it to 1 to avoid healing.
				health.Current += amount * (1 - resistance);
			}

			if (effects != null)
			{
				foreach(string effect in effects)
					AddEffect(effect, "DamageTypes", "Stats/Effects/DamageTypes");
			}

			if (health.Current <= 0)
				Die(source);
		}

		/// <summary>
		/// Death script
		/// </summary>
		/// <param name="source">Source of death.</param>
		public virtual void Die(System.Object source = null)
		{
			Destroy(gameObject);
		}
	}
}
