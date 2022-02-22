using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData {
	
	public string questionText;
	public string questionImage;
	public string questionSound;
	public AnswerData[] answers;
	public int gradeIndex;
	public int courseIndex;
	public int lessonIndex;
}
