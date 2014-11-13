using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using Utils.Loaders;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	/// <summary>
	/// Base Object that can have <see cref="Stats"/> and <see cref="StatEffects"/>.
	/// More info about writing the JSON data in Readme.txt file, Samples and Demo.
	/// </summary>
	public class StatObject : MonoBehaviour
	{
		private Dictionary<string, Stat> stats = new Dictionary<string, Stat>();
		private Dictionary<string, StatEffect> effects = new Dictionary<string, StatEffect>();
		
		protected JSONNode json;
		protected JSONNode parentJSON;

		/// <summary>
		/// Initializes the <see cref="StatObject"/>.
		/// </summary>
		/// <param name="statsFilePath">Path of the JSON file. Inside Resources folder, default JSONLoader path is prefixed with: <see cref="Data/JSON/"/></param>
		/// <param name="statsJSONPath">Path of the Stats inside the JSON file.</param>
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

		/// <summary>
		/// Increments or Multiplies the <see cref="Stat"/>.
		/// </summary>
		/// <param name="stat"><see cref="Stat"/>.</param>
		/// <param name="value">Value.</param>
		/// <param name="multiplication">If set to <c>true</c> use multiplication otherwise addition.</param>
		public virtual void ChangeStat(Stat stat, float value, bool multiplication = false)
		{
			if (multiplication)
				stat.Current = stat * value;
			else
				stat.Current = stat + value;
		}

		/// <summary>
		/// Increments or Multiplies the <see cref="Stat"/>.
		/// </summary>
		/// <param name="statId"><see cref="Stat"/> ID.</param>
		/// <param name="value">Value.</param>
		/// <param name="multiplication">If set to <c>true</c> use multiplication otherwise addition.</param>
		public virtual void ChangeStat(string statId, float value, bool multiplication = false)
		{
			ChangeStat(GetStat(statId), value, multiplication);
		}

		/// <summary>
		/// Changes the <see cref="Stat"/>'s Current value to a given value.
		/// </summary>
		/// <param name="stat"><see cref="Stat"/>.</param>
		/// <param name="value">Value.</param>
		public virtual void ChangeStatTo(Stat stat, float value)
		{
			ChangeStat(stat, value - stat);
		}

		/// <summary>
		/// Changes the <see cref="Stat"/>'s Current value to a given value.
		/// </summary>
		/// <param name="statId"><see cref="Stat"/> ID.</param>
		/// <param name="value">Value.</param>
		public virtual void ChangeStatTo(string statId, float value)
		{
			ChangeStatTo(GetStat(statId), value);
		}

		/// <summary>
		/// Adds the <see cref="Stat"/>.
		/// </summary>
		/// <returns>The <see cref="Stat"/>.</returns>
		/// <param name="id">ID.</param>
		/// <param name="value">Base and Curent value.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value.</param>
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
			stats[id] = s;
			return s;
		}

		/// <summary>
		/// Adds the <see cref="Stat"/>.
		/// </summary>
		/// <returns>The <see cref="Stat"/>.</returns>
		/// <param name="stat"><see cref="Stat"/>.</param>
		public Stat AddStat(Stat stat)
		{
			Stat s = AddStat(stat.Id, stat.Base, stat.Min, stat.Max);
			s.Current = stat.Current;
			return s;
		}

		/// <summary>
		/// Gets the <see cref="Stat"/> by ID.
		/// </summary>
		/// <returns>The <see cref="Stat"/>.</returns>
		/// <param name="id"><see cref="Stat"/> ID.</param>
		public Stat GetStat(string id)
		{
			if (!HasStat(id))
			{
				Debug.LogWarning("There is no Stat with id: " + id);
				return null;
			}
			return stats[id];
		}

		/// <summary>
		/// If it has the <see cref="Stat"/>.
		/// </summary>
		/// <returns>If the <see cref="Stat"/> exists <c>True</c> otherwise <c>False</c></returns>
		/// <param name="id"><see cref="Stat"/> ID.</param>
		public bool HasStat(string id)
		{
			return stats.ContainsKey(id);
		}

		/// <summary>
		/// Removes the <see cref="Stat"/> by ID.
		/// </summary>
		/// <param name="id"><see cref="Stat"/> ID.</param>
		public void RemoveStat(string id)
		{
			stats.Remove(id);
		}

		/// <summary>
		/// Removes the <see cref="Stat"/>.
		/// </summary>
		/// <param name="stat"><see cref="Stat"/>.</param>
		public void RemoveStat(Stat stat)
		{
			RemoveStat(stat.Id);
		}

		/// <summary>
		/// Removes the <see cref="Stats"/>.
		/// </summary>
		public void RemoveStats()
		{
			RemoveEffects();
			stats.Clear();
		}

		/// <summary>
		/// Adds the effect.
		/// </summary>
		/// <returns>The effect.</returns>
		/// <param name="type">Type.</param>
		/// <param name="category">Category.</param>
		/// <param name="effectsFilePath">Effects file path.</param>
		public StatEffect AddEffect(string type, string category, string effectsFilePath = StatEffect.EFFECTS_PATH)
		{
			string id = category + "_" + type;
			
			StatEffect effect = gameObject.AddComponent<StatEffect>();
			effect.target = this;
			effect.id = id;
			
			effects[id] = effect;
			effect.Init(type, category, effectsFilePath);
			return effect;
		}

		/// <summary>
		/// Gets the effect by ID.
		/// </summary>
		/// <returns>The effect.</returns>
		/// <param name="id">Effect ID</param>
		public StatEffect GetEffect(string id)
		{
			if (!HasEffect(id))
			{
				Debug.LogWarning("There is no StatEffect with id: " + id);
				return null;
			}
			return effects[id];
		}

		/// <summary>
		/// If it has the <see cref="Effect"/>.
		/// </summary>
		/// <returns>If the <see cref="Effect"/> exists <c>True</c> otherwise <c>False</c></returns>
		/// <param name="id"><see cref="Effect"/> ID.</param>
		public bool HasEffect(string id)
		{
			return effects.ContainsKey(id);
		}

		/// <summary>
		/// Removes a <see cref="Stat"/> effect.
		/// Do not call this, call Effect's Remove instead.
		/// </summary>
		/// <param name="id"><see cref="Effect"/> ID.</param>
		internal void RemoveEffect(string id)
		{
			effects.Remove(id);
		}

		/// <summary>
		/// Removes a <see cref="Stat"/> effect.
		/// Do not call this, call Effect's Remove instead.
		/// </summary>
		/// <param name="effect">Effect.</param>
		internal void RemoveEffect(StatEffect effect)
		{
			RemoveEffect(effect.id);
		}

		/// <summary>
		/// Removes the <see cref="Stat"/> effects.
		/// </summary>
		public void RemoveEffects()
		{
			int le = effects.Count;
			for (int i = 0; i < le; i++)
			{
				effects.ElementAt(0).Value.Remove();
			}
		}

		/// <summary>
		/// Get the <see cref="Stat"/> by ID. Equivalent to <see cref="GetStat(id)"/>.
		/// </summary>
		/// <param name="id">ID.</param>
		public Stat this[string id]
		{
			get { return GetStat(id); }
		}

		/// <summary>
		/// Gets the stats count.
		/// </summary>
		/// <value>The stats count.</value>
		public int StatsCount
		{
			get { return stats.Count; }
		}

		/// <summary>
		/// Gets the effects count.
		/// </summary>
		/// <value>The effects count.</value>
		public int EffectsCount
		{
			get { return effects.Count; }
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Stats.StatObject"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Stats.StatObject"/>.</returns>
		public override string ToString()
		{
			return string.Format("[StatObject: {0}, Stats: {1}, Effects: {2}]", name, StatsCount, EffectsCount);
		}
	}
}