using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FloatingActions : MonoBehaviour {

	private float upTime = 0.2f;
	private float waitTime = 1f;
	private float disappearTime = 0.5f;
	private GameObject f;

	/// <summary>
	/// Calls in floating.
	/// </summary>
	/// <param name="str">Float text.</param>
	/// <param name="floatType">Float type:0Good,1Bad.</param>
	public void CallInFloating(string str,int floatType){
		f = Instantiate (Resources.Load ("floatingCell")) as GameObject;;
		f.transform.SetParent (this.gameObject.transform);
		f.SetActive (true);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = str;

		Color c = new Color ();
		switch (floatType) {
		case 0:
			c = Color.green;
			break;
		case 1:
			c = Color.red;
			break;
		default:
			c = Color.green;
			break;
		}
		t.color = c;
		StartFloat ();
	}
	void StartFloat(){
		f.transform.localPosition = Vector3.zero;
		f.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
		f.transform.DOLocalMoveY (100, upTime);
		f.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f),upTime);
		f.GetComponentInChildren<Text> ().DOFade (1, upTime);
		StartCoroutine (WaitAndNext ());
	}

	IEnumerator WaitAndNext(){
		yield return new WaitForSeconds (upTime + waitTime);
		EndFloat ();
	}

	void EndFloat(){
		f.transform.DOLocalMoveY (250, disappearTime);
		f.GetComponentInChildren<Text>().DOFade (0, disappearTime);
		StartCoroutine (WaitAndEnd ());
	}

	IEnumerator WaitAndEnd(){
		yield return new WaitForSeconds (disappearTime);
		Destroy (f);
	}
}
