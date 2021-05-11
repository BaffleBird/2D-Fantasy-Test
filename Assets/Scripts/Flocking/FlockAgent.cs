using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
	Flock agentFlock;
	public Flock AgentFlock { get { return agentFlock; } }

	Collider2D agentCollider;
	public Collider2D AgentCollider { get { return agentCollider; } }

	public Vector3 offset = new Vector3(0,0,0.12f);

    void Start()
    {
		agentCollider = GetComponent<Collider2D>();

		Vector3 temp = transform.position;
		temp.z = temp.y;
		transform.position = temp + offset;
	}

	public void Initialize(Flock flock)
	{
		agentFlock = flock;
	}

	public void Move(Vector2 nextMove)
	{
		transform.up = nextMove;
		Vector3 temp = (Vector3)nextMove * Time.deltaTime;
		temp.z = temp.y;
		transform.position += temp;
	}
}
