using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour {

	public Text titleText;
	public GameObject resultsDisplayPrefab;
	public Transform contentTransform;

	private List<GameObject> resultDispleyList = new List<GameObject> ();

	public void Setup(int gradeId, int courseId){
		string gradeName = DataManager.instance.GetGradeName (gradeId);
		string courseName = DataManager.instance.GetCourseName (gradeId, courseId);
		titleText.text = courseName + " - " + gradeName;

		HideAllButtons ();
		int lessonCount = DataManager.instance.GetLessonCount (gradeId, courseId);

		int groupSize = DataManager.instance.GetGroupSize ();
		int remainder = lessonCount % groupSize;
		int groupCount = lessonCount / groupSize;
		if (remainder >= (float)groupSize / 2f) {
			groupCount++;
		}
		DataManager.instance.SetGroupCount (groupCount);

		string unlockedKey;
		int isUnlocked;
		for (int i = 0; i < lessonCount; i++) {
			for (int j = 0; j < groupCount; j++) {
				unlockedKey = DataManager.instance.GetUnlocekdKey (gradeId, courseId, i, -j);
				isUnlocked = PlayerPrefs.GetInt (unlockedKey, 0);

				if (isUnlocked == 1) {
					GameObject resultDisplay = Instantiate (resultsDisplayPrefab, contentTransform);
					resultDisplay.GetComponent<ResultDisplay> ().Setup (gradeId, courseId, i);
					resultDispleyList.Add (resultDisplay);
					return;
				}
			}

		}
	}

	private void HideAllButtons(){
		foreach (var resultDisplay in resultDispleyList) {
			Destroy (resultDisplay);
		}
		resultDispleyList.Clear ();
	}
}
