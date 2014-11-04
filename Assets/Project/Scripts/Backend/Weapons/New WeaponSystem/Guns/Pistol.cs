using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;

namespace Excelsion.WeaponSystem.Weapons
{
	public class Pistol : GunRay
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
					if( FireType.SemiAuto( value, _lastFireInput ) )
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