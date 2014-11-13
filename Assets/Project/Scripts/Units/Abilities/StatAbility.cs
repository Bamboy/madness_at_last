using UnityEngine;

//By Cristian "vozochris" Vozoca
namespace Stats.Abilities
{
	public class StatAbility : Ability
	{
		internal override void Init(string id, AbilityUnit owner)
		{
			base.Init(id, owner);
			PostInit();
		}

		internal override void PostInit()
		{
			category = "StatAbilities";
			base.PostInit();
			if (type == AbilityType.Passive.ToString())//Auto cast passive
				Cast(owner);
		}

		public override bool Cast(StatObject target)
		{
			if (!base.Cast(target))
				return false;
			target.AddEffect(id, category, dataPath);
			return true;
		}
	}
}