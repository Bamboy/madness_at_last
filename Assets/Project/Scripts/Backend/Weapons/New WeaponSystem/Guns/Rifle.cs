using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem.Weapons
{
	public class Rifle : GunRay
	{



		protected override void Start ()
		{
			base.Start ();
		}
		
		public override bool InputFire 
		{
			set{
				if( CanFire() )
				{
					if( FireType.Automatic( value, _lastFireInput ) ) //This gun uses SemiAuto
						_lastFireInput = value;
				}
			}
		}
	}
}