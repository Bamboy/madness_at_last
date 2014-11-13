using UnityEngine;
using System.Collections;

namespace Excelsion.Objects
{
	public class SpawnMessage : MonoBehaviour {
		public GameObject enemy;
		public int SpawnNumber;
		private int multiplier;
		private Vector3 position;

		void Enemy(int count){
			if(count == SpawnNumber){
				Instantiate(enemy, position, Quaternion.identity);
			}
		}
		void Increase(int multi){
			position.x += multi;
		}
	}
}