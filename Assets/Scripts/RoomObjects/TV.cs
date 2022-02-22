using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour {

	private AudioSource audioSource;
	private Animator animator;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
	}

	public void TurnOn(){
		audioSource.Play ();
		animator.SetBool ("TurnOn", true);
	}

	public void TurnOff(){
		audioSource.Stop ();
		animator.SetBool ("TurnOn", false);
	}
}
