using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class ClearTarget : RAINAction
{
    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		ai.WorkingMemory.SetItem<GameObject>("chaseTarget", null);
		ai.WorkingMemory.SetItem<float>("targetDistance", 0.0f);
		ai.WorkingMemory.SetItem<bool>("canSeeTarget", false);
		ai.WorkingMemory.SetItem<bool>("alerted", false);
		return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}