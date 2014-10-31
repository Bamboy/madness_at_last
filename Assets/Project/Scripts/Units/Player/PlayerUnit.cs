using UnityEngine;

// By: Cristian "vozochris" Vozoca
namespace Stats
{
	[System.Serializable]
	public class PlayerUnit : Unit
	{
		public static PlayerUnit instance;

		protected Stat movementSpeed;

		private FirstPersonDrifter fpDrifter;
		public bool isDead;

		protected override void Awake()
		{
			instance = this;
			base.Awake();

			movementSpeed = GetStat("movementSpeed");

			fpDrifter = GetComponent<FirstPersonDrifter>();
			movementSpeed.OnChangeCurrent = delegate(Stat stat) {
				fpDrifter.walkSpeed = stat.Current;
			};
		}

		public override void Die (System.Object source = null)
		{
			print("Died from " + source);
			isDead = true;
			//TODO - Player death script
		}
	}
}
