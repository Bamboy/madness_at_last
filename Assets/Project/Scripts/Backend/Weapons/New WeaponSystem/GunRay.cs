using UnityEngine;
using System.Collections;

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
		internal int bulletsPerShot = 1;
		internal float distance = 999999.999999f;
		internal LayerMask objectsToHit;
		#endregion
		#region Accessors
		public int BulletsPerShot{ get{ return bulletsPerShot; } set{bulletsPerShot = Mathf.Abs( value );} }
		public float MaxDistance{ get{ return distance; } set{distance = Mathf.Abs( value );} }
		public LayerMask Filter{ get{ return objectsToHit; } set{ objectsToHit = value; } }
		#endregion


		protected override void Start () 
		{
			base.Start();
		}


		public override void Fire()
		{
			base.Fire();

			BulletInfo[] bullets = FireAsRaycast();
			//Sends a message out to all attached gameobjects.
			transform.root.gameObject.BroadcastMessage("OnGunRaycastFired", bullets, SendMessageOptions.DontRequireReceiver ); 
		}


		private BulletInfo[] FireAsRaycast () //Return array of raycasthit so we can apply damage to all hit objects.
		{
			RaycastHit[] rays = new RaycastHit[ bulletsPerShot ];
			BulletInfo[] bullets = new BulletInfo[ bulletsPerShot ];

			Vector3 pos = origin.position; Vector3 dir = origin.forward;
			if( distance <= 0.0f ) distance = 999999.999999f; //Get our distance and if its zero or less, set it to (near) infinity.
			for( int i = 0; i < bulletsPerShot; i++ )
			{
				//Adds a randomized offset to the direction vector based on accuracy. 0 is no change, 10 is about 90 degrees.
				Vector3 bulletDirection = VectorExtras.DirectionalCone( pos, dir, accuracy );
				Ray dataRay = new Ray( pos, bulletDirection );

				if( Physics.Raycast( pos, bulletDirection, out rays[i], distance, objectsToHit ) )
				{
					//Do shit
					bullets[i] = new BulletInfo( dataRay, rays[i] );

					//if( doDebug ) Debug.Log("Hit "+ rays[i].transform.name +"!", this);
				}
				
				//if( doDebug )
				//{ Debug.DrawLine( pos, VectorExtras.OffsetPosInDirection(pos, bulletDirection, distance), Color.red, 1.5f ); }
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