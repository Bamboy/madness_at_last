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

		protected override void Init (string statsFilePath, params string[] statsJSONPath)
		{
			base.Init(statsFilePath, statsJSONPath);
			
			movementSpeed = GetStat("movementSpeed");

			// Every time movement speed is changed, change drifter's walk speed
			fpDrifter = GetComponent<FirstPersonDrifter>();
			movementSpeed.OnChangeCurrent = delegate(Stat stat) {
				fpDrifter.walkSpeed = stat.Current;
			};
			fpDrifter.walkSpeed = movementSpeed.Current;
		}

		private void Awake()
		{
			instance = this;
			Init("Units/Player");
		}

		public override void Die (System.Object source = null)
		{
			print("Died from " + source);
			isDead = true;
			//TODO - Player death script
		}
	}
}
