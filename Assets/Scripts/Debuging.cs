using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debuging : MonoBehaviour {

	public Text fpsText;

	float deltaTime;

	void Awake(){
//		DontDestroyOnLoad(this);
	}

	void Update ()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		SetFPS();
	}

	void SetFPS()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fpsText.text = string.Format("FPS: {0:00.} ({1:00.0} ms)", fps, msec);
	}

	public void GetExtraCoin(){
		int currentCoin = PlayerPrefs.GetInt ("totalCoins", 0);
		PlayerPrefs.SetInt ("totalCoins", currentCoin + 200);
	}

	public void ClearAllData(){
		PlayerPrefs.DeleteAll ();
		DeletePurchases (ShopUI.instance.glassesShopList);
		DeletePurchases (ShopUI.instance.hatsShopList);
		DeletePurchases (ShopUI.instance.objectsShopList);
		DeletePurchases (ShopUI.instance.shirtsShopList);
	}

	private void DeletePurchases(List<ShopItem> shopList){
		foreach (var item in shopList) {
			if (item.name != "None") {
				item.isPurchased = false;
			}
			item.isSelected = false;
		}
	}
}
