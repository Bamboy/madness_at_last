using UnityEngine;
using Stats;

//By Cristian "vozochris" Vozoca
namespace Excelsion.Objects.Pickups
{
	public enum Drug
	{
		Cocaine, Heroin
	}
	public class DrugPickup : Pickup
	{
		public Drug drug;

		protected override void OnPlayerEnter(GameObject player)
		{
			base.OnPlayerEnter(player);
			//Adds the Drug's Stat effects to the Player
			player.GetComponent<StatObject>().AddEffect(drug.ToString(), "Drugs", "Stats/Effects/Pickups");
		}
	}
}