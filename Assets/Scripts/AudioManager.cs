using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }
	public AudioSource musicSource;
	public AudioSource sfxSource;
	public Sound[] musicSounds;
	public Sound[] sfxSounds;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void PlayMusic(string name, float volume = 1) {
		Sound sound = Array.Find(musicSounds, x => x.name == name);

		if (sound != null) {
			musicSource.clip = sound.clip;
			musicSource.Play();
		}
	}

	public void PlayMusicAtPoint(string name, Vector3 position, float volume = 1) {
		Sound sound = Array.Find(musicSounds, x => x.name == name);

		if (sound != null) {
			AudioSource.PlayClipAtPoint(sound.clip, position);
		}
	}

	public void PlaySFX(string name, float volume = 1) {
		Sound sound = Array.Find(sfxSounds, x => x.name == name);

		if (sound != null) {
			sfxSource.PlayOneShot(sound.clip, volume);
		}
	}

	public void PlaySFXAtPoint(string name, Vector3 position, float volume = 1) {
		Sound sound = Array.Find(sfxSounds, x => x.name == name);

		if (sound != null) {
			AudioSource.PlayClipAtPoint(sound.clip, position);
		}
	}
}
