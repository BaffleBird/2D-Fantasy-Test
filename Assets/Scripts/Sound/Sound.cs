using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;

	public bool isLoop;

	public float maxDistance = 16.4f;
}
