using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SM : StateMachine
{
	protected override void Awake()
	{
		base.Awake();

		//Add possible Inputs

		//Add all possible States
		stateList.Add("Idle", new IdleState(this));
		stateList.Add("Move", new MoveState(this));

		//Set a default state
		currentState = stateList["Idle"];
	}
}
