using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementActions : MonoBehaviour {

	public GameObject ContentA;
	private GameObject achievementCell;
	private ArrayList achievementCells;
	private AchieveActions _achieveActions;

	void Start () {
		achievementCell = Instantiate (Resources.Load ("achievementCell")) as GameObject;
		achievementCell.SetActive (false);
		achievementCells = new ArrayList ();
		_achieveActions = this.gameObject.GetComponentInParent<AchieveActions> ();
	}

	public void UpdateAchievement(){

		for (int i = achievementCells.Count; i < LoadTxt.AchievementDic.Count; i++) {
			GameObject o = Instantiate (achievementCell) as GameObject;
			o.SetActive (true);
			o.transform.SetParent (ContentA.transform);
			o.transform.localPosition = Vector3.zero;
			o.transform.localScale = Vector3.one;
			achievementCells.Add (o);
		}

		int index = 0;
		foreach(int key in LoadTxt.AchievementDic.Keys) {
			GameObject o = achievementCells [index] as GameObject;
			SetAchievement (o, LoadTxt.AchievementDic [key]);
			index++;
		}
		ContentA.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,110 * achievementCells.Count);
	}

	void SetAchievement(GameObject o,Achievement a){
		Text[] t = o.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = a.name;
		t [1].text = a.desc + "(" + _achieveActions.GetProgress (a.id) + ")";
	}
}
