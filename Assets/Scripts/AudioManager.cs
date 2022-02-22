using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[Header("Music Clips")]
	public AudioClip mainMenuBackgroundClip;
	public AudioClip quizGameBackgroundClip;
	public AudioClip bunnyRoomBackgroundClip;

	[Header("Button Clips")]
	public AudioClip[] answerButtonClips;
	public AudioClip[] purchaseClips;

	[Header("UI Clips")]
	public AudioClip nextButtonClip;
	public AudioClip backButtonClip;
	public AudioClip[] starClips;
	public AudioClip starChargeClip;
	public AudioClip lockedClip;
	public AudioClip unlockedClip;

	[Header("Mixer Group")]
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup effectsGroup;

	private AudioSource musicSource;
	private AudioSource buttonSource;
	private AudioSource questionSource;
	private AudioSource uiButtonSource;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		musicSource = gameObject.AddComponent<AudioSource> () as AudioSource;
		buttonSource = gameObject.AddComponent<AudioSource> () as AudioSource;
		questionSource = gameObject.AddComponent<AudioSource> () as AudioSource;
		uiButtonSource = gameObject.AddComponent<AudioSource> () as AudioSource;

		musicSource.loop = true;

		musicSource.outputAudioMixerGroup = musicGroup;
		questionSource.outputAudioMixerGroup = effectsGroup;
		buttonSource.outputAudioMixerGroup = effectsGroup;
		uiButtonSource.outputAudioMixerGroup = effectsGroup;
	}

	void Start(){
//		if (SceneManager.GetActiveScene ().buildIndex != 0) {
//			return;
//		} else {
//			ChangeBackgroundMusic(0);
//		}
		ChangeBackgroundMusic(2);
	}

	public void ChangeBackgroundMusic(int musicNumber){
		AudioClip backgroundClip = null;

		switch (musicNumber) {
		case 0:
			backgroundClip = mainMenuBackgroundClip;
			break;
		case 1:
			backgroundClip = quizGameBackgroundClip;
			break;
		case 2:
			backgroundClip = bunnyRoomBackgroundClip;
			break;
		}

		musicSource.clip = backgroundClip;
		musicSource.Play ();
	}

	public void StopMusic(){
		musicSource.Stop ();
	}

	public void PlayMusic(){
		musicSource.Play ();
	}

	public void PlayAnswerButtonSound(bool isCorrect){
		if (isCorrect) {
			buttonSource.clip = answerButtonClips [1];
		} else {
			buttonSource.clip = answerButtonClips [0];
		}

		buttonSource.Play ();
	}

	public void PlayePurchaseSound(int isPurchased){
		if (isPurchased == 1) {
			buttonSource.clip = purchaseClips [1];
		} else {
			buttonSource.clip = purchaseClips [0];
		}
		buttonSource.Play ();
	}

	public void PlayeQuestionSound(AudioClip clip){
		questionSource.clip = clip;
		questionSource.Play ();
	}

	public void ReplayQuestionSound(){
		questionSource.Play ();
	}

	public void StopQuestionSound(){
		questionSource.Stop ();
	}

	public void PlayNextButtonSound(){
		uiButtonSource.clip = nextButtonClip;
		uiButtonSource.Play ();
	}

	public void PlayBackButtonSound(){
		uiButtonSource.clip = backButtonClip;
		uiButtonSource.Play ();
	}

	public void PlayStarSound(int level){
		uiButtonSource.clip = starClips [level];
		uiButtonSource.Play ();
	}

	public void PlayChargingSound(){
		buttonSource.clip = starChargeClip;
		buttonSource.Play ();
	}

	public void StopChargingSound(){
		buttonSource.Stop ();
	}

	public void PlayLockedSound(){
		uiButtonSource.clip = lockedClip;
		uiButtonSource.Play ();
	}

	public void PlayUnlockedSound(){
		uiButtonSource.clip = unlockedClip;
		uiButtonSource.Play ();
	}
}
