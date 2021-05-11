using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
	{
		List<Transform> filtered = new List<Transform>();
		for(int i = 0; i < original.Count; i++)
		{
			FlockAgent itemAgent = original[i].GetComponent<FlockAgent>();
			if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
			{
				filtered.Add(original[i]);
			}
		}
		return filtered;
	}
}
