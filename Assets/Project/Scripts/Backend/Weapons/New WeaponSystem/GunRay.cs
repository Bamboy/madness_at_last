using UnityEngine;
using System.Collections;
using Stats;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////

//Messages sent:
//OnGunRaycastFired( BulletInfo[] bullets );

namespace Excelsion.WeaponSystem
{
	//Inherit from this class if you want your gun to use raycasts instead of firing prefabs.
	public abstract class GunRay : GunBase
	{
		#region Variables
		internal DamageType internal_damageType;
		internal Vector2 internal_damageRange;
		internal int internal_bulletsPerShot;
		internal float internal_distance;
		internal LayerMask internal_objectsToHit;
		#endregion
		#region Accessors
		public DamageType DamageEnum{ get{ return internal_damageType; } set{internal_damageType = value;} }
		public Vector2 DamageRange{ get{ return internal_damageRange; } set{internal_damageRange = value;} }
		public int BulletsPerShot{ get{ return internal_bulletsPerShot; } set{internal_bulletsPerShot = Mathf.Abs( value );} }
		public float MaxDistance{ get{ return internal_distance; } set{internal_distance = Mathf.Abs( value );} }
		public LayerMask Filter{ get{ return internal_objectsToHit; } set{ internal_objectsToHit = value; } }
		#endregion


		protected override void Start () 
		{
			base.Start();
		}

		public override void UpdateSettings(){}
		public override void UpdateEffectSettings(){}
		public override bool InputFire{ set{ _lastFireInput = value; Debug.Log("Gunray input fire"); } }

		public override void Fire()
		{
			base.Fire();
			Debug.Log("Gunray fire");
			BulletInfo[] bullets = FireAsRaycast();
			//Sends a message out to all attached gameobjects.
			transform.root.gameObject.BroadcastMessage("OnGunRaycastFired", bullets, SendMessageOptions.DontRequireReceiver ); 
		}


		private BulletInfo[] FireAsRaycast () //Return array of raycasthit so we can apply damage to all hit objects.
		{
			RaycastHit[] rays = new RaycastHit[ internal_bulletsPerShot ];
			BulletInfo[] bullets = new BulletInfo[ internal_bulletsPerShot ];

			Vector3 pos = Origin.position; Vector3 dir = Origin.forward;
			if( internal_distance <= 0.0f ) internal_distance = 999999.999999f; //Get our distance and if its zero or less, set it to (near) infinity.
			for( int i = 0; i < internal_bulletsPerShot; i++ )
			{
				//Adds a randomized offset to the direction vector based on accuracy. 0 is no change, 10 is about 90 degrees.
				Vector3 bulletDirection = VectorExtras.DirectionalCone( pos, dir, Accuracy );
				Ray dataRay = new Ray( pos, bulletDirection );

				if( Physics.Raycast( pos, bulletDirection, out rays[i], internal_distance, internal_objectsToHit ) )
				{
					//Do shit
					bullets[i] = new BulletInfo( dataRay, rays[i] );

					//if( doDebug ) Debug.Log("Hit "+ rays[i].transform.name +"!", this);
				}
				
				//if( doDebug ) {
					Debug.DrawLine( pos, VectorExtras.OffsetPosInDirection(pos, bulletDirection, internal_distance), Color.red, 1.5f ); 
				//}
			}
			//if( recoilTransform != null )
			//{
			//	float totalRecoil = gunDefs.GetRecoil( weapon ) * (float)bulletsPerShot; //Multiply recoil by how many bullets we shot
			//	Quaternion recoilQuaternion = Quaternion.AngleAxis( totalRecoil, Vector3.left);
			//	recoilTransform.localRotation = recoilTransform.localRotation * recoilQuaternion;
			//}
			return bullets;
		}
	}
}