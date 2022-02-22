using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonButton : MonoBehaviour {

	public Text titleText;
	public GameObject lockPanel;
	public StarsPanel starsPanel;

	private int gradeIndex;
	private int courseIndex;
	private int lessonIndex;
	private int groupIndex;
	private int groupCount;
	private bool isLocked = true;
	private bool isSetup = false;
	private string lessonTitle;

	public void Setup(int gradeId, int courseId, int lessonId, int groupId){
		gradeIndex = gradeId;
		courseIndex = courseId;
		lessonIndex = lessonId;
		groupIndex = groupId;
		groupCount = DataManager.instance.GetGroupCount ();

		string unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex, groupIndex);
		int unlocked = PlayerPrefs.GetInt (unlockedKey, 0); 
		if (lessonId == 0 || unlocked == 1) {
			isLocked = false;
		} else {
			isLocked = true;
		}
		lockPanel.SetActive (isLocked);

		if (groupIndex <= 0) {
//			string lessonName = DataManager.instance.GetLessonName (gradeIndex, courseIndex, lessonIndex);
//			lessonTitle = "درس " + (lessonIndex + 1).ToString () + " : " + lessonName;
			lessonTitle = DataManager.instance.GetLessonTitle(gradeIndex, courseIndex, lessonIndex);
			titleText.text = lessonTitle;
		} else {
			if (groupIndex <= groupCount) {
				lessonTitle = "مرور شماره " + groupIndex.ToString ();
			} else {
				lessonTitle = "آزمون نهایی";
			}
		}

		titleText.text = lessonTitle;
		isSetup = true;
	}

	void Update(){
		string scoreKey = DataManager.instance.GetScoreKey (gradeIndex, courseIndex, lessonIndex, groupIndex);
		float lessonScore = PlayerPrefs.GetFloat (scoreKey, 0f);
		starsPanel.FillStarsInstantly (lessonScore);
	}

	public void ClickHandler(){
		if (isLocked) {
			GetComponent<Animator> ().SetTrigger (Animator.StringToHash ("IsLocked"));
			AudioManager.instance.PlayLockedSound ();
		} else {
			DataManager.instance.SetSelectedIndexes (gradeIndex, courseIndex, lessonIndex, groupIndex);
			DataManager.instance.SetLessonTitle (lessonTitle);
			AudioManager.instance.PlayNextButtonSound ();
		}
	}

	public int GetGroupIndex(){
		return groupIndex;
	}

	public void SetGroupIndex(int groupId){
		groupIndex = groupId;
	}

	public bool IsSetup(){
		return isSetup;
	}
}
