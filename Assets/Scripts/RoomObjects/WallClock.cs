using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClock : MonoBehaviour {

	private Transform hourHandle;
	private Transform minuteHandle;
	private Transform secondHandle;

	void Start () {
		hourHandle = transform.Find ("Hour");
		minuteHandle = transform.Find ("Minute");
		secondHandle = transform.Find ("Second");

		StartCoroutine (HandlsCo ());
	}

	IEnumerator HandlsCo(){
		while (true) {
			ClockHandlesSetup ();
			yield return new WaitForSeconds (1f);
		}
	}

	private void ClockHandlesSetup(){
		float currentMinute = System.DateTime.Now.Minute;
		float currentHour = System.DateTime.Now.Hour + currentMinute / 60f;
		if (currentHour > 12f)
			currentHour -= 12f;
		float currentSecond = System.DateTime.Now.Second;

		RotateGameobject (hourHandle, currentHour, 12f);
		RotateGameobject (minuteHandle, currentMinute, 60f);
		RotateGameobject (secondHandle, currentSecond, 60f);
	}

	private void RotateGameobject(Transform tr, float currentValue, float maxValue){
		float currentDegree = ConvertToDegrees (currentValue, maxValue);
		Vector3 newRotation = new Vector3 (0, 180f, currentDegree);

		tr.localRotation = Quaternion.Euler (newRotation);
	}
	
	private float ConvertToDegrees(float currentValue, float maxValue){
		float currentDegree = currentValue * 360f / maxValue;
		return currentDegree;
	}
}
