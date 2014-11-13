using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/13/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem.Weapons
{
	public class RPG : GunPrefab 
	{
		//GunBase settings
		public GameObject muzzleFlashEffect;
		public AudioClip shotFiredAudio;
		public Transform origin;
		
		public int maxAmmo = 5;
		public int clipSize = 1;
		[Range(0.0f, 10.0f)]public float accuracy = 0.23f;
		public float reloadTime = 1.75f;
		public float fireRate = 1.0f;

		//GunPrefab settings
		public GameObject prefab;
		public float spawnDistance = 0.65f;
		public float force = 0.0f;

		#region Setting Updates
		public override void UpdateSettings ()
		{
			base.UpdateSettings();
			Origin = this.origin;
			MaxAmmo = this.maxAmmo;
			ClipSize = this.clipSize;
			Accuracy = this.accuracy;
			ReloadTime = this.reloadTime;
			FireRate = this.fireRate;

			ProjectilePrefab = this.prefab;
			SpawnDistance = this.spawnDistance;
			Force = this.force;
		}
		public override void UpdateEffectSettings()
		{
			base.UpdateEffectSettings();
			muzzleFlashEffect = this.muzzleFlashEffect;
			ShotFiredAudio = this.shotFiredAudio;
		}
		#endregion

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
			Debug.Log("RPG fire");
		}
	}
}