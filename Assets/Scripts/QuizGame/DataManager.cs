using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataManager : MonoBehaviour {

	public static DataManager instance { get; private set; }

	private GradeData[] allGradesData;
	private string gameDataFileName = "data.json";

	private int gradeIndex = -1;
	private int courseIndex;
	private int lessonIndex;
	private int groupIndex;
	private int groupCount;
	private string lessonTitle;

	private int resultsPerLesson = 30;
	private int groupSize = 3;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
		DontDestroyOnLoad (gameObject);
		StartCoroutine (LoadGameData ());
	}
		
	IEnumerator LoadGameData(){
		string filePath = Path.Combine (Application.streamingAssetsPath, gameDataFileName);
		string dataAsJson = "";

		if (filePath.Contains ("://")) {
			WWW www = new WWW (filePath);
			yield return www;
			dataAsJson = www.text;
		} else {
			dataAsJson = File.ReadAllText (filePath);
		}

		GameData loadedData = JsonUtility.FromJson<GameData> (dataAsJson);
		allGradesData = loadedData.gradeData;

		// make sure data is loaded;
//		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		TransitionManager.instance.FadeOut();
	}

	public int GetGradeCount(){
		int gradeCount = allGradesData.Length;
		return gradeCount;
	}

	public string GetGradeName(int gradeId){
		string gradeName = allGradesData [gradeId].gradeName;
		return gradeName;
	}

	public int GetCourseCount(int gradeId){
		int courseCount = allGradesData [gradeId].courseData.Length;
		return courseCount;
	}

	public string GetCourseName(int gradeId, int courseId){
		string courseName = allGradesData [gradeId].courseData [courseId].courseName;
		return courseName;
	}

	public int GetLessonCount(int gradeId, int courseId){
		int lessonCount = allGradesData [gradeId].courseData [courseId].lessonData.Length;
		return lessonCount;
	}

	public string GetLessonName(int gradeId, int courseId, int lessonId){
		string lessonName = allGradesData [gradeId].courseData [courseId].lessonData [lessonId].lessonName;
		return lessonName;
	}

	public string GetLessonTitle(int gradeId, int courseId, int lessonId){
		string lessonName = GetLessonName (gradeId, courseId, lessonId);
		string lessonTitle = "درس " + (lessonId + 1).ToString () + " : " + lessonName;
		return lessonTitle;
	}

	public void SetLessonTitle(string title){
		lessonTitle = title;
	}

	public string GetLessonTitle(){
		return lessonTitle;
	}

	public void SetSelectedIndexes(int gradeId, int courseId, int lessonId, int groupId){
		gradeIndex = gradeId;
		courseIndex = courseId;
		lessonIndex = lessonId;
		groupIndex = groupId;

		StartGame ();
	}

	public int GetGroupSize(){
		return groupSize;
	}

	public void SetGroupCount(int buttonsCount){
		groupCount = buttonsCount;
	}

	public int GetGroupCount(){
		return groupCount;
	}

	public int[] GetSelectedIndexes(){
		int[] indexes = {gradeIndex, courseIndex, lessonIndex, groupIndex};
		return indexes;
	}

	public void ResetGradeIndex(){
		gradeIndex = -1;
	}

	public void StartGame(){
		AudioManager.instance.ChangeBackgroundMusic (1);
		TransitionManager.instance.ChangeScene("GameScene");
	}

	public QuestionData[] GetQuestions(int gradeId, int courseId, int lessonId){
		QuestionData[] questions = allGradesData [gradeId].courseData [courseId].lessonData [lessonId].questions;
		foreach (var question in questions) {
			question.gradeIndex = gradeId;
			question.courseIndex = courseId;
			question.lessonIndex = lessonId;
		}
		return questions;
	}

	public string GetUnlocekdKey(int gradeId, int courseId, int lessonId, int groupId){
		string unlockedKey = gradeId.ToString () + "." + courseId.ToString () + "." + lessonId.ToString () + "." + groupId.ToString () + "." + "isUnlocked";
		return unlockedKey;
	}

	public string GetScoreKey(int gradeId, int courseId, int lessonId, int groupId){
		string scoreKey = gradeId.ToString () + "." + courseId.ToString () + "." + lessonId.ToString () + "." + groupId.ToString () + "." + "Score";
		return scoreKey;
	}

	public string GetResultKey(int gradeId, int courseId, int lessonId, int elementNumber){
		string resultKey = gradeId.ToString () + "." + courseId.ToString () + "." + lessonId.ToString () + "." + elementNumber.ToString () + "." + "Result";
		return resultKey;
	}

	public void AddResult(int gradeId, int courseId, int lessonId, int newResult){
		List<int> resultsList = LoadResultsList (gradeId, courseId, lessonId);
		resultsList.RemoveAt (0);
		resultsList.Add (newResult);
		SaveResultsList (gradeId, courseId, lessonId, resultsList);
//		print (GetLessonResult (gradeId, courseId, lessonId));
	}

	public void SaveResultsList(int gradeId, int courseId, int lessonId, List<int> resultsList){
		for (int i = 0; i < resultsList.Count; i++) {
			string resultKey = GetResultKey (gradeId, courseId, lessonId, i);
			PlayerPrefs.SetInt (resultKey, resultsList [i]);
		}
	}

	public List<int> LoadResultsList(int gradeId, int courseId, int lessonId){
		List<int> resultsList = new List<int> ();
		for (int i = 0; i < resultsPerLesson ; i++) {
			string resultKey = GetResultKey (gradeId, courseId, lessonId, i);
			resultsList.Add (PlayerPrefs.GetInt (resultKey, 0));
		}
		return resultsList;
	}

	public float GetLessonResult(int gradeId, int courseId, int lessonId){
		List<int> resultsList = LoadResultsList (gradeId, courseId, lessonId);
		int resultSum = 0;
		foreach (var result in resultsList) {
			resultSum += result;
		}
		float lessonResult = (float)resultSum / (float)resultsPerLesson;
		return lessonResult;
	}
}
