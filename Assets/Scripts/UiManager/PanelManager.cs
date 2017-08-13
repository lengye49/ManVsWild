using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour {

	public RectTransform Home;
	public RectTransform BedRoom;
	public RectTransform Warehouse;
	public RectTransform Workshop;
	public RectTransform Study;
	public RectTransform Farm;
	public RectTransform Pets;
	public RectTransform Well;
	public RectTransform Achievement;
	public RectTransform Altar;
	public RectTransform Death;
	public RectTransform Making;
	public RectTransform Backpack;
	public RectTransform Explore;
	public RectTransform Place;
	public RectTransform Battle;
	public LogManager _logManager;

//	public Text locationText;

	private RectTransform _FatherPanel;
	private RectTransform _PanelNow;
	private RectTransform _GrandFatherPanel;
	private float restPointLeftX = -3000f;
	private float restPointRightX = 3000f;
	private float tweenerTime = 0.5f;
	private Maps mapGoing;
	private GameData _gameData;

	public Maps MapGoing{
		set{ mapGoing = value;}
	}

	private TipManager _tipManager;

	void Start(){
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		if (GameData._playerData.placeNowId == 0)
			GoToPanel ("Home");
		else {
			mapGoing = LoadTxt.MapDic [GameData._playerData.placeNowId];
			GoToPanel ("Place");
		}
			
	}

	public void GoToPanel(string panelName){
		switch (panelName) {
		case "Home":
			Home.DOLocalMoveX (0, tweenerTime);
			Home.gameObject.GetComponentInChildren<HomeManager> ().UpdateContent ();
			if (_PanelNow == Explore && _FatherPanel == Place)
				CheckThiefActivities ();
			_FatherPanel = null;
			_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);

			_PanelNow = Home;
			GameData._playerData.placeNowId = 0;
			_gameData.StoreData ("PlaceNowId", 0);
			break;
		case "BedRoom":
			if (GameData._playerData.BedRoomOpen > 0) {
				if (BedRoom.localPosition.x > 10)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				BedRoom.DOLocalMoveX (0, tweenerTime);
				BedRoom.gameObject.GetComponent<RoomActions> ().UpdateRoomStates ();
				_FatherPanel = Home;
				_PanelNow = BedRoom;
			} else {
				_tipManager.ShowBuildingConstruct ("BedRoom");
			}
			break;
		case "Warehouse":
			if (GameData._playerData.WarehouseOpen > 0) {
				if (Warehouse.localPosition.x > 10) {
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				}else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Warehouse.DOLocalMoveX (0, tweenerTime);
				Warehouse.gameObject.GetComponent<WarehouseActions> ().UpdatePanel ();
				_FatherPanel = Home;
				_PanelNow = Warehouse;
			} else {
				_tipManager.ShowBuildingConstruct ("Warehouse");
			}
			break;
		case "Kitchen":
			if (GameData._playerData.KitchenOpen > 0) {
				if (Making.localPosition.x > 10)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Making.DOLocalMoveX (0, tweenerTime);
				this.gameObject.GetComponentInChildren<MakingActions> ().UpdatePanel (panelName);
				_FatherPanel = _PanelNow;
				_PanelNow = Making;
				break;
			} else {
				_tipManager.ShowBuildingConstruct ("Kitchen");
			}
			break;
		case "Workshop":
			if (GameData._playerData.WorkshopOpen > 0) {
				if (Workshop.localPosition.x > 10)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Workshop.DOLocalMoveX (0, tweenerTime);
				_FatherPanel = Home;
				_PanelNow = Workshop;
			} else {
				_tipManager.ShowBuildingConstruct ("Workshop");
			}
			break;
		case "Well":
			if (GameData._playerData.WellOpen > 0) {
				if(Well.localPosition.x>10)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Well.DOLocalMoveX (0, tweenerTime);
				this.gameObject.GetComponentInChildren<WellActions> ().UpdateWell ();
				_FatherPanel = Home;
				_PanelNow = Well;
			} else {
				_tipManager.ShowBuildingConstruct ("Well");
			}
			break;
		case "Backpack":
			if (_PanelNow == Backpack) {
				if (_FatherPanel == Explore) {
					if (GameData._playerData.placeNowId == 0)
						GoToPanel ("Home");
					else
						GoToPanel ("Explore");
				}
				else
					GoToPanel ("Father");
				break;
			}
			if (Backpack.localPosition.x > 10)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
			Backpack.DOLocalMoveX (0, tweenerTime);
			Backpack.gameObject.GetComponent<BackpackActions> ().UpdataPanel ();
			_GrandFatherPanel = _FatherPanel;
			_FatherPanel = _PanelNow;
			_PanelNow = Backpack;
			break;
		case "Explore":
			if (_PanelNow == Explore) {
				if (_FatherPanel == Backpack)
					GoToPanel ("Home");
				else
					GoToPanel ("Father");
				break;
			}
			if (Explore.localPosition.x > 10)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
			Explore.DOLocalMoveX (0, tweenerTime);
			Explore.gameObject.GetComponent<ExploreActions> ().UpdateExplore ();
			_GrandFatherPanel = _FatherPanel;
			_FatherPanel = _PanelNow;
			_PanelNow = Explore;
			break;
		case "Battle":
			if (Battle.localPosition.x > 10)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
			Battle.DOLocalMoveX (0, tweenerTime);
			_GrandFatherPanel = _FatherPanel;
			_FatherPanel = _PanelNow;
			_PanelNow = Battle;
			break;
		case "Place":
			Place.gameObject.GetComponent<PlaceActions> ().PlayBackGroundMusic (mapGoing.id);

			if (mapGoing.id == 0) {
				GoToPanel ("Home");
				break;
			}
			if (Place.localPosition.x > 0)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);	
			Place.DOLocalMoveX (0, tweenerTime);
			if (_PanelNow == Battle) {
				Place.gameObject.GetComponent<PlaceActions> ().UpdatePlace (mapGoing, false);
			} else {
				Place.gameObject.GetComponent<PlaceActions> ().UpdatePlace (mapGoing, true);
			}

			GameData._playerData.placeNowId = mapGoing.id;
			_gameData.StoreData ("PlaceNowId", mapGoing.id);
			_GrandFatherPanel = _FatherPanel;
			_FatherPanel = _PanelNow;
			_PanelNow = Place;
			break;
		case "Study":
			if (GameData._playerData.StudyOpen > 0) {
				if(Study.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Study.DOLocalMoveX (0, tweenerTime);
				Study.gameObject.GetComponent<StudyActions> ().UpdateStudy ();
				_FatherPanel = Home;
				_PanelNow = Study;
			} else {
				_tipManager.ShowBuildingConstruct ("Study");
			}
			break;
		case "Farm":
			if (GameData._playerData.FarmOpen > 0) {
				if(Farm.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Farm.DOLocalMoveX (0, tweenerTime);
				Farm.gameObject.GetComponent<FarmActions> ().UpdateFarm ();
				_FatherPanel = Home;
				_PanelNow = Farm;
			} else {
				_tipManager.ShowBuildingConstruct ("Farm");
			}
			break;
		case "Pets":
			if (GameData._playerData.PetsOpen > 0) {
				if(Pets.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Pets.DOLocalMoveX (0, tweenerTime);
				Pets.gameObject.GetComponent<PetsActions> ().UpdatePets ();
				_FatherPanel = Home;
				_PanelNow = Pets;
			} else {
				_tipManager.ShowBuildingConstruct ("Pets");
			}
			break;
		case "Achievement":
			if (GameData._playerData.AchievementOpen > 0) {
				if(Achievement.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Achievement.DOLocalMoveX (0, tweenerTime);
				Achievement.gameObject.GetComponent<AchievementActions> ().UpdateAchievement ();
				_FatherPanel = Home;
				_PanelNow = Achievement;
			} else {
				_tipManager.ShowBuildingConstruct ("Achievement");
			}
			break;
		case "Altar":
			if (GameData._playerData.AltarOpen > 0) {
				if(Altar.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				Altar.DOLocalMoveX (0, tweenerTime);
				this.gameObject.GetComponentInChildren<AlterActions> ().UpdateAltar ();
				_FatherPanel = Home;
				_PanelNow = Altar;
			} else {
				_tipManager.ShowBuildingConstruct ("Altar");
			}
			break;
		case "Death":
			break;
		case "Melee":
		case "Ranged":
		case "Magic":
		case "Head":
		case "Body":
		case "Shoe":
		case "Accessory":
			if (Making.localPosition.x > 0)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
			Making.DOLocalMoveX (0, tweenerTime);
			this.gameObject.GetComponentInChildren<MakingActions> ().UpdatePanel (panelName);
			_FatherPanel = _PanelNow;
			_PanelNow = Making;
			break;
		case "Making":
			if (Making.localPosition.x > 0)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
			Making.DOLocalMoveX (0, tweenerTime);
			_FatherPanel = _GrandFatherPanel;
			_PanelNow = Making;
			break;
		case "Father":
			GoToPanel (_FatherPanel.name);
			break;
		default:
			Debug.Log ("The panel name is wrong with " + panelName);
			break;
		}
	}

	void CheckThiefActivities(){

		if (GameData._playerData.dayNow <= GameConfigs.StartThiefEvent)
			return;
		if (GameData._playerData.lastThiefTime >= GameData._playerData.dayNow - 3)
			return;
		if (Algorithms.GetIndexByRange (0, 100) >= (int)(GameData._playerData.ThiefDefence*100f))
			return;

		GameData._playerData.lastThiefTime = GameData._playerData.dayNow;
		_gameData.StoreData ("LastThiefTime", GameData._playerData.lastThiefTime);

		int[] weight = new int[LoadTxt.ThiefDic.Count];
		int[] ids = new int[weight.Length];
		int i = 0;
		foreach (int key in LoadTxt.ThiefDic.Keys) {
			weight [i] = LoadTxt.ThiefDic [key].weight;
			ids [i] = key;
			i++;
		}
		i = Algorithms.GetResultByWeight (weight);
		Thief _thisThief = LoadTxt.ThiefDic [ids [i]];

		int antiAlert = LoadTxt.MonsterDic [_thisThief.monsterId].level;
		int alert = GetGuardAlert ();

		//发现概率
		int p = 0;
		if (alert > 2 * antiAlert)
			p = 10000;
		else if (antiAlert > alert * 5)
			p = 0;
		else
			p = alert * 10000 / (alert + antiAlert);

		int r = Random.Range (0, 10000);

		if (r <= p) {
			CatchThief (_thisThief);
		} else {
			BeStolen (_thisThief);
		}
	}

	int GetGuardAlert(){
		//警惕性 = 宠物的等级
		int alert = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			if (GameData._playerData.Pets [key].state == 2) {
				alert += GameData._playerData.Pets [key].alertness;
			}
		}
		//获得警惕性之和再除以3
		return (int)(alert / 3f);
	}

	void CatchThief(Thief t){
		
		string s = "你获得了";
		Dictionary<int,int> drop = Algorithms.GetReward (LoadTxt.MonsterDic [t.monsterId].drop);
		foreach (int key in drop.Keys) {
			_gameData.AddItem (key * 10000, drop [key]);
			s += LoadTxt.MatDic [key].name + " ×" + drop [key];
			break;
		}
		_logManager.AddLog (t.name + "试图盗窃，但是被守卫抓住了。" + s);
//		Debug.Log (t.name + "试图盗窃，但是被守卫抓住了。" + s);
		//Achievement
		this.gameObject.GetComponentInParent<AchieveActions>().CatchThief(t.id);
	}

	void BeStolen(Thief t){

		Dictionary<int,int> target = new Dictionary<int, int> ();

		int totalValue = 0;//计算总价值
		foreach (int key in GameData._playerData.wh.Keys) {
			totalValue += LoadTxt.MatDic [(int)(key / 10000)].price * GameData._playerData.wh [key];
		}

		int max = (int)(10000f / GameData._playerData.wh.Count);//把仓库物品分成max份
		int num = 0;
		int value = 0;
		int i = 1;

		foreach (int key in GameData._playerData.wh.Keys) {
			int r = Algorithms.GetIndexByRange (0, 10000);
			if (r < i * max && r > ((i - 1) * max)) {
				float f = Algorithms.GetIndexByRange (0, 30) / 100f * GameData._playerData.wh [key];
				f *= GameData._playerData.TheftLossDiscount;

				num = (int)Mathf.Round (f);
				if (num > 0) {
					target.Add (key, num);
					value += LoadTxt.MatDic [(int)(key / 10000)].price * num;
				}
			}

			//价值保护
			if (value >= totalValue * 0.05f || value >= 300)
				break;
		}
		_gameData.DeleteItemInWh (target);
		string s = "";
		foreach (int key in target.Keys) {
			s += LoadTxt.MatDic [(int)(key / 10000)].name + " -" + target [key] + ",";
		}
		if (s != "") {
			s = s.Substring (0, s.Length - 1) + "。";
			_logManager.AddLog ("警告!" + t.name + "闯入家中。" + s);
//			Debug.Log ("警告!" + t.name + "闯入家中。" + s);	
		} else {
			_logManager.AddLog ("警告!" + t.name + "闯入家中。但什么都没看上，空手而去。");
//			Debug.Log ("警告!" + t.name + "闯入家中。但什么都没看上，空手而去。");
		}
			
	}

	public void CheckThief100(){
		for (int i = 0; i < 100; i++) {
			CheckThiefActivities ();
		}
	}

}

