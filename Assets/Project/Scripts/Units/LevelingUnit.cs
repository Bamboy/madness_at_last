using UnityEngine;

//By Cristian "vozochris" Vozoca
namespace Stats
{
	public class LevelingUnit : AbilityUnit
	{
		public Stat level;
		public Stat experience;

		protected override void Init(string statsFilePath, params string[] statsJSONPath)
		{
			base.Init(statsFilePath, statsJSONPath);

			level = AddStat("level", 1, 1, 20);
			experience = AddStat("experience", 0, 0);
			experience.OnChangeCurrent = delegate(Stat obj) {
				CheckExperience();
			};
		}

		protected virtual void LevelUp()
		{
			level.Current++;
		}

		public void GainExperience(float amount)
		{
			experience.Current += amount;
		}

		private void CheckExperience()
		{
			float nextLevelExp = GetNextLevelExperience();
			while(experience >= nextLevelExp)
			{
				experience.Current -= nextLevelExp;
				LevelUp();

				nextLevelExp = GetNextLevelExperience();
			}
		}

		private float GetNextLevelExperience()
		{
			return level * 100;// TODO - Experience formula
		}
	}
}