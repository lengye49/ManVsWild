using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackpackActions : MonoBehaviour {

	public Text Melee;
	public Text Ranged;
	public Text Magic;
	public Text Head;
	public Text Body;
	public Text Shoe;
//	public Text Accessory;
	public Text Ammo;
	public Text AmmoNum;
	public Text Mount;

	public Button hotkey0;
	public Button hotkey1;

	public Text soulStone;

	public GameObject contentB;
	private GameObject bpCell;
	private ArrayList bpCells;
	private int _bpNum;

	void Start () {
		bpCell = Instantiate (Resources.Load ("bpCellNormal")) as GameObject;
		bpCell.SetActive (false);
		bpCells = new ArrayList ();
		UpdataPanel ();
	}
	
	public void UpdataPanel(){
		_bpNum = GameData._playerData.bpNum;
		UpdateCharacter();
		UpdateBpContent ();
		UpdateHotkeys ();
	}

	public void UpdateHotkeys(){
		if (GameData._playerData.Hotkey0 != 0) {
			hotkey0.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey0 / 10000)].name;
			hotkey0.interactable = true;
		} else {
			hotkey0.gameObject.GetComponentInChildren<Text> ().text = "尚未设置";
			hotkey0.interactable = false;
		}

		if (GameData._playerData.Hotkey1 != 0) {
			hotkey1.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey1 / 10000)].name;
			hotkey1.interactable = true;
		} else {
			hotkey1.gameObject.GetComponentInChildren<Text> ().text = "尚未设置";
			hotkey1.interactable = false;
		}
	}

	void UpdateCharacter(){
		Melee.text = (GameData._playerData.MeleeId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.MeleeId / 10000)].name) : "";
		Ranged.text = (GameData._playerData.RangedId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.RangedId / 10000)].name) : "";
		Magic.text = (GameData._playerData.MagicId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.MagicId / 10000)].name) : "";
		Head.text = (GameData._playerData.HeadId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.HeadId / 10000)].name) : "";
		Body.text = (GameData._playerData.BodyId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.BodyId / 10000)].name) : "";
		Shoe.text = (GameData._playerData.ShoeId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.ShoeId / 10000)].name) : "";
//		Accessory.text = (GameData._playerData.AccessoryId > 0) ? (LoadTxt.MatDic [(int)(GameData._playerData.MeleeId / 10000)].name) : "";
		if (GameData._playerData.AmmoId > 0 && GameData._playerData.AmmoNum > 0) {
			Ammo.text = LoadTxt.MatDic [(int)(GameData._playerData.AmmoId / 10000)].name;
			AmmoNum.text = "×" + GameData._playerData.AmmoNum;
		} else {
			Ammo.text = "";
			AmmoNum.text = "";
		}
		Mount.text = (GameData._playerData.Mount.monsterId > 0) ? (GameData._playerData.Mount.name) : "";
	}

	void UpdateBpContent (){
		if (bpCells.Count<_bpNum) {
			int n = bpCells.Count;
			for (int i = n; i < _bpNum; i++) {
				GameObject o = Instantiate (bpCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentB.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				bpCells.Add (o);
				ClearContent (o);
			}
			contentB.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,64 * bpCells.Count);
		}

		for (int i = 0; i < bpCells.Count; i++) {
			ClearContent (bpCells [i] as GameObject);
		}

		int j = 0;
		foreach (int key in GameData._playerData.bp.Keys) {
			if (j >= _bpNum)
				return;
			GameObject o = bpCells [j] as GameObject;
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.bp [key].ToString();
			j++;
		}

		soulStone.text = GameData._playerData.SoulStone.ToString();
	}

	void ClearContent(GameObject o){
		Text[] ts = o.gameObject.GetComponentsInChildren<Text> ();
		for (int i = 0; i < ts.Length; i++)
			ts [i].text = "";
		o.GetComponent<Button> ().interactable = false;
	}
}
