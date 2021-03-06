﻿using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;
using Stats;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem.Weapons
{
	public class Pistol : GunRay
	{
		#region GunBase variables
		//Make 'fake' inspector variables here, copy them to the real ones in UpdateSettings()
		//Effects
		public GameObject muzzleFlashEffect;
		public AudioClip shotFiredAudio;
		public Transform origin;

		public bool infiniteAmmo = false;
		public int maxAmmo = 60;
		public int clipSize = 8;
		[Range(0.0f, 10.0f)]public float accuracy = 1.0f;
		public float reloadTime = 1.25f;
		public float fireRate = 0.25f;

		#endregion
		#region GunRay variables
		//Same here...
		public DamageType damageType = DamageType.Physical;
		public Vector2 damageRange = new Vector2( 3, 5 );
		public int bulletsPerShot = 1;
		public float maximumDistance = 9999.9999f;
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

			InfiniteAmmo = this.infiniteAmmo;
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
			base.UpdateEffectSettings();
			MuzzleEffectPrefab = muzzleFlashEffect;
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
					if( FireType.SemiAuto( value, _lastFireInput ) ) //This gun uses SemiAuto
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
			base.Fire ();
		}
		
	}
}

//if( TryFire() == true )
//	_lastFireInput = value;
//else
//	_lastFireInput = false; //We can't fire, so set our fire input to false so we will retry the next time this is called.