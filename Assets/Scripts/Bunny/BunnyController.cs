using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunnyController : MonoBehaviour {
	
	public static BunnyController instance;

	public Text actionsListText;
	public GameObject bodyGameObject;
	public GameObject hatGameObject;
	public GameObject shirtGameObject;
	public GameObject glassesGameObject;

	private ShopItem hatItem;
	private ShopItem shirtItem;
	private ShopItem glassesItem;
	private Animator animator;
	private List<string> actionsList = new List<string> ();
	private float moveSpeed = 2f;
	private float airTime = 32f / 60f;
	private bool isMoving = false;
	private string nextAction = "";
	private bool isFacingFront = true;

	private IEnumerator moveAndDoCo;
	private IEnumerator moveCo;
	private GameObject currentGameObject;


	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		animator = GetComponent<Animator> ();

		actionsList.Add ("IsMoving");
		actionsList.Add ("SaysHi");
		actionsList.Add ("ScratchEar");
	}

	public List<string> GetActionsList(){
		return actionsList;
	}

	public void RemoveItemFromActionsList(string actionName){
		actionsList.Remove (actionName);
	}

	public void AddItemToActionsList(string actionName){
		if (!actionsList.Contains (actionName)) {
			actionsList.Add (actionName);
		}
	}

	public void PerformAction(string action){
		if (action == "IsMoving") {
			MoveToRandomPosition ();
		} else if (action == "SaysHi" || action == "ScratchEar") {
			Invoke (action, 0f);
		} else {
			moveAndDoCo = MoveAndDoSomethingCo (action);
			StartCoroutine (moveAndDoCo);
		}
	}

	public void SaysHi(){
		StartCoroutine (DoSomethingCo ("SaysHi", 20f, 50f));
	}

	public void ScratchEar(){
		StartCoroutine (DoSomethingCo ("ScratchEar", 30f, 60f));
	}

	IEnumerator MoveAndDoSomethingCo(string triggerName){
		currentGameObject = ObjectManager.instance.GetGameObjectByTrigger (triggerName);
		ObjectSetup objSetup = currentGameObject.GetComponent<ObjectSetup> ();
		Vector3 newPosition = objSetup.interactionPosition.position;
		MoveToNewPosition (newPosition);
		while (isMoving) {
			yield return null;
		}
		//		nextAction = "";
		actionsList.Remove (triggerName);
		animator.SetTrigger (triggerName);

		yield return new WaitForSeconds (Random.Range (objSetup.minResetTime, objSetup.maxResetTime));
		AddItemToActionsList (triggerName);
	}

	IEnumerator DoSomethingCo(string triggerName, float addTimerMin, float addTimerMax){
		animator.SetTrigger (triggerName);
		actionsList.Remove (triggerName);

		yield return new WaitForSeconds (Random.Range (addTimerMin, addTimerMin));
		AddItemToActionsList (triggerName);
	}

	public void SetNextAction(string playerRequest){
		if (isMoving == true && playerRequest != "") {
			StopCoroutine (moveCo);
			StopCoroutine (moveAndDoCo);
			isMoving = false;
			animator.SetBool ("IsMoving", isMoving);
		}
		nextAction = playerRequest;
	}

	public string GetNextAction(){
		return nextAction;
	}

	public void MoveToRandomPosition(){
		moveCo = MoveToPositionCo (RandomTargetLocation ());
		StartCoroutine (moveCo);
	}

	public void MoveToNewPosition(Vector3 newPosition){
		moveCo = MoveToPositionCo (newPosition);
		StartCoroutine (moveCo);
	}
		
	IEnumerator MoveToPositionCo(Vector3 targetLocation){
		isMoving = true;

		float distance = Vector3.Distance (this.transform.position, targetLocation);
		float newSpeed = CalculateNewSpeed (distance);

		animator.SetBool ("IsMoving", isMoving);
		while (isMoving) {
			distance = Vector3.Distance (this.transform.position, targetLocation);
			if (distance < float.Epsilon) {
				isMoving = false;
				yield return null;
			}

			float bodyPositionY = bodyGameObject.transform.localPosition.y;
			if (distance > float.Epsilon && bodyPositionY > 0f) {
				this.transform.position = Vector3.MoveTowards (this.transform.position, targetLocation, newSpeed * Time.deltaTime);
			}

			yield return null;
		}

		animator.SetBool ("IsMoving", isMoving);
	}

	private Vector3 RandomTargetLocation(){
		float targetLocationX = Random.Range (-1.3f, 0.5f);
		float targetLocationY = Random.Range (-3.0f, 0.5f);
		Vector3 targetLocation = new Vector3 (targetLocationX, targetLocationY, 0);
		return targetLocation;
	}

	private float CalculateNewSpeed(float distance){
		float eachJumpTravel = moveSpeed * airTime;
		float numberOfJumps = distance / eachJumpTravel;
		float roundedNumberOfJumps = Mathf.Ceil (numberOfJumps);
		float newSpeed = distance / roundedNumberOfJumps / airTime;
		return newSpeed;
	}

	public void WhiteboardAniamtion(){
		GameObject.Find ("Whiteboard").GetComponent<Whiteboard> ().DrawOnBoard ();
	}

	public void TurnOnTV(){
		GameObject.Find ("TV").GetComponent<TV> ().TurnOn ();
	}

	public void TurnOffTV(){
		GameObject.Find ("TV").GetComponent<TV> ().TurnOff ();
	}

	public void DisplayActionsList(){
		string actionsListLog = "";
		foreach (var item in actionsList) {
			actionsListLog += item + "\n";
		}
		actionsListLog += "---------" + "\n";
		actionsListLog += nextAction;
		actionsListText.text = actionsListLog;
	}

	void Update(){
		DisplayActionsList ();
	}

	public void SetItems(ShopItem newItem){
		if (newItem.category == "Hats") {
			hatItem = newItem;
		} else if (newItem.category == "Shirts") {
			shirtItem = newItem;
		} else if (newItem.category == "Glasses") {
			glassesItem = newItem;
		}

		UpdateLook ();
	}

	private void UpdateLook(){
		UpdateItemsLook (hatGameObject, hatItem);
		UpdateItemsLook (shirtGameObject, shirtItem);
		UpdateItemsLook (glassesGameObject, glassesItem);
	}

	private void UpdateItemsLook(GameObject gameObject, ShopItem item){
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();
		if (item != null && item.name != "None") {
			if (isFacingFront) {
				sr.sprite = item.frontImage;
			} else {
				if (item.backImage != null) {
					sr.sprite = item.backImage;
				} else {
					sr.sprite = item.frontImage;
				}
			}
		} else {
			sr.sprite = null;
		}
	}

	public void TurnAround(){
		if (isFacingFront) {
			isFacingFront = false;
		} else {
			isFacingFront = true;
		}

		UpdateLook ();
	}
}
