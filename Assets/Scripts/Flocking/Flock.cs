using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	public FlockAgent agentPrefab;
	List<FlockAgent> agents = new List<FlockAgent>();
	public FlockBehavior behavior;

	[Range(1, 32)]
	public int flockCount = 4;
	const float AgentDensity = 0.08f;

	[Range(0.001f, 100f)]
	public float driveFactor = 10f;
	[Range(0.001f, 100f)]
	public float maxSpeed = 5f;
	[Range(0.001f, 10f)]
	public float neighborRadius = 1.5f;
	[Range(0.001f, 10f)]
	public float avoidanceRadiusMultiplier = 1f;

	float squareMaxSpeed;
	float squareNeightborRadius;
	float squareAvoidanceRadius;
	public float SquareAvoidanceRadius{ get { return squareAvoidanceRadius; } }

	void Start()
    {
		squareMaxSpeed = maxSpeed * maxSpeed;
		squareNeightborRadius = squareNeightborRadius * squareNeightborRadius;
		squareAvoidanceRadius = squareNeightborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

		for (int i = 0; i < flockCount; i++)
		{
			FlockAgent newAgent = Instantiate(
				agentPrefab,
				transform.position + (Vector3)(Random.insideUnitCircle * flockCount * AgentDensity),
				Quaternion.Euler(45,0, Random.Range(0, 360)),
				transform
				);
			newAgent.name = "Agent " + i;
			newAgent.Initialize(this);
			agents.Add(newAgent);
		}

	}

    void Update()
    {
        for (int i = 0; i < agents.Count; i++)
		{
			List<Transform> context = GetNearbyObjects(agents[i]);
			Vector2 move = behavior.CalculateMove(agents[i], context, this);
			move *= driveFactor;
			if (move.sqrMagnitude > squareMaxSpeed)
			{
				move = move.normalized * maxSpeed;
			}
			agents[i].Move(move);
		}
    }

	List<Transform> GetNearbyObjects(FlockAgent agent)
	{
		List<Transform> context = new List<Transform>();
		Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
		for (int i = 0; i < contextColliders.Length; i++)
		{
			if (contextColliders[i] != agent.AgentCollider)
			{
				context.Add(contextColliders[i].transform);
			}
		}
		return context;
	}
}
