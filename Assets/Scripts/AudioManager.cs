using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }
	public AudioSource musicSource;
	public AudioSource sfxSource;
	public AudioSource fireSource;
	public AudioSource getHitSource;
	public Sound[] musicSounds;
	public Sound[] sfxSounds;

	private bool isSourcePlayingMultipleClip;

	private void OnEnable() {
		Campfire.setAudioSource += SetAudioSourceParent;
	}

	private void OnDisable() {
		Campfire.setAudioSource -= SetAudioSourceParent;
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void PlayMusic(string clipName) {
		AudioSource source = AudioSourceToUse(clipName);
		if (source == null)
			source = musicSource;
		Sound sound = Array.Find(musicSounds, x => x.name == clipName);

		if (sound != null) {
			source.clip = sound.clip;
			source.Play();
		}
	}

	public void PlaySFX(string clipName) {
		AudioSource source = AudioSourceToUse(clipName);
		Sound sound = Array.Find(sfxSounds, x => x.name == clipName);

		if (sound != null) {
			if (source != null) {
				if (isSourcePlayingMultipleClip)
					source.PlayOneShot(sound.clip);
				else {
					source.clip = sound.clip;
					source.Play();
				}
			}
			else {
				sfxSource.PlayOneShot(sound.clip);
			}
		}
	}

	public void PlayMusicAtPoint(string clipName, Vector3 position, float volume = 1) {
		Sound sound = Array.Find(musicSounds, x => x.name == clipName);

		if (sound != null) {
			AudioSource.PlayClipAtPoint(sound.clip, position);
		}
	}

	public void PlaySFXAtPoint(string clipName, Vector3 position, float volume = 1) {
		Sound sound = Array.Find(sfxSounds, x => x.name == clipName);

		if (sound != null) {
			AudioSource.PlayClipAtPoint(sound.clip, position);
		}
	}

	public AudioSource AudioSourceToUse(string clipName) {
		AudioSource source = null;

		switch (clipName) {
			case "Fire":
				source = fireSource;
				isSourcePlayingMultipleClip = false;
				break;

			case "Hit" or "Grunt":
				source = getHitSource;
				isSourcePlayingMultipleClip = true;
				break;
		}

		return source;
	}

	public void SetAudioSourceParent(Transform parent, string clipName) {
		switch (clipName) {
			case "Fire":
				fireSource.transform.position = parent.position;
				break;
		}
	}
}
