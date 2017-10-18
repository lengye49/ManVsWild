﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FarmActions : MonoBehaviour {

	public GameObject ContentF;
	public RectTransform plantingTip;
	public Button upgradeButton;
    public LoadingBar _loading;

	private GameObject farmCell;
	private ArrayList farmCells;
	private int openFarmlands;
	private GameData _gameData;
	private FloatingActions _floating;

	void Start () {
		farmCell = Instantiate (Resources.Load ("farmCell")) as GameObject;
		farmCell.SetActive (false);
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
				o.SetActive (true);
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
		Button b = o.GetComponentInChildren<Button> ();

		if (f.plantType==0)
			t[0].text = "农田";
		else
			t[0].text = "酒窖";

		if (f.plantTime <= 0) {
			t [1].text = "(闲置)";
			t [1].color = Color.grey;
			if (f.plantType == 0)
				t [2].text = "可在此种植作物。";
			else if (f.plantType == 1)
				t [2].text = "可在此酿造红酒";
			else if (f.plantType == 2)
				t [2].text = "可在此酿造啤酒";
			else if (f.plantType == 3)
				t [2].text = "可在此酿造白酒";
			else
				Debug.Log ("wrong plantType!!");
			b.interactable = true;
			b.name = key.ToString()+"|Prepare";
			t [3].text = "准备";
		} else {
			bool isMature = IsMature (f.plantTime, LoadTxt.PlantsDic [f.plantType]);
			t [1].text = isMature ? "(收获)" : "(等待)";
			t [1].color = isMature ? Color.green : Color.black;
			if (isMature) {
				if (f.plantType == 0)
					t [2].text = "作物已经成熟。";
				else if (f.plantType == 1)
					t [2].text = "红酒已经酿制完成。";
				else if (f.plantType == 2)
					t [2].text = "啤酒已经酿制完成。";
				else if (f.plantType == 3)
					t [2].text = "白酒已经酿制完成。";
				else
					Debug.Log ("wrong plantType!!");

				b .interactable = true;
				b.name = key.ToString()+"|Charge";
				t [3].text = "收获";
			} else {
				t [2].text = "Time left : " + GetLeftTime (f.plantTime, LoadTxt.PlantsDic [f.plantType]);
				b .interactable = false;
				b .name = key.ToString()+"|Charge";
				t [3].text = "收获";
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
			t [0].text = "作物";
			break;
		case 1:
			t [0].text = "红酒";
			break;
		case 2:
			t [0].text = "啤酒";
			break;
		case 3:
			t [0].text = "白酒";
			break;
		default:
			break;
		}
                
        int i = 2;
        bool canPrepare = true;

        Dictionary<int,int> d = LoadTxt.PlantsDic[plantType].plantReq;
        foreach (int key in d.Keys) {
            t[i].text= LoadTxt.MatDic [key].name + " ×" + d [key];
            if (_gameData.CountInBp(key) < d[key])
            {
                t[i].color = Color.red;
                canPrepare = false;
            }
            else
                t[i].color = Color.green;
            i++;
        }
        plantingTip.GetComponentInChildren<Button>().interactable = canPrepare;

		t[5].text= LoadTxt.PlantsDic [plantType].plantTime + " 时";
		t[7].text = LoadTxt.PlantsDic[plantType].plantGrowCycle + " 天";
		t[8].text= "准备";
	}

	public void Prepare(){
        


		int index = int.Parse (plantingTip.gameObject.name);
		int plantType = GameData._playerData.Farms [index].plantType;;

        foreach (int key in LoadTxt.PlantsDic[plantType].plantReq.Keys)
        {
            if (_gameData.CountInBp(key) < LoadTxt.PlantsDic[plantType].plantReq[key])
                return;
        }
            
        int t = _loading.CallInLoadingBar(60);

        StartCoroutine(GetPrepared(plantType, index, t));
	}

    IEnumerator GetPrepared(int plantType,int index,int waitTime){
        yield return new WaitForSeconds(waitTime);

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
}
