using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
	[SerializeField] GameObject uiBlock;

    void Start()
    {
		uiBlock.SetActive(false);
    }

    void Update()
    {
		if (Keyboard.current.tabKey.isPressed && !uiBlock.activeSelf)
			uiBlock.SetActive(true);
		else if (!Keyboard.current.tabKey.isPressed && uiBlock.activeSelf)
			uiBlock.SetActive(false);

		if (Keyboard.current.rKey.isPressed)
			SceneManager.LoadScene(0);
	}

	public void EndGame()
	{
		Application.Quit();
	}
}
