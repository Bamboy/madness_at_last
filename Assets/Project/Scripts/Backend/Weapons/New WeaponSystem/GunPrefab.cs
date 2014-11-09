using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/04/14     ////
///////////////////////////////////

//Messages sent:
//OnGunPrefabFired();

namespace Excelsion.WeaponSystem
{
	//Inherit from this class if you want your gun to fire prefabs instead of using raycasts.
	public abstract class GunPrefab : GunBase 
	{
		internal float force = 0.0f;

		protected override void Start () 
		{
			base.Start();
		}


		public override void Fire()
		{
			base.Fire();
		}

	}
}