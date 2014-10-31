using UnityEngine;
using System.Collections;

public class MimicRotation : MonoBehaviour 
{
	public Transform objToMimic;
	
	// Update is called once per frame, but only after all other Updates have called first.
	void LateUpdate () 
	{
		if( objToMimic != null )
			this.transform.rotation = objToMimic.rotation;
	}
}
