using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior
{
	public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		//If no neighbhors, maintain alignment
		if (context.Count == 0)
			return agent.transform.up;

		//Otherwise, Average out all neighbors and average out
		Vector2 alignmentMove = Vector2.zero;
		List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
		for (int i = 0; i < filteredContext.Count; i++)
		{
			alignmentMove += (Vector2)context[i].transform.up;
		}
		alignmentMove /= context.Count;

		return alignmentMove;
	}
}
