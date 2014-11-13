using UnityEngine;
using System;
using Utils;

//By Cristian "vozochris" Vozoca
namespace Stats.Abilities
{
	public class EventAbility : Ability
	{
		protected Action<object, int> endEventAction;

		internal virtual void Init(string id, AbilityUnit owner, Action<object, int> eventAction, Action<object, int> endEventAction = null)
		{
			base.Init(id, owner);
			PostInit();
			
			EventSystem.AddEventListener(data["event"].Value, eventAction);
			this.endEventAction = endEventAction;
		}

		internal override void PostInit()
		{
			if (category == null)
				category = "EventAbilities";
			base.PostInit();
		}
		
		public override bool Cast(StatObject target)
		{
			return base.Cast(target);
		}

		protected virtual void EndEvent()
		{
			if (endEventAction == null)
				return;
			string endEvent = data["endEvent"].Value;
			if (endEvent != "")
				EventSystem.AddEventListener(endEvent, endEventAction, EndEventCount);
		}

		public int EndEventCount
		{
			get { return data["endEventCount"].Value != "" ? data["endEventCount"].AsInt : 1; }
		}
	}
}