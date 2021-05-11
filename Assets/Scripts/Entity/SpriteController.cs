using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
	//Play Animations, manage sprites and materials for one entity
	//If you got a sprite swapping component, track those instead

	//[SerializeField] SpriteRenderer mainSprite = null;
	[SerializeField] List<Animator> linkedAnimators = new List<Animator>();

	public void PlayLinked(string clipName)
	{
		for(int i = 0; i < linkedAnimators.Count; i++)
		{
			linkedAnimators[i].Play(clipName);
		}
	}

	public bool GetLinkedBool(string booleanName) => linkedAnimators[0].GetBool(booleanName);
	public void SetLinkedBool(string booleanName, bool newBool)
	{
		for (int i = 0; i < linkedAnimators.Count; i++)
			linkedAnimators[i].SetBool(booleanName, newBool);
	}

	public float GetLinkedFloat(string floatName) => linkedAnimators[0].GetFloat(floatName);
	public void SetLinkedFloat(string floatName, float newFloat)
	{
		for (int i = 0; i < linkedAnimators.Count; i++)
			linkedAnimators[i].SetFloat(floatName, newFloat);
	}

	public int GetLinkedInteger(string intName) => linkedAnimators[0].GetInteger(intName);
	public void SetLinkedInteger(string intName, int newInt)
	{
		for (int i = 0; i < linkedAnimators.Count; i++)
			linkedAnimators[i].SetInteger(intName, newInt);
	}
}
