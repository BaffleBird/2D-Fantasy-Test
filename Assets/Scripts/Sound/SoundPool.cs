using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPool : MonoBehaviour
{
	private static SoundPool _instance;
	public static SoundPool instance { get { return _instance; } }

	[SerializeField] Sound[] sounds;
	private static Dictionary<string, int> soundDictionary;

	[SerializeField] AudioSource SFXSource;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		soundDictionary = new Dictionary<string, int>();

		for (int i = 0; i < sounds.Length; i++)
		{
			soundDictionary.Add(sounds[i].name, i);
		}
	}
    public void PlaySound(string soundName)
	{
		SetSFXSound(soundName);
		SFXSource.PlayOneShot(SFXSource.clip);
	}

	public void PlaySoundAtPoint(string soundName, Vector3 soundPosition)
	{
		SetSFXSound(soundName);
		AudioSource.PlayClipAtPoint(SFXSource.clip, soundPosition);
	}

	public void SetSFXSound(string soundName)
	{
		Sound sound = sounds[soundDictionary[soundName]];
		SFXSource.clip = sound.clip;
		SFXSource.volume = sound.volume;
		SFXSource.pitch = sound.pitch;
		SFXSource.loop = sound.isLoop;
		SFXSource.maxDistance = sound.maxDistance;
	}
}
