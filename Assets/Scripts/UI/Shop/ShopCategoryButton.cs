using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryButton : MonoBehaviour {
	public string Category;

	public void ClickHadler(){
		AudioManager.instance.PlayNextButtonSound ();
		ShopUI.instance.SwitchCurrentShopList (Category);
	}
}
