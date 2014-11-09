using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 11/09/14     ////
///////////////////////////////////


namespace Excelsion.WeaponSystem.Weapons
{
	public class Pistol : GunRay
	{
		#region GunBase variables

		//Make 'fake' inspector variables here, then set them to the real ones on Start()

		#endregion
		#region GunRay variables

		//Same here...

		#endregion


		protected override void Start ()
		{
			base.Start ();
			FireRate = 2.0f;
		}

		public override bool InputFire 
		{
			set{
				if( CanFire() )
				{
					if( FireType.SemiAuto( value, _lastFireInput ) ) //This gun uses SemiAuto
						_lastFireInput = value;
				}
			}
		}
		
		
	}
}

//if( TryFire() == true )
//	_lastFireInput = value;
//else
//	_lastFireInput = false; //We can't fire, so set our fire input to false so we will retry the next time this is called.