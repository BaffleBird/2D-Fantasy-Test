using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{
	public float springConstant = 0.05f;
	public float damping = 0.07f;
	public float velocity = 0f;
	public float acceleration = 0f;

	float springPosition = 0f;
	float neutralPosition = 0f;

	public float Simulate()
	{
		var force = springConstant * (springPosition - neutralPosition) + velocity * damping;
		acceleration -= force;
		springPosition += velocity;
		velocity += acceleration;

		return springPosition;
	}

	public void ApplyForceAtPosition(float force, float position)
	{
		acceleration = 0f;
		springPosition = position;
		velocity = force;
	}

	public void ApplyConstantForce(float force)
	{
		velocity += force;
	}
}
