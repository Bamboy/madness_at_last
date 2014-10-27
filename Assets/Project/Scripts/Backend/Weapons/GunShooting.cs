using UnityEngine;
using System;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 09/20/14     ////
///////////////////////////////////

//MESSAGES SENT:
/*
 * public void OnShotFired( BulletInfo[] bullets ) //Sends a message out to all attached gameobjects when the gun is fired. 
 * See BulletInfo.cs for what variables this holds.
 * 
 * OnReloadStart( float time ) //When we first start reloading. time is how long the reload will take
 * OnReloadFinished() //When the reload is finished
 * */


public class GunShooting : MonoBehaviour 
{
	public bool doDebug = false;
	public WeaponInventory inv;
	public LayerMask objectsToHit = -1; //Any objects NOT in these layers will not be hit at all. (not even decals)
	public bool autoReload = true;
	
	//Used for where the raycasts should start, and which direction they should travel.
	public Transform rayTransform; //If this script belongs to the player, you should set this to the camera!
	public Transform recoilTransform; //This is the object that recoil gets applied to.

	[SerializeField]private string weapon; //Our current weapon ID.

	private bool fireRateTimerDone;
	private bool reloadTimerDone;

	private bool _lastFireInput;
	private bool _lastReloadInput;

	private GunDefinitions gunDefs;
	void Start () 
	{
		gunDefs = GunDefinitions.Get();
		weapon = "null";
		Weapon = weapon;
		fireRateTimerDone = true;
		reloadTimerDone = true;
	}

	#region Timers
	void TimerStart_fireRate()
	{ 
		fireRateTimerDone = false;
		float time = gunDefs.GetFireRate( weapon );
		Invoke( "Timer_fireRate", time );
	}
	void Timer_fireRate()
	{ fireRateTimerDone = true; }
	void TimerStart_reload()
	{ 
		reloadTimerDone = false;
		float time = gunDefs.GetReloadTime( weapon );
		transform.parent.gameObject.BroadcastMessage("OnReloadStart", time, SendMessageOptions.DontRequireReceiver );
		Invoke( "Timer_reload", time );
	}
	void Timer_reload()
	{ 
		reloadTimerDone = true;
		transform.parent.gameObject.BroadcastMessage("OnReloadFinished", SendMessageOptions.DontRequireReceiver );
		inv.TransferAmmoToClip( weapon ); 
	}
	#endregion

	#region Inputs
	public bool FireWeapon
	{
		set {
			switch( gunDefs.GetFiretype( weapon ) )
			{
				case Firetype.Auto:
					if( fireRateTimerDone && reloadTimerDone && value == true )
						TryFire ();
					break;
				case Firetype.SemiAuto:
					if( _lastFireInput != value && value == true ) //Did the value change to true since the last time this was called?
					{
						if( TryFire() == true )
							_lastFireInput = value;
						else
							_lastFireInput = false; //We can't fire, so set our fire input to false so we will retry the next time this is called.

						return;
					}
					break;
				default:
					Debug.LogError( "This script doesn't know how to shoot this kind of gun! " + weapon, this );
					Debug.Break ();
					break;
			}
			_lastFireInput = value;
		}
	}
	public bool ReloadWeapon
	{
		set {
			if( _lastReloadInput != value && value == true ) //Did the value change to true since the last time this was called?
			{
				if( TryReload() == true )
					_lastReloadInput = value;
				else
					_lastReloadInput = false;
			}
		}
	}
	#endregion
	#region Behaviour Attempts
	private bool TryReload()
	{
		if( autoReload && inv.CanReload( weapon ) && reloadTimerDone )
		{
			if( doDebug )
				Debug.Log("Reloading...", this);

			TimerStart_reload(); //This will automatically transfer ammo when its done.

			//Do other reload animations here TODO

			return true;
		}
		else
			return false;
	}
	public bool CanFire()
	{
		if( fireRateTimerDone && reloadTimerDone && inv.GetClipAmmoFor( weapon ) > 0 )
			return true;
		else 
			return false;
	}
	private bool TryFire()
	{
		if( fireRateTimerDone && reloadTimerDone )
		{
			if( inv.GetClipAmmoFor( weapon ) > 0 ) //Do we have ammo in our current clip?
			{
				Fire();
				return true;
			}
			else
			{
				if( autoReload ) TryReload ();
				return false;
			}
		}
		else
			return false;
	}
	#endregion
	#region Firing
	private void Fire() //Tries to fire the current weapon.
	{
		TimerStart_fireRate();

		if( doDebug ) Debug.Log("Firing...", this);

		inv.SetClipAmmoFor( weapon, inv.GetClipAmmoFor( weapon ) - 1 ); //TODO - make a function that decreases clip ammo by one in the inventory script!

		if( gunDefs.GetBulletPrefab( weapon ) != null ) //Decide if we need to create prefab(s) or use raycast(s).
		{
			FireAsPrefab();
		}
		else
		{
			BulletInfo[] bullets = FireAsRaycast();
			transform.parent.gameObject.BroadcastMessage("OnShotFired", bullets, SendMessageOptions.DontRequireReceiver ); //Sends a message out to all attached gameobjects.
		}
	}
	private BulletInfo[] FireAsRaycast () //Return array of raycasthit so we can apply damage to all hit objects.
	{
		RaycastHit[] rays = new RaycastHit[ gunDefs.GetBulletsPerShot( weapon ) ];
		BulletInfo[] bullets = new BulletInfo[ gunDefs.GetBulletsPerShot( weapon ) ];

		float gunAccuracy = gunDefs.GetAccuracy( weapon );
		Vector3 pos = rayTransform.position; Vector3 dir = rayTransform.forward;
		float distance = gunDefs.GetDistance( weapon ); if( distance <= 0.0f ) distance = 999999.999999f; //Get our distance and if its zero or less, set it to infinity.
		for( int i = 0; i < gunDefs.GetBulletsPerShot( weapon ); i++ )
		{
			//Adds a randomized offset to the direction vector based on accuracy. 0 is no change, 10 is about 90 degrees.
			Vector3 bulletDirection = VectorExtras.DirectionalCone( pos, dir, gunAccuracy );
			Ray dataRay = new Ray( pos, bulletDirection );


			if( Physics.Raycast( pos, bulletDirection, out rays[i], distance, objectsToHit ) )
			{
				//Do shit
				bullets[i] = new BulletInfo( dataRay, rays[i] );

				if( doDebug ) Debug.Log("Hit "+ rays[i].transform.name +"!", this);
			}

			if( doDebug )
			{ Debug.DrawLine( pos, VectorExtras.OffsetPosInDirection(pos, bulletDirection, distance), Color.red, 1.5f ); }
		}
		if( recoilTransform != null )
		{
			float totalRecoil = gunDefs.GetRecoil( weapon ) * (float)gunDefs.GetBulletsPerShot( weapon ); //Multiply recoil by how many bullets we shot
			Quaternion recoilQuaternion = Quaternion.AngleAxis( totalRecoil, Vector3.left);
			recoilTransform.localRotation = recoilTransform.localRotation * recoilQuaternion;
		}
		return bullets;
	}

	private void FireAsPrefab () //The created prefab will handle damage output and effects.
	{
		
	}
	#endregion


	#region Read / Write Accessors
	
	public string Weapon //weapon of the current weapon. Changing this value will effectively change the weapon.
	{
		get{ return weapon; }
		set{
			if( GunDefinitions.Get().IDisValid( value ) == true )
			{
				if( value != weapon )
				{
					CancelInvoke(); //Cancel any timers.
					fireRateTimerDone = true;
					reloadTimerDone = true;

					weapon = value; //Set the new weapon.
				}
			}
		}
	}
	/*public int Ammo
	{
		get{ return ammo; }
		set{
			ammo = value;
		}
	}
	public int ClipAmmo
	{
		get{ return clipAmmo; }
		set{
			clipAmmo = value;
		}
	} */
	#endregion

















}






















