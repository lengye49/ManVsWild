﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlaceActions : MonoBehaviour {

	public GameObject contentP;
	public RectTransform resourceDetail;
	public RectTransform observeDetail;
	public RectTransform placeRect;
	public RectTransform dungeonRect;
	public BattleActions _battleActions;
	public LoadingBar _loadingBar;
	public LogManager _logManager;

	private GameObject placeCell;
	private ArrayList placeCells = new ArrayList ();
	private Places _place;
	private LoadTxt _loadTxt;
	private FloatingActions _floating;
	private GameData _gameData;
	private PlaceUnit _puNow;
	private Maps _mapNow;
	private PanelManager _panelManager;

//	private GameObject goodsCell;
//	private ArrayList goodsCells = new ArrayList ();
//	private ShopItem _shopItemSelected;
//	private int goodsMax;
//	private int goodsNum;
//	private float popTime= 0.3f;

	private int[] dungeonCellState;//0 Cannot open;1 Can open;2 Opened;3 Door
	public Button[] dungeonCells;
	public Sprite frontImage;
	public Sprite backImage;
	public Sprite nextLevelImage;
	private int dungeonLevel;
	private int thisExitIndex;

	void Awake(){
		placeCell = Instantiate (Resources.Load ("placeCell")) as GameObject;
	}

	void Start(){
		
//		goodsCell = Instantiate (Resources.Load ("goodsCell")) as GameObject;
		placeCell.SetActive (false);
//		goodsCell.SetActive (false);
		_loadTxt = this.gameObject.GetComponentInParent<LoadTxt> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
	}

	public void UpdatePlace(Maps m,bool newPlace){
		_mapNow = m;
		_place = LoadTxt.PlaceDic [m.id];

		if (m.id == 21) {
			if (!newPlace)
				return;
			dungeonLevel = GameData._playerData.dungeonLevelMax + 1;
			InitializeDungeon ();
		} else {
			SetDetailPosition ();
			SetPlace (m);
		}
	}

	public void PlayBackGroundMusic(int mapId){
		int isRain = Random.Range (0, 10);
		string bgSoundName = "";
		if (isRain < 2)
			bgSoundName = "rain_small";
		else if (isRain < 3)
			bgSoundName = "rain_heavy";
		else {
			switch (mapId) {
			case 0:
				bgSoundName = "";
				break;
			case 1:
				bgSoundName = "river";
				break;
			case 2:
				bgSoundName = "woods";
				break;
			case 3:
				bgSoundName = "mountain";
				break;
			case 4:
				bgSoundName = "";
				break;
			case 5:
				bgSoundName = "mountain";
				break;
			case 6:
				bgSoundName = "cave";
				break;
			case 7:
				bgSoundName = "island";
				break;
			case 8:
			case 9:
			case 10:
				bgSoundName = "";
				break;
			case 11:
				bgSoundName = "mountain";
				break;
			case 12:
				bgSoundName = "";
				break;
			case 13:
				bgSoundName = "woods";
				break;
			case 14:
				bgSoundName = "woods";
				break;
			case 15:
				bgSoundName = "cave";
				break;
			case 16:
			case 17:
			case 18:
				bgSoundName = "";
				break;
			case 19:
				bgSoundName = "hawk";
				break;
			case 20:
				bgSoundName = "";
				break;
			case 21:
				bgSoundName = "dungeon";
				break;
			case 22:
			case 23:
				bgSoundName = "island";
				break;
			case 24:
				bgSoundName = "sea";
				break;
			default:
				break;
			}
		}

		this.gameObject.GetComponentInParent<PlaySound> ().PlayEnvironmentSound (bgSoundName);
	}

	public void UpdatePlace(int lv){
		dungeonLevel = lv;
		InitializeDungeon ();
	}

	void InitializeDungeon(){
		Debug.Log ("Welcome to Dungeon Lv." + dungeonLevel);
		dungeonRect.localPosition = Vector3.zero;
		placeRect.localPosition = new Vector3 (-10000, 0, 0);
		dungeonCellState = new int[20];
		for (int i = 0; i < dungeonCellState.Length; i++) {
			if (i == 0)
				dungeonCellState [i] = 1;
			else
				dungeonCellState [i] = 0;
		}
		SetDungeonState ();
		thisExitIndex = Algorithms.GetIndexByRange (1, 20);
	}

	void SetDungeonState(){
		for(int i=0;i<dungeonCells.Length;i++){
			SetDungeonState (i);
		}
	}

	void SetDungeonState(int i){
		if (dungeonCellState [i] == 0) {
			dungeonCells [i].image.sprite = frontImage;
			dungeonCells [i].image.color = new Color (200, 200, 200, 128);
			dungeonCells [i].interactable = false;
		} else if (dungeonCellState [i] == 1) {
			dungeonCells [i].image.sprite = frontImage;
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;
		} else if (dungeonCellState [i] == 2) {
			dungeonCells [i].image.sprite = backImage;				
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;			
		} else if (dungeonCellState [i] == 3) {
			dungeonCells [i].image.sprite = nextLevelImage;				
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;	
		}
	}

	public void OpenDungeonCover(int index){
		if (dungeonCellState [index] == 1) {
			if (index == thisExitIndex) {
				dungeonCellState [index] = 3;
				Debug.Log ("Find the way down.");
			} else {
				dungeonCellState [index] = 2;
				DungeonEvent ();
			}
			CheckRoad (index);
			ChangeImage (index);
		} else if (dungeonCellState [index] == 3) {
			if (GameData._playerData.dungeonLevelMax < dungeonLevel)
				_gameData.StoreData ("DungeonLevelMax", dungeonLevel);
			dungeonLevel++;
			InitializeDungeon ();
		}
	}

	void DungeonEvent(){
		
		//Get Dungeon Rewards According to the Level;

		//1 Reward 10%, 2 Monster 15%, 3 Buff&Debuff 10%, 4 Trade 1%, 5 Nothing: Text 12%, 6 Nothing ***********Need a csv.
		int r = Algorithms.GetIndexByRange(0,100);
		if (r < 10) {
			Debug.Log ("You Found + Reward:");
			int r4 = (int)(dungeonLevel / 10) + 1;
			int matId = Algorithms.GetResultByDic (LoadTxt.DungeonTreasureList [r4].reward);
			int num = 1;
			if (LoadTxt.MatDic [matId].price < 100) {
				num = Algorithms.GetIndexByRange (1, 4);
			}
			_gameData.AddItem (matId * 10000, num);
		} else if (r < 25) {
			Debug.Log ("You Found Monster List");
			int r1 = Algorithms.GetIndexByRange (1, 5);
			Monster[] ms = new Monster[r1];
			for (int i = 0; i < r1; i++) {
				ms [i] = GetNewMonster ();
			}
			_panelManager.GoToPanel ("Battle");
			r1 = Algorithms.GetIndexByRange(0,100);
			bool isSpoted = r1 < (GameData._playerData.SpotRate * 100);
			_battleActions.InitializeBattleField (ms, isSpoted);
		} else if (r < 35) {
			//Hp+3~5 10%,Hp-2~4 10%,Spirit+3~5 10% Spirit-2~4 10%,Water+4~8 10%,Food+4~8 10%,Temp+1~3 10%,Temp-1~3 10%,Hp+99 5%,Spirit+99 5%
			//HpMax+1 1%,SpiritMax+1 1%,StrengthMax+1 2%,WaterMax+1 2%,FoodMax+1 2%,TempMax+1 1%,TempMin-1 1%
			int r2 = Algorithms.GetIndexByRange (0, 100);
			int r3;
			if (r2 < 10) {
				r3 = Algorithms.GetIndexByRange (3, 6);
				_gameData.ChangeProperty (0, r3);
				Debug.Log ("Add Hp");
			} else if (r2 < 20) {
				r3 = -Algorithms.GetIndexByRange (2, 5);
				_gameData.ChangeProperty (0, r3);
				Debug.Log ("Reduce Hp");
			} else if (r2 < 30) {
				r3 = Algorithms.GetIndexByRange (3, 6);
				_gameData.ChangeProperty (2, r3);
				Debug.Log ("Add Spirit");
			} else if (r2 < 40) {
				r3 = -Algorithms.GetIndexByRange (2, 5);
				_gameData.ChangeProperty (2, r3);
				Debug.Log ("Reduce Spirit");
			} else if (r2 < 50) {
				r3 = Algorithms.GetIndexByRange (4, 9);
				_gameData.ChangeProperty (4, r3);
				Debug.Log ("Add Food");
			} else if (r2 < 60) {
				r3 = Algorithms.GetIndexByRange (4, 9);
				_gameData.ChangeProperty (6, r3);
				Debug.Log ("Add Water");
			} else if (r2 < 70) {
				r3 = Algorithms.GetIndexByRange (1, 4);
				_gameData.ChangeProperty (10, r3);
				Debug.Log ("Add Temp");
			} else if (r2 < 80) {
				r3 = -Algorithms.GetIndexByRange (1, 4);
				_gameData.ChangeProperty (10, r3);
				Debug.Log ("Reduce Temp");
			} else if (r2 < 85) {
				_gameData.ChangeProperty (0, 99);
				Debug.Log ("Add Hp");
			} else if (r2 < 90) {
				_gameData.ChangeProperty (2, 99);
				Debug.Log ("Add Spirit");
			} else if (r2 < 91) {
				_gameData.ChangeProperty (1, 1);
				Debug.Log ("Add MaxHp");
			} else if (r2 < 92) {
				_gameData.ChangeProperty (3, 1);
				Debug.Log ("Add MaxSpirit");
			} else if (r2 < 94) {
				_gameData.ChangeProperty (5, 1);
				Debug.Log ("Add MaxFood");
			} else if (r2 < 96) {
				_gameData.ChangeProperty (7, 1);
				Debug.Log ("Add MaxWater");
			} else if (r2 < 98) {
				_gameData.ChangeProperty (9, 1);
				Debug.Log ("Add MaxStrength");
			} else if (r2 < 99) {
				_gameData.ChangeProperty (11, -1);
				Debug.Log ("Reduce TempMin");
			} else if (r2 < 100) {
				_gameData.ChangeProperty (12, 1);
				Debug.Log ("Add TempMax");
			} else {
				Debug.Log ("Nothing Happened");
			}
		} else if (r < 50) {
			Debug.Log ("Nothing Text");
		} else {
			Debug.Log ("Nothing");
		}
	}
        

	Monster GetNewMonster(){
		int minLv=1;
		int maxLv=35+(int)(dungeonLevel/10)*5;
		int num=0;
		if (dungeonLevel%10==0){
			minLv=maxLv-5;
		}
		ArrayList monsterList = new ArrayList ();
		foreach(int key in LoadTxt.MonsterDic.Keys){
			if(LoadTxt.MonsterDic[key].level<minLv || LoadTxt.MonsterDic[key].level>maxLv)
				continue;
			num++;
			monsterList.Add (LoadTxt.MonsterDic [key]);
		}
		if (num < 1) {
			Debug.Log ("No Monster Found!");
			return new Monster ();
		}
		int index = Algorithms.GetIndexByRange (1, num+1);
		Monster m = monsterList [index] as Monster;
		return m;
	}

	void CheckRoad(int index){
		if (index >= 5) {
			if (dungeonCellState [index - 5] == 0) {
				dungeonCellState [index - 5] = 1;
				SetDungeonState (index - 5);
			}
		}
		if (index <= 14) {
			if (dungeonCellState [index + 5] == 0) {
				dungeonCellState [index + 5] = 1;
				SetDungeonState (index + 5);
			}
		}
		if (index % 5 != 0) {
			if (dungeonCellState [index - 1] == 0) {
				dungeonCellState [index - 1] = 1;
				SetDungeonState (index - 1);
			}
		}
		if (index % 5 != 4) {
			if (dungeonCellState [index + 1] == 0) {
				dungeonCellState [index + 1] = 1;
				SetDungeonState (index + 1);
			}
		}
	}

	void ChangeImage(int index){
		dungeonCells [index].transform.DOBlendableRotateBy (new Vector3 (0, 90f, 0), 0.4f);
		StartCoroutine (WaitAndTurn (index));
	}

	IEnumerator WaitAndTurn(int index){
		yield return new WaitForSeconds (0.4f);
		dungeonCells [index].gameObject.SetActive (false);
		dungeonCells [index].image.sprite = (dungeonCellState [index] == 2) ? backImage : nextLevelImage;
		dungeonCells [index].transform.Rotate (new Vector3 (0, -180, 0));
		dungeonCells [index].gameObject.SetActive (true);
		dungeonCells [index].transform.DOBlendableRotateBy (new Vector3 (0, 0, 0), 0.3f);
	}

	void SetPlace(Maps m){
		dungeonRect.localPosition = new Vector3 (-6000, 420, 0);
		placeRect.localPosition = new Vector3 (0, 420, 0);

		int count = 0;

		for (int i = 0; i < _place.placeUnits.Count; i++) {
			PlaceUnit pu = _place.placeUnits [i] as PlaceUnit;
			string[] s = pu.actionParam.Split (';');
			if (s [0] == "1")
				count++;
		}

		for (int i = 0; i < placeCells.Count; i++) {
			GameObject o = placeCells [i] as GameObject;
			o.SetActive (true);
			ClearContents (o);
		}

		if (count > placeCells.Count) {
			for (int i = placeCells.Count; i < count; i++) {
				GameObject o = Instantiate (placeCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentP.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				placeCells.Add (o);
				ClearContents (o);
			}
		}
			
		for (int i = 0; i < _place.placeUnits.Count; i++) {
			PlaceUnit pu = _place.placeUnits [i] as PlaceUnit;
			GameObject o = placeCells [i] as GameObject;
			o.gameObject.name = pu.unitId.ToString ();
			SetPlaceCellState (o, pu);
		}

		if (placeCells.Count > count)
			for (int i = count; i < placeCells.Count; i++) {
				GameObject o = placeCells [i] as GameObject;
				o.SetActive (false);
			}
		contentP.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (900, 115 * count);
	}

	void SetPlaceCellState(GameObject o,PlaceUnit pu){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = pu.desc;
		t [2].text = "";
		switch (pu.actionType) {
		case 0:
			t [3].text ="伐木";
			break;
		case 1:
			t [3].text ="挖掘";
			break;
		case 2:
			t [3].text ="提水";
			break;
		case 3:
			t [3].text = "探索";
			break;
		case 4:
			t [3].text = "收获";
			break;
		case 5:
			t [3].text = "出发";
			break;
		case 6:
			t [3].text = "交谈";
			break;
		default:
			t [3].text = "";
			break;
		}

	}
		
	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t[i].text = "";
		}
	}

	/*ActionType
	 * 0 Cutting trees
	 * 1 Mining
	 * 2 Fetching water
	 * 3 Searching
	 * 4 Collect
	 * 5 Hunting
	 * 6 Tasks&Observe
	 * 7 
	 * 8 Treasure Box
	 */
	public void CallInDetail(int unitId){
		PlaceUnit pu = new PlaceUnit ();
		foreach (PlaceUnit p in _place.placeUnits) {
			if (p.unitId == unitId)
				pu = p;
		}
			
		switch (pu.actionType) {
		case 0:
			CallInResourceDetail ();
			SetCuttingWood (pu);
			break;
		case 1:
			CallInResourceDetail ();
			SetMining (pu);
			break;
		case 2:
			CallInResourceDetail ();
			SetFetchingWater (pu);
			break;
		case 3:
			CallInResourceDetail ();
			SetSearching (pu);
			break;
		case 4:
			CallInResourceDetail ();
			SetCollect (pu);
			break;
		case 5:
			CallInResourceDetail ();
			SetHunting (pu);
			break;
		case 6:
			CallInObserveDetail ();
			SetObserving (pu);
			break;
		default:
			Debug.Log ("wrong unitId " + unitId);
			break;
		}
	}

	void SetDetailPosition(){
		observeDetail.localPosition = new Vector3 (0, 0, 0);
		resourceDetail.localPosition = new Vector3 (150, 0, 0);
//		goodsDetail.localPosition = new Vector3 (150, -3000, 0);

		observeDetail.gameObject.SetActive (false);
		resourceDetail.gameObject.SetActive (false);
//		goodsDetail.gameObject.SetActive (false);
	}

	void CallInResourceDetail(){
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.SetActive (false);

		resourceDetail.gameObject.SetActive (true);
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	void CallInObserveDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);

		observeDetail.gameObject.SetActive (true);
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	public void CallOutDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.SetActive (false);
	}

	void SetCuttingWood(PlaceUnit pu){
		_puNow = pu;
		float lastTrees = 0f;
		int lastTime = 0;
		//open;lastTrees;lastTime;id|num|prop;...
		string[] s = pu.actionParam.Split (';');
		lastTrees = float.Parse (s [1]);
		lastTime = int.Parse (s [2]);
		float nowTrees = lastTrees + (GameData._playerData.minutesPassed - lastTime) / 60 / 24 * GameConfigs.TreeGrowSpeed;

		string a = "1;" + nowTrees + ";" + GameData._playerData.minutesPassed + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i]+";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		string ac = "";
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}		
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = _puNow.name;
		t [1].text = "";
		t [2].text = "增速: " + GameConfigs.TreeGrowSpeed + " /天" + "\n剩余: " + ((int)nowTrees).ToString ();
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.CuttingTime + "分";
		t [6].text = "消耗:"+ GameConfigs.CuttingStrength + "力量";

		Button[] b = resourceDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].gameObject.GetComponentInChildren<Text> ().text = "伐木";
		b [0].interactable = (nowTrees >= 1);
	}

	void SetMining(PlaceUnit pu){
		_puNow = pu;
		//open;totalAmount;nowAmount;id|num|prop;...
		string[] s = _puNow.actionParam.Split (';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);
		string ac = "";
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);
			
		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = "";
		t [2].text = "剩余:" + now.ToString()+"/"+total.ToString ();
		t [3].text = "可能获得:";
		t [4].text = "石料,各类矿产";
		t [5].text = "耗时: "+GameConfigs.DiggingTime + "分";
		t [6].text = "消耗: " + GameConfigs.DiggingStrength + "力量";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "挖掘";
		b.interactable = (now > 0);
	}

	void SetFetchingWater(PlaceUnit pu){
		_puNow = pu;
		//open;id|num|prop;id|num|prop...
		string[] s = _puNow.actionParam.Split (';');
		string ac = "";
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.FetchingTime + "分";
		t [6].text = "消耗: " + GameConfigs.FetchingStrength + " 力量";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "提水";
		b.interactable = true;
	}

	void SetSearching(PlaceUnit pu){
		_puNow = pu;
		//open;total;left;id|weight;id|weight;...
		string[] s=pu.actionParam.Split(';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "探索进度:";
		t [4].text = (100 - (int)(now * 100 / total)) + "%";
		t [5].text = "耗时: "+GameConfigs.SearchTime + "分";
		t [6].text = "";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "探索";
		b.interactable = (now > 0);
	}

	void SetCollect(PlaceUnit pu){
		_puNow = pu;
		//open;id|num|prop;...
		string	[] s = pu.actionParam.Split(';');
		string ac = "";
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac+=LoadTxt.MatDic[int.Parse(ss[0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.CollectTime + "分";
		t [6].text = "消耗: " + GameConfigs.CollectStrength + "力量";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "收获";
		b.interactable = true;

	}

	void SetHunting(PlaceUnit pu){
		_puNow = pu;
		//open;id|weight;...
		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "猎物:";
		t [4].text = "各类野兽";
		t [5].text = "耗时: "+GameConfigs.HuntTime + "分";
		t [6].text = "";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "捕猎";
		b.interactable = true;
	}

	void SetObserving(PlaceUnit pu){
		_puNow = pu;
		//param格式:open;Name;Description;MonsterId  New
		string[] s = pu.actionParam.Split (';');
		Text[] t = observeDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = s [1];
		t [1].text = s [2];
		Button[] b = observeDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].interactable = (s.Length >= 3);
	}
		
	public void ResourceAct(){
		if (GameData._playerData.dayNow > GameConfigs.StartGhostEvent) {
			bool ghostComing = CheckGhost ();
			if (ghostComing) {
				_panelManager.GoToPanel ("Battle");
				Monster[] m = new Monster[1];
				m [0] = FindGhost ();

				int r = Algorithms.GetIndexByRange (0, 100);
				bool isSpoted = r < (GameData._playerData.SpotRate * 100);
				_battleActions.InitializeBattleField (m, isSpoted);

				return;
			}
		}

		string t = resourceDetail.gameObject.GetComponentInChildren<Button> ().gameObject.GetComponentInChildren<Text> ().text;
		switch (t) {
		case "伐木":
			StartCoroutine (StartCut ());
			break;
		case "挖掘":
			StartCoroutine (StartDig ());
			break;
		case "提水":
			StartCoroutine (StartFetch ());
			break;
		case "收获":
			StartCoroutine (StartFetch ());
			break;
		case "探索":
			StartCoroutine (StartSearch ());
			break;
		case "捕猎":
			Hunt ();
			break;
		default:
			Debug.Log ("Wrong action : " + t);
			break;
		}
		SetPlace (_mapNow);
	}

	IEnumerator StartCut(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Cut();
	}

	IEnumerator StartDig(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Dig();
	}

	IEnumerator StartFetch(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Fetch();
	}

	IEnumerator StartSearch(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Search();
	}

	bool CheckGhost(){

		if (GameData._playerData.hourNow >= 5 && GameData._playerData.hourNow <= 19)
			return false;
		int r = Algorithms.GetIndexByRange (0, 100);
		if (r < (100f * GameData._playerData.GhostComingProp))
			return true;
		return false;
	}

	Monster FindGhost(){
		int[] weight = new int[GameConfigs.GhostDic.Count];
		Monster[] m = new Monster[weight.Length];
		int i = 0;
		foreach(int key in GameConfigs.GhostDic.Keys){
			m [i] = LoadTxt.MonsterDic [key];
			weight [i++] = GameConfigs.GhostDic [key];
		}
		i = Algorithms.GetResultByWeight (weight);
		return m [i];
	}

	void Cut(){
		if (GameData._playerData.strengthNow < GameConfigs.CuttingStrength) {
			_floating.CallInFloating ("力量不足", 1);
			return;
		}
		//open;lastTrees;lastTime;id|num|prop;...
		float lastTrees = 0f;
		int lastTime = 0;
		string[] s = _puNow.actionParam.Split (';');
		lastTrees = float.Parse (s [1]);
		lastTime = int.Parse (s [2]);
		float nowTrees = lastTrees + (GameData._playerData.minutesPassed - lastTime) / 60 / 24 * GameConfigs.TreeGrowSpeed;
		if (nowTrees < 1)
			return;
		
		_gameData.ChangeProperty (8, -GameConfigs.CuttingStrength);
		_gameData.ChangeTime (GameConfigs.CuttingTime);
		nowTrees--;

		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 3] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}

		string a = "1;" + nowTrees + ";" + GameData._playerData.minutesPassed + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetCuttingWood (_puNow);
	}

	void Dig(){
		if (GameData._playerData.strengthNow < GameConfigs.DiggingStrength) {
			_floating.CallInFloating ("力量不足", 1);
			return;
		}
		//open;totalAmount;nowAmount;id|num|prop;...

		string[] s = _puNow.actionParam.Split (';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);

		_gameData.ChangeProperty (8, -GameConfigs.DiggingStrength);
		_gameData.ChangeTime (GameConfigs.DiggingTime );
		now--;

		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 3] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}

		string a = "1;" + total + ";" + now + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetMining (_puNow);
	}

	void Fetch(){
		if (GameData._playerData.strengthNow < GameConfigs.FetchingStrength) {
			_floating.CallInFloating ("力量不足", 1);
			return;
		}
		//open;id|num|prop;id|num|prop...
		_gameData.ChangeProperty (8, -GameConfigs.FetchingStrength);
		_gameData.ChangeTime (GameConfigs.FetchingTime);

		string[] s = _puNow.actionParam.Split (';');
		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 1];
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 1] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}
	}

	void Search(){

		//open;total;left;id|weight;id|weight;...
		_gameData.ChangeTime (GameConfigs.SearchTime);

		string[] s=_puNow.actionParam.Split(';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);
		int[] weight = new int[s.Length - 3];
		int[] ids = new int[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ids [i - 3] = int.Parse (ss [0]);
			weight [i - 3] = int.Parse (ss [1]);
		}

		int num = Algorithms.GetIndexByRange ((int)(GameConfigs.SearchRewardsNum * GameData._playerData.SearchRate * 0.8f), (int)(GameConfigs.SearchRewardsNum * GameData._playerData.SearchRate * 1.2f));
		num = (num > now) ? now : num;
		now = now - num;
		Dictionary<int,int> rewards = new Dictionary<int, int> ();
		for (int i = 0; i < num; i++) {
			int index = Algorithms.GetResultByWeight (weight);
			if (rewards.ContainsKey (ids [index])) {
				rewards [ids [index]]++;
			} else {
				rewards.Add (ids [index], 1);
			}
		}

		if (rewards.Count >= 1) {
			string newItems = "获得 ";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_logManager.AddLog (newItems);

			//Achievement
			if (now == 0)
				this.gameObject.GetComponentInParent<AchieveActions> ().TotalSearch ();
		}

		string a = "";
		if (now > 0)
			a = "1;" + total + ";" + now + ";";
		else
			a = "0;" + total + ";" + now + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetSearching (_puNow);
		_gameData.SearchNewPlace (_mapNow.id, now * 100 / total);
	}

	void Hunt(){
		//open;id|weight;...
		_gameData.ChangeTime (GameConfigs.HuntTime);
		string[] s=_puNow.actionParam.Split(';');
		int[] weight = new int[s.Length - 1];
		int[] ids = new int[s.Length - 1];
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ids [i - 1] = int.Parse (ss [0]);
			weight [i - 1] = int.Parse (ss [1]);
		}
		int index = Algorithms.GetResultByWeight (weight);
		int num = Algorithms.GetIndexByRange (1, 1 + LoadTxt.MonsterDic [ids [index]].groupNum);
//		Debug.Log ("Hunt for fun : " + LoadTxt.MonsterDic [ids [index]].name);

		Monster[] m = new Monster[num];
		for(int i=0;i<m.Length;i++)
			m [i] = LoadTxt.MonsterDic [ids [index]];
		_panelManager.GoToPanel ("Battle");

		int r = Algorithms.GetIndexByRange(0,100);
		bool isSpoted = r < (GameData._playerData.SpotRate * 100);
		_battleActions.InitializeBattleField (m, isSpoted);
	}


	public void Rob(){
		//param格式:open;Name;Description;MonsterId
		string[] s = _puNow.actionParam.Split (';');
		if (s.Length < 5)
			return;
		int monsterId = int.Parse (s [3]);
		Monster[] m = new Monster[1];
		m [0] = LoadTxt.MonsterDic [monsterId];
		Debug.Log ("Challange Monster : " + m [0].name);
		_panelManager.GoToPanel ("Battle");
		_battleActions.InitializeBattleField (m, false);
	}


}
