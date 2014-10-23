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
			//TODO - Player death script
		}
	}
}
