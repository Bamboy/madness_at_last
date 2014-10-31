using UnityEngine;

/// By: Cristian "vozochris" Vozoca
public class Pickup : MonoBehaviour
{
	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
			OnPlayerEnter(c.gameObject);
	}

	protected virtual void OnPlayerEnter(GameObject player)
	{
		Destroy(gameObject);
	}
}
