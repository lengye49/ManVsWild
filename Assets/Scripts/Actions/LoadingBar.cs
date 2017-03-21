using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	private float value;
	private Slider loadingBar;
	private Text loadingText;
	private string loadingTxt;
	public Image fillImage;
	// Use this for initialization
	void Start () {
		loadingBar = this.gameObject.GetComponent<Slider> ();
		loadingTxt = "Loading";
		loadingText = this.gameObject.GetComponentInChildren<Text> ();
		CallInLoadingBar ();
	}

	public void CallInLoadingBar(){
		value = 0;
		this.gameObject.SetActive (true);
		this.gameObject.transform.localPosition = Vector3.zero;
		StartLoading ();
	}

	void StartLoading(){
		value = value + 0.005f;
		if ((int)(value * 10) % 4 == 0) {
			loadingTxt = "Loading";
		}else if((int)(value * 10) % 4 == 1){
			loadingTxt = "Loading.";
		}else if((int)(value * 10) % 4 == 2){
			loadingTxt = "Loading..";
		}else{
			loadingTxt = "Loading...";
		}
		loadingBar.value = value;
		loadingText.text = loadingTxt;
		fillImage.color = new Color ((255f * (1 - value)), 255, 0, 255);
		if (value < 1)
			StartCoroutine (LoadingInProgress ());
		else {
			this.gameObject.SetActive (false);
		}

	}
	IEnumerator LoadingInProgress(){
		float f = Algorithms.GetIndexByRange (2, 5) / 100;
		yield return new WaitForSeconds (f);
		StartLoading ();
	}

}
