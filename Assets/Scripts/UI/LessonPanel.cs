using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonPanel : MonoBehaviour {

	public Text titleText;
	public GameObject lessonButtoPrefab;
	public Transform contentTransform;

	private List<GameObject> lessonButtonsList = new List<GameObject> ();
	
	public void Setup(int gradeId, int courseId){
		string gradeName = DataManager.instance.GetGradeName (gradeId);
		string courseName = DataManager.instance.GetCourseName (gradeId, courseId);
		titleText.text = courseName + " - " + gradeName;

		HideAllButtons ();
		int lessonCount = DataManager.instance.GetLessonCount (gradeId, courseId);

		int groupIndex = 1;
		int groupSize = DataManager.instance.GetGroupSize ();
		int remainder = lessonCount % groupSize;
		int groupCount = lessonCount / groupSize;
		if (remainder >= (float)groupSize / 2f) {
			groupCount++;
		}
		DataManager.instance.SetGroupCount (groupCount);

		for (int i = 0; i < lessonCount; i++) {
			// lesson buttons
			GameObject lessonButton = Instantiate(lessonButtoPrefab, contentTransform);
			lessonButton.GetComponent<LessonButton> ().Setup (gradeId, courseId, i, 0);
			lessonButtonsList.Add (lessonButton);

			// group buttons
			int j = i + 1;
			if (remainder >= (float)groupSize / 2f) {
				if (j % groupSize == 0 || j == lessonCount) {
					GameObject groupButton = Instantiate (lessonButtoPrefab, contentTransform);
					groupButton.GetComponent<LessonButton> ().Setup (gradeId, courseId, i, groupIndex);
					lessonButtonsList.Add (groupButton);
					groupIndex++;
				}
			} else {
				if ((j < (lessonCount - remainder) && j % groupSize == 0) || (j == lessonCount)) {
					GameObject groupButton = Instantiate (lessonButtoPrefab, contentTransform);
					groupButton.GetComponent<LessonButton> ().Setup (gradeId, courseId, i, groupIndex);
					lessonButtonsList.Add (groupButton);
					groupIndex++;
				}
			}

			// final button
			if (j == lessonCount) {
				GameObject finalButton = Instantiate(lessonButtoPrefab, contentTransform);
				finalButton.GetComponent<LessonButton> ().Setup (gradeId, courseId, i, groupIndex);
				lessonButtonsList.Add (finalButton);
			}
		}

		for (int i = 1; i < lessonButtonsList.Count; i++) {
			groupIndex = lessonButtonsList [i].GetComponent<LessonButton> ().GetGroupIndex ();
			if (groupIndex > 0 && groupIndex <= groupCount) {
				lessonButtonsList [i - 1].GetComponent<LessonButton> ().SetGroupIndex (-groupIndex);
			}
		}
	}

	private void HideAllButtons(){
		foreach (var button in lessonButtonsList) {
			Destroy (button);
		}
		lessonButtonsList.Clear ();
	}
}
