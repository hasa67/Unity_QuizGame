using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseButton : MonoBehaviour {
	public Text courseTitle;

	private MenuController menuController;
	private int gradeIndex;
	private int courseIndex;

	public void Setup(int gradeId, int courseId){
		string courseName = DataManager.instance.GetCourseName (gradeId, courseId);
		courseTitle.text = courseName;

		courseIndex = courseId;
		gradeIndex = gradeId;

		menuController = FindObjectOfType<MenuController> ();
	}

	public void ClickHandler(){
		bool showResults = menuController.IsShowingResults ();
		if (showResults) {
			menuController.ShowResultsPanel (gradeIndex, courseIndex);
		} else {
			menuController.ShowLessonsPanel (gradeIndex, courseIndex);
		}
		AudioManager.instance.PlayNextButtonSound ();
	}
}