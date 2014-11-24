using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using Excelsion.WeaponSystem;

[RAINAction]
public class ShootAtTarget : RAINAction
{
	//GunShooting gunShoot;
	public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		ThugAI thug = (ThugAI)ai.Body.gameObject.GetComponentInChildren< ThugAI >();

		GunInventory gunInv = thug.gunInv;
		
		GameObject obj = ai.WorkingMemory.GetItem<GameObject>("Player");
		if( obj == null || gunInv == null )
		{
			gunInv.InputFire = false;
			return ActionResult.FAILURE;
		}

		thug.PointAt( obj );

		if( gunInv.guns[gunInv.ActiveWeapon].ClipAmmo == 0 )
			gunInv.InputReload = true;
		else
			gunInv.InputReload = false;

		gunInv.InputFire = VectorExtras.SplitChance ();


		return ActionResult.RUNNING;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}