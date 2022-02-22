using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSetup : MonoBehaviour {

	public string triggerName;
	public Transform interactionPosition;
	public float minResetTime = 10f;
	public float maxResetTime = 20f;

	void Awake(){
//		gameObject.SetActive (false);
	}

	public void OnEnable(){
		if (triggerName != "") {
			BunnyController.instance.AddItemToActionsList (triggerName);
		}
		ObjectManager.instance.AddGameObject (gameObject);
	}

	public void OnDisable(){
		if (triggerName != "") {
			if (BunnyController.instance != null)
				BunnyController.instance.RemoveItemFromActionsList (triggerName);
		}
		ObjectManager.instance.RemoveGameObject (gameObject);
	}

	void OnMouseDown(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			BunnyController.instance.SetNextAction (triggerName);
			StartCoroutine (Flashing ());
		}
	}

	IEnumerator Flashing(){
		SpriteRenderer sr = transform.Find ("Sprite").GetComponent<SpriteRenderer> ();
		Color32 orgColor = new Color32 (255, 255, 255, 255);
		Color32 dimColor = new Color32 (155, 155, 155, 255);

		int i = 3;
		while (i > 0) {
			sr.color = dimColor;
			yield return new WaitForSeconds (0.1f);
			sr.color = orgColor;
			yield return new WaitForSeconds (0.1f);
			i--;
			yield return null;
		}
	}
}
