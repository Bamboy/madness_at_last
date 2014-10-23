using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public class NPCUnit : Unit
	{
		public override void Die(System.Object source)
		{
			base.Die(source);
			GlobalStats.current.kills++;
		}
	}
}
