using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour {

	public Sprite[] drawingSprites;

	private List<Sprite> drawingList = new List<Sprite> ();
	private SpriteRenderer spriteRenderer;
	private Sprite oldDrawing;
	private Animator animator;

	void Start () {
		spriteRenderer = transform.Find ("Drawing").GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = null;
		animator = GetComponent<Animator> ();
		foreach (var drawing in drawingSprites) {
			drawingList.Add (drawing);
		}
	}

	public void DrawOnBoard(){
		animator.SetTrigger ("Draw");
	}

	public void GetRandomSprite(){
		int index = Random.Range (0, drawingList.Count);
		Sprite newDrawing = drawingList [index];
		spriteRenderer.sprite = newDrawing;
		drawingList.RemoveAt (index);
		if (oldDrawing != null) {
			drawingList.Add (oldDrawing);
		}
		oldDrawing = newDrawing;
	}
}
