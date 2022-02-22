using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

	public static ObjectManager instance;
	public List<GameObject> objectsList = new List<GameObject>();

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void AddGameObject(GameObject selectedObject){
		if (!objectsList.Contains (selectedObject)) {
			objectsList.Add (selectedObject);
		}
	}

	public void RemoveGameObject(GameObject selectedObject){
		if (objectsList.Contains (selectedObject)) {
			objectsList.Remove (selectedObject);
		}
	}

	public bool IsGameObjectNew(GameObject selectedObject){
		bool isNewObject = true;
		foreach (var item in objectsList) {
			if (item.name == selectedObject.name) {
				isNewObject = false;
				break;
			}
		}
		return isNewObject;
	}

	public GameObject GetGameObjectByTrigger(string myTriggerName){
		GameObject go = null;
		foreach (var item in objectsList) {
			if (item.GetComponent<ObjectSetup> ().triggerName == myTriggerName) {
				go = item;
				break;
			}
		}
		return go;
	}
}
