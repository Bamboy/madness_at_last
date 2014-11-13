using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;
using Stats;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/13/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem.Weapons
{
	public class Shotgun : GunRay 
	{
		#region GunBase variables
		//Make 'fake' inspector variables here, then set them to the real ones in UpdateSettings()
		//Effects
		public GameObject muzzleFlashEffect;
		public AudioClip shotFiredAudio;
		public Transform origin;
		
		public int maxAmmo = 48;
		public int clipSize = 8;
		[Range(0.0f, 10.0f)]public float accuracy = 1.0f;
		public float reloadTime = 1.25f;
		public float fireRate = 1.15f;
		
		#endregion
		#region GunRay variables
		//Same here...
		public DamageType damageType = DamageType.Physical;
		public Vector2 damageRange = new Vector2( 5, 8 );
		public int bulletsPerShot = 12;
		public float maximumDistance = 50.0f;
		public LayerMask filter;
		#endregion
		
		public int debugAmmo;
		public int debugClipAmmo;
		void Update()
		{
			debugAmmo = Ammo;
			debugClipAmmo = ClipAmmo;
		}
		
		//These two should not be modified in the inspector, only by scripts.
		//int ammo;
		//int clipAmmo;
		
		//Update all of our settings. A script should call this if it changes anything.
		public override void UpdateSettings()
		{
			base.UpdateSettings();
			
			Origin = this.origin;
			MaxAmmo = this.maxAmmo;
			ClipSize = this.clipSize;
			Accuracy = this.accuracy;
			ReloadTime = this.reloadTime;
			FireRate = this.fireRate;
			
			DamageEnum = this.damageType;
			DamageRange = this.damageRange;
			BulletsPerShot = this.bulletsPerShot;
			MaxDistance = this.maximumDistance;
			Filter = this.filter;
		}
		public override void UpdateEffectSettings()
		{
			muzzleFlashEffect = muzzleFlashEffect;
			ShotFiredAudio = shotFiredAudio;
		}
		
		
		protected override void Start ()
		{
			UpdateSettings();
			UpdateEffectSettings();
			base.Start();
			
			ClipAmmo = 500;
			Ammo = 500;
		}
		
		
		
		public override bool InputFire 
		{
			set{
				if( CanFire() )
				{
					if( FireType.SemiAuto( value, _lastFireInput ) ) //This gun uses Automatic
					{
						_lastFireInput = value;
						Fire();
					}
					else
						_lastFireInput = value;
				}
				else
					_lastFireInput = value;
			}
		}
		public override void Fire()
		{
			base.Fire();
			Debug.Log("Rifle fire");
		}
	}
}