using UnityEngine;
using System;
using Excelsion.WeaponSystem.Weapons;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/18/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem
{
	//Enum for our finalized weapons. This enum needs to be added or removed from as weapons are added or removed.
	//If a weapon is removed, DO NOT SET ANOTHER WEAPON TO THE SAME ID AS THE ONE THAT WAS REMOVED.
	public enum Weapon
	{
		Null = -1,
		Pistol = 0,
		Rifle = 1,
		Shotgun = 2,
		RPG = 3
	};

	public class GunHelper
	{
		public static Type GetWeaponClass( Weapon wep ) //Converts our Weapon enum to an actual class type. (NOT an instance or reference)
		{
			int id = (int)wep;
			switch( id )
			{
				case 0:
					return Type.GetType("Excelsion.WeaponSystem.Weapons.Pistol");
				case 1:
					return Type.GetType("Excelsion.WeaponSystem.Weapons.Rifle");
				case 2:
					return Type.GetType("Excelsion.WeaponSystem.Weapons.Shotgun");
				case 3: 
					return Type.GetType("Excelsion.WeaponSystem.Weapons.RPG");
				default:
					return null;
			}
		}
	}
}