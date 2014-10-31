using UnityEngine;
using SimpleJSON;
using Utils.Loaders;

//By Cristian "vozochris" Vozoca
namespace Stats.Abilities
{
	public enum AbilityType
	{
		Active, Passive, Toggle
	}
	public class Ability
	{
		public AbilityUnit owner;

		protected string id;
		protected string category;
		protected string type;

		protected float cooldown;
		protected float castTime;

		public const string dataPath = "Stats/Effects/Abilities";
		protected JSONNode data;
		
		internal virtual void Init(string id, AbilityUnit owner)
		{
			this.id = id;
			this.owner = owner;
		}

		internal virtual void PostInit()
		{
			data = JSONLoader.Load(dataPath)[category][id];
			type = data["type"].Value;
			cooldown = data["cooldown"].AsFloat;
			castTime = -cooldown;// Fix for being able to cast from start
		}

		public virtual bool Cast(StatObject target)
		{
			if (data == null)
				return false;
			if (Time.time - castTime < cooldown)
				return false;
			castTime = Time.time;
			return true;
		}

		public virtual void ToggleOff(StatObject target)
		{
			if (type == AbilityType.Toggle.ToString())
				castTime = Time.time;

			StatEffect effect = target.GetEffect(id);
			if (effect != null)
				effect.Remove();
		}

		public float Cooldown
		{
			get { return cooldown; }
		}

		public float CastTime
		{
			get { return castTime; }
		}
	}
}