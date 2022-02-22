using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {

	public static ShopUI instance { get; private set; }

	public List<ShopItem> objectsShopList = new List<ShopItem> ();
	public List<ShopItem> hatsShopList = new List<ShopItem> ();
	public List<ShopItem> shirtsShopList = new List<ShopItem> ();
	public List<ShopItem> glassesShopList = new List<ShopItem> ();

	public GameObject shopButtonPrefab;
	private List<GameObject> shopButtonsList = new List<GameObject> ();
	public Transform shopContentTransform;
	public Text TotalCoinsText;

	private BunnyController bc;
	private int totalCoins;
	private string currentShopList;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {

		objectsShopList.Sort (SortByPrice);
		hatsShopList.Sort (SortByPrice);
		shirtsShopList.Sort (SortByPrice);
		glassesShopList.Sort (SortByPrice);

		currentShopList = "Objects";
		bc = GameObject.FindWithTag ("Player").GetComponent<BunnyController> ();
		totalCoins = PlayerPrefs.GetInt ("totalCoins", 0);

		LoadStat ();
		UpdateShop ();
	}

	private int SortByPrice(ShopItem item1, ShopItem item2){
		return item1.price.CompareTo (item2.price);
	}

	public void PurcahseItem(ShopItem item){
		List<ShopItem> shopList = new List<ShopItem> ();
		shopList = SelectCurrentShopList (currentShopList);

		if (!item.isPurchased) {
			if (shopList.Contains (item) && totalCoins >= item.price) {
				shopList [shopList.IndexOf (item)].isPurchased = true;
				if (item.go == null) {
					DeselectAllItemsInList ();
					shopList [shopList.IndexOf (item)].isSelected = true;
					bc.SetItems (item);
				}
				totalCoins -= item.price;
				AudioManager.instance.PlayePurchaseSound (1);
			} else {
				AudioManager.instance.PlayePurchaseSound (0);
			}
		} else {
			AudioManager.instance.PlayNextButtonSound ();
			DeselectAllItemsInList ();
			shopList [shopList.IndexOf (item)].isSelected = true;
			bc.SetItems (item);
		}

		UpdateShop ();
	}

	public void UpdateShop(){
		PlayerPrefs.SetInt ("totalCoins", totalCoins);
		RemoveButtons ();
		AddButtons ();
		TotalCoinsText.text = totalCoins.ToString ();
		SaveStats ();
		LoadStat ();
	}

	private void DeselectAllItemsInList(){
		List<ShopItem> shopList = new List<ShopItem> ();
		shopList = SelectCurrentShopList (currentShopList);
		foreach (var item in shopList) {
			item.isSelected = false;
		}
	}

	private void RemoveButtons(){
		foreach (var item in shopButtonsList) {
			Destroy (item);
		}
		shopButtonsList.Clear ();
	}

	private void AddButtons(){
		List<ShopItem> shopList = new List<ShopItem> ();
		shopList = SelectCurrentShopList (currentShopList);

		foreach (var item in shopList) {
			GameObject newButton = Instantiate(shopButtonPrefab, shopContentTransform);
			newButton.GetComponent<ShopButton> ().Setup (item);
			shopButtonsList.Add (newButton);
		}
	}

	private void SaveStats(){
		SaveList (objectsShopList);
		SaveList (hatsShopList);
		SaveList (shirtsShopList);
		SaveList (glassesShopList);
	}

	private void SaveList(List<ShopItem> shopList){
		if (shopList.Count > 0) {
			foreach (var item in shopList) {
				int purchased = 0;
				if (item.isPurchased)
					purchased = 1;
				PlayerPrefs.SetInt (item.category + "_" + item.name + "_" + "Purchased", purchased);

				int selected = 0;
				if (item.isSelected)
					selected = 1;
				PlayerPrefs.SetInt (item.category + "_" + item.name + "_" + "Selected", selected);
			}
		}
	}

	public void LoadStat(){
		LoadList (objectsShopList);
		LoadList (hatsShopList);
		LoadList (shirtsShopList);
		LoadList (glassesShopList);
	}

	private void LoadList(List<ShopItem> shopList){
		if (shopList.Count > 0) {
			foreach (var item in shopList) {
				int purchased = PlayerPrefs.GetInt (item.category + "_" + item.name + "_" + "Purchased", 0);
				if (purchased == 1)
					item.isPurchased = true;
				if (item.go != null && item.isPurchased) {
					if (ObjectManager.instance.IsGameObjectNew (item.go)) {
						GameObject newObject = Instantiate (item.go);
						newObject.name = item.name;
					}
				}

				int selected = PlayerPrefs.GetInt (item.category + "_" + item.name + "_" + "Selected", 0);
				if (selected == 1) {
					item.isSelected = true;
					bc.SetItems (item);
				} else {
					item.isSelected = false;
				}
			}
		}
	}


	public void SwitchCurrentShopList(string newShopList){
		currentShopList = newShopList;
		UpdateShop ();
	}

	private List<ShopItem> SelectCurrentShopList(string currentShopList){
		List<ShopItem> shopList = new List<ShopItem> ();

		switch (currentShopList) {
		case "Objects":
			shopList = objectsShopList;
			break;
		case "Hats":
			shopList = hatsShopList;
			break;
		case "Shirts":
			shopList = shirtsShopList;
			break;
		case "Glasses":
			shopList = glassesShopList;
			break;
		default:
			Debug.Log ("Check currentShopList: " + currentShopList);
			break;
		}

		return shopList;
	}
}
