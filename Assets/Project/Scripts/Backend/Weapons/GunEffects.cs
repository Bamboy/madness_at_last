using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen //// 
/// Last Updated: 10/28/14     ////
///////////////////////////////////

//This script handles all of the gun effects, visuals, and sounds.
//Newly created gunmodels will be a child of this script's transform!
public class GunEffects : MonoBehaviour 
{
	public GameObject muzzleEffect;

	public GameObject[] weapons;


	private string weapon; //Our current weapon ID.
	private Dictionary<string, WeaponModelData> gunModels;
	private GunDefinitions gunDefs;
	void Start () { Init(); }
	void Init()
	{
		if( gunDefs == null )
			gunDefs = GunDefinitions.Get();
		if( gunModels == null )
			gunModels = new Dictionary<string, WeaponModelData>();
	}

	public string Weapon //Name of the current weapon. Changing this value will effectively change the weapon.
	{
		get{ return weapon; }
		set{
			if( GunDefinitions.Get().IDisValid( value ) == true )
			{
				weapon = value;
				SwitchToWeapon( value );
			}
			else
				weapon = "null";
		}
	}

	bool WeaponExistsForUs( string id ) //Returns if the weapon has been spawned for us yet.
	{
		if( gunModels == null )
		{
			gunModels = new Dictionary<string, WeaponModelData>();
			return false;
		}

		if( gunModels.ContainsKey( id ) )
			return true;
		else
			return false;
	}

	public void SwitchToWeapon( string id )
	{
		if( WeaponExistsForUs( id ) )
		{
			SetWeapon( id );
		}
		else
		{
			AddWeapon( id, true );
		}
	}

	#region Weapon adding
	public void AddWeapon( string id, bool startActive ) //Spawns the weapon and adds its values to our dictionary.
	{													  //Optionally, it will also switch to that weapon.
		Init(); //Sometimes this function can be called before our vars are set up. Do this now if we have not!
		Vector3 eulerAngles = gunDefs.GetRotationOffset( id );
		Quaternion rotation = this.transform.rotation * Quaternion.Euler( eulerAngles.x, eulerAngles.y, eulerAngles.z ); //THIS IS BROKEN.. as soon as transform.rotation is non-zero!
		//Quaternion.Euler( this.transform.rotation.eulerAngles + eulerAngles )
		//Quaternion.Euler( eulerAngles.x, eulerAngles.y, eulerAngles.z );
		GameObject newModel = (GameObject)Instantiate( gunDefs.GetGunPrefab( id ), 
		                                               this.transform.position + gunDefs.GetPositionOffset( id ), rotation );
		WeaponModelData newData = new WeaponModelData( id, newModel, startActive );

		foreach( Transform child in newModel.transform )
		{
			if( child.gameObject.tag == "WeaponMuzzle" )
			{
				newData.muzzle = child;
				break;
			}
		}

		newData.rootTransform.parent = this.transform;

		gunModels.Add( id, newData );

		if( startActive )
			SetWeapon( id );
	}

	public void ManualSetup()
	{
		if( weapons == null )
			return;
		else
		{
			foreach( GameObject model in weapons )
			{
				if( WeaponExistsForUs( model.name ) == false && gunDefs.guns.ContainsKey( model.name ) == true )
				{
					WeaponModelData newModel = new WeaponModelData( model.name, model, false );
					foreach( Transform child in model.transform )
					{
						if( child.gameObject.tag == "WeaponMuzzle" )
						{
							newModel.muzzle = child;
							break;
						}
					}
					gunModels.Add( model.name, newModel );
				}
			}
		}
	}
	#endregion Weapon adding


	private void SetWeapon( string id )
	{
		SetModelStatuses( gunModels, id );
	}
	private void SetModelStatuses( Dictionary<string, WeaponModelData> dictionary, string exclusion )
	{
		foreach(WeaponModelData modelData in dictionary.Values)
		{
			if( modelData.weapon == exclusion )
				modelData.SetEnabled( true );
			else
				modelData.SetEnabled( false );
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}



	//Create muzzle flash effect
	public void DoMuzzleBlast()
	{
		Transform muzzle = gunModels[ weapon ].muzzle;
		if( muzzle != null )
		{
			GameObject effect = (GameObject)Instantiate( muzzleEffect, muzzle.position, muzzle.rotation );
			effect.transform.parent = muzzle;
		}
			                             
	}




















}
#region WeaponModelData
//Holds various data about our gun models. All of these values are ACTUAL instances, unlike with the GunData class.
[System.Serializable] //Note that there is no representation for null!
public class WeaponModelData : System.Object 
{
	public string weapon; //The weapon id that this prefab belongs to.
	public bool objectActive; //Is this object and its children active?
	public GameObject model;
	public Transform rootTransform; //This is the highest transform for this weapon, and not the ultimate root parent!
	public Transform muzzle; //this is where the muzzle effect will be created.

	//public Light muzzleLight;
	//public ParticleSystem muzzleParticles;

	public WeaponModelData( string weaponName, GameObject prefab, bool startActive )
	{
		weapon = weaponName;
		model = prefab;
		rootTransform = model.transform;
		objectActive = startActive;
		model.SetActive( startActive );
	}

	public void SetEnabled( bool state )
	{
		objectActive = state;
		model.SetActive( state );
	}
}
#endregion



















