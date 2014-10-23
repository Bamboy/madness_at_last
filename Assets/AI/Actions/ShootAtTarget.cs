using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

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
		GunShooting gunShoot = ai.Body.gameObject.GetComponentInChildren<GunShooting>();

		GameObject obj = ai.WorkingMemory.GetItem<GameObject>("chaseTarget");
		if( obj == null || gunShoot == null )
		{
			gunShoot.FireWeapon = false;
			return ActionResult.FAILURE;
		}
		
		gunShoot.FireWeapon = VectorExtras.SplitChance ();


		return ActionResult.RUNNING;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}