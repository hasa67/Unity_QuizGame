using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour {

	public Color defaultColor = new Color32 (255, 255, 255, 125);
	public GameObject progressImagePrefab;

	private int cellCount;
	private List<GameObject> progressImagesList = new List<GameObject> ();

	public void Setup(int maxValue){
		for (int i = 0; i < maxValue; i++) {
			GameObject newObject = Instantiate (progressImagePrefab, this.transform);
			progressImagesList.Add (newObject);
		}
		cellCount = maxValue;
	}

	public void ProgressAnimation(bool isCorrect, int cellIndex){
		if (isCorrect) {
			progressImagesList [cellCount - cellIndex - 1].GetComponent<Animator> ().SetTrigger ("isCorrect");
		} else {
			progressImagesList [cellCount - cellIndex - 1].GetComponent<Animator> ().SetTrigger ("isFalse");
		}
	}
}
