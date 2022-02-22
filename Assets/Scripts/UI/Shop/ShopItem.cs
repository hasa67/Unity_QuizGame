using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ShopItem")]
public class ShopItem:ScriptableObject{
	public string farsiName;
	public string category;
	public int price;
	public GameObject go;
	public Sprite frontImage;
	public Sprite backImage;
	public bool isPurchased = false;
	public bool isSelected = false;
}
