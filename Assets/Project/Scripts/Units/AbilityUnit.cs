using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using Utils.Loaders;
using Stats.Abilities;

//By Cristian "vozochris" Vozoca
namespace Stats
{
	public class AbilityUnit : Unit
	{
		protected Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

		private JSONNode data;

		protected override void Init(string statsFilePath, params string[] statsJSONPath)
		{
			base.Init(statsFilePath, statsJSONPath);
			data = JSONLoader.Load(Ability.dataPath);
		}

		public bool CastAbility(string id, StatObject target)
		{
			if (!abilities.ContainsKey(id))
				return false;
			return abilities[id].Cast(target);
		}

		public Ability AddAbility(string id)
		{
			if (abilities.ContainsKey(id))
				return null;
			if (data == null)
				return null;

			int category = -1;

			string[] categories = new string[] { "StatAbilities", "StatEventAbilities", "EventAbilities" };
			int le = categories.Length;
			for(int i = 0; i < le; i++)
			{
				string cat = categories[i];
				if (data[cat][id].AsObject.Count != 0)
					category = i;
			}

			Ability ability = null;
			switch(category)
			{
				case 0:
					ability = new StatAbility();
					((StatAbility)ability).Init(id, this);
					break;

				case 1:
					ability = new StatEventAbility();
					((StatEventAbility)ability).Init(id, this, null, null);
					break;

				case 2:
					Debug.LogError("EventAbilities are not ready yet!");
					ability = new EventAbility();
					((EventAbility)ability).Init(id, this, null, null);
					break;

				default:
					Debug.LogError("Ability category not found! (" + id + ")");
					break;
			}
			abilities[id] = ability;
			return ability;
		}

		public void RemoveAbility(string id)
		{
			abilities.Remove(id);
		}
	}
}