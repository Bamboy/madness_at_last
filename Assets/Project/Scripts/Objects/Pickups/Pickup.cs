using UnityEngine;

/// By: Cristian "vozochris" Vozoca
namespace Excelsion.Objects.Pickups
{
	[RequireComponent(typeof(Collider))]
	public abstract class Pickup : MonoBehaviour
	{
		private void OnTriggerEnter(Collider c)
		{
			if (c.tag == "Player")
				OnPlayerEnter(c.gameObject);
		}

		protected virtual void OnPlayerEnter(GameObject player)
		{
			Destroy(gameObject);
		}
	}
}
