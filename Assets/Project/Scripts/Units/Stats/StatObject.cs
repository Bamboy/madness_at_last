using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	[System.Serializable]
	public class StatObject : MonoBehaviour
	{
		public List<StatInfo> statsInspector;

		private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();
		private Dictionary<string, StatEffect> effects = new Dictionary<string, StatEffect>();

		public void Init()
		{
			foreach(StatInfo statInfo in statsInspector)
				AddStat(statInfo);
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
			
			s = gameObject.AddComponent<Stat>();
			s.Init(id, value, min, max);
			stats.Add(id, s);
			return s;
		}
		
		public Stat AddStat(Stat stat)
		{
			Stat s = AddStat(stat.Id, stat.Base, stat.Min, stat.Max);
			s.Current = stat.Current;
			return s;
		}

		public Stat AddStat(StatInfo statInfo)
		{
			return AddStat(statInfo.id, statInfo.baseValue, statInfo.min, statInfo.max);
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
		
		public void AddEffect(string id)
		{
			AddEffect(Resources.Load<GameObject>("Prefabs/Stats/Effects/" + id));
		}

		public void AddEffect(GameObject effectObj)
		{
			effectObj = (GameObject)(Instantiate(effectObj));
			StatEffect effect = effectObj.GetComponent<StatEffect>();
			effect.transform.parent = transform;
			effect.target = this;
			effect.id = effectObj.name;
			
			if (effects.ContainsKey(effect.id))
			{
				effects[effect.id].Remove();
				effects.Remove(effect.id);
			}
			
			effects.Add(effect.id, effect);
			effect.Init();
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
}