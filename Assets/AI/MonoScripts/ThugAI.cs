using UnityEngine;
using System.Collections;
using Excelsion.WeaponSystem;

public class ThugAI : MonoBehaviour 
{
	public GunInventory gunInv;
	public Transform shootTransform;

	// Use this for initialization
	void Start () {
		gunInv = transform.parent.GetComponent< GunInventory >();
	}

	void Update()
	{

	}

	public void PointAt( GameObject target )
	{
		shootTransform.LookAt( target.transform, Vector3.up );
	}
}
