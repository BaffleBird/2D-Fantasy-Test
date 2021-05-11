using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{
	Vector2 currentVelocity;
	public float agentSmoothTime = 0.5f;

	public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		//If no neighbhors, no adjustment
		if (context.Count == 0)
			return Vector2.zero;

		//Otherwise, Average out all neighbors and average out
		Vector2 cohesionMove = Vector2.zero;
		List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
		for (int i = 0; i < filteredContext.Count; i++)
		{
			cohesionMove += (Vector2)context[i].position;
		}
		cohesionMove /= context.Count;

		//Create offset
		cohesionMove -= (Vector2)agent.transform.position;
		cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
		return cohesionMove;
	}
}
