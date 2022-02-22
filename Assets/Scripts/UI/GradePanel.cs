using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradePanel : MonoBehaviour {

	public Text gradeTitleText;
	public GameObject courseButtonPrefab;
	public Transform coursePanelTransform;

	private List<GameObject> courseButtonsList = new List<GameObject> ();


	public void Setup(int gradeIndex){
		string gradeName = DataManager.instance.GetGradeName (gradeIndex);
		gradeTitleText.text = gradeName;
		HideCouseButtons ();

		int courseCount = DataManager.instance.GetCourseCount (gradeIndex);
		for (int i = 0; i < courseCount; i++) {
			GameObject newButton = Instantiate(courseButtonPrefab,coursePanelTransform);
			newButton.GetComponent<CourseButton> ().Setup (gradeIndex, i);
			courseButtonsList.Add (newButton);
		}
	}

	private void HideCouseButtons(){
		foreach (var item in courseButtonsList) {
			Destroy (item);
		}
		courseButtonsList.Clear ();
	}
}
