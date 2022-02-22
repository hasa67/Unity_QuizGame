using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsPanel : MonoBehaviour {

	public Image fullStarsImage;
	public Animator lockAnimator;

	private bool isRunning = true;
	private int starsFilled = 0;

	void Start(){
		fullStarsImage.fillAmount = 0;
	}

	public void HideLock(bool isHide){
		lockAnimator.gameObject.SetActive (!isHide);
	}

	public void FillStars(float starRate, float unlockedRate){
		StartCoroutine (FillStarsCo(starRate, unlockedRate));
	}

	IEnumerator FillStarsCo(float starRate, float unlockedRate){
		if (starRate > 0f) {
			yield return new WaitForSeconds (0.5f);
		}

		AudioManager.instance.PlayChargingSound ();
		float filler = 0f;
		while (filler < starRate) {
			filler += 0.5f * Time.deltaTime;
			PlayStarSound (filler);
			fullStarsImage.fillAmount = Mathf.Lerp (0f, 1f, filler);
			yield return null;
		}
		AudioManager.instance.StopChargingSound ();

		if (lockAnimator.gameObject.activeSelf == true) {
			yield return new WaitForSeconds (0.5f);

			if (starRate >= unlockedRate) {
				lockAnimator.SetTrigger (Animator.StringToHash ("IsUnlocked"));
				AudioManager.instance.PlayUnlockedSound ();
				yield return new WaitForSeconds (0.5f);
			} else {
				lockAnimator.SetTrigger (Animator.StringToHash ("IsLocked"));
				AudioManager.instance.PlayLockedSound ();
				yield return new WaitForSeconds (0.1f);
			}
		}

		isRunning = false;
	}

	public bool CoroutineRunning(){
		return isRunning;
	}

	public void FillStarsInstantly(float starRate){
		fullStarsImage.fillAmount = starRate;
	}

	private void PlayStarSound(float starRate){

		if (starsFilled == 0 && starRate >= 0.2) {
			AudioManager.instance.PlayStarSound (starsFilled);
			starsFilled++;
		} else if (starsFilled == 1 && starRate >= 0.4) {
			AudioManager.instance.PlayStarSound (starsFilled);
			starsFilled++;
		} else if (starsFilled == 2 && starRate >= 0.6) {
			AudioManager.instance.PlayStarSound (starsFilled);
			starsFilled++;
		} else if (starsFilled == 3 && starRate >= 0.8) {
			AudioManager.instance.PlayStarSound (starsFilled);
			starsFilled++;
		} else if (starsFilled == 4 && starRate >= 1) {
			AudioManager.instance.PlayStarSound (starsFilled);
			starsFilled++;
		}
	}
}
