using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class SetTargetDistance : RAINAction
{
    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		GameObject target = ai.WorkingMemory.GetItem<GameObject>("chaseTarget");
		if( target == null )
		{
			ai.WorkingMemory.SetItem<float>("targetDistance", 0.0f);
			return ActionResult.FAILURE;
		}

		ai.WorkingMemory.SetItem<float>("targetDistance", Vector3.Distance( ai.Body.transform.position, target.transform.position ));
		return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}