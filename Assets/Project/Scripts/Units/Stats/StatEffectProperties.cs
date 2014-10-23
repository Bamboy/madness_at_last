using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public class StatEffectProperties : MonoBehaviour
	{
		[Tooltip("Unique id")]
		public string statId;
		[Tooltip("Value")]
		public float value;
		[Tooltip("Duration")]
		public float duration;
		[Tooltip("Start delay")]
		public float delay;
		[Tooltip("Tick time")]
		public float tick;
		[Tooltip("If effects should return")]
		public bool returnEffects;
		[Tooltip("If it should set to value instead of incrementing")]
		public bool setStat;
		[Tooltip("If it should multiply instead of incrementing")]
		public bool multiplyStat;
		
		internal float startingValue;
		internal float valueDifference;
		
		internal int tickTimes = 0;
		internal int currentTick = 0;
		
		void Awake()
		{
			this.startingValue = this.value;
			
			if (this.duration != 0 && this.tick != 0 && !this.setStat)
				this.tickTimes = (int)((duration - delay) / tick);
		}
	}
}
