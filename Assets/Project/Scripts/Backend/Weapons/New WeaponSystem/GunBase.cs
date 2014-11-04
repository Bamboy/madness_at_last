using UnityEngine;
using System.Collections;


///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/04/14     ////
///////////////////////////////////

//Messages sent:
//OnReloadStart( float time );
//OnReloadFinished();
namespace Excelsion.WeaponSystem
{
	//Abstract means that this class can only be built upon by other scripts. It cannot be used 'as is'.
	public abstract class GunBase : MonoBehaviour 
	{
		#region Ammo Variables
		private int ammo; private int clipAmmo;
		public int Ammo {
			get{ return ammo; } 
			set{ ammo = Mathf.Min( value, maxAmmo ); }
		}
		public int ClipAmmo {
			get{ return clipAmmo; } 
			set{ clipAmmo = Mathf.Min( value, clipSize ); }
		}

		//Internal means that the variable is only accessible by scripts in the same namespace or by children of the script.
		internal int maxAmmo; //Child script needs to set this!
		internal int clipSize; //Child script needs to set this!
		#endregion

		#region Inputs
		//Variables with underscores '_' are left over from previous executions.
		internal bool _lastReloadInput;
		public virtual bool InputReload { 
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

		#endregion

		void Start()
		{
			fireRateTimerDone = true;
			reloadTimerDone = true;
		}


		//Note that virtual means that the functions CAN be overridden, but it is not required.
		#region Reloading
		private float reloadTime = 1.0f;
		public float ReloadTime {
			get{ return reloadTime; } set{ reloadTime = Mathf.Abs(value); }
		}

		//Do we have at least one bullet in reserve, and are we missing at least one bullet from our clip?
		public bool CanReload()
		{ if( ammo >= 1 && clipAmmo < clipSize ) { return true; } else { return false; } }

		//Reloads the gun.
		public virtual void Reload()
		{
			if( CanReload() && reloadTimerDone )
			{

			}
		}

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
		public virtual bool TransferAmmoToClip() { return TransferAmmoToClip( clipSize ); }
		public virtual bool TransferAmmoToClip( int count )
		{
			if( count >= 0 )
			{
				//The total number of bullets we could possibly transfer.
				int missingAmmo = clipSize - clipAmmo;
				//This should insure that we don't break anything...
				int ammoToTransfer = Mathf.Min(Mathf.Min( count, ammo ), missingAmmo);

				//Stop now if something bad happened...
				if( ammoToTransfer < 0 )
				{
					Debug.LogWarning("Negative ammo transfer requested! Mathf.Min[ Count: "+ count +", Ammo: "+ ammo +", MissingAmmo: "+ missingAmmo +" ]", this); 
					return false;
				}

				//Finalize the transfer...
				int newClip = clipAmmo + ammoToTransfer; 
				int newReserve = ammo - ammoToTransfer;
				clipAmmo = newClip; 
				ammo = newReserve;

				return true;
			}
			return false;
		}
		#endregion
		#endregion

		#region Firing
		private float fireRate = 1.0f;
		public float FireRate {
			get{ return fireRate; } set{ fireRate = Mathf.Abs(value); }
		}

		public bool CanFire()
		{ if( fireRateTimerDone && reloadTimerDone && clipAmmo > 0 ){ return true; } else { return false; } }

		#region Fire
		//Fires the gun. BE SURE TO CALL base.Fire()!!
		public virtual void Fire()
		{
			TimerStart_fireRate();
			clipAmmo -= 1;
		}
		#endregion

		#region Fire Rate Timer
		internal bool fireRateTimerDone = true;
		protected virtual void TimerStart_fireRate()
		{ 
			fireRateTimerDone = false;
			Invoke( "Timer_fireRate", FireRate );
		}
		protected virtual void Timer_fireRate() { fireRateTimerDone = true; }
		#endregion
		#endregion



	}















}