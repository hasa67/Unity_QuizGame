using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotation : MonoBehaviour {

	private float rotationSpeed = 2f;
	private float zRotation;
	private float rotationTime;

	void Start () {
		StartCoroutine (RandomRotation ());
	}

	IEnumerator RandomRotation(){
		zRotation = Random.Range (0, 360);
		transform.rotation = Quaternion.Euler (0, 0, zRotation);
		yield return new WaitForSeconds (Random.Range (5f, 10f));

		while (true) {
			float direction = Mathf.Pow (-1, Random.Range (1, 3));
			rotationTime = Random.Range (10f, 20f);

			while (rotationTime > 0) {
				zRotation += rotationSpeed * direction * Time.deltaTime;
				transform.rotation = Quaternion.Euler (0, 0, zRotation);
				rotationTime -= Time.deltaTime;
				yield return null;
			}

			yield return new WaitForSeconds (Random.Range (5f, 10f));
		}
	}
}
