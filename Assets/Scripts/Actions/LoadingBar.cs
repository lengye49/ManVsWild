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
		loadingTxt = "进行中";
		loadingText = this.gameObject.GetComponentInChildren<Text> ();
	}

	public int CallInLoadingBar(int costMin){
		int max = Mathf.Min (2000, 1000 + costMin * 4);
		totalTime = (int)(Random.Range (1000, max)/1000);
		value = 0;
		this.gameObject.SetActive (true);
		this.gameObject.transform.localPosition = new Vector3 (0f, -666f, 0f);
		StartLoading ();
		return totalTime + 1;
	}

	void StartLoading(){
		value = value + 0.01f;
		if ((int)(value * 10) % 4 == 0) {
			loadingTxt = "进行中";
		}else if((int)(value * 10) % 4 == 1){
			loadingTxt = "进行中.";
		}else if((int)(value * 10) % 4 == 2){
			loadingTxt = "进行中..";
		}else{
			loadingTxt = "进行中...";
		}
		loadingBar.value = value;
		loadingText.text = loadingTxt;
		fillImage.color = new Color (0, Mathf.Max (0, (value + 1) / 2), 0f, 1f);
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
