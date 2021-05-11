using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
	public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		//If no neighbhors, no adjustment
		if (context.Count == 0)
			return Vector2.zero;

		//Otherwise, Average out all neighbors and average out
		Vector2 avoidanceMove = Vector2.zero;
		int nAvoid = 0;
		List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
		for (int i = 0; i < filteredContext.Count; i++)
		{
			if (Vector2.SqrMagnitude(context[i].position - agent.transform.position) < flock.SquareAvoidanceRadius)
			{
				nAvoid++;
				avoidanceMove += (Vector2)(agent.transform.position - context[i].position);
			}
			
		}
		if (nAvoid > 0)
			avoidanceMove /= nAvoid;

		return avoidanceMove;
	}
}
