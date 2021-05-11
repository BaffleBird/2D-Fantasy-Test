using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRadio : MonoBehaviour
{
	public bool[] signals = new bool[4];

    public void SetSignal(int i)
	{
		if( i >= 0 && i < signals.Length)
		{
			signals[i] = true;
		}
	}

	public void ResetSignals()
	{
		for(int i = 0; i < signals.Length; i++)
		{
			signals[i] = false;
		}
	}
}
