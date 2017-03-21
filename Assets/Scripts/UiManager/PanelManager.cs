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
	public RectTransform MailBox;
	public RectTransform Altar;
	public RectTransform Death;
	public RectTransform Making;
	public RectTransform Backpack;
	public RectTransform Explore;
	public RectTransform Place;
	public RectTransform Battle;

	public Text locationText;

	private RectTransform _FatherPanel;
	private RectTransform _PanelNow;
	private RectTransform _GrandFatherPanel;
	private float restPointLeftX = -1000f;
	private float restPointRightX = 1000f;
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
		GoToPanel ("Home");
	}

	public void GoToPanel(string panelName){
		switch (panelName) {
		case "Home":
			Home.DOLocalMoveX (0, tweenerTime);
			Home.gameObject.GetComponentInChildren<HomeManager> ().UpdateContent ();
			_FatherPanel = null;
			_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			if (_PanelNow == Place)
				CheckThiefActivities ();
			_PanelNow = Home;
			UpdateLocation ("Home");
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
				UpdateLocation ("BedRoom");
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
				UpdateLocation ("Warehouse");
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
				UpdateLocation ("Kitchen");
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
				UpdateLocation ("Workshop");
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
				UpdateLocation ("Well");
			} else {
				_tipManager.ShowBuildingConstruct ("Well");
			}
			break;
		case "Backpack":
			if (_PanelNow == Backpack) {
				if (_FatherPanel == Explore)
					GoToPanel ("Home");
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
			if (mapGoing.id == 0) {
				GoToPanel ("Home");
				break;
			}
			if (Place.localPosition.x > 0)
				_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
			else
				_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);	
			Place.DOLocalMoveX (0, tweenerTime);
			Place.gameObject.GetComponent<PlaceActions> ().UpdatePlace (mapGoing);
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
				UpdateLocation ("Study");
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
				UpdateLocation ("Farm");
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
				UpdateLocation ("Pets");
			} else {
				_tipManager.ShowBuildingConstruct ("Pets");
			}
			break;
		case "MailBox":
			if (GameData._playerData.MailBoxOpen > 0) {
				if(MailBox.localPosition.x>0)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				else
					_PanelNow.DOLocalMoveX (restPointRightX, tweenerTime);
				MailBox.DOLocalMoveX (0, tweenerTime);
				MailBox.gameObject.GetComponent<MailBoxActions> ().UpdateMails ();
				_FatherPanel = Home;
				_PanelNow = MailBox;
				UpdateLocation ("MailBox");
			} else {
				_tipManager.ShowBuildingConstruct ("MailBox");
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
				UpdateLocation ("Altar");
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

	void UpdateLocation(string location){
		locationText.text = location;
	}

	void CheckThiefActivities(){
		Debug.Log ("Check Thief Activities");
		if (GameData._playerData.dayNow <= GameConfigs.StartThiefEvent)
			return;
		if (GameData._playerData.lastThiefTime >= GameData._playerData.dayNow - 3)
			return;
		if (Algorithms.GetIndexByRange (0, 100) >= (int)(GameData._playerData.ThiefDefence*100f))
			return;

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

		if (LoadTxt.MonsterDic [_thisThief.monsterId].level <= GetGuardAlert ()) {
			CatchThief (_thisThief);
		} else {
			BeStolen (_thisThief);
		}
	}

	int GetGuardAlert(){
		int alert = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			if (GameData._playerData.Pets [key].state == 2) {
				alert = GameData._playerData.Pets [key].alertness;
			}
		}
		return (int)(alert / 3f);
	}

	void CatchThief(Thief t){
		Mails m = new Mails ();
		m.addresser="Marks";
		m.subject="Notice";
		m.mainText = t.name+"was trying to invade my house but was caught by you guard.";

		Dictionary<int,int> drop = Algorithms.GetReward (LoadTxt.MonsterDic [t.monsterId].drop);
		foreach (int key in drop.Keys) {
			m.attachmentId = key;
			m.attachmentNum = drop [key];
			break;
		}
		m.isRead = 0;
		m.type = 0;
		GameData._playerData.Mails.Add (GameData._playerData.Mails.Count, m);
		_gameData.StoreData ("Mails", _gameData.GetStrFromMails (GameData._playerData.Mails));
	}

	void BeStolen(Thief t){

		Dictionary<int,int> target = new Dictionary<int, int> ();

		int totalValue = 0;
		foreach (int key in GameData._playerData.wh.Keys) {
			totalValue += LoadTxt.MatDic [(int)(key / 10000)].price * GameData._playerData.wh [key];
		}

		int max = (int)(10000f / GameData._playerData.wh.Count);
		int num = 0;
		int value = 0;
		int i = 1;
		foreach (int key in GameData._playerData.wh.Keys) {
			int r = Algorithms.GetIndexByRange (0, 10000);
			if (r < i * max && r > ((i - 1) * max)) {
				num = (int)(Algorithms.GetIndexByRange (0, 30) / 100 * GameData._playerData.wh [key]);
				target.Add (key, num);
				value += LoadTxt.MatDic [(int)(key / 10000)].price * GameData._playerData.wh [key];
			}
			if (value >= totalValue * 0.05f)
				break;
		}
		_gameData.DeleteItemInWh (target);
		string s = "";
		foreach (int key in target.Keys) {
			s+= LoadTxt.MatDic[(int)(key/10000)].name+" ×"+target[key]+",";
		}
		s = s.Substring (0, s.Length - 1) + ".";

		Mails m = new Mails ();
		m.addresser = "Marks";
		m.subject = "Warning";
		m.mainText = t.name + " invaded my house and stole " + s;
		m.attachmentId = 0;
		m.attachmentNum = 0;
		m.isRead = 0;
		m.type = 1;
		GameData._playerData.Mails.Add (GameData._playerData.Mails.Count, m);
		_gameData.StoreData ("Mails", _gameData.GetStrFromMails (GameData._playerData.Mails));
	}

}

