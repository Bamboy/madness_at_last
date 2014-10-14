using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class SetRandomFacing : RAINAction
{
    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		Vector3 facing = new Vector3( Random.Range( -10.0f, 10.0f ), Random.Range( -360.0f, 360.0f ), 0.0f );
		ai.WorkingMemory.SetItem<Vector3>("searchFacing", facing);

		return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}