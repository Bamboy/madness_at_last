using UnityEngine;
using System.Collections;

//Keeps track of our NPC so that it is removed once the player is too far away.
public class AIFactoryRemove : MonoBehaviour 
{
	private Main main;

	void Start()
	{
		main = Main.Get();
	}

	void Update()
	{
		if( AIFactory.aiCount >= (AIFactory.aiCountMax - 7) )
		{
			if( Random.value >= 0.95f )
			{
				float distance = Vector3.Distance( main.Player.position, transform.position );
				if( distance > AIFactory.spawnDistanceMax )
				{
					Destroy( gameObject );
				}
			}
		}
	}

	void OnDestroy()
	{
		AIFactory.aiCount -= 1;
	}
}
