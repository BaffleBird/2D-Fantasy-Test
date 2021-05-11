using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputHandler : MonoBehaviour
{
	protected Vector2 move = new Vector2();
	protected Vector2 direction = new Vector2();
	protected Dictionary<string, bool> inputs = new Dictionary<string, bool>();

	public Vector2 GetMovement()
	{
		return move;
	}

	public Vector2 GetDirection()
	{
		return direction;
	}

	public bool GetInput(string s)
	{
		return inputs[s];
	}
}