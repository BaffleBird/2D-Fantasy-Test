using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : InputHandler
{
	private PlayerControls controls;

	private void Awake()
	{
		controls = new PlayerControls();
	}

	private void OnEnable()
	{
		controls.Enable();
	}

	private void Update()
	{
		move = controls.PlayerCharacter.Movement.ReadValue<Vector2>();
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			Application.Quit();
		}
	}

	private void OnDisable()
	{
		controls.Disable();
	}
}
