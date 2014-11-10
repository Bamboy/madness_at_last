using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////

//Messages sent:
//OnReloadStart( float time );
//OnReloadFinished();


namespace Excelsion.WeaponSystem
{
	//Abstract means that this class can only be built upon by other scripts. It cannot be used 'as is'.
	public abstract class GunBase : MonoBehaviour 
	{
		//NOTE: Sorry about the mess, but backend variable names were conflicting with frontend variables. 
		//		The workaround was to rename variables, so I put internal_ as a prefix of all references to backend.
		#region Variables
		[Tooltip("The model of this weapon. It should be a child of this script. Leave null for automatic search, unless you have multiple children.")]
		public GameObject model;
		private Transform muzzle;

		//Internal means that the variable is only accessible by scripts in the same namespace or by children of the script.
		internal Transform internal_origin;
		internal int internal_ammo; 
		internal int internal_clipAmmo;
		internal int internal_maxAmmo; //Child script needs to set this!
		internal int internal_clipSize; //Child script needs to set this!
		[Range(0.0f, 10.0f)]internal float internal_accuracy;
		internal float internal_reloadTime;
		internal float internal_fireRate;
		#endregion

		#region Effects
		[Tooltip("The muzzle flash effect to create when a shot is fired. Leave null for no effect.")]
		internal GameObject internal_muzzleEffect;
		internal AudioClip internal_shotFiredAudio;
		
		#endregion

		#region Accessors
		public Transform Origin{ get{ return internal_origin; } set{ internal_origin = value; }}
		public int MaxAmmo{ get{ return internal_maxAmmo; }     set{ internal_maxAmmo = Mathf.Abs( value ); }}
		public int ClipSize{ get{ return internal_clipSize; }   set{ internal_clipSize = Mathf.Abs( value ); }}
		public float Accuracy{ get{ return internal_accuracy; } set{ internal_accuracy = Mathf.Clamp( value, 0.0f, 10.0f ); }}
		public float ReloadTime { get{ return internal_reloadTime; } set{ internal_reloadTime = Mathf.Abs(value); }}
		public float FireRate{ get{ return internal_fireRate; } set{ internal_fireRate = Mathf.Abs(value); }}

		public GameObject MuzzleEffectPrefab{ get{ return internal_muzzleEffect; } set{ internal_muzzleEffect = value; } }
		public AudioClip ShotFiredAudio{ get{ return internal_shotFiredAudio; } set{ internal_shotFiredAudio = value; } }

		public int Ammo{ get{ return internal_ammo; }           set{ internal_ammo = Mathf.Min( value, internal_maxAmmo ); }}
		public int ClipAmmo{ get{ return internal_clipAmmo; }   set{ internal_clipAmmo = Mathf.Min( value, internal_clipSize ); }}
		#endregion



		#region Inputs
		//Variables with underscores '_' are left over from previous executions.
		internal bool _lastReloadInput;
		public virtual bool InputReload 
		{ 
			set {
				if( _lastReloadInput != value && value == true ) //Did the value change to true since the last time this was called?
				{
					if( CanReload() && reloadTimerDone )
					{
						TimerStart_reload();
						_lastReloadInput = value;
					}
					else
						_lastReloadInput = false;

					return;
				}
				_lastReloadInput = value;
			}
		}
		internal bool _lastFireInput;
		public abstract bool InputFire { set; }

		//Call this whenever inspector settings are changed.
		public abstract void UpdateSettings();
		public abstract void UpdateEffectSettings();
		#endregion

		#region Checks
		public bool CanFire()
		{ if( fireRateTimerDone && reloadTimerDone && internal_clipAmmo > 0 ){ return true; } else { return false; } }

		//Do we have at least one bullet in reserve, and are we missing at least one bullet from our clip?
		public bool CanReload()
		{ if( internal_ammo >= 1 && internal_clipAmmo < internal_clipSize ) { return true; } else { return false; } }
		#endregion



		protected virtual void Start()
		{
			fireRateTimerDone = true;
			reloadTimerDone = true;

			if( model == null )
			{
				//Search for a model...
				if( transform.childCount > 1 || transform.childCount == 0 )
				{
					Debug.LogError("Automatic model search failed: Number of children is zero or greater than one. Please specify the model gameobject, and place it as a child of this gameobject.", this);
					Debug.Break();
					return; //Stop running code here!
				}
				else
				{
					model = transform.GetChild(0).gameObject;
				}
			}

			//Search for muzzle flash object within the model.
			foreach( Transform child in model.transform )
			{
				if( child.gameObject.tag == "WeaponMuzzle" )
				{
					muzzle = child;
					break;
				}
			}
			if( muzzle == null )
			{
				Debug.LogError("Could not find the muzzle location within the model! Please put an empty gameobject as a child of the weapon model and tag it with 'WeaponMuzzle'.", this);
				Debug.Break();
			}

			if( internal_origin == null )
			{
				Debug.LogError("This weapon was not given an origin!", this);
				Debug.Break();
			}
		}


		public virtual void ShowWeapon( bool show )
		{ model.SetActive( show ); }

		#region Reload Timer
		internal bool reloadTimerDone = true;
		protected virtual void TimerStart_reload() //These are both marked virtual so we can do reloading differently for weapons like the shotgun.
		{ 
			reloadTimerDone = false;
			transform.root.gameObject.BroadcastMessage("OnReloadStart", ReloadTime, SendMessageOptions.DontRequireReceiver );
			Invoke( "Timer_reload", ReloadTime );
		}
		protected virtual void Timer_reload()
		{ 
			reloadTimerDone = true;
			transform.root.gameObject.BroadcastMessage("OnReloadFinished", SendMessageOptions.DontRequireReceiver );
			TransferAmmoToClip();
		}
		#endregion

		#region Ammo Transfer
		//Move 'count' bullets from the reserve ammo to the clip. (Leftover bullets still in the clip are saved.)
		//Returns true if at least one bullet was transfered...
		public virtual bool TransferAmmoToClip() { return TransferAmmoToClip( internal_clipSize ); }
		public virtual bool TransferAmmoToClip( int count )
		{
			if( count >= 0 )
			{
				//The total number of bullets we could possibly transfer.
				int missingAmmo = internal_clipSize - internal_clipAmmo;
				//This should insure that we don't break anything...
				int ammoToTransfer = Mathf.Min(Mathf.Min( count, internal_ammo ), missingAmmo);

				//Stop now if something bad happened...
				if( ammoToTransfer < 0 )
				{
					Debug.LogWarning("Negative ammo transfer requested! Mathf.Min[ Count: "+ count +", Ammo: "+ internal_ammo +", MissingAmmo: "+ missingAmmo +" ]", this); 
					return false;
				}

				//Finalize the transfer...
				int newClip = internal_clipAmmo + ammoToTransfer; 
				int newReserve = internal_ammo - ammoToTransfer;
				internal_clipAmmo = newClip; 
				internal_ammo = newReserve;

				return true;
			}
			return false;
		}
		#endregion


		#region Firing
		//Fires the gun. BE SURE TO CALL base.Fire()!!
		public virtual void Fire()
		{
			Debug.Log("Gunbase fire");
			TimerStart_fireRate();
			internal_clipAmmo -= 1;

			DoMuzzleBlast();
			DoShotFiredAudio();
		}
		//Create muzzle flash effect
		protected virtual void DoMuzzleBlast()
		{
			if( internal_muzzleEffect != null )
			{
				GameObject effect = (GameObject)Instantiate( internal_muzzleEffect, muzzle.position, muzzle.rotation );
				effect.transform.parent = muzzle;
			}
		}
		protected virtual void DoShotFiredAudio()
		{
			if( internal_shotFiredAudio != null )
			{
				Transform sound = AudioHelper.PlayClipAtPoint( internal_shotFiredAudio, muzzle.position, 1.0f, SoundType.Effect ).transform;
				sound.parent = muzzle;
			}
		}

		internal bool fireRateTimerDone = true;
		protected virtual void TimerStart_fireRate()
		{ 
			fireRateTimerDone = false;
			Invoke( "Timer_fireRate", FireRate );
			Debug.Log("TimerStart");
		}
		protected virtual void Timer_fireRate() { fireRateTimerDone = true; Debug.Log("TimerDone"); }

		#endregion


		 
	}















}