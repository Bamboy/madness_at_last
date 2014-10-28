using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	public class NPCUnit : Unit
	{
		public override void Die(System.Object source)
		{
			//AIFactory.aiCount -= 1;
			GlobalStats.current.kills++;
			base.Die(source);


		}
	}
}
