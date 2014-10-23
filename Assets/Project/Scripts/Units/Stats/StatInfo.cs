using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	[System.Serializable]
	public class StatInfo
	{
		public string id;
		public float baseValue;
		public float min;
		public float max;
		
		public StatInfo(string id, float value, float min = float.MinValue, float max = float.MaxValue)
		{
			Init(id, value, min, max);
		}

		public void Init(string id, float value, float min = float.MinValue, float max = float.MaxValue)
		{
			this.id = id;
			this.baseValue = value;
			this.min = min;
			this.max = max;
		}
	}
}