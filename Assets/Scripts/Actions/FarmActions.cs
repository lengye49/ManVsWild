﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FarmActions : MonoBehaviour {

	public GameObject ContentF;
	public RectTransform plantingTip;
	public Button upgradeButton;

	private GameObject farmCell;
	private ArrayList farmCells;
	private int openFarmlands;
	private GameData _gameData;
	private FloatingActions _floating;

	void Start () {
		farmCell = Instantiate (Resources.Load ("farmCell")) as GameObject;
		farmCells = new ArrayList ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
	}

	public void UpdateFarm(){
		plantingTip.localPosition = new Vector3 (150, 2000, 0);
		openFarmlands = 0;

		upgradeButton.gameObject.SetActive (!(GameData._playerData.FarmOpen >= GameConfigs.MaxLv_Farm));

		foreach (int key in GameData._playerData.Farms.Keys) {
			if (GameData._playerData.Farms [key].open == 1)
				openFarmlands++;
		}

		if (openFarmlands > farmCells.Count) {
			for (int i = farmCells.Count; i < openFarmlands; i++) {
				GameObject o = Instantiate (farmCell) as GameObject;
				o.transform.SetParent (ContentF.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				farmCells.Add (o);
				ClearContents (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.Farms.Keys) {
			if (GameData._playerData.Farms [key].open > 0) {
				GameObject o = farmCells [j] as GameObject;
				SetFarmState (o, GameData._playerData.Farms [key], j,key);
				j++;
			}
		}
		ContentF.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,250 * j);
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	void SetFarmState(GameObject o, FarmState f,int j,int key){
		Text[] t = o.GetComponentsInChildren<Text> ();
		Button[] b = o.GetComponentsInChildren<Button> ();

		if (f.plantType==0)
			t[0].text = "Farmland";
		else
			t[0].text = "Cellar";

		if (f.plantTime <= 0) {
			t [1].text = "(Empty)";
			t [1].color = Color.grey;
			if (f.plantType == 0)
				t [2].text = "I can grow crops here.";
			else if (f.plantType == 1)
				t [2].text = "I can make wine here.";
			else if (f.plantType == 2)
				t [2].text = "I can make beer here.";
			else if (f.plantType == 3)
				t [2].text = "I can make whiskey here.";
			else
				Debug.Log ("wrong plantType!!");
			t [3].text = "";
			t [4].text = "Remove";
			b [0].interactable = false;
			b [0].name = key.ToString();
			t [5].text = "Prepare";
			b [1].interactable = true;
			b[1].name= "Prepare";
		} else {
			bool isMature = IsMature (f.plantTime, LoadTxt.PlantsDic [f.plantType]);
			t [1].text = isMature ? "(Ready)" : "(In Progress)";
			t [1].color = isMature ? Color.green : Color.black;
			if (isMature) {
				if (f.plantType == 0)
					t [2].text = "The crops are mature.";
				else if (f.plantType == 1)
					t [2].text = "The wine is ready.";
				else if (f.plantType == 2)
					t [2].text = "The beer is ready.";
				else if (f.plantType == 3)
					t [2].text = "The whiskey is ready.";
				else
					Debug.Log ("wrong plantType!!");

				t [4].text = "Remove";
				b [0].interactable = true;
				b [0].name = key.ToString();
				t [5].text = "Charge";
				b [1].interactable = true;
				b[1].name= "Charge";
			} else {
				t [2].text = "Time left : " + GetLeftTime (f.plantTime, LoadTxt.PlantsDic [f.plantType]);
				t [4].text = "Remove";
				b [0].interactable = true;
				b [0].name = key.ToString();
				t [5].text = "Charge";
				b [1].interactable = false;
				b[1].name= "Charge";
			}
		}
	}

	bool IsMature(int t,Plants p){
		return (t + p.plantGrowCycle * 24 * 60 <= GameData._playerData.minutesPassed);
	}

	string GetLeftTime(int t,Plants p){
		int t1 = t + p.plantGrowCycle * 24 * 60;
		int t2 = t1 - GameData._playerData.minutesPassed;
		if (t2 >= 24 * 60)
			return (int)((t2) / 60 / 24) + " days";
		else {
			int h = (int)(t2 / 60);
			int m = t2 - 60 * h;
			return h + " hours" + m + " minutes";
		}
	}

	public void RemoveCrop(int index){
		GameData._playerData.Farms [index].plantTime = 0;
		_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		UpdateFarm ();
	}

	public void ChargeCrop(int index){
		Dictionary<int,int> r = new Dictionary<int, int> ();
		Plants p = LoadTxt.PlantsDic [GameData._playerData.Farms [index].plantType];
		int num;
		switch (p.plantType) {
		case 0:
			foreach (int key in p.plantObtain.Keys) {
				num = (int)(p.plantObtain [key] * Algorithms.GetIndexByRange (80, 120) / 100 * GameData._playerData.HarvestIncrease);
				if (num > 0)
					r.Add (key, num);
			}
			break;
		default:
			foreach (int key in p.plantObtain.Keys) {
				num = (int)(p.plantObtain [key] * Algorithms.GetIndexByRange (75, 125) / 100 * GameData._playerData.OenologyIncrease);
				if (num > 0)
					r.Add (key, num);
			}
			break;
		}

		foreach (int key in r.Keys) {
			_gameData.AddItem (key * 10000, r [key]);
			_floating.CallInFloating (LoadTxt.MatDic [key].name + " +" + r [key], 0);
		}
		GameData._playerData.Farms [index].plantTime = 0;
		_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		UpdateFarm ();
	}
		
	public void CallInPlantingTip(int index){
		plantingTip.gameObject.SetActive (true);
		if (plantingTip.transform.localPosition.y != 0)
			plantingTip.DOLocalMoveY (0, 0.3f);
		int plantType = GameData._playerData.Farms [index].plantType;
		plantingTip.gameObject.name = index.ToString();
		Text[] t = plantingTip.gameObject.GetComponentsInChildren<Text> ();

		switch (plantType) {
		case 0:
			t [0].text = "Crops";
			break;
		case 1:
			t [0].text = "Wine";
			break;
		case 2:
			t [0].text = "Beer";
			break;
		case 3:
			t [0].text = "Whiskey";
			break;
		default:
			break;
		}

		t[2].text = GetReq (LoadTxt.PlantsDic [plantType].plantReq);
		t[4].text=LoadTxt.PlantsDic [plantType].plantTime + " hours";
		t[6].text = LoadTxt.PlantsDic[plantType].plantGrowCycle + " days";
		t[7].text= "Prepare";
	}

	public void Prepare(){
		Debug.Log ("Prepare");
		int index = int.Parse (plantingTip.gameObject.name);
		int plantType = GameData._playerData.Farms [index].plantType;;
		_gameData.ChangeTime (LoadTxt.PlantsDic [plantType].plantTime * 60);
		foreach (int key in LoadTxt.PlantsDic[plantType].plantReq.Keys) {
			_gameData.ConsumeItem (key, LoadTxt.PlantsDic [plantType].plantReq [key]);
		}
		GameData._playerData.Farms [index].plantTime = GameData._playerData.minutesPassed;
		CallOutPlantingTip ();
		UpdateFarm ();
	}

	public void CallOutPlantingTip(){
		plantingTip.DOLocalMoveY (2000, 0.3f);
	}

	string GetReq(Dictionary<int,int> d){
		string s = "";
		foreach (int key in d.Keys) {
			s += LoadTxt.MatDic [key].name + " ×" + d [key] + "\n";
		}
		s = s.Substring (0, s.Length - 1);
		return s;
	}
}
