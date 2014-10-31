using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Utils.Loaders;

public class StatEffect : MonoBehaviour
{
	public const string EFFECTS_PATH = "Stats/Effects"; // JSON: Resources/Data/JSON/[PATH]
	
	public StatObject target;
	public string id;
	
	private List<KeyValuePair<Stat, EffectProperties>> stats;
	
	protected static JSONNode json;
	protected JSONNode properties;
	
	public void Init(string type, string category, string effectsFilePath = EFFECTS_PATH)
	{
		json = JSONLoader.Load(effectsFilePath);
		properties = json[category][type];

		EffectProperties effectProps = null;
		
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
		
		if (totalDuration != 0)
			Invoke("Dispose", totalDuration + 0.1f);
	}
	
	internal IEnumerator ApplyFirst(Stat stat, EffectProperties effects)
	{
		if (effects.delay != 0)
			yield return new WaitForSeconds(effects.delay);

		StatObject t = GetTarget(effects);

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
	
	internal IEnumerator Apply(Stat stat, EffectProperties effects)
	{
		yield return new WaitForSeconds(effects.tick);
		effects.currentTick++;

		StatObject t = GetTarget(effects);
		
		if (effects.multiplyStat)
			t.ChangeStat(stat, -effects.valueDifference);
		float previousValue = stat.Current;
		t.ChangeStat(stat, effects.value, effects.multiplyStat);
		effects.valueDifference = stat.Current - previousValue;
		
		if (effects.currentTick != effects.tickTimes - 1)
			StartCoroutine(Apply(stat, effects));
	}
	
	internal IEnumerator End(Stat stat, EffectProperties effects)
	{
		yield return new WaitForSeconds(effects.duration + effects.delay);
		ReturnEffects(stat, effects);
	}
	
	private void ReturnEffects(Stat stat, EffectProperties effects)
	{
		if (effects.returnEffects)
		{
			StatObject t = GetTarget(effects);

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
	
	private void Dispose()
	{
		StopAllCoroutines();
		CancelInvoke();
		target.RemoveEffect(this);
		Destroy(this);
	}

	private StatObject GetTarget(EffectProperties effects)
	{
		return effects.child == "" ? target : target.transform.FindChild(effects.child).GetComponent<StatObject>();
	}
	
	internal class EffectProperties
	{
		internal float value;
		internal float duration;
		internal float delay;
		internal float tick;
		internal bool returnEffects;
		internal bool setStat;
		internal bool multiplyStat;
		internal string child;
		
		internal float startingValue;
		internal float valueDifference;

		internal int tickTimes = 0;
		internal int currentTick = 0;
		
		internal EffectProperties(JSONNode properties)
		{
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
		
		public override string ToString()
		{
			return string.Format("[EffectProperties: value={0}, duration={1}, delay={2}, tick={3}, return={4}, set={5}, multiply={6}]", value, duration, delay, tick, returnEffects, setStat, multiplyStat);
		}
	}
}
