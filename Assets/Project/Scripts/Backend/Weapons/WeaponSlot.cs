using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 08/29/14     ////
///////////////////////////////////

//This will represent an actual weapon.
[System.Serializable]
public class WeaponSlot : System.Object 
{
	[SerializeField] private string name; //Name of the current weapon.
	
	public int ammo;
	public int clipAmmo;

	#region Initilization
	private GunDefinitions gunDefs;
	public WeaponSlot( string startName = "null" )
	{
		this.gunDefs = GunDefinitions.Get();
		this.Weapon = startName;
		this.ammo = 250; this.clipAmmo = 0;

	}
	#endregion
	public string Weapon //Name of the current weapon. Changing this value will effectively change the weapon.
	{
		get{ return name; }
		set{
			ammo = 0; //We dont want the new weapon using the old weapon's ammo.
			clipAmmo = 0;
			if( GunDefinitions.Get().IDisValid( value ) == true )
				name = value;
			else
				name = "null";
		}
	}




	//TODO - put ammo functions here
}
