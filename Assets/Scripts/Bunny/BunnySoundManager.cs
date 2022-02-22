using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BunnySoundManager : MonoBehaviour {
	[Header("Audio Clips")]
	public AudioClip[] hiClips;
	public AudioClip drawingClip;

	[Header("Mixer Group")]
	public AudioMixerGroup bunnyGroup;
	public AudioMixerGroup objectsGroup;

	private AudioSource bunnySource;
	private AudioSource objectSource;

	void Awake(){
		bunnySource = gameObject.AddComponent<AudioSource> () as AudioSource;
		objectSource = gameObject.AddComponent<AudioSource> () as AudioSource;

		bunnySource.outputAudioMixerGroup = bunnyGroup;
		objectSource.outputAudioMixerGroup = objectsGroup;
	}

	public void PlayHi(){
		AudioClip hiClip = hiClips [Random.Range (0, hiClips.Length)];
		bunnySource.clip = hiClip;
		bunnySource.Play ();
	}

	public void PlayDrawingSound(){
		objectSource.clip = drawingClip;
		objectSource.Play ();
	}

	public void MuteObjectSource(){
		objectSource.Stop ();
	}
}
