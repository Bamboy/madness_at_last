using UnityEngine;

//By Cristian "vozochris" Vozoca
namespace Stats.Abilities
{
	public class StatEventAbility : EventAbility
	{
		private StatEffect effect;

		internal override void Init(string id, AbilityUnit owner, System.Action<object, int> eventAction = null, System.Action<object, int> endEventAction = null)
		{
			eventAction = delegate(object obj, int calledCount) {
				int value = (int)obj;
				if (value % data["eventModulo"].AsInt == 0)
					Cast(owner);
			};
			base.Init(id, owner, eventAction, endEventAction);
		}

		internal override void PostInit()
		{
			category = "StatEventAbilities";
			base.PostInit();
		}

		public override bool Cast(StatObject target)
		{
			if (!base.Cast(target))
				return false;
			effect = target.AddEffect(id, category, dataPath);
			EndEvent();
			return true;
		}

		protected override void EndEvent()
		{
			endEventAction = delegate(object obj, int calledCount) {
				if (calledCount == EndEventCount)
					effect.Remove();
			};
			base.EndEvent();
		}
	}
}