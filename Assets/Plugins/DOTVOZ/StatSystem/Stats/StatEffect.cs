using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Utils.Loaders;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	/// <summary>
	/// More info about writing the JSON data in Readme.txt file, Samples and Demo.
	/// </summary>
	public class StatEffect : MonoBehaviour
	{
		public const string EFFECTS_PATH = "Stats/Effects";

		internal StatObject target;
		internal string id;
		
		private List<KeyValuePair<Stat, EffectProperties>> stats;
		
		protected JSONNode json;
		protected JSONNode properties;
		
		/// <summary>
		/// Initializes the <see cref="StatEffect"/>.
		/// Properties are set to: JSON -> Category -> Type.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="category">Category.</param>
		/// <param name="effectsFilePath">Effects file path.</param>
		public void Init(string type, string category, string effectsFilePath = EFFECTS_PATH)
		{
			json = JSONLoader.Load(effectsFilePath);
			properties = json[category][type];
			
			EffectProperties effectProps = null;
			
			//Get effects from JSON and Add them to the List
			stats = new List<KeyValuePair<Stat, EffectProperties>>();
			if (properties["effects"].AsArray.Count != 0)
			{
				JSONArray effects = properties["effects"].AsArray;
				foreach(JSONNode effect in effects)
				{
					effectProps = new EffectProperties(effect);
					stats.Add(new KeyValuePair<Stat, EffectProperties>(GetTarget(effectProps).GetStat(effect["stat"]), effectProps));
				}
			}
			else
			{
				effectProps = new EffectProperties(properties);
				stats.Add(new KeyValuePair<Stat, EffectProperties>(GetTarget(effectProps).GetStat(properties["stat"]), effectProps));
			}
			
			//Get effects from the List and Start Coroutines
			float totalDuration = 1;
			foreach(KeyValuePair<Stat, EffectProperties> statEffectPair in stats)
			{
				Stat stat = statEffectPair.Key;
				EffectProperties effects = statEffectPair.Value;
				
				StartCoroutine(ApplyFirst(stat, effects));
				if (effects.duration != 0)
					StartCoroutine(End(stat, effects));
				else
					totalDuration = 0;
				
				if (totalDuration != 0)
					totalDuration = Mathf.Max(totalDuration, effects.duration + effects.delay);
				
			}
			
			//If duration is not 0 (infinite) then Invoke the Dispose.
			if (totalDuration != 0)
				Invoke("Dispose", totalDuration + 0.1f);
		}
		
		/// <summary>
		/// Applies the first effect.
		/// </summary>
		/// <param name="stat">Stat.</param>
		/// <param name="effects">Effects.</param>
		internal IEnumerator ApplyFirst(Stat stat, EffectProperties effects)
		{
			if (effects.delay != 0)
				yield return new WaitForSeconds(effects.delay);
			
			StatObject t = GetTarget(effects);
			
			if (t is Unit && ((stat.Id == "health" && effects.value < 0) || (stat.Id == "health" && effects.value > 0 && effects.damageType == DamageType.Heal)))
			{
				((Unit)t).Damage(effects.value, effects.damageType, null, this);
				
				if (effects.tickTimes != 0)
					StartCoroutine(Apply(stat, effects));
			}
			else
			{
				if (effects.setStat)
					t.ChangeStatTo(stat, effects.value);
				else
				{
					float previousValue = stat.Current;
					t.ChangeStat(stat, effects.value, effects.multiplyStat);
					effects.valueDifference = stat.Current - previousValue;
					
					if (effects.tickTimes != 0)
						StartCoroutine(Apply(stat, effects));
				}
			}
		}
		
		/// <summary>
		/// Applies the effect.
		/// </summary>
		/// <param name="stat">Stat.</param>
		/// <param name="effects">Effects.</param>
		internal IEnumerator Apply(Stat stat, EffectProperties effects)
		{
			yield return new WaitForSeconds(effects.tick);
			effects.currentTick++;
			
			StatObject t = GetTarget(effects);
			
			if (t is Unit && ((stat.Id == "health" && effects.value < 0) || (stat.Id == "health" && effects.value > 0 && effects.damageType == DamageType.Heal)))
				((Unit)t).Damage(effects.value, effects.damageType, null, this);
			else
			{
				if (effects.multiplyStat)
					t.ChangeStat(stat, -effects.valueDifference);
				float previousValue = stat.Current;
				t.ChangeStat(stat, effects.value, effects.multiplyStat);
				effects.valueDifference = stat.Current - previousValue;
			}
			
			if (effects.currentTick != effects.tickTimes - 1)
				StartCoroutine(Apply(stat, effects));
		}
		
		/// <summary>
		/// Ends the effect.
		/// </summary>
		/// <param name="stat">Stat.</param>
		/// <param name="effects">Effects.</param>
		internal IEnumerator End(Stat stat, EffectProperties effects)
		{
			yield return new WaitForSeconds(effects.duration + effects.delay);
			ReturnEffects(stat, effects);
		}
		
		/// <summary>
		/// Removes the effect.
		/// Only if "return" is set to true.
		/// </summary>
		/// <param name="stat">Stat.</param>
		/// <param name="effects">Effects.</param>
		private void ReturnEffects(Stat stat, EffectProperties effects)
		{
			StatObject t = GetTarget(effects);

			if (t is Unit && ((stat.Id == "health" && effects.value < 0) || (stat.Id == "health" && effects.value > 0 && effects.damageType == DamageType.Heal)))
				return;
			if (effects.returnEffects)
			{
				if (effects.setStat)
					t.ChangeStatTo(stat, effects.startingValue);
				else
				{
					int ticks = effects.currentTick + 1;
					for (int i = 0; i < ticks; i++)
					{
						if (effects.multiplyStat)
							t.ChangeStat(stat, -effects.valueDifference);
						else
							t.ChangeStat(stat, -effects.value);
					}
				}
			}
		}
		
		/// <summary>
		/// Removes the <see cref="StatEffect"/>.
		/// </summary>
		public void Remove()
		{
			Dispose();
			foreach(KeyValuePair<Stat, EffectProperties> statEffectPair in stats)
			{
				Stat stat = statEffectPair.Key;
				EffectProperties effects = statEffectPair.Value;
				ReturnEffects(stat, effects);
			}
		}
		
		/// <summary>
		/// Disposes the <see cref="StatEffect"/> 
		/// </summary>
		private void Dispose()
		{
			StopAllCoroutines();
			CancelInvoke();
			target.RemoveEffect(this);
			Destroy(this);
		}
		
		/// <summary>
		/// Returns the <see cref="StatEffect"/>'s target.
		/// </summary>
		/// <returns>The target.</returns>
		/// <param name="effects">Effects.</param>
		private StatObject GetTarget(EffectProperties effects)
		{
			StatObject statObject = target;
			if (effects.child != "")
			{
				Transform child = target.transform.FindChild(effects.child);
				if (child == null)
					Debug.LogError(target + " has not child with name: " + effects.child);
				statObject = child.GetComponent<StatObject>();
				if (statObject == null)
					Debug.LogError(target + "'s child (" + child.name + ") has no StatObject script component!");
			}
			return statObject;
		}
		
		/// <summary>
		/// A class containing Effect properties.
		/// </summary>
		internal class EffectProperties
		{
			#region Properties
			internal DamageType damageType;
			internal float value;
			internal float duration;
			internal float delay;
			internal float tick;
			internal bool returnEffects;
			internal bool setStat;
			internal bool multiplyStat;
			internal string child;
			#endregion
			
			internal float startingValue;
			internal float valueDifference;
			
			internal int tickTimes = 0;
			internal int currentTick = 0;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="Stats.StatEffect+EffectProperties"/> class.
			/// </summary>
			/// <param name="properties">JSON.</param>
			internal EffectProperties(JSONNode properties)
			{
				this.damageType = properties["damageType"].Value == "" ? DamageType.NoResistance : (DamageType)System.Enum.Parse(typeof(DamageType), properties["damageType"].Value);
				this.value = properties["value"].AsFloat;
				this.duration = properties["duration"].AsFloat;
				this.delay = properties["delay"].AsFloat;
				this.tick = properties["tick"].AsFloat;
				this.returnEffects = properties["return"].Value == "" ? true : properties["return"].AsBool;
				this.setStat = properties["set"].AsBool;
				this.multiplyStat = properties["multiply"].AsBool;
				this.child = properties["child"].Value;
				
				this.startingValue = this.value;
				
				if (this.duration != 0 && this.tick != 0 && !this.setStat)
					this.tickTimes = (int)((duration - delay) / tick);
			}
			
			/// <summary>
			/// Returns a <see cref="System.String"/> that represents the current <see cref="Stats.StatEffect+EffectProperties"/>.
			/// </summary>
			/// <returns>A <see cref="System.String"/> that represents the current <see cref="Stats.StatEffect+EffectProperties"/>.</returns>
			public override string ToString()
			{
				return string.Format("[EffectProperties: value={0}, duration={1}, delay={2}, tick={3}, return={4}, set={5}, multiply={6}]", value, duration, delay, tick, returnEffects, setStat, multiplyStat);
			}
		}
	}
}