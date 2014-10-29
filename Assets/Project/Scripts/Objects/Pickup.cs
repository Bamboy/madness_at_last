using UnityEngine;

/// By: Cristian "vozochris" Vozoca
public class Pickup : MonoBehaviour
{
	void OnCollisionEnter(Collision c)
	{
		if (c.collider.tag == "Player")
			OnPlayerEnter(c.collider.gameObject);
	}

	protected virtual void OnPlayerEnter(GameObject player)
	{
		Destroy(gameObject);
	}
}
