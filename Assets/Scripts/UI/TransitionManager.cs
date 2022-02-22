using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

	public static TransitionManager instance { get; private set; }

	public Animator animator;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		animator = GetComponent<Animator> ();
	}

	public void ChangeScene(string sceneName){
		StartCoroutine (FadeInFadeOut (sceneName));
	}

	IEnumerator FadeInFadeOut(string sceneName){
		FadeIn ();
		float transitionTime = GetTransitionTime ();
		yield return new WaitForSeconds (transitionTime);
		SceneManager.LoadScene (sceneName);
//		SetCanvasCamera ();
		FadeOut ();
	}

	public void FadeIn(){
		animator.SetTrigger (Animator.StringToHash ("FadeIn"));
	}

	public void FadeOut(){
		animator.SetTrigger (Animator.StringToHash ("FadeOut"));
	}

	public float GetTransitionTime(){
		return animator.GetCurrentAnimatorStateInfo (0).length;
	}

	public void SetCanvasCamera(){
		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
	}
}
