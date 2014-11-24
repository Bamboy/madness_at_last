using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/13/14     ////
///////////////////////////////////

//Messages sent:
//OnGunPrefabFired();

namespace Excelsion.WeaponSystem
{
	//Inherit from this class if you want your gun to fire prefabs instead of using raycasts. The prefab spawned will be in charge of dealing damage.
	public abstract class GunPrefab : GunBase 
	{
		internal GameObject internal_prefab;
		internal float internal_spawnDistance;
		internal float internal_force;

		public GameObject ProjectilePrefab{ get{ return internal_prefab; } set{ internal_prefab = value; } }
		public float SpawnDistance{ get{ return internal_spawnDistance; } set{ internal_spawnDistance = value; } }
		public float Force{ get{ return internal_force; } set{ internal_force = value; } }

		protected override void Start () 
		{
			base.Start();
		}

		public override void UpdateSettings(){}
		public override void UpdateEffectSettings(){}
		public override bool InputFire{ set{ _lastFireInput = value; Debug.Log("Gunprefab input fire"); } }


		public override void Fire()
		{
			base.Fire();
			FirePrefab ();
			transform.root.gameObject.BroadcastMessage("OnGunPrefabFired", null, SendMessageOptions.DontRequireReceiver); 
		}
		private void FirePrefab()
		{
			Vector3 bulletDirection = VectorExtras.DirectionalCone( internal_origin.position, internal_origin.forward, Accuracy );

			//Gets a point in front of the camera.
			Vector3 spawnPos = VectorExtras.OffsetPosInDirection( internal_origin.position, bulletDirection, internal_spawnDistance );



			GameObject projectile = (GameObject)Instantiate( internal_prefab, spawnPos, internal_origin.rotation );
			//projectile.transform.rotation = internal_origin.rotation;

			if( projectile.rigidbody != null && internal_force != 0.0f )
			{
				projectile.rigidbody.AddForce( bulletDirection * internal_force, ForceMode.Impulse );
			}

		}

















	}
}