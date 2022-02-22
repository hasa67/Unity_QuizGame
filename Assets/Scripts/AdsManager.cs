using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour {

	public static AdsManager instance;

//	private string tapsellKey = "kilkhmaqckffopkpfnacjkobgrgnidkphkcbtmbcdhiokqetigljpnnrbfbnpnhmeikjbq";
//	private string BannerZONE_ID = "5a28f47539086d0001670416";
//	private string InterstitialZONE_ID = "5a3f5063fca4f000014b70a8";
//	private string RewardedZONE_ID = "5a5dbd5cc21bf000010d1686";

	private string tapsellKey = "acfgfhiiknggjrbbdkfesnmslncesefatfofnjqabqsroprtsaqmkglelkqbhtassgpsrg";
	private string BannerZONE_ID = "5fa1a92d60e3d00001cc0e30";
	private string InterstitialZONE_ID = "5fa1a97f87df370001c167e7";
	private string RewardedZONE_ID = "5fa1a8f387df370001c167e6";

	public static TapsellAd banAd;
	public static TapsellAd intAd;
	public static TapsellAd rewAd;

	private GameController gameController;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (this);
		}
	}

	void Start () {
		Tapsell.Initialize(tapsellKey);

//		RequestBannerAd ();
//		RequestInterstitial ();
//		RequestRewarded ();

		Tapsell.SetRewardListener (
			(TapsellAdFinishedResult result) => {
				if(result.completed && result.rewarded){
					gameController.DoubleTheCoins ();
					gameController.DelayedNextAction();
				}
				Debug.Log("adId:" + result.adId.ToString() + ", " +
					"zoneId:" + result.zoneId.ToString() + ", " +
					"completed:" + result.completed.ToString() + ", " +
					"rewarded:" + result.rewarded.ToString());
			}
		);
	}

	void Update(){
//		if (SceneManager.GetActiveScene ().buildIndex == 1) {
//			ShowBanner ();
//		} else {
//			HideBanner ();
//		}
	}

	public void RequestBannerAd(){
		Tapsell.RequestBannerAd (BannerZONE_ID, BannerType.BANNER_320x50, Gravity.BOTTOM, Gravity.CENTER,
			(string zoneId) => {
				Debug.Log ("on Ad Available");
			},
			(string zoneId) => {
				Debug.Log ("no Ad Available");
			},
			(TapsellError error) => {
				Debug.Log (error.message);
			},
			(string zoneId) => {
				Debug.Log ("no Network");
			},
			(string zoneId) => {
				Debug.Log ("Hide Banner");
			});
	}

	public void HideBanner () {
//		#if UNITY_EDITOR
//		return;
//		#endif
//
//		Tapsell.HideBannerAd (BannerZONE_ID);
	}

	public void ShowBanner () {
//		#if UNITY_EDITOR
//		return;
//		#endif
//
//		Tapsell.ShowBannerAd (BannerZONE_ID);
	}

	public void RequestInterstitial () {

		Tapsell.RequestAd (InterstitialZONE_ID, true,
			(TapsellAd result) => {
				// onAdAvailable
				Debug.Log ("on Ad Available");
				intAd = result;
			},

			(string zoneId) => {
				// onNoAdAvailable
				Debug.Log ("no Ad Available");
			},

			(TapsellError error) => {
				// onError
				Debug.Log (error.message);
			},

			(string zoneId) => {
				// onNoNetwork
				Debug.Log ("no Network");
			},

			(TapsellAd result) => {
				// onExpiring
				Debug.Log ("expiring");
			},

			(TapsellAd result) => {
				// onOpen
				Debug.Log ("open");
			},

			(TapsellAd result) => {
				// onClose
				Debug.Log ("close");
			}

		);
	}

	public void ShowInterstitial () {
		Tapsell.ShowAd (intAd, new TapsellShowOptions ());
	}

	public void RequestRewarded () {

		Tapsell.RequestAd (RewardedZONE_ID, true,
			(TapsellAd result) => {
				// onAdAvailable
				Debug.Log ("on Ad Available");
				rewAd = result;
			},

			(string zoneId) => {
				// onNoAdAvailable
				Debug.Log ("no Ad Available");
			},

			(TapsellError error) => {
				// onError
				Debug.Log (error.message);
			},

			(string zoneId) => {
				// onNoNetwork
				Debug.Log ("no Network");
			},

			(TapsellAd result) => {
				// onExpiring
				Debug.Log ("expiring");
			},

			(TapsellAd result) => {
				// onOpen
				Debug.Log ("open");
			},

			(TapsellAd result) => {
				// onClose
				Debug.Log ("close");
			}

		);
	}

	public void ShowRewarded () {
		Tapsell.ShowAd (rewAd, new TapsellShowOptions ());
		RequestRewarded ();
	}

	public void SetGameController(GameController gc){
		gameController = gc;
	}
}
