using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 10/29/14     ////
///////////////////////////////////



//Holds the weapons we currently have, and then passes weapon ids to GunShooting so it can simulate behaviours.
public class WeaponInventory : MonoBehaviour
{
	//TODO - seperate script for input control

	public GunShooting gunShoot;
	public GunEffects gunEffects;
	public string[] startingWeapons; //This also controls inventory size!!! Use 'null' if you want blank slots.
	public WeaponSlot[] slots;
	public int activeSlot = 0; //Which weapon we are currently weilding. (The index)

	private GunDefinitions gunDef;
	void Start() 
	{
		gunDef = GunDefinitions.Get();

		slots = new WeaponSlot[ startingWeapons.Length ];
		for( int i = 0; i < slots.Length; i++ )
		{ slots[i] = new WeaponSlot( startingWeapons[i] ); } //Initalize our WeaponSlots.

		//BUG: pistol is not rendering for the viewport camera.
		#region BUG: Rotation calculation is broken as soon as the camera rotates. This is a workaround...
		//gunEffects.AddWeapon( "Pistol", true ); //AddWeapon needs to be changed back to private when this gets fixed!
		//gunEffects.AddWeapon( "Shotgun", false );
		//gunEffects.AddWeapon( "Rifle", false );
		//gunEffects.AddWeapon( "RPG", false );
		gunEffects.ManualSetup();
		#endregion

		SetActiveWeapon( activeSlot );
	}

	#region Ammo Manipulation
	public int GetAmmoFor( string weapon )
	{
		if( HasWeapon( weapon ) == true )
			return slots[GetWeaponIndex( weapon )].ammo;
		else
			return -1;
	}
	public bool SetAmmoFor( string weapon, int newAmmo )
	{
		if( HasWeapon( weapon ) == true )
		{
			slots[GetWeaponIndex( weapon )].ammo = Mathf.Clamp( newAmmo, 0, gunDef.GetMaxAmmo( weapon ) );
			return true;
		}
		return false;
	}
	public bool AddAmmoFor(string weapon, int ammo)
	{
		return SetAmmoFor(weapon, GetAmmoFor(weapon) + ammo);
	}
	public int GetClipAmmoFor( string weapon )
	{
		if( HasWeapon( weapon ) == true )
			return slots[GetWeaponIndex( weapon )].clipAmmo;
		else
			return -1;
	}
	public void SetClipAmmoFor( string weapon, int newAmmo )
	{
		if( HasWeapon( weapon ) == true )
			slots[GetWeaponIndex( weapon )].clipAmmo = Mathf.Clamp( newAmmo, 0, gunDef.GetClipSize( weapon ) );
	}
	public bool CanReload( string weapon )
	{
		if( HasWeapon( weapon ) == false )
			return false;

		//Do we have at least one bullet in reserve, and are we missing at least one bullet from our clip?
		if( slots[GetWeaponIndex( weapon )].ammo >= 1 && slots[GetWeaponIndex( weapon )].clipAmmo < gunDef.GetClipSize( weapon ) )
			return true;
		else
			return false;

	}
	public bool TransferAmmoToClip( string weapon ) //Tries to transfer ammo to the clip, returns true if at least one bullet was transfered.
	{
		if( HasWeapon( weapon ) == false )
			return false;

		WeaponSlot ourWeapon = slots[GetWeaponIndex( weapon )];
		int clipSize = gunDef.GetClipSize( weapon );
		//Do we have at least one bullet in reserve, and are we missing at least one bullet from our clip?
		if( CanReload( weapon ) ) //ourWeapon.ammo >= 1 && ourWeapon.clipAmmo < clipSize
		{
			if( gunDef.GetCanSaveUnusedBullets( weapon ) == false )
			{ 
				if( ourWeapon.ammo <= clipSize ) //Do we have enough ammo to reload the whole clip?
				{
					ourWeapon.clipAmmo = ourWeapon.ammo;
					ourWeapon.ammo = 0;
				}
				else
				{
					ourWeapon.ammo -= clipSize; 
					ourWeapon.clipAmmo = clipSize; 
				}
				return true;
			}
			else
			{
				int missingAmmo = clipSize - ourWeapon.clipAmmo;
				int ammoToTransfer = Mathf.Min( ourWeapon.ammo, missingAmmo ); //This should ensure that we don't lose any ammo during the transfer.
				//Debug.Log("Transfer for "+ weapon +". Missing from clip: "+ missingAmmo +", Reserve Ammo: "+ ourWeapon.ammo +", Ammo being transfered: "+ ammoToTransfer +".",this);

				int newClip = ourWeapon.clipAmmo + ammoToTransfer;
				int newReserve = ourWeapon.ammo - ammoToTransfer;
				//Debug.Log("Transfer results: New clip: "+ newClip +", New reserve: "+ newReserve +".", this);
				slots[GetWeaponIndex( weapon )].clipAmmo = newClip; slots[GetWeaponIndex( weapon )].ammo = newReserve;
				return true;
			}
		}
		else
			return false;
	}
	#endregion

	#region Inputs & Weapon Selection
	public void NextWeapon()
	{
		if( WeaponCount() > 1 ) //Only enable switching if we have 2 or more weapons.
		{
			activeSlot++;
			if( activeSlot == slots.Length ) 
				activeSlot = 0; //We would get an Index Out Of Range error if we didn't change it!

			while( slots[activeSlot].Weapon == "null" ) //Don't stop on a weapon unless we have it!
			{
				activeSlot++;
				if( activeSlot == slots.Length ) 
					activeSlot = 0; //We would get an Index Out Of Range error if we didn't change it!
			}
			SetActiveWeapon( activeSlot ); //Finalize our weapon switch.
		}
	}
	public void PreviousWeapon()
	{
		if( WeaponCount() > 1 ) //Only enable switching if we have 2 or more weapons.
		{
			activeSlot--;
			if( activeSlot == -1 ) 
				activeSlot = slots.Length - 1; //We would get an Index Out Of Range error if we didn't change it!

			while( slots[activeSlot].Weapon == "null" ) //Don't stop on a weapon unless we have it!
			{
				activeSlot--;
				if( activeSlot == -1 ) 
					activeSlot = slots.Length - 1; //We would get an Index Out Of Range error if we didn't change it!
			}
			SetActiveWeapon( activeSlot ); //Finalize our weapon switch.
		}
	}
	public bool SwitchToWeapon( string id )
	{
		if( HasWeapon( id ) == true && id != "null" )
		{
			SetActiveWeapon( GetWeaponIndex( id ) );
			return true;
		}
		else
			return false;
	}

	private void SetActiveWeapon( int index )
	{
		activeSlot = index;
		//Switch weapon behaviour
		gunShoot.Weapon = slots[ activeSlot ].Weapon;
		//Switch weapon visuals
		gunEffects.Weapon = slots[ activeSlot ].Weapon;

	}
	#endregion
		                                 


	#region Slot Modifications & Info Functions
	public bool HasAnyWeapon() //Returns true if we have at least one valid weapon.
	{
		if( WeaponCount() > 0 ) { return true; }
		else { return false; }
	}
	public int WeaponCount() //Returns the number of valid weapons we have. //TODO - make this save the number of weapons we have
	{
		int weaponCount = 0;
		for( int i = 0; i < slots.Length; i++ )
			{ if( slots[i].Weapon != "null" ) { weaponCount++; } }
		return weaponCount;
	}

	public bool HasWeapon( string id ) //Returns if we have at least one of the specified weapon.
	{                                  //You can use this to check if we have an open slot as well. (id = "null")
		for( int i = 0; i < slots.Length; i++ )
			{ if( slots[i].Weapon == id ) { return true; } }
		return false;
	}
	public int GetWeaponIndex( string id ) //Gets the first slot that has this weapon. Returns -1 if it doesn't exist in this inventory.
	{
		for( int i = 0; i < slots.Length; i++ )
			{ if( slots[i].Weapon == id ) { return i; } }
		return -1;
	}
	public bool AddWeapon( WeaponSlot slot ) //Try to replace the weapon with a 'null' slot. Returns if we were successful.
	{
		if( HasWeapon( slot.Weapon ) )
			return false;
		else
		{
			int index = GetWeaponIndex( "null" );
			if( index != -1 )
			{
				ReplaceWeapon( index, slot );
				return true;
			}
			else
				return false; //We dont have any empty slots to put a weapon in.
		}
	}
	public bool AddNewWeapon( string id, int ammo ) //Adds a new weapon more easily than the above function.
	{
		if( HasWeapon( id ) )
			return false;

		int index = GetWeaponIndex( "null" );
		if( index != -1 )
		{
			slots[ index ].Weapon = id; //Note that this clears all of the weaponslot data!

			//Choose a random amount of ammo to put in the clip...
			//Choose a random range between 0 and the size of clip.. but DO NOT exceed 'ammo'.
			int ammoForClip = Mathf.Min( Random.Range(0, gunDef.GetClipSize(id)), ammo );
			//Take our clip ammo out of total, DO NOT exceed the total ammo allowed to be carried.
			ammo = Mathf.Min( ammo - ammoForClip, gunDef.GetMaxAmmo(id) );

			slots[ index ].clipAmmo = ammoForClip;
			slots[ index ].ammo = ammo;
			return true;
		}
		else
			return false; //We dont have any empty slots to put a weapon in.
	}





	public WeaponSlot ReplaceWeapon( int index, WeaponSlot newSlot ) 
	{ //Forcefully replaces the weapon in the specified slot. The returned WeaponSlot was the weapon that got replaced. (In case we want to drop it on the ground, or something.)
		WeaponSlot oldSlot = slots[ index ];
		slots[ index ] = newSlot;
		return oldSlot;
	}

	#endregion
































}





