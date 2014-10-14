using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public class StatEffect : MonoBehaviour
	{
		[HideInInspector]
		public StatObject target;
		[Tooltip("Unique id")]
		public string id;
		
		private List<KeyValuePair<Stat, StatEffectProperties>> stats = new List<KeyValuePair<Stat, StatEffectProperties>>();
		
		public void Init()
		{
			foreach(StatEffectProperties statEffectProperties in GetComponents<StatEffectProperties>())
				stats.Add(new KeyValuePair<Stat, StatEffectProperties>(target.GetStat(statEffectProperties.statId), statEffectProperties));

			float totalDuration = 0;
			foreach(KeyValuePair<Stat, StatEffectProperties> statEffectPair in stats)
			{
				Stat stat = statEffectPair.Key;
				StatEffectProperties effects = statEffectPair.Value;
				
				StartCoroutine(ApplyFirst(stat, effects));
				if (effects.duration != -1)
					StartCoroutine(End(stat, effects));
				else
					totalDuration = -1;
				
				if (totalDuration != -1)
					totalDuration = Mathf.Max(totalDuration, effects.duration + effects.delay);
				
			}
			
			if (totalDuration != -1)
				Invoke("Dispose", totalDuration + 0.1f);
		}
		
		internal IEnumerator ApplyFirst(Stat stat, StatEffectProperties effects)
		{
			if (effects.delay != 0)
				yield return new WaitForSeconds(effects.delay);
			
			if (effects.setStat)
				target.ChangeStatTo(stat, effects.value);
			else
			{
				float previousValue = stat.Current;
				target.ChangeStat(stat, effects.value, effects.multiplyStat);
				effects.valueDifference = stat.Current - previousValue;
				
				if (effects.tickTimes != 0)
					StartCoroutine(Apply(stat, effects));
			}
		}
		
		internal IEnumerator Apply(Stat stat, StatEffectProperties effects)
		{
			yield return new WaitForSeconds(effects.tick);
			effects.currentTick++;
			
			if (effects.multiplyStat)
				target.ChangeStat(stat, -effects.valueDifference);
			float previousValue = stat.Current;
			target.ChangeStat(stat, effects.value, effects.multiplyStat);
			effects.valueDifference = stat.Current - previousValue;
			
			if (effects.currentTick != effects.tickTimes - 1)
				StartCoroutine(Apply(stat, effects));
		}
		
		internal IEnumerator End(Stat stat, StatEffectProperties effects)
		{
			yield return new WaitForSeconds(effects.duration + effects.delay);
			ReturnEffects(stat, effects);
		}
		
		private void ReturnEffects(Stat stat, StatEffectProperties effects)
		{
			if (effects.returnEffects)
			{
				if (effects.setStat)
					target.ChangeStatTo(stat, effects.startingValue);
				else
				{
					int ticks = effects.currentTick + 1;
					for (int i = 0; i < ticks; i++)
					{
						if (effects.multiplyStat)
							target.ChangeStat(stat, -effects.valueDifference);
						else
							target.ChangeStat(stat, -effects.value);
					}
				}
			}
		}
		
		public void Remove()
		{
			Dispose();
			foreach(KeyValuePair<Stat, StatEffectProperties> statEffectPair in stats)
			{
				Stat stat = statEffectPair.Key;
				StatEffectProperties effects = statEffectPair.Value;
				ReturnEffects(stat, effects);
			}
		}
		
		private void Dispose()
		{
			StopAllCoroutines();
			CancelInvoke();
			target.RemoveEffect(this);
			Destroy(gameObject);
		}
	}
}