using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public GameObject coursesPanel;
	public GameObject lessonsPanel;
	public GameObject resultsPanel;
	public GameObject shopUI;
	public GameObject gradePanelPrefab;
	public Transform gradesScrollViewTransform;
	public GameObject mainMenuPanel;

	private List<GameObject> gradePanelsList = new List<GameObject>();
	private bool showResults = false;
		
	void Start(){
		HideAllMenuPanels ();

		int[] indexes = DataManager.instance.GetSelectedIndexes ();

		if (indexes[0] != -1) {
			lessonsPanel.SetActive (true);
			lessonsPanel.GetComponent<LessonPanel> ().Setup (indexes [0], indexes [1]);
		}
		DataManager.instance.ResetGradeIndex ();
	}

	private void HideAllMenuPanels(){
		shopUI.SetActive (false);
		coursesPanel.SetActive (false);
		lessonsPanel.SetActive (false);
		resultsPanel.SetActive (false);
	}

	public void ShowShopUI(){
		CloseMainMenu ();
		AudioManager.instance.PlayNextButtonSound ();
		shopUI.SetActive (true);
	}

	public void ShowCoursesPanel(){
		CloseMainMenu ();
		AudioManager.instance.PlayNextButtonSound ();
		HideAllMenuPanels ();
		coursesPanel.SetActive (true);
		HideAllGradePanels ();
		int gradeCount = DataManager.instance.GetGradeCount ();
		for (int i = 0; i < gradeCount; i++) {
			GameObject newGradePanel = Instantiate(gradePanelPrefab, gradesScrollViewTransform);
			newGradePanel.GetComponent<GradePanel> ().Setup (i);
			gradePanelsList.Add (newGradePanel);
		}
	}

	private void HideAllGradePanels(){
		foreach (var panel in gradePanelsList) {
			Destroy (panel);
		}
		gradePanelsList.Clear ();
	}

	public void ShowLessonsPanel(int gradeId, int courseId){
		HideAllMenuPanels ();
		lessonsPanel.SetActive (true);
		lessonsPanel.GetComponent<LessonPanel> ().Setup (gradeId, courseId);
	}

	public bool IsShowingResults(){
		return showResults;
	}

	public void ShowingResults(){
		showResults = true;
		ShowCoursesPanel ();
	}

	public void ShowResultsPanel(int gradeId, int courseId){
		HideAllMenuPanels ();
		resultsPanel.SetActive (true);
		resultsPanel.GetComponent<ResultPanel> ().Setup (gradeId, courseId);
	}

	public void MainMenuBackButton(){
		if (lessonsPanel.activeSelf == true || resultsPanel.activeSelf == true) {
			HideAllMenuPanels ();
			ShowCoursesPanel ();
		} else if (coursesPanel.activeSelf == true || shopUI.activeSelf == true) {
			showResults = false;
			HideAllMenuPanels ();
		} else {
			Debug.Log ("main menu back error");
		}

		AudioManager.instance.PlayBackButtonSound ();
	}

	public void OpenCloseMainMenu(){
		Animator animator = mainMenuPanel.GetComponent<Animator> ();
		bool isOpen = animator.GetBool ("IsOpen");
		animator.SetBool ("IsOpen", !isOpen);

		if (!isOpen) {
			AudioManager.instance.PlayNextButtonSound ();
		} else {
			AudioManager.instance.PlayBackButtonSound ();
		}
	}

	public void CloseMainMenu(){
		Animator animator = mainMenuPanel.GetComponent<Animator> ();
		animator.SetBool ("IsOpen", false);
	}

	public void ExitGame(){
		AudioManager.instance.PlayBackButtonSound ();
		StartCoroutine (ExitAnimationCo ());
	}

	IEnumerator ExitAnimationCo(){
		TransitionManager.instance.FadeIn ();
		yield return new WaitForSeconds (TransitionManager.instance.GetTransitionTime());
		AudioManager.instance.StopMusic ();
//		AdsManager.instance.ShowInterstitial ();
//		yield return new WaitForSeconds (1f);
		Application.Quit ();
	}
}
