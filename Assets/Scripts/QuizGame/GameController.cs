using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

	public Text questionDisplayText;
	public Image questionDisplayImage;
	public Text questionDisplayTextPlus;
	public Image questionDisplayImagePlus;
	public Text courseDisplayText;
	public Slider timerSlider;
	public Transform answerButtonParent;

	public GameObject[] answerButtonsGroup;

	public GameObject roundStartPanel;
	public GameObject roundStartBackground;
	public GameObject roundOvePanel;
	public GameObject roundOverBackground;
	public GameObject progressPanel;
	public GameObject adsPanel;
	public GameObject adsButtons;
	public Text adsText;
	public Text[] coinTexts;
	public Button RetryButton;
	public Button HomeButton;
	public Sprite soundPlayingIcon;
	public StarsPanel starsPanel;

	private QuestionData[] lessonQuestions;
	private List<QuestionData> questionPool = new List<QuestionData> ();
	private List<int> lessonIndexList = new List<int> ();

	private bool isQuestionActive;
	private float timeRemaining;
	private int playerScore;

	private float questionWait = 0.5f;

	private int gradeIndex;
	private int courseIndex;
	private int lessonIndex;
	private int groupIndex;
	private int groupCount;
	private int groupSize;
	private string gradeName;
	private string courseName;
	private string lessonTitle;

	private float timeLimitPerQuestion = 10f;
	private int questionsPerRound = 10;
	private int questionPerLesson = 1;
	private int questionHasAudio = 0; // 1:true

	private int questionCounter;
	private int rewardCoins = 0;

	private float unlockedRate = 0.6f; // = 3 stars
	private int nextAction = 0; // 1:rety 2:mainMenu 3:nextLevel


	void Start () {
		int[] indexes = DataManager.instance.GetSelectedIndexes ();
		gradeIndex = indexes [0];
		courseIndex = indexes [1];
		lessonIndex = indexes [2];
		groupIndex = indexes [3];
		groupCount = DataManager.instance.GetGroupCount ();
		lessonTitle = DataManager.instance.GetLessonTitle ();

		if (groupIndex <= 0) {
			lessonQuestions = DataManager.instance.GetQuestions (gradeIndex, courseIndex, lessonIndex);
			if (lessonQuestions.Length < questionsPerRound) {
				questionsPerRound = lessonQuestions.Length;
			}
			MyFunctions.ShuffleQuestionsArray (lessonQuestions);
			for (int i = 0; i < questionsPerRound; i++) {
				questionPool.Add (lessonQuestions [i]);
				lessonIndexList.Add (lessonIndex);
			}
		} else {
			if (groupIndex < groupCount) {
				groupSize = DataManager.instance.GetGroupSize ();
			} else {
				int lessonCount = DataManager.instance.GetLessonCount (gradeIndex, courseIndex);
				if (groupIndex == groupCount) {
					groupSize = lessonCount - groupSize * (groupIndex - 1);
				} else {
					groupSize = lessonCount;
				}
			}

			for (int i = lessonIndex; i > lessonIndex - groupSize; i--) {
				lessonQuestions = DataManager.instance.GetQuestions (gradeIndex, courseIndex, i);
				MyFunctions.ShuffleQuestionsArray (lessonQuestions);
				for (int j = 0; j < questionPerLesson; j++) {
					questionPool.Add (lessonQuestions [j]);
					lessonIndexList.Add (i);
				}
			}
		}
		MyFunctions.ShuffleQuestionsList2 (questionPool, lessonIndexList);
		questionsPerRound = questionPool.Count;

		playerScore = 0;
		questionCounter = 0;

		progressPanel.GetComponent<ProgressPanel>().Setup(questionsPerRound);

		roundStartPanel.SetActive (true);
		roundStartBackground.SetActive (true);
		roundOvePanel.SetActive (false);
		roundOverBackground.SetActive (false);

		ShowQuestions ();
	}

	private void ShowQuestions(){
		isQuestionActive = true;
		timeRemaining = timeLimitPerQuestion;
		UpdateTimeRemaining ();
		RemoveAnswerButtons ();
		QuestionData questionData = questionPool [questionCounter];
		AnswerData[] answersArray = questionData.answers;
		MyFunctions.ShuffleAnswers (answersArray);
		courseDisplayText.text = lessonTitle;

		string questionText = "";
		Sprite questionImage = null;
		AudioClip questionSound = null;

		questionHasAudio = 0;
		if (questionData.questionText != "")
			questionText = questionData.questionText;
		if (questionData.questionImage != "")
			questionImage = Resources.Load<Sprite> ("Images/" + questionData.questionImage);
		if (questionData.questionSound != "") {
			questionSound = Resources.Load<AudioClip> ("Sounds/" + questionData.questionSound);
			questionHasAudio = 1;
		}

		if (questionSound != null) {
			QuestionTypeDisplay (2);
			AudioManager.instance.PlayeQuestionSound (questionSound);
			if (questionImage != null) {
				questionDisplayImage.sprite = questionImage;
			} else {
				questionDisplayImage.sprite = soundPlayingIcon;
			}
		} else if (questionImage != null) {
			if (questionText != "") {
				QuestionTypeDisplay (3);
				questionDisplayTextPlus.text = questionText;
				questionDisplayImagePlus.sprite = questionImage;
			} else {
				QuestionTypeDisplay (2);
				questionDisplayImage.sprite = questionImage;
			}
		} else if (questionText != "") {
			QuestionTypeDisplay (1);
			questionDisplayText.text = questionText;
		} else {
			Debug.Log ("question is empty!");
		}

		for (int i = 0; i < answersArray.Length; i++) {
			answerButtonsGroup [i].SetActive (true);
			answerButtonsGroup [i].GetComponent<AnswerButton> ().Setup (answersArray [i]);
		}
	}

	private void QuestionTypeDisplay(int questionType){
		questionDisplayText.gameObject.SetActive(false);
		questionDisplayImage.gameObject.SetActive(false);
		questionDisplayTextPlus.gameObject.SetActive (false);
		questionDisplayImagePlus.gameObject.SetActive(false);

		switch (questionType) {
		case 1:   // text question
			questionDisplayText.gameObject.SetActive(true);
			break;
		case 2:   // sound or sound+image or image question
			questionDisplayImage.gameObject.SetActive(true);
			break;
		case 3:   // text+image question
			questionDisplayTextPlus.gameObject.SetActive (true);
			questionDisplayImagePlus.gameObject.SetActive(true);
			break;
		default:
			Debug.Log ("question type error!");
			break;
		}
	}

	public void AnswerButtonClicked(bool isCorrect){
		AudioManager.instance.StopQuestionSound ();
		if (isCorrect) {
			playerScore ++;
		}

		isQuestionActive = false;
		AudioManager.instance.PlayAnswerButtonSound (isCorrect);
		StartCoroutine (ButtonsColorCo ());
	}

	IEnumerator ButtonsColorCo(){
		AnswerButtonsOff ();

		if (timeRemaining > 0) {
			GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
			selectedButton.GetComponent<Animator> ().SetTrigger (Animator.StringToHash ("isSelected"));
			Image selectedButtonImage = selectedButton.GetComponent<Image> ();
			Color oldColor = selectedButtonImage.color;
			bool isCorrect = selectedButton.GetComponent<AnswerButton> ().answerData.isCorrect;
			progressPanel.GetComponent<ProgressPanel> ().ProgressAnimation (isCorrect, questionCounter);

			if (isCorrect) {
				selectedButtonImage.color = Color.green;
				yield return new WaitForSeconds (questionWait);
				selectedButtonImage.color = oldColor;

				DataManager.instance.AddResult (gradeIndex, courseIndex, lessonIndexList [questionCounter], 1);
			} else {
				selectedButtonImage.color = Color.red;
				foreach (var button in answerButtonsGroup) {
					if (button.GetComponent<AnswerButton> ().answerData.isCorrect) {
						Image buttonImage = button.GetComponent<Image> ();
						buttonImage.color = Color.green;
						yield return new WaitForSeconds (questionWait);
						buttonImage.color = oldColor;
						selectedButtonImage.color = oldColor;
					}
				}

				DataManager.instance.AddResult (gradeIndex, courseIndex, lessonIndexList [questionCounter], 0);
			}
		} else {
			progressPanel.GetComponent<ProgressPanel> ().ProgressAnimation(false, questionCounter);
			AudioManager.instance.PlayAnswerButtonSound (false);

			foreach (var button in answerButtonsGroup) {
				if (button.GetComponent<AnswerButton> ().answerData.isCorrect) {
					Image buttonImage = button.GetComponent<Image> ();
					Color oldColor = buttonImage.color;
					buttonImage.color = Color.green;
					yield return new WaitForSeconds (questionWait);
					buttonImage.color = oldColor;
				}
			}

			DataManager.instance.AddResult (gradeIndex, courseIndex, lessonIndexList [questionCounter], 0);
		}

		AnswerButtonsOn ();

		if (questionCounter < questionsPerRound - 1) {
			questionCounter++;
			ShowQuestions ();
		} else {
			EndRound ();
		}
	}

	void Update(){
		if (isQuestionActive) {
			timeRemaining -= Time.deltaTime;
			UpdateTimeRemaining ();
			if (timeRemaining <= 0) {
				isQuestionActive = false;
				StartCoroutine (ButtonsColorCo ());
			}
		}
	}

	private void AnswerButtonsOn(){
		foreach (var answerButton in answerButtonsGroup) {
			answerButton.GetComponent<Button> ().enabled = true;
		}
	}

	private void AnswerButtonsOff(){
		foreach (var answerButton in answerButtonsGroup) {
			answerButton.GetComponent<Button> ().enabled = false;
		}
	}

	private void RemoveAnswerButtons(){
		foreach (var answerButton in answerButtonsGroup) {
			answerButton.SetActive (false);
		}
	}

	public void EndRound(){
		AnswerButtonsOff ();
		StartCoroutine (ShowRoundOverPanel ());
	}

	IEnumerator ShowRoundOverPanel(){
		yield return new WaitForSeconds (0.5f);
		TransitionManager.instance.FadeIn ();
		float transitionLength = TransitionManager.instance.GetTransitionTime ();
		yield return new WaitForSeconds (transitionLength);

		roundStartPanel.SetActive (false);
		roundStartBackground.SetActive (false);
		roundOvePanel.SetActive (true);
		roundOverBackground.SetActive (true);
		adsPanel.SetActive (false);
		RetryButton.interactable = false;
		HomeButton.interactable = false;

		// checks if lock should be shown
		bool isLockHidden = false;
		int nextLevelUnlocekd = 0;

		if (groupIndex == 0) {
			string unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex + 1, 0);
			nextLevelUnlocekd = PlayerPrefs.GetInt (unlockedKey, 0);
		} else if (groupIndex < 0) {
			string unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex, -groupIndex);
			nextLevelUnlocekd = PlayerPrefs.GetInt (unlockedKey, 0);
		} else if (groupIndex < groupCount) {
			string unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex + 1, 0);
			nextLevelUnlocekd = PlayerPrefs.GetInt (unlockedKey, 0);
		} else if (groupIndex == groupCount) {
			string unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex, groupIndex + 1);
			nextLevelUnlocekd = PlayerPrefs.GetInt (unlockedKey, 0);
		} else {
			nextLevelUnlocekd = 1;
		}

		if (nextLevelUnlocekd == 1) {
			isLockHidden = true;
			starsPanel.HideLock (isLockHidden);
		}


		AudioManager.instance.ChangeBackgroundMusic (0);
		TransitionManager.instance.FadeOut ();

		foreach (var item in coinTexts) {
			item.text = rewardCoins.ToString () + " +";
		}
		for (int i = 0; i < playerScore; i++) {
			yield return new WaitForSeconds (0.3f);
			rewardCoins++;
			UpdateCoinsText (rewardCoins);
		}

		int oldTotalCoins = PlayerPrefs.GetInt ("totalCoins", 0);
		int newTotalCoins = oldTotalCoins + rewardCoins;
		PlayerPrefs.SetInt ("totalCoins", newTotalCoins);

		yield return new WaitForSeconds (0.5f);
		float normalizedScore = (float)playerScore / (float)questionsPerRound;
		if (normalizedScore >= unlockedRate && !isLockHidden) {
			string unlockedKey = "";
			if (groupIndex == 0) {
				unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex + 1, 0);
			} else if (groupIndex < 0) {
				unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex, -groupIndex);
			} else if (groupIndex < groupCount) {
				unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex + 1, 0);
			} else if (groupIndex == groupCount) {
				unlockedKey = DataManager.instance.GetUnlocekdKey (gradeIndex, courseIndex, lessonIndex, groupIndex + 1);
			}
			PlayerPrefs.SetInt (unlockedKey, 1);
		}
		SavePlayerScore (normalizedScore);
		starsPanel.FillStars (normalizedScore, unlockedRate);
		while (starsPanel.CoroutineRunning ()) {
			yield return null;
		}

		yield return new WaitForSeconds (0.5f);
		RetryButton.interactable = true;
		HomeButton.interactable = true;
	}


	private void UpdateCoinsText(int newValue){
		foreach (var item in coinTexts) {
			if (item.enabled == true) {
				Animator animator = item.GetComponent<Animator> ();
				animator.SetTrigger (Animator.StringToHash ("IsPlus"));
				item.text = newValue.ToString () + " +";
			}
		}
	}

	public void ShowAdsPanel(){
		if (rewardCoins > 0) {
			AdsManager.instance.RequestRewarded ();
			adsPanel.SetActive (true);
		} else {
			PerformNextAction ();
		}
	}

	public void SetNextActionToRetry(){
		AudioManager.instance.PlayBackButtonSound ();
		nextAction = 1;
	}

	public void SetNextActionToMainMenu(){
		AudioManager.instance.PlayNextButtonSound ();
		nextAction = 2;
	}

	public void RetryLesson(){
		AudioManager.instance.ChangeBackgroundMusic (1);
		TransitionManager.instance.ChangeScene ("GameScene");
	}
		
	public void ReturnToMenu(){
		AudioManager.instance.ChangeBackgroundMusic(2);
		TransitionManager.instance.ChangeScene ("BunnyRoomScene");
	}

	public void ShowVideoAd(){
		AudioManager.instance.PlayNextButtonSound ();
		AdsManager.instance.SetGameController (this);
		AdsManager.instance.ShowRewarded ();
	}

	public void DelayedNextAction(){
		adsButtons.SetActive (false);
		adsText.text = "الان دو برابر سکه گرفتی :)";
		StartCoroutine (DelayedNextActionCo ());
	}

	IEnumerator DelayedNextActionCo(){
		yield return new WaitForSeconds (1f);
		UpdateCoinsText (rewardCoins);
		yield return new WaitForSeconds (1f);
		PerformNextAction ();
	}

	public void PerformNextAction(){
		AudioManager.instance.PlayBackButtonSound ();

		switch (nextAction) {
		case 1:
			RetryLesson ();
			break;
		case 2:
			ReturnToMenu ();
			break;
		default:
			break;
		}
	}

	public void DoubleTheCoins(){
		rewardCoins *= 2;
	}

	private void UpdateTimeRemaining(){
		timerSlider.value = timeRemaining / timeLimitPerQuestion;
	}

	public void ReplayQuestioSound(){
		if (questionHasAudio == 1)
			AudioManager.instance.ReplayQuestionSound ();
	}

	private void SavePlayerScore(float normalizedScore){
		// score between 0 and 1;
		string scoreKey = DataManager.instance.GetScoreKey(gradeIndex, courseIndex, lessonIndex, groupIndex);
		float oldScore = PlayerPrefs.GetFloat (scoreKey, 0f);
		float newScore = Mathf.Floor( normalizedScore*10)/10;
		if (newScore > oldScore) {
			PlayerPrefs.SetFloat (scoreKey, newScore);
		}
	}

}
