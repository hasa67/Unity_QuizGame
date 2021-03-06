using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoveScript : MonoBehaviour {

	// Background scroll speed can be set in Inspector with slider
	[Range(1f, 20f)]
	private float scrollSpeed = 1f;

	// Scroll offset value to smoothly repeat backgrounds movement
	public float scrollOffset;

	// Start position of background movement
	Vector2 startPos;

	// Backgrounds new position
	float newPos;

	// Use this for initialization
	void Start () {
		// Getting backgrounds start position
		startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		// Calculating new backgrounds position repeating it depending on scrollOffset
		newPos = Mathf.Repeat (Time.time * - scrollSpeed, scrollOffset);

		// Setting new position
		transform.localPosition = startPos + Vector2.right * newPos;
	}
}
