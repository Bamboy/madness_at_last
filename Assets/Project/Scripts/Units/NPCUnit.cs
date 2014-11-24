using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public class NPCUnit : Unit
	{
		//private string type;

		public void Init(string type)
		{
			//this.type = type;
			Init("Units/NPC", type);
		}

		private void Awake()
		{
			Init("basic");// Only one type of NPC currently exists.
		}

		public override void Die(System.Object source)
		{
			base.Die(source);
			GlobalStats.current.kills++;
		}
	}
}
