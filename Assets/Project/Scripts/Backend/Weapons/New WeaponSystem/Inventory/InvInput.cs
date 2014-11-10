using UnityEngine;
using System.Collections;

namespace Excelsion.WeaponSystem
{
	public class InvInput : MonoBehaviour {
		public GunInventory inv;
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			inv.InputFire = Input.GetMouseButton(0);
			inv.InputReload = Input.GetKey( KeyCode.R );
		}
	}
}
