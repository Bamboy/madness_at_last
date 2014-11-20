using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 09/20/14     ////
///////////////////////////////////

namespace Excelsion.WeaponSystem {

	[System.Serializable]
	public class BulletInfo
	{
		public Ray ray;
		public RaycastHit data;
		
		public BulletInfo( Ray myRay, RaycastHit myData )
		{
			ray = myRay;
			data = myData;
		}
	}
}