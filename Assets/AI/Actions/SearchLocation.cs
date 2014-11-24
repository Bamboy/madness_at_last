using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Graph;

[RAINAction]
public class SearchLocation : RAINAction {
	private float time = 0.0f;
    public override void Start(RAIN.Core.AI ai){
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai){
		Vector3 location = Vector3.zero;
		List<RAINNavigationGraph> found = new List<RAINNavigationGraph>();
		do{
			location = new Vector3(ai.Kinematic.Position.x + Random.Range(-8.0f, 8.0f), ai.Kinematic.Position.y, ai.Kinematic.Position.z + Random.Range(-8.0f, 8.0f));
			found = NavigationManager.Instance.GraphForPoint(location, ai.Motor.StepUpHeight, NavigationManager.GraphType.Navmesh,((BasicNavigator)ai.Navigator).GraphTags);
		} while ((Vector3.Distance(ai.Kinematic.Position, location) < 2f) || (found.Count == 0));
		ai.WorkingMemory.SetItem<Vector3>("nextNode", location);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai){
        base.Stop(ai);
    }
}