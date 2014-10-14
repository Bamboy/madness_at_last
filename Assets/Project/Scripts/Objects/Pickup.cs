using UnityEngine;

/// By: Cristian "vozochris" Vozoca
public class Pickup : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
			OnPlayerEnter(collider.gameObject);
	}

	protected virtual void OnPlayerEnter(GameObject player)
	{
		Destroy(gameObject);
	}
}
