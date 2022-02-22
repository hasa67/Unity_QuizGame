using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

	public Image itemImage;
	public Text itemName;
	public Text itemPrice;
	public GameObject pricePanel;
	public GameObject purchasedPanel;
	public GameObject lowMoneyPanel;
	public GameObject selectedPanel;

	private ShopItem shopItem;

	public void Setup (ShopItem item) {
		itemImage.sprite = item.frontImage;
		itemName.text = item.farsiName;
		itemPrice.text = item.price.ToString ();
		shopItem = item;

		Button button = GetComponent<Button> ();
		int totalCoins = PlayerPrefs.GetInt ("totalCoins", 0);

		pricePanel.SetActive (true);
		purchasedPanel.SetActive (false);
		lowMoneyPanel.SetActive (false);
		selectedPanel.SetActive (false);
		button.interactable = true;

//		if (item.go != null) {
//			item.go.SetActive (false);
//		}

		if (item.isPurchased) {
			if (item.go != null) {
//				if (!ObjectManager.instance.GameObjectExist (item.go)) {
//					Instantiate (item.go);
//				}
				button.GetComponent<Button> ().interactable = false;
//				item.go.SetActive (true);
			}
			if (item.isSelected) {
				selectedPanel.SetActive (true);
			}
			purchasedPanel.SetActive (true);
			pricePanel.SetActive (true);
		} else if (totalCoins < item.price) {
			lowMoneyPanel.SetActive (true);
		}
	}

	public void ClickHadler(){
		ShopUI.instance.PurcahseItem (shopItem);
	}
}
