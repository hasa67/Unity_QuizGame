using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

	public Text answerText;
	public Image answerImage;

	public AnswerData answerData;
	private GameController gameController;

	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}

	public void Setup(AnswerData data){
		answerData = data;

		if (answerData.answerText.Contains (".png")) {
			answerText.enabled = false;
			answerImage.enabled = true;

			string[] fileName = answerData.answerText.Split('.');
			answerImage.sprite = Resources.Load<Sprite> ("Images/" + fileName[0]);
		} else {
			answerImage.enabled = false;
			answerText.enabled = true;

			answerText.text = answerData.answerText;
		}
	}

	IEnumerator ButtonImage(){
		yield return null;
	}
	
	public void HandleClick(){
		gameController.AnswerButtonClicked (answerData.isCorrect);
	}
}
