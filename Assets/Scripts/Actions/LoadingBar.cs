using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	private float value;
	private Slider loadingBar;
	private Text loadingText;
	private string loadingTxt;
	public Image fillImage;
	private int totalTime;
	// Use this for initialization
	void Start () {
		loadingBar = this.gameObject.GetComponent<Slider> ();
		loadingTxt = "Loading";
		loadingText = this.gameObject.GetComponentInChildren<Text> ();
	}

	public int CallInLoadingBar(){
		totalTime = Algorithms.GetIndexByRange (2, 4);
		value = 0;
		this.gameObject.SetActive (true);
		this.gameObject.transform.localPosition = Vector3.zero;
		StartLoading ();
		return totalTime + 1;
	}

	void StartLoading(){
		value = value + 0.01f;
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
		fillImage.color = new Color (Mathf.Max (0, (1f - value)), 1f, 0f, 1f);
		if (value < 1)
			StartCoroutine (LoadingInProgress ());
		else {
			this.gameObject.SetActive (false);
		}

	}
	IEnumerator LoadingInProgress(){
		float f = 0.01f*totalTime;
		yield return new WaitForSeconds (f);
		StartLoading ();
	}

}
