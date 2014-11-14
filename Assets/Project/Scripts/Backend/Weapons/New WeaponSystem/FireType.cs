using UnityEngine;
using System.Collections;

namespace Excelsion.WeaponSystem
{
	public static class FireType : System.Object
	{
		public static bool SemiAuto( bool thisInput, bool lastInput )
		{
			if( lastInput != thisInput && thisInput == true ) //Did the value change to true since the last time this was called?
			{
				//TODO finish this!
				
				return true;
			}
			return false;
		}

		public static bool Automatic( bool thisInput, bool lastInput )
		{
			return thisInput;
		}

	}
}