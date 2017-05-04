using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlaceActions : MonoBehaviour {

	public GameObject contentP;
	public GameObject contentG;
	public RectTransform resourceDetail;
	public RectTransform observeDetail;
//	public RectTransform treasureDetail;
	public RectTransform goodsDetail;
	public RectTransform placeRect;
	public RectTransform dungeonRect;
	public BattleActions _battleActions;
	public LoadingBar _loadingBar;

	private GameObject placeCell;
	private ArrayList placeCells;
	private Places _place;
	private LoadTxt _loadTxt;
	private FloatingActions _floating;
	private GameData _gameData;
	private PlaceUnit _puNow;
	private Maps _mapNow;
	private PanelManager _panelManager;

	private GameObject goodsCell;
	private ArrayList goodsCells;
	private ShopItem _shopItemSelected;
	private int goodsMax;
	private int goodsNum;
	private float popTime= 0.3f;

	private int[] dungeonCellState;//0 Cannot open;1 Can open;2 Opened;3 Door
	public Button[] dungeonCells;
	public Sprite frontImage;
	public Sprite backImage;
	public Sprite nextLevelImage;
	private int dungeonLevel;
	private int thisExitIndex;

	void Start(){
		placeCell = Instantiate (Resources.Load ("placeCell")) as GameObject;
		goodsCell = Instantiate (Resources.Load ("goodsCell")) as GameObject;
		placeCell.SetActive (false);
		goodsCell.SetActive (false);
		placeCells = new ArrayList ();
		goodsCells = new ArrayList ();
		_loadTxt = this.gameObject.GetComponentInParent<LoadTxt> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
//		UpdatePlace (1);
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
//		} else if (r < 99) {
//			SetDungeonShop ();
//			Debug.Log ("NPC Trade");
		} else if (r < 50) {
			Debug.Log ("Nothing Text");
		} else {
			Debug.Log ("Nothing");
		}
	}

//	void SetDungeonShop(){
//		CallInObserveDetail ();
//		int j = 0;
//		int r = Algorithms.GetIndexByRange (2, 5);
//		for (int i = 0; i < r; i++) {
//			GameObject o;
//			if (goodsCells.Count <= j) {
//				o = Instantiate (goodsCell) as GameObject;
//				o.transform.SetParent (contentG.transform);
//				o.transform.localPosition = Vector3.zero;
//				o.transform.localScale = Vector3.one;
//				goodsCells.Add (o);
//			} else {
//				o = goodsCells [i] as GameObject;
//				o.SetActive (true);
//			}
//
//			ShopItem shopItem = new ShopItem(Algorithms.GetRandomGoods(dungeonLevel));
//			o.gameObject.name = shopItem.itemId.ToString ();
//			Text[] _texts = o.gameObject.GetComponentsInChildren<Text> ();
//			_texts [0].text = GetGoodsName (shopItem.reward);
//
//			j++;
//		}
//	}

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
		dungeonRect.localPosition = new Vector3 (-3000, 420, 0);
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
		contentP.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,115 * placeCells.Count);
	}

	void SetPlaceCellState(GameObject o,PlaceUnit pu){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = pu.actionParam;
		switch (pu.actionType) {
		case 0:
			t [2].text = GameConfigs.CuttingTime + "m";
			t [3].text = GameConfigs.CuttingStrength + " Strength";
			t [4].text = "";
			break;
		case 1:
			t [2].text = GameConfigs.DiggingTime + "m";
			t [3].text = GameConfigs.DiggingStrength + " Strength";
			t [4].text = "";
			break;
		case 2:
			t [2].text = GameConfigs.FetchingTime + "m";
			t [3].text = GameConfigs.FetchingStrength + " Strength";
			t [4].text = "";
			break;
		case 3:
			t [2].text ="";
			t [3].text = "";
			t [4].text = GameConfigs.SearchTime + "m";
			break;
		case 4:
			t [2].text = GameConfigs.CollectTime + "m";
			t [3].text = GameConfigs.CollectStrength + " Strength";
			break;
		case 5:
			t [2].text ="";
			t [3].text = "";
			t [4].text = GameConfigs.HuntTime + "m";
			break;
		default:
			t [2].text = "";
			t [3].text = "";
			t [4].text = "";
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
//		treasureDetail.localPosition = new Vector3 (150, 0, 0);
		goodsDetail.localPosition = new Vector3 (150, 0, 0);

		observeDetail.gameObject.SetActive (false);
		resourceDetail.gameObject.SetActive (false);
//		treasureDetail.gameObject.SetActive (false);
		goodsDetail.gameObject.SetActive (false);
	}

	void CallInResourceDetail(){
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.SetActive (false);
//		treasureDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
//		treasureDetail.gameObject.SetActive (false);

		resourceDetail.gameObject.SetActive (true);
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	void CallInObserveDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);
//		treasureDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
//		treasureDetail.gameObject.SetActive (false);

		observeDetail.gameObject.SetActive (true);
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	public void CallOutDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);
//		treasureDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
//		treasureDetail.gameObject.SetActive (false);
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
		t [1].text = "pu.desc";
		t [2].text = "Grow: " + GameConfigs.TreeGrowSpeed + " /Day" + "\nLeft: " + ((int)nowTrees).ToString ();
		t [3].text = "Get:";
		t [4].text = ac;
		t [5].text = "Cost:";
		t [6].text = GameConfigs.CuttingTime + " Minutes," + GameConfigs.CuttingStrength + " Strength";

		Button[] b = resourceDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].gameObject.GetComponentInChildren<Text> ().text = "Cut";
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
		t [1].text = "pu.desc";
		t [2].text = "Resources Left:" + now.ToString()+"/"+total.ToString ();
		t [3].text = "Get:";
		t [4].text = ac;
		t [5].text = "Cost:";
		t [6].text = GameConfigs.DiggingTime + " Minutes," + GameConfigs.DiggingStrength + " Strength";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "Dig";
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
		t [1].text = "pu.desc";
		t [2].text = "";
		t [3].text = "Get:";
		t [4].text = ac;
		t [5].text = "Cost:";
		t [6].text = GameConfigs.FetchingTime + " Minutes," + GameConfigs.FetchingStrength + " Strength";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "Fetch";
		b.interactable = true;
	}

	void SetSearching(PlaceUnit pu){
		_puNow = pu;
		//open;total;left;id|weight;id|weight;...
		string[] s=pu.actionParam.Split(';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);
		string ac = "";
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac+=LoadTxt.MatDic[int.Parse(ss[0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "pu.desc";
		t [2].text = "Progress: " + (int)(now / total) + "%";
		t [3].text = "Get:";
		t [4].text = ac;
		t [5].text = "Cost:";
		t [6].text = GameConfigs.SearchTime + " Minutes";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "Search";
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
		t [1].text = "pu.desc";
		t [2].text = "";
		t [3].text = "Get:";
		t [4].text = ac;
		t [5].text = "Cost:";
		t [6].text = GameConfigs.CollectTime + " Minutes," + GameConfigs.CollectStrength + " Strength";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "Collect";
		b.interactable = true;

	}

	void SetHunting(PlaceUnit pu){
		_puNow = pu;
		//open;id|weight;...
		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "pu.desc";
		t [2].text = "";
		t [3].text = "Target:";
		t [4].text = "";
		t [5].text = "Cost:";
		t [6].text = GameConfigs.HuntTime + " Minutes";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "Hunt";
		b.interactable = true;
	}

	void SetObserving(PlaceUnit pu){
		_puNow = pu;
		//param格式:open;Name;Description;ShopId;MonsterId
		string[] s = pu.actionParam.Split (';');
		Shop thisShop = LoadTxt.ShopDic [int.Parse (s[3])];
		Text[] t = observeDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = s [1];
		t [1].text = s [2];
		Button[] b = observeDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].interactable = (s.Length >= 5);

		for (int i = 0; i < goodsCells.Count; i++) {
			GameObject o = goodsCells [i] as GameObject;
			o.gameObject.SetActive (false);
		}

		int j = 0;

		for (int i = 0; i < thisShop.shopItemList.Count; i++) {
			ShopItem shopItem = thisShop.shopItemList [i] as ShopItem;
			if (CheckSaleOut (shopItem))
				continue;

			GameObject o;
			if (goodsCells.Count <= j) {
				o = Instantiate (goodsCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentG.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				goodsCells.Add (o);
			} else {
				o = goodsCells [i] as GameObject;
				o.SetActive (true);
			}

			o.gameObject.name = shopItem.itemId.ToString ();
			Text[] _texts = o.gameObject.GetComponentsInChildren<Text> ();
			_texts [0].text = GetGoodsName (shopItem.reward);

			j++;

		}

		contentG.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(540,65 * j);
	}
		
	bool CheckSaleOut(ShopItem si){
		switch (si.itemType) {
		case "0":
			if (si.buyTimes > 0)
				return true;
			else
				return false;
		case "1":
			return false;
		case "-1":
			return false;
		default:
			string[] s = si.itemType.Split ('|');
			int limit = int.Parse (s [1]);
			if (si.buyTimes < limit)
				return false;
			else
				return true;
		}
	}

	int GetGoodsMax(ShopItem si){
		switch (si.itemType) {
		case "0":
			if (si.buyTimes >= 1)
				return 0;
			else
				return 1;
		case "1":
			return -1;
		case "-1":
			return 1;
		default:
			string[] s = si.itemType.Split ('|');
			int limit = int.Parse (s [1]);
			return limit-si.buyTimes;
		}
	}

	string GetGoodsName(Dictionary<int,int> d){
		foreach (int key in d.Keys) {
			if (key == 0)
				return "A Map";
			else if (key == 1)
				return "Fomula of " + LoadTxt.MatDic [d [key]].name;
			else
				return LoadTxt.MatDic [key].name + "×" + d [key];
		}
		return string.Empty;
	}

	string GetDescription(Dictionary<int,int> d){
		foreach (int key in d.Keys) {
			if (key == 0)
				return "A small piece of map, very normal.";
			else if (key == 1)
				return "Learn how to make " + LoadTxt.MatDic [d[key]].name;
			else
				return LoadTxt.MatDic [key].desc;
		}
		return string.Empty;
	}

	string GetCost(Dictionary<int,int> d){
		if(d.Count==0)
			return "Accept Mission";
		foreach (int key in d.Keys) {
			return LoadTxt.MatDic [key].name + " ×" + d [key];
		}
		return string.Empty;
	}

	bool CheckIsEnough(Dictionary<int,int> d){
		if (d.Count == 0)
			return true;
		foreach (int key in d.Keys) {
			if (_gameData.CountInBp (key) >= d [key])
				return true;
			else
				return false;
		}
		return false;
	}

	public void CallInGoodsDetail(int shopItemId){
		
		if (goodsDetail.localPosition.y > 10 || goodsDetail.localPosition.y < -10)
			goodsDetail.DOLocalMoveY (-300, popTime);
		goodsDetail.gameObject.name = LoadTxt.MatDic [shopItemId].name;
		_shopItemSelected = LoadTxt.ShopItemDic [shopItemId];
		Text[] t = goodsDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = GetGoodsName (_shopItemSelected.reward);
		t [1].text = GetDescription (_shopItemSelected.reward);
		t [2].text = GetCost (_shopItemSelected.cost);
		t [2].color = CheckIsEnough (_shopItemSelected.cost) ? Color.green : Color.red;
		goodsMax = GetGoodsMax(_shopItemSelected);
		goodsNum = (goodsMax > 0) ? 1 : 0;
		ChangeGoodsNumText ();
		Button[] b = goodsDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].interactable = (goodsNum <= goodsMax && goodsNum>=1);
		b [1].interactable = true;
	}

	void ChangeGoodsNumText(){
		string s = goodsNum.ToString ();
		if (goodsMax != -1)
			s += "/" + goodsMax;
		Text[] t = goodsDetail.gameObject.GetComponentsInChildren<Text> ();
		t [3].text = s;
	}

	public void AddGoodsNum(){
		if (goodsNum < goodsMax)
			goodsNum++;
		else
			goodsNum = goodsMax;
		ChangeGoodsNumText ();
	}

	public void ReduceGoodsNum(){
		if (goodsNum > 1)
			goodsNum--;
		ChangeGoodsNumText ();
	}

	public void CallOutGoodsDetail(){
		goodsDetail.DOLocalMoveY (-3000, popTime);
	}
		
	void SetTreasureChest(PlaceUnit pu){

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
		case "Cut":
			StartCoroutine (StartCut ());
			break;
		case "Dig":
			StartCoroutine (StartDig ());
			break;
		case "Fetch":
			StartCoroutine (StartFetch ());
			break;
		case "Collect":
			StartCoroutine (StartFetch ());
			break;
		case "Search":
			StartCoroutine (StartSearch ());
			break;
		case "Hunt":
			Hunt ();
			break;
		default:
			Debug.Log ("Wrong action : " + t);
			break;
		}
		SetPlace (_mapNow);
	}

	IEnumerator StartCut(){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		Cut();
	}

	IEnumerator StartDig(){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		Dig();
	}

	IEnumerator StartFetch(){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		Fetch();
	}

	IEnumerator StartSearch(){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Insufficient Strength", 1);
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
			_floating.CallInFloating ("Insufficient Strength", 1);
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
			_floating.CallInFloating ("Insufficient Strength", 1);
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
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";

				//Achievement
				switch (LoadTxt.MatDic [key].type) {
				case 3:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMeleeWeapon (key);
					break;
				case 4:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectRangedWeapon (key);
					break;
				case 5:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMagicWeapon (key);
					break;
				default:
					break;
				}
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);

			//Achievement
			if (now == 0)
				this.gameObject.GetComponentInParent<AchieveActions> ().TotalSearch ();
		}

		string a = "1;" + total + ";" + now + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetSearching (_puNow);
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
		Debug.Log ("Hunt for fun : " + LoadTxt.MonsterDic [ids [index]].name);

		Monster[] m = new Monster[num];
		for(int i=0;i<m.Length;i++)
			m [i] = LoadTxt.MonsterDic [ids [index]];
		_panelManager.GoToPanel ("Battle");

		int r = Algorithms.GetIndexByRange(0,100);
		bool isSpoted = r < (GameData._playerData.SpotRate * 100);
		_battleActions.InitializeBattleField (m, isSpoted);
	}

	public void Trade(){
		if (goodsNum < 1)
			return;
		if (!CheckIsEnough (_shopItemSelected.cost)) {
			_floating.CallInFloating ("Insufficient Resources", 1);
		} else {
			TradeProcess ();
		}
	}

	void TradeProcess(){
		ConsumeItems ();
		GainResult ();
		SetObserving (_puNow);
	}

	void GainResult(){
		switch (_shopItemSelected.itemType) {
		case "0":
			if (_shopItemSelected.buyTimes >= 1)
				return;
			else {
				_shopItemSelected.buyTimes = 1;
				_loadTxt.StoreShopItemBuyTimes (_shopItemSelected.itemId, 1);
				CallOutGoodsDetail ();
			}
			break;
		case "1":
			break;
		case "-1":
			break;
		default:
			string[] s = _shopItemSelected.itemType.Split ('|');
			int limit = int.Parse (s [1]);
			if (_shopItemSelected.buyTimes >= limit)
				return;
			_shopItemSelected.buyTimes++;
			_loadTxt.StoreShopItemBuyTimes (_shopItemSelected.itemId, _shopItemSelected.buyTimes);
			if (_shopItemSelected.buyTimes >= limit)
				CallOutDetail ();
			break;
		}

		foreach (int key in _shopItemSelected.reward.Keys) {
			int index =  _shopItemSelected.reward [key];
			if (key == 0) {
				if (GameData._playerData.MapOpenState [index] == 0) {
					GameData._playerData.MapOpenState [index] = 1;
					_gameData.StoreData ("MapOpenState", _gameData.GetStrFromMapOpenState (GameData._playerData.MapOpenState));
					_floating.CallInFloating ("New place : " + LoadTxt.MapDic [index].name,0);

					//Achievement
					this.gameObject.GetComponentInParent<AchieveActions> ().NewPlaceFind ();
				} else {
					_floating.CallInFloating ("Another way to : " + LoadTxt.MapDic [index].name, 0);
				}
			} else if (key == 1) {
				if (index == 4209) {
					GameData._playerData.Farms [0].open = 1;
					_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
					_floating.CallInFloating ("I can make Wine in my farms now", 0);
				} else if (index == 4210) {
					GameData._playerData.Farms [1].open = 1;
					_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
					_floating.CallInFloating ("I can make Beer in my farms now", 0);
				} else if (index == 4208) {
					GameData._playerData.Farms [2].open = 1;
					_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
					_floating.CallInFloating ("I can make Whiskey in my farms now", 0);
				} else {
					_gameData.LearnBlueprint (index);
					_floating.CallInFloating ("I can make " + LoadTxt.MatDic [index].name + " in my farms now", 0);
				}
			} else {
				_gameData.AddItem (key, index);
				_floating.CallInFloating (LoadTxt.MatDic [key].name + " +" + index, 0);

				//Achievement
				switch (LoadTxt.MatDic [key].type) {
				case 3:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMeleeWeapon (key);
					break;
				case 4:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectRangedWeapon (key);
					break;
				case 5:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMagicWeapon (key);
					break;
				default:
					break;
				}
			}
		}
	}
		

	void ConsumeItems(){
		if (_shopItemSelected.cost.Count == 0)
			return;
		else {
			foreach (int key in _shopItemSelected.cost.Keys) {
				_gameData.ConsumeItem (key, _shopItemSelected.cost [key]);
			}
		}
	}
		

	public void Rob(){
		//param格式:open;Name;Description;ShopId;MonsterId
		string[] s = _puNow.actionParam.Split (';');
		if (s.Length < 5)
			return;
		int monsterId = int.Parse (s [4]);
		Monster[] m = new Monster[1];
		m [0] = LoadTxt.MonsterDic [monsterId];
		_battleActions.InitializeBattleField (m, false);
	}


}
