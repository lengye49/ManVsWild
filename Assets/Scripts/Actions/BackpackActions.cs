﻿using UnityEngine;
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
	public Text Ammo;
	public Text AmmoNum;
	public Text Mount;
    public Text BpNum;

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
        BpNum.text = "(" + GameData._playerData.bp.Count + "/" + _bpNum + ")";
		UpdateCharacter();
		UpdateBpContent ();
	}


	void UpdateCharacter(){
        
        UpdateEquip(Melee, GameData._playerData.MeleeId);
        UpdateEquip(Ranged, GameData._playerData.RangedId);
        UpdateEquip(Magic, GameData._playerData.MagicId);
        UpdateEquip(Head, GameData._playerData.HeadId);
        UpdateEquip(Body, GameData._playerData.BodyId);
        UpdateEquip(Shoe, GameData._playerData.ShoeId);


		if (GameData._playerData.AmmoId > 0 && GameData._playerData.AmmoNum > 0) {
			Ammo.text = LoadTxt.MatDic [(int)(GameData._playerData.AmmoId / 10000)].name;
			AmmoNum.text = "×" + GameData._playerData.AmmoNum;
            Color c = new Color();
            c = GameConfigs.MatColor[LoadTxt.MatDic[(int)(GameData._playerData.AmmoId / 10000)].quality];
            Ammo.color = c;
            AmmoNum.color = c;
		} else {
			Ammo.text = "";
			AmmoNum.text = "";
		}
		Mount.text = (GameData._playerData.Mount.monsterId > 0) ? (GameData._playerData.Mount.name) : "";
	}

    void UpdateEquip(Text t,int itemId){
        if (itemId > 0)
        {
            Mats mat = new Mats();
            mat = LoadTxt.MatDic[(int)(itemId / 10000)];
            t.text = mat.name;
            t.color = GameConfigs.MatColor[mat.quality];
        }else{
            t.text = "";
        }
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
			contentB.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,80 * bpCells.Count);
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
	}

	void ClearContent(GameObject o){
		Text[] ts = o.gameObject.GetComponentsInChildren<Text> ();
		for (int i = 0; i < ts.Length; i++)
			ts [i].text = "";
		o.GetComponent<Button> ().interactable = false;
	}
}
