using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using Utils.Loaders;

public class StatObject : MonoBehaviour
{
	private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();
	private Dictionary<string, StatEffect> effects = new Dictionary<string, StatEffect>();
	
	protected JSONNode json;
	protected JSONNode parentJSON;
	
	protected virtual void Init(string statsFilePath, params string[] statsJSONPath)
	{
		parentJSON = JSONLoader.Load(statsFilePath);
		
		json = parentJSON;
		foreach(string path in statsJSONPath)
		{
			json = json[path];
			if (json == null)
			{
				print("StatObject.Init Error: No path " + path + " in JSON.");
				return;
			}
		}
		
		foreach(KeyValuePair<string, JSONNode> stat in json.AsObject)
		{
			if (stat.Value.AsObject != null)
				continue;// Objects are paths, should not count as a Stat
			
			JSONArray statArr = stat.Value.AsArray;
			if (statArr != null)
			{
				string strVal = statArr[0].Value;
				float val = strVal == "min" ? float.MinValue : (strVal == "max" ? float.MaxValue : statArr[0].AsFloat);
				strVal = statArr[1].Value;
				float minVal = strVal == "min" ? float.MinValue : (strVal == "max" ? float.MaxValue : statArr[1].AsFloat);
				strVal = statArr[2].Value;
				float maxVal = strVal == "min" ? float.MinValue : (strVal == "max" ? float.MaxValue : statArr[2].AsFloat);
				AddStat(stat.Key, val, minVal, maxVal);
			}
			else
				AddStat(stat.Key, stat.Value.AsFloat);
		}
	}
	
	public virtual void ChangeStat(Stat stat, float value, bool multiplication = false)
	{
		if (multiplication)
			stat.Current = stat * value;
		else
			stat.Current = stat + value;
	}
	
	public virtual void ChangeStat(string statId, float value, bool multiplication = false)
	{
		ChangeStat(GetStat(statId), value, multiplication);
	}
	
	public virtual void ChangeStatTo(Stat stat, float value)
	{
		ChangeStat(stat, value - stat);
	}
	
	public virtual void ChangeStatTo(string statId, float value)
	{
		ChangeStatTo(GetStat(statId), value);
	}
	
	public Stat AddStat(string id, float value = 0, float min = float.MinValue, float max = float.MaxValue)
	{
		Stat s;
		
		if (stats.ContainsKey(id))
		{
			s = stats[id];
			s.Current = s.Base = value;
			s.Min = min;
			s.Max = max;
			return s;
		}
		
		s = new Stat(id, value, min, max);
		stats.Add(id, s);
		return s;
	}
	
	public Stat AddStat(Stat stat)
	{
		Stat s = AddStat(stat.Id, stat.Base, stat.Min, stat.Max);
		s.Current = stat.Current;
		return s;
	}
	
	public Stat GetStat(string id)
	{
		if (!stats.ContainsKey(id))
		{
			Debug.LogWarning("There is no Stat with id: " + id);
			return null;
		}
		return stats[id];
	}
	
	public bool HasStat(string id)
	{
		return stats.ContainsKey(id);
	}
	
	public void RemoveStat(string id)
	{
		stats.Remove(id);
	}
	
	public void RemoveStat(Stat stat)
	{
		RemoveStat(stat.Id);
	}
	
	public void RemoveStats()
	{
		RemoveEffects();
		stats.Clear();
	}
	
	public StatEffect AddEffect(string type, string category, string effectsFilePath = StatEffect.EFFECTS_PATH)
	{
		StatEffect effect = gameObject.AddComponent<StatEffect>();
		effect.target = this;
		effect.id = category + "_" + type;
		
		if (effects.ContainsKey(effect.id))
		{
			effects[effect.id].Remove();
			effects.Remove(effect.id);
		}
		
		effects.Add(effect.id, effect);
		effect.Init(type, category, effectsFilePath);
		return effect;
	}
	
	public StatEffect GetEffect(string id)
	{
		if (!effects.ContainsKey(id))
		{
			Debug.LogWarning("There is no StatEffect with id: " + id);
			return null;
		}
		return effects[id];
	}
	
	internal void RemoveEffect(string id)
	{
		effects.Remove(id);
	}
	
	internal void RemoveEffect(StatEffect effect)
	{
		RemoveEffect(effect.id);
	}
	
	public void RemoveEffects()
	{
		int le = effects.Count;
		for (int i = 0; i < le; i++)
		{
			effects.ElementAt(0).Value.Remove();
		}
	}
	
	public Stat this[string id]
	{
		get { return GetStat(id); }
	}
	
	public override string ToString()
	{
		return string.Format("[StatObject: {0}, Stats: {1}, Effects: {2}]", name, stats.Count, effects.Count);
	}
}