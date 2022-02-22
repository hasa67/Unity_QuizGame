using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour {
	public Text lessonName;
	public Text resultText;

	public void Setup(int gradeId, int courseId, int lessonId){
		lessonName.text = DataManager.instance.GetLessonTitle ();
		float lessonResult = DataManager.instance.GetLessonResult (gradeId, courseId, lessonId - 1);
		resultText.text = lessonResult.ToString ();
	}
}
