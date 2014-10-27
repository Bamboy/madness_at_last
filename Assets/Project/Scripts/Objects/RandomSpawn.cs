using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {
	protected int capEnemy = 10;
	private GameObject main;
	private bool CanSpawn;
	private int enemyCount;

	void Awake(){
		main = this.gameObject;
		Debug.Log(main.transform.childCount);
	}
	void Update(){
		if(enemyCount >= capEnemy){
			CanSpawn = false;
		} else {
			CanSpawn = true;
		}
		for(int s = 0; s < main.transform.childCount; s++){
			for(int i = 0; i <= capEnemy; i++){
				if(CanSpawn){
					enemyCount++;
					gameObject.BroadcastMessage("Enemy", s);
				}
			}
		}
	}
}