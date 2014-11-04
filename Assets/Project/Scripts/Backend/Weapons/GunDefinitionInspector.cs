using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Stats;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 08/29/14     ////
///////////////////////////////////

//Only one of this class should exist in the scene!
public class GunDefinitions : MonoBehaviour
{
	#region Global Access
	private static GunDefinitions gundef;
	public static GunDefinitions Get() //This makes it so we can remotely access this script from anywhere in the scene.
	{
		if( !gundef ) //Check if we already have an instance of this script in the scene, if so just return it.
		{
			gundef = GameObject.FindObjectOfType< GunDefinitions >(); //Find this script in the scene.
			if( !gundef ) //Make sure we found something.
			{
				//Make a new object and attach this script to it. 
				GameObject newObj = new GameObject("=== Gun Definitions ===");
				gundef = newObj.AddComponent< GunDefinitions >();

				//(This will cause issues because our gun defs are set up in the inspector! Just throw an error.)
				//Debug.LogError ("GunDefinitions does not exist in the scene and a script tried to access it!");
				//Debug.Break ();
			}
		}
		return gundef;
	}
	#endregion
	#region Initialization
	public Dictionary<string, GunData> guns;

	public bool AddElement( GunData data )
	{
		if( guns == null || guns.Count <= 0 ) //Make sure that our dictionary exists.
		{ MakeNewDictionary(); }

		if( guns.ContainsKey( data.name ) || data.name == "" || data.name == null || data.name == "null" )
			return false;
		else
		{
			guns.Add( data.name, data );
			return true;
		}
	}
	private void MakeNewDictionary()
	{
		guns = new Dictionary<string, GunData>();
		guns.Add( "null", new GunData() ); //Create a fake weapon that will represent a lack of a weapon.
	}
	void Start()
	{
		Debug.Log ("Successfully created "+ (guns.Count - 1) + " weapon definitions.", this); //Remove 1 because the 'null' weapon doesn't count.
	}
	#endregion
	#region Get Variable Functions
	///////// Shooting Behaviour /////////
	public Firetype GetFiretype( string id ) {
		if( !IDisValid( id ) )
			return Firetype.Auto; //TODO - Make a null firetype?
		else
			return guns[id].firetype;
	}
	public float GetFireRate( string id ) {
		if( !IDisValid( id ) )
			return 0.0f; //TODO - Make a null firetype?
		else
			return guns[id].fireRate;
	}
	public int GetBulletsPerShot( string id ) {
		if( !IDisValid( id ) )
			return 1; //TODO - Make a null firetype?
		else
			return guns[id].bulletsPerShot;
	}
	public float GetAccuracy( string id ) {
		if( !IDisValid( id ) )
			return 0.0f; //TODO - Make a null firetype?
		else
			return guns[id].accuracy;
	}
	public float GetRecoil( string id ) {
		if( !IDisValid( id ) )
			return 0.0f; //TODO - Make a null firetype?
		else
			return guns[id].recoil;
	}
	public float GetDistance( string id ) {
		if( !IDisValid( id ) )
			return 0.0f; //TODO - Make a null firetype?
		else
			return guns[id].distance;
	}
	public Vector2 GetDamageRange( string id ) {
		if( !IDisValid( id ) )
			return Vector2.zero; //TODO - Make a null firetype?
		else
			return guns[id].damageRange;
	}
	public DamageType GetDamageType(string id)
	{
		if (!IDisValid(id))
			return DamageType.Physical;
		else
			return guns[id].damageType;
	}
	public string[] GetDamageEffects(string id)
	{
		if (!IDisValid(id))
			return null;
		else
			return guns[id].damageEffects;
	}
	///////// Bullet Prefab /////////
	public GameObject GetBulletPrefab( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].bulletPrefab;
	}
	public float GetBulletPrefabForce( string id ) {
		if( !IDisValid( id ) )
			return 0.0f; //TODO - Make a null firetype?
		else
			return guns[id].bulletPrefabForce;
	}
	////////////////// Ammo //////////////////
	public int GetMaxAmmo( string id ) {
		if( !IDisValid( id ) )
			return 1; //TODO - Make a null firetype?
		else
			return guns[id].ammoMax;
	}
	public int GetClipSize( string id ) {
		if( !IDisValid( id ) )
			return 1; //TODO - Make a null firetype?
		else
			return guns[id].clipSize;
	}
	public float GetReloadTime( string id ) {
		if( !IDisValid( id ) )
			return 1.0f; //TODO - Make a null firetype?
		else
			return guns[id].reloadTime;
	}
	public bool GetCanSaveUnusedBullets( string id ) {
		if( !IDisValid( id ) )
			return false; //TODO - Make a null firetype?
		else
			return guns[id].canSaveUnusedBullets;
	}
	///////// Gun Visuals /////////
	public GameObject GetGunPrefab( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].prefab;
	}
	public Vector3 GetPositionOffset( string id ) {
		if( !IDisValid( id ) )
			return Vector3.zero; //TODO - Make a null firetype?
		else
			return guns[id].positionOffset;
	}
	public Vector3 GetRotationOffset( string id ) {
		if( !IDisValid( id ) )
			return Vector3.zero; //TODO - Make a null firetype?
		else
			return guns[id].rotationOffset;
	}
	public Vector3 GetSizeOffset( string id ) {
		if( !IDisValid( id ) )
			return Vector3.zero; //TODO - Make a null firetype?
		else
			return guns[id].sizeOffset;
	}
	///////// Gun Sounds /////////
	public AudioClip GetSoundShotFired( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].shotFired;
	}
	public AudioClip GetSoundClipEmpty( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].clipEmpty;
	}
	public AudioClip GetSoundReload( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].reloading;
	}
	public AudioClip GetSoundAmmoAdded( string id ) {
		if( !IDisValid( id ) )
			return null; //TODO - Make a null firetype?
		else
			return guns[id].ammoAdded;
	}





	public bool IDisValid( string id )
	{
		if( id == "" || id == null )
		{
			Debug.LogWarning("A script tried to request a bad id! ("+ id +")", this);
			return false;
		}
		else
			return true;
	}
	#endregion

}

#region InspectorScript
//We use this so we can modify our GunData in the editor. (Before we start the game)
public class GunDefinitionInspector : MonoBehaviour
{
	//Dictionaries cannot be viewable in the inspector. This is a workaround.
	//Make a builtin array and then populate the dictionary as soon as the game starts.
	//This object gets deleted after this, as to save resources.
	public GunData[] gunDefinitions = new GunData[0];
	private GunDefinitions dictionary;

	void Awake() //This runs as soon as the object exists.
	{
		dictionary = GunDefinitions.Get(); //We need to be the first script that calls this...

		//Copy the array instances to our dictionary.
		for( int i = 0; i < gunDefinitions.Length; i++ )
		{

			if( dictionary.AddElement( gunDefinitions[ i ] ) == false ) //Check if adding the element failed.
			{
				Debug.LogError("Adding dictionary element '"+ gunDefinitions[i].name +"' at index #"+ i +" failed because the name already exists or was left null or blank.", this);
				Debug.Break ();
			}


		}

		GameObject.Destroy( this.gameObject ); //Remove this object because we don't need it anymore.
	}

}
#endregion





















