using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MakingActions : MonoBehaviour {
	public GameObject contentM;
	public GameObject upgradeButton;

	private GameObject makingCell;
	private ArrayList makingCells;

	void Start(){
		makingCell= Instantiate (Resources.Load ("makingCell")) as GameObject;
		makingCell.SetActive (false);
		makingCells = new ArrayList ();
	}

	public void UpdatePanel(string makingType){
		ClearContents ();
//		Debug.Log(GameData._playerData.KitchenOpen + "")
		if (makingType == "Kitchen" && GameData._playerData.KitchenOpen<GameConfigs.MaxLv_Kitchen)
			upgradeButton.SetActive (true);
		else
			upgradeButton.SetActive (false);
		
		int i = 0;
		foreach (Mats m in LoadTxt.mats) {
			if ((m.makingType != makingType) || (m.needBlueprint == 1 && GameData._playerData.LearnedBlueprints.ContainsKey (m.id)))
				continue;
			GameObject o;
			if (i >= makingCells.Count) {
				o = Instantiate (makingCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentM.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				makingCells.Add (o);
			} else {
				o = makingCells [i] as GameObject;
				o.SetActive (true);
			}

			o.name = m.id.ToString();
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = m.name;
			i++;
		}
		if (i < makingCells.Count) {
			for (int j = i; j < makingCells.Count; j++) {
				GameObject o = makingCells [j] as GameObject;
				o.SetActive (false);
			}
		}
		contentM.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,120 * i);
	}

	void ClearContents(){
		for (int i = 0; i < makingCells.Count; i++) {
			GameObject o = makingCells [i] as GameObject;
			Text[] t = o.GetComponentsInChildren<Text> ();
			for (int j = 0; j < t.Length; j++)
				t [j].text = "";
		}
	}
}
