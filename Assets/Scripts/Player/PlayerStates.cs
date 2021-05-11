using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public IdleState(StateMachine stateMachine) : base(stateMachine) { }

	public sealed override void StartState()
	{
		stateMachine.EntitySprite.PlayLinked("Idle");
	}

	public sealed override void UpdateState()
	{
		if (stateMachine.InputSource.GetMovement() != Vector2.zero)
			stateMachine.SwitchState("Move");
	}

	public sealed override void FixedUpdateState()
	{
	}

	public sealed override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public sealed override void EndState()
	{
		
	}
}

public class MoveState : State
{
	//Shorthanding some stuff
	Vector2 movement;
	Vector2 oldMovement;

	public MoveState(StateMachine stateMachine) : base(stateMachine) { }

	public sealed override void StartState()
	{
		stateMachine.EntitySprite.PlayLinked("Move");
		movement = stateMachine.InputSource.GetMovement();
		stateMachine.EntitySprite.SetLinkedFloat("Vertical", movement.y);
		stateMachine.EntitySprite.SetLinkedFloat("Horizontal", movement.x);
		
	}
	
	public sealed override void UpdateState()
	{
		movement = stateMachine.InputSource.GetMovement();

		if (movement == Vector2.zero)
			stateMachine.SwitchState("Idle");
		if (movement.x == 0 ^ movement.y == 0)
		{
			stateMachine.EntitySprite.SetLinkedFloat("Horizontal", movement.x);
			stateMachine.EntitySprite.SetLinkedFloat("Vertical", movement.y);
		}

		if (stateMachine.EntityRadio.signals[0])
		{
			SoundPool.instance.PlaySound("Hard Step");
			stateMachine.EntityRadio.signals[0] = false;
		}
	}

	public sealed override void FixedUpdateState()
	{
	}

	public sealed override Vector3 MotionUpdate()
	{
		return movement * stateMachine.CharacterStatus.moveSpeed * Time.fixedDeltaTime;
	}

	public sealed override void EndState()
	{
		
	}
}