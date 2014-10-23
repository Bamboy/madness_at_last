using UnityEngine;
using System.Collections;
using Stats;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 08/29/14     ////
///////////////////////////////////

public enum Firetype
{
	Auto = 0,
	SemiAuto = 1
}

[System.Serializable] //Makes it so that this object's values can be viewed in the inspector.
public class GunData : System.Object 
{
	[Tooltip("Acts as our id")]
	public string name = "null";
	
	//Shooting Behaviour
	[Tooltip("")]
	public Firetype firetype;
	[Tooltip("Time between each shot for automatic weapons, or for semi-auto, this is the minimum time that the gun can be shot again.")]
	public float fireRate;
	[Tooltip("How many bullets are fired per shot.")]
	public int bulletsPerShot;
	[Tooltip(""), Range(0.0f, 10.0f)]
	public float accuracy;
	[Tooltip("The camera gets pushed upwards by this ammount per shot.")]
	public float recoil;
	[Tooltip("Beyond this distance, bullets wont damage anything. Zero is equal to infinity."), Range(0.0f, float.MaxValue)]
	public float distance;
	[Tooltip("Base damage this weapon will do when it hits something. Differing values will calculate a random value between the two.")]
	public Vector2 damageRange;
	[Tooltip("Damage type of the bullet")]
	public DamageType damageType = DamageType.Physical;
	[Tooltip("Damage effects, use Prefabs")]
	public GameObject[] damageEffects;
	
	//Bullet prefab
	[Tooltip("If this isn't null, this object will be created instead of using raycasts.")]
	public GameObject bulletPrefab;
	[Tooltip("If using a non-raycast, this is how much force will be applied when the object is created. (Only if the object also has a rigidbody)")]
	public float bulletPrefabForce;
	
	//Ammo
	[Tooltip("How much ammo we can carry at once.")]
	public int ammoMax;
	[Tooltip("How many bullets can be in the clip.")]
	public int clipSize;
	[Tooltip("How long it takes to reload this weapon. (Seconds)"), Range(0.0f, 60.0f)]
	public float reloadTime;
	[Tooltip("Should we carry over any unused bullets in the clip when reloading?")]
	public bool canSaveUnusedBullets;

	//Gun Visuals
	[Tooltip("The object's model.")]
	public GameObject prefab; // TODO - Add a seperate prefab for the viewport?
	[Tooltip("")]
	public Vector3 positionOffset;	
	[Tooltip("")]
	public Vector3 rotationOffset;
	[Tooltip("")]
	public Vector3 sizeOffset;

	//GUI
	[Tooltip("")]
	public Texture2D guiWeaponIcon;
	[Tooltip("")]
	public Texture2D guiAmmoIcon;
	
	//Gun Sounds
	[Tooltip("")]
	public AudioClip shotFired;
	[Tooltip("")]
	public AudioClip clipEmpty;
	[Tooltip("")]
	public AudioClip reloading;
	[Tooltip("")]
	public AudioClip ammoAdded;
}







