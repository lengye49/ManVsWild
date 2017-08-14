using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TipManager : MonoBehaviour {

//	public GameObject headTipPanel;
	public GameObject commonTipPanel;
	public GameObject buildTip;
	public GameObject MakingTip;
	public GameObject TechTip;

	public StudyActions _studyActions;
	public LogManager _logManager;

	private Text[] tipText;

	private Text[] commonTipText;
	private Text[] buildTipText;
	private Text[] makingTipText;
	private Text[] techTipText;

	private Button[] commonTipButton;
	private Button[] buildTipButton;
	private Button[] makingTipButton;
	private Button[] techTipButton;

	private GameData _gameData;
	private HomeManager _homeManager;
	private WarehouseActions _warehouseActions;
	private FloatingActions _floating;
	private BackpackActions _backpackActions;
	public LoadingBar _loadingBar;

//	private Vector3 hpPos;
//	private Vector3 foodPos;
//	private Vector3 strengthPos;
//	private Vector3 spiritPos;
//	private Vector3 waterPos;
//	private Vector3 tempPos;
//	private Vector3 dayPos;
//	private Vector3 timePos;

	//tipPanelEnlargeTimePeriod
	private float tipPanelEnlarge = 0.3f;

	// Use this for initialization
	void Start () {
//		tipText = headTipPanel.GetComponentsInChildren<Text> ();
		_gameData = this.gameObject.GetComponent<GameData> ();
		_homeManager = this.gameObject.GetComponentInChildren<HomeManager> ();
		_warehouseActions = this.gameObject.GetComponentInChildren<WarehouseActions> ();
		_floating = this.gameObject.GetComponentInChildren<FloatingActions> ();
		_backpackActions = this.gameObject.GetComponentInChildren<BackpackActions> ();

		commonTipText = commonTipPanel.gameObject.GetComponentsInChildren<Text> ();
		commonTipButton = commonTipPanel.gameObject.GetComponentsInChildren<Button> ();

		buildTipText = buildTip.gameObject.GetComponentsInChildren<Text> ();
		buildTipButton = buildTip.gameObject.GetComponentsInChildren<Button> ();

		makingTipText = MakingTip.gameObject.GetComponentsInChildren<Text> ();
		makingTipButton = MakingTip.gameObject.GetComponentsInChildren<Button> ();

		techTipText = TechTip.gameObject.GetComponentsInChildren<Text> ();
		techTipButton = TechTip.gameObject.GetComponentsInChildren<Button> ();
	}


	/// <summary>
	/// Shows the building construct (Tip).
	/// </summary>
	/// <param name="buildingName">Building name.</param>
	public void ShowBuildingConstruct(string buildingName){
		ShowBuidTip ();
		ClearBuildTip ();

//		buildTipText [0].text = buildingName;
		buildTipButton [0].gameObject.SetActive (true);
		buildTipButton[0].gameObject.GetComponentInChildren<Text>().text = "取消";
//		commonTipButton [1].gameObject.SetActive (false);
		buildTipButton [1].gameObject.SetActive (true);
		buildTipButton[1].gameObject.GetComponentInChildren<Text>().text = "开工"; 
		buildTipButton [1].gameObject.name = buildingName;

		switch (buildingName) {
		case "BedRoom":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "休息室" && b.id == GameData._playerData.BedRoomOpen + 1) {
					buildTipText [0].text = "休息室";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Warehouse":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "仓库" && b.id == GameData._playerData.WarehouseOpen + 1) {
					buildTipText [0].text = "仓库";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Kitchen":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "厨房" && b.id == GameData._playerData.KitchenOpen + 1) {
					buildTipText [0].text = "厨房";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Workshop":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "工作台" && b.id == GameData._playerData.WorkshopOpen + 1) {
					buildTipText [0].text = "工作台";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Well":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "水井" && b.id == GameData._playerData.WellOpen + 1) {
					buildTipText [0].text= "水井";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Study":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "研究室" && b.id == GameData._playerData.StudyOpen + 1) {
					buildTipText [0].text = "研究室";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Farm":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "农田" && b.id == GameData._playerData.FarmOpen + 1) {
					buildTipText [0].text = "农田";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Pets":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "宠物笼" && b.id == GameData._playerData.PetsOpen + 1) {
					buildTipText [0].text = "宠物笼";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Achievement":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "成就" && b.id == GameData._playerData.AchievementOpen + 1) {
					buildTipText [0].text = "成就";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Altar":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "祭坛" && b.id == GameData._playerData.AltarOpen + 1) {
					buildTipText [0].text = "祭坛";
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		default:
			Debug.Log ("Wrong buildingName!");
			break;
		}
	}

	void SetTipDesc(Building b){
		int i = 2;
		buildTipText [1].text = b.tips;
		foreach (int key in b.combReq.Keys) {
			if (_gameData.CountInBp(key)<b.combReq[key]) {
				buildTipText [i].text = LoadTxt.MatDic[key].name + " × " + b.combReq [key] + " /" + _gameData.CountInBp(key);
				buildTipText [i].color = Color.red;
			} else {
				buildTipText [i].text = LoadTxt.MatDic[key].name + " × " + b.combReq [key] + " /" + _gameData.CountInBp(key);
				buildTipText [i].color = Color.green;
			}
			i++;
		}
		buildTipText [i].text = "耗时: " + GetTime (b.timeCost * GameData._playerData.ConstructTimeDiscount);
		buildTipText [i].color = Color.white;
	}

	string GetTime(float hFloat){
		int hInt = (int)hFloat;
		if (hFloat - hInt == 0)
			return hInt + "时";
		else {
			return hInt + "时" + (int)(60 * hFloat - 60 * hInt) + "分";
		}
	}

	void ShowBuidTip(){
		buildTip.gameObject.SetActive (true);
		buildTip.gameObject.transform.localPosition = Vector3.zero;
		buildTip.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		buildTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), tipPanelEnlarge);
	}

	void ClearBuildTip(){
		for (int i = 0; i < buildTipText.Length; i++) {
			buildTipText [i].text = "";
		}
	}

	IEnumerator WaitAndBuildRoom(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildRoom (b);
	}
	void BuildRoom(Building b){
		foreach (int key in b.combReq.Keys) {
			_gameData.ConsumeItem (key, b.combReq [key]);
		}
		GameData._playerData.BedRoomOpen++;
		_gameData.StoreData ("BedRoomOpen", GameData._playerData.BedRoomOpen);
		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.BedRoomOpen == 1) {
			_logManager.AddLog ("休息室已建造完毕。");
		} else {
			_logManager.AddLog ("休息室已升级到等级"+ GameData._playerData.BedRoomOpen);
		}

		_homeManager.UpdateContent ();
		this.gameObject.GetComponentInChildren<RoomActions> ().UpdateRoomStates ();
	}

	IEnumerator WaitAndBuildWarehouse(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildWarehouse (b);
	}
	void BuildWarehouse(Building b){
		foreach (int key in b.combReq.Keys) {
			_gameData.ConsumeItem (key, b.combReq [key]);
		}

		GameData._playerData.WarehouseOpen++;
		_gameData.StoreData ("WarehouseOpen", GameData._playerData.WarehouseOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.WarehouseOpen == 1) {
			_logManager.AddLog ("仓库已建造完毕。");

			//第一次获得进贡
			_gameData.GetFirstTribute ();
		} else {
			_logManager.AddLog ("仓库已升级到等级"+ GameData._playerData.WarehouseOpen);
		}

		_homeManager.UpdateContent ();
		this.gameObject.GetComponentInChildren<WarehouseActions> ().UpdatePanel ();
	}

	IEnumerator WaitAndBuildKitchen(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildKitchen (b);
	}
	void BuildKitchen(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.KitchenOpen++;
		_gameData.StoreData ("KitchenOpen", GameData._playerData.KitchenOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.KitchenOpen == 1) {
			_logManager.AddLog ("厨房已建造完毕。");
			_logManager.AddLog (1f, "建造完[休息室]后，你可以点击页面右下角的[地图]到四周转转。");
		} else {
			_logManager.AddLog ("厨房已升级到等级"+ GameData._playerData.KitchenOpen);
		}

		_homeManager.UpdateContent ();
		this.gameObject.GetComponentInChildren<MakingActions> ().UpdatePanel ("Kitchen");
	}

	IEnumerator WaitAndBuildWorkshop(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildWorkshop (b);
	}
	void BuildWorkshop(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.WorkshopOpen++;
		_gameData.StoreData ("WorkshopOpen", GameData._playerData.WorkshopOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.WorkshopOpen == 1) {
			_logManager.AddLog ("工作台已建造完毕。");
		} else {
			_logManager.AddLog ("工作台已升级到等级"+ GameData._playerData.WorkshopOpen);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildStudy(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildStudy (b);
	}
	void BuildStudy(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.StudyOpen++;
		_gameData.StoreData ("StudyOpen", GameData._playerData.StudyOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.StudyOpen == 1) {
			_logManager.AddLog ("研究室已建造完毕。");
		} else {
			_logManager.AddLog ("研究室已升级到等级"+ GameData._playerData.StudyOpen);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildFarm(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildFarm (b);
	}
	void BuildFarm(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.FarmOpen++;
		_gameData.StoreData ("FarmOpen", GameData._playerData.FarmOpen);

		//田地的序号从3开始
		if (GameData._playerData.FarmOpen == 1) {
			GameData._playerData.Farms [0].open = 1;
			GameData._playerData.Farms [1].open = 1;
			GameData._playerData.Farms [2].open = 1;
			GameData._playerData.Farms [3].open = 1;
		} else {
			GameData._playerData.Farms [GameData._playerData.FarmOpen + 2].open = 1;
		}
		_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.FarmOpen == 1) {
			_logManager.AddLog ("农田已准备妥当。");
		} else {
			_logManager.AddLog ("农田已升级到等级"+ GameData._playerData.FarmOpen);
		}
		this.gameObject.GetComponentInChildren<FarmActions> ().UpdateFarm ();
		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildPets(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildPets (b);
	}
	void BuildPets(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.PetsOpen++;
		_gameData.StoreData ("PetsOpen", GameData._playerData.PetsOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.PetsOpen == 1) {
			_logManager.AddLog ("宠物笼已建造完毕。");
		} else {
			_logManager.AddLog ("宠物笼已升级到等级"+ GameData._playerData.PetsOpen);
		}
		this.gameObject.GetComponentInChildren<PetsActions> ().UpdatePets ();
		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildWell(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildWell (b);
	}
	void BuildWell(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.WellOpen++;
		_gameData.StoreData ("WellOpen", GameData._playerData.WellOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.WellOpen == 1) {
			_logManager.AddLog ("水井已建造完毕。");
		} else {
			_logManager.AddLog ("水井已升级到等级"+ GameData._playerData.WellOpen);
		}

		_homeManager.UpdateContent ();

		GameData._playerData.LastWithdrawWaterTime = GameData._playerData.minutesPassed;
		_gameData.StoreData ("LastWithdrawWaterTime", GameData._playerData.LastWithdrawWaterTime);

	}

	IEnumerator WaitAndBuildAltar(Building b){
		int t = _loadingBar.CallInLoadingBar (b.timeCost * 60);
		yield return new WaitForSeconds (t);
		BuildAltar (b);
	}
	void BuildAltar(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.AltarOpen++;
		_gameData.StoreData ("AltarOpen", GameData._playerData.AltarOpen);

		GameData._playerData.AchievementOpen++;
		_gameData.StoreData ("AchievementOpen", GameData._playerData.AchievementOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.AltarOpen == 1) {
			_logManager.AddLog ("祭坛已建造完毕，成就系统已开启。");
		} else {
			_logManager.AddLog ("祭坛已升级到等级"+ GameData._playerData.AltarOpen);
		}





		_homeManager.UpdateContent ();
	}

	public void ConstructBuilding(){
		string s = buildTipButton [1].gameObject.name;
		switch (s) {
		case "BedRoom":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "休息室" && b.id == GameData._playerData.BedRoomOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildRoom (b));
						break;
					}
				}
			}
			break;
		case "Warehouse":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "仓库" && b.id == GameData._playerData.WarehouseOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildWarehouse (b));
						break;
					}
				}
			}
			break;
		case "Kitchen":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "厨房" && b.id == GameData._playerData.KitchenOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildKitchen (b));
						break;
					}
				}
			}
			break;
		case "Workshop":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "工作台" && b.id == GameData._playerData.WorkshopOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildWorkshop (b));
						break;
					}
				}
			}
			break;
		case "Study":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "研究室" && b.id == GameData._playerData.StudyOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildStudy (b));
						break;
					}
				}
			}
			break;
		case "Farm":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "农田" && b.id == GameData._playerData.FarmOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildFarm (b));
						break;
					}
				}
			}
			break;
		case "Pets":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "宠物笼" && b.id == GameData._playerData.PetsOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildPets (b));
						break;
					}
				}
			}
			break;
		case "Well":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "水井" && b.id == GameData._playerData.WellOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildWell (b));
						break;
					}
				}
			}
			break;
		case "MailBox":
			break;
		case "Altar":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "祭坛" && b.id == GameData._playerData.AltarOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildAltar (b));
						break;
					}
				}
			}
			break;
		default:
			break;
		}

		MoveBuildTip ();
		this.gameObject.GetComponent<AchieveActions> ().ConstructBulding ();
	}

	public void OnBuildTipCover(){
		MoveBuildTip ();
	}
	void MoveBuildTip(){
		buildTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (-1f, -1f, 0f), tipPanelEnlarge);
		buildTip.gameObject.SetActive (false);
	}





	/// <summary>
	/// Shows item tips in making list.
	/// </summary>
	/// <param name="matName">Mat name.</param>
	public void ShowMakingTips(int matId){
		ShowMakingTip ();
		ClearMakingTipTexts ();
		makingTipText [0].text = LoadTxt.MatDic[matId].name;
		makingTipText [0].color = GameConfigs.MatColor [LoadTxt.MatDic [matId].quality];
		makingTipButton[0].gameObject.GetComponentInChildren<Text>().text ="取消"; 
		makingTipButton[1].gameObject.GetComponentInChildren<Text>().text ="制作"; 
		makingTipButton [1].interactable = CheckReq (LoadTxt.MatDic[matId].combReq);
		makingTipButton [1].gameObject.name = matId.ToString ();
		SetMakingTipDesc (LoadTxt.MatDic [matId]);
	}

	void ShowMakingTip(){
		if (MakingTip.gameObject.activeSelf && MakingTip.gameObject.transform.localPosition == Vector3.zero)
			return;
		MakingTip.gameObject.SetActive (true);
		MakingTip.gameObject.transform.localPosition = new Vector3(200,0,0);
		MakingTip.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		MakingTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), tipPanelEnlarge);
	}

	void ClearMakingTipTexts(){
		for (int i = 2; i < makingTipText.Length; i++) {
			makingTipText [i].text = "";
			makingTipText [i].color = Color.white;
		}
	}

	public void OnMakingTipCover(){
		MoveMakingTipPanel ();
	}

	void MoveMakingTipPanel(){
		MakingTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (-1f, -1f, 0f), tipPanelEnlarge);
		MakingTip.gameObject.SetActive (false);
	}

	void SetMakingTipDesc(Mats m){
		int i = 2;
		if (m.property != null) {
			foreach (int key in m.property.Keys) {
				makingTipText [i].text = PlayerData.GetPropName (key) + " " + (m.property [key] > 0 ? "+" : "") + m.property [key];
				i++;
				makingTipText [i].alignment = TextAnchor.MiddleCenter;
			}
		} else {
			makingTipText [i].text = "无法直接使用。";
			makingTipText [i].alignment = TextAnchor.MiddleCenter;
			i++;
		}
		makingTipText [i].text = "原料:";
		makingTipText [i].color = new Color (24f / 255f, 193f / 255f, 172f / 255f, 1f);
		makingTipText [i].alignment = TextAnchor.MiddleLeft;
		i++;
		foreach (int key in m.combReq.Keys) {
			if (_gameData.CountInBp (key) < m.combReq [key]) {
				makingTipText [i].text = LoadTxt.MatDic [key].name + " × " + m.combReq [key] + " /" + _gameData.CountInBp (key);
				makingTipText [i].color = Color.red;
				makingTipText [i].alignment = TextAnchor.MiddleCenter;
			} else {
				makingTipText [i].text = LoadTxt.MatDic [key].name + " × " + m.combReq [key] + " /" + _gameData.CountInBp (key);
				makingTipText [i].color = Color.green;
				makingTipText [i].alignment = TextAnchor.MiddleCenter;
			}
			i++;
		}
		float discount = (m.makingType == "Kitchen") ? GameData._playerData.CookingTimeDiscount : GameData._playerData.BlackSmithTimeDiscount;
		makingTipText [i].text = "耗时: "+GetTime (m.makingTime * discount);
		makingTipText [i].color = new Color (24f / 255f, 193f / 255f, 172f / 255f, 1f);
		makingTipText [i].alignment = TextAnchor.MiddleLeft;
	}



	public void OnMakingItem(){
		StartCoroutine (StartMakeLoading ());
	}

	IEnumerator StartMakeLoading(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		MakeItem();
	}

	void MakeItem(){
		int targetId = int.Parse (makingTipButton [1].gameObject.name);
		foreach (int key in LoadTxt.MatDic[targetId].combReq.Keys) {
			_gameData.ConsumeItem (key, LoadTxt.MatDic [targetId].combReq [key]);
		}

		bool isKitchen = LoadTxt.MatDic [targetId].makingType == "Kitchen";
		float discount = isKitchen ? GameData._playerData.CookingTimeDiscount : GameData._playerData.BlackSmithTimeDiscount;
		_gameData.ChangeTime ((int)(LoadTxt.MatDic [targetId].makingTime * 60 *discount));
//		MoveMakingTipPanel ();

		_floating.CallInFloating (LoadTxt.MatDic [targetId].name + " +" + LoadTxt.MatDic[targetId].combGet, 0);
		int combGet = LoadTxt.MatDic [targetId].combGet;
		if (isKitchen)
			combGet = (int)(LoadTxt.MatDic [targetId].combGet * Algorithms.GetIndexByRange (100, (int)(100 * GameData._playerData.CookingIncreaseRate) + 1) / 100);
		_gameData.AddItem (GenerateItemId (targetId), combGet);

		//Achievement
		if (isKitchen)	
			this.gameObject.GetComponent<AchieveActions> ().CookFood (targetId);
		switch (LoadTxt.MatDic [targetId].type) {
		case 3:
			this.gameObject.GetComponent<AchieveActions> ().CollectMeleeWeapon (targetId);
			break;
		case 4:
			this.gameObject.GetComponent<AchieveActions> ().CollectRangedWeapon (targetId);
			break;
		case 5:
			this.gameObject.GetComponent<AchieveActions> ().CollectMagicWeapon (targetId);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Generates an ItemId for New Item.
	/// </summary>
	int GenerateItemId(int orgId){
		int i = 0;
		int j = 0;
		if (LoadTxt.MatDic [orgId].type == 3) {
			i = Algorithms.GetIndexByRange (0, 10);
			j = _gameData.meleeIdUsedData;
			_gameData.meleeIdUsedData++;
			PlayerPrefs.SetInt ("MeleeIdUsed", _gameData.meleeIdUsedData);

		} else if (LoadTxt.MatDic [orgId].type == 4) {
			i = Algorithms.GetIndexByRange (0, 10);
			j = _gameData.rangedIdUsedData;
			_gameData.rangedIdUsedData++;
			PlayerPrefs.SetInt ("RangedIdUsed", _gameData.rangedIdUsedData);
		} 
		int newId = orgId * 10000 + i * 1000 + j;
		Debug.Log ("new itemId = " +newId);
		return newId;
	}


	public void ShowTechTips(int techId){
		ShowTechTip ();
		ClearTechTipTexts ();
		techTipText [0].text = LoadTxt.TechDic[techId].name;
		techTipButton[0].gameObject.GetComponentInChildren<Text>().text ="取消"; 
		techTipButton[1].gameObject.GetComponentInChildren<Text>().text ="研究"; 
		techTipButton [1].interactable = CheckReq (LoadTxt.TechDic[techId].req);
		techTipButton [1].gameObject.name = techId.ToString ();
		SetTechTipDesc (LoadTxt.TechDic [techId]);
	}

	void SetTechTipDesc(Technique t){
		techTipText [1].text = t.desc;
		int i = 2;
		foreach (int key in t.req.Keys) {
			if (_gameData.CountInBp (key) < t.req [key]) {
				techTipText [i].text = LoadTxt.MatDic [key].name + " × " + t.req [key] + " /" + _gameData.CountInBp (key);
				techTipText [i].color = Color.red;
			} else {
				techTipText [i].text = LoadTxt.MatDic [key].name + " × " + t.req [key] + " /" + _gameData.CountInBp (key);
				techTipText [i].color = Color.green;
			}
			i++;
		}
	}

	void ShowTechTip(){
		if (TechTip.gameObject.activeSelf && TechTip.gameObject.transform.localPosition == Vector3.zero)
			return;
		TechTip.gameObject.SetActive (true);
		TechTip.gameObject.transform.localPosition = new Vector3(200,0,0);
		TechTip.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		TechTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), tipPanelEnlarge);
	}

	void ClearTechTipTexts(){
		for (int i = 0; i < techTipText.Length; i++) {
			techTipText [i].text = "";
		}
	}

	public void OnTechTipCover(){
		MoveTechTipPanel ();
	}

	void MoveTechTipPanel(){
		TechTip.gameObject.transform.DOBlendableScaleBy (new Vector3 (-1f, -1f, 0f), tipPanelEnlarge);
		TechTip.gameObject.SetActive (false);
	}

	public void OnStudyTech(){
		StartCoroutine (StartStudyLoading ());
	}

	IEnumerator StartStudyLoading(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		StudyTech();
	}

	void StudyTech(){
		int techId = int.Parse (techTipButton [1].gameObject.name);
		foreach (int key in LoadTxt.TechDic[techId].req.Keys) {
			_gameData.ConsumeItem (key, LoadTxt.TechDic [techId].req [key]);
		}
		MoveTechTipPanel ();
		_floating.CallInFloating (LoadTxt.TechDic [techId].name + " Completed!", 0);
		LearntTech (techId);
		_studyActions.UpdateStudy();

		//Achievement
		this.gameObject.GetComponent<AchieveActions>().TechUpgrade();
	}

	void LearntTech(int techId){
		Technique t = LoadTxt.TechDic [techId];
		GameData._playerData.techLevels [t.type] = t.lv;
		switch (t.type) {
		case 1:
			GameData._playerData.bpNum = t.lv * GameConfigs.IncBpNum + GameConfigs.BasicBpNum;
			break;
		case 2:
			GameData._playerData.ConstructTimeDiscount = GameConfigs.ConstructionDiscount [t.lv];
			break;
		case 3:
			GameData._playerData.TheftLossDiscount = GameConfigs.TheftDiscount [t.lv];
			break;
		case 4:
			GameData._playerData.HarvestIncrease = GameConfigs.HarvestIncrease [t.lv];
			break;
		case 5:
			GameData._playerData.OenologyIncrease = GameConfigs.OenologyIncrease [t.lv];
			break;
		case 6:
			_gameData.UpdateProperty ();
			break;
		case 7:
			_gameData.UpdateProperty ();
			break;
		case 8:
			GameData._playerData.MagicPower = GameConfigs.WitchcraftPower [t.lv];
			break;
		case 9:
			
			break;
		case 10:
			GameData._playerData.MagicCostRate = GameConfigs.WitchcraftCostRate [t.lv];
			break;
		case 11:
			GameData._playerData.CaptureRate = GameConfigs.CaptureRate [t.lv];
			break;
		case 12:
			GameData._playerData.SpotRate = GameConfigs.SpotRate [t.lv];
			break;
		case 13:
			GameData._playerData.SearchRate = GameConfigs.SearchRate [t.lv];
			break;
		case 14:
			
			break;
		case 15:
			GameData._playerData.BlackSmithTimeDiscount = GameConfigs.BlackSmithTimeDiscount [t.lv];
			break;
		case 16:
			GameData._playerData.CookingTimeDiscount = GameConfigs.CookingTimeDiscount [t.lv];
			GameData._playerData.CookingIncreaseRate = GameConfigs.CookingIncreaseRate [t.lv];
			break;
		case 17:
			GameData._playerData.WaterCollectingRate = GameConfigs.WaterCollectingRate [t.lv];
			break;
		default:
			Debug.Log ("错误的科技ID = " + techId);
			break;
		}
	}

	/// <summary>
	/// Shows item tips.
	/// </summary>
	/// <param name="itemId">type:0 warehouseCell;1 bpCell in warehouse;2 bpCell in Backpack;3 character Cell;4 character Ammo</param>
	public void ShowNormalTips(int itemId,int type){
		ShowTipPanel ();
		ClearCommonTipTexts ();

		int orgId = (int)(itemId / 10000);
		int ex = (int)((itemId % 10000) / 1000);
		Mats m = LoadTxt.MatDic [orgId];

		string itemName = m.name;
		if (itemId % 10000 != 0) {
			if (LoadTxt.MatDic [orgId].type == 3) {
				itemName += "[" +LoadTxt.ExtraMelee [ex].name +"]";
			}
			if (LoadTxt.MatDic [orgId].type == 4) {
				itemName += "[" + LoadTxt.ExtraRanged [ex].name +"]";
			}
		}

		commonTipText [0].text = itemName + ((type == 4) ? (" ×" + GameData._playerData.AmmoNum) : "");
		commonTipText [0].color = GameConfigs.MatColor [m.quality];
		string[] tags = m.tags.Split (',');
		for (int j = 0; j < tags.Length; j++) {
			commonTipText [j + 1].text = tags [j];
		}
		int i = 4;
		if (m.property != null) {
			foreach (int key in m.property.Keys) {
				commonTipText [i].text = PlayerData.GetPropName (key) + " " + (m.property [key] > 0 ? "+" : "-") + m.property [key];
				commonTipText [i].color = Color.white;
				i++;
			}
		} else {
			commonTipText[i].text = "无法直接使用。";
			commonTipText [i].color = Color.white;
			i++;
		}

		//附加属性
		string de="";
		float p;
		if (itemId % 10000 != 0) {
			if (LoadTxt.MatDic [orgId].type == 3) {
				foreach (int key in LoadTxt.ExtraMelee[ex].property.Keys) {
					de = PlayerData.GetPropName (key)+" ";
					p = LoadTxt.ExtraMelee [ex].property [key];
					de += (p > 0 ? "+" : "-");
					if (key < 25)
						de += p.ToString ();
					else
						de += ((int)(p * 100f)).ToString () + "%";
					commonTipText [i].text = de;

					if(p>0)
						commonTipText [i].color = Color.green;
					else
						commonTipText [i].color = Color.red;
					i++;
				}
			}
			if (LoadTxt.MatDic [orgId].type == 4) {
				foreach (int key in LoadTxt.ExtraRanged[ex].property.Keys) {
					de = PlayerData.GetPropName (key)+" ";
					p = LoadTxt.ExtraRanged [ex].property [key];
					de += (p > 0 ? "+" : "-");
					if (key < 25)
						de += p.ToString ();
					else
						de += ((int)(p * 100f)).ToString () + "%";
					commonTipText [i].text = de;

					if(p>0)
						commonTipText [i].color = Color.green;
					else
						commonTipText [i].color = Color.red;
					i++;
				}
			}
		}

		commonTipButton [0].gameObject.GetComponentInChildren<Text> ().text = "丢弃";
		commonTipButton [2].gameObject.GetComponentInChildren<Text> ().text = "快捷键1";
		commonTipButton [3].gameObject.GetComponentInChildren<Text> ().text = "快捷键2";
		commonTipButton [1].gameObject.SetActive (true);
		switch (type) {
		case 0:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "warehouse_warehouse|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "取出";
			commonTipButton [2].gameObject.SetActive (false);
			commonTipButton [3].gameObject.SetActive (false);
			break;
		case 1:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "warehouse_backpack|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "存放";
			commonTipButton [2].gameObject.SetActive (false);
			commonTipButton [3].gameObject.SetActive (false);
			break;
		case 2:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "backpack_backpack|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "使用";
			bool canUse = !(m.property == null);
			commonTipButton [1].interactable = canUse;
			commonTipButton [2].gameObject.SetActive (m.type==2);
			commonTipButton [3].gameObject.SetActive (m.type==2);
			break;
		case 3:
		case 4:
			commonTipButton [0].gameObject.SetActive (false);
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "卸下";
			commonTipButton [2].gameObject.SetActive (false);
			commonTipButton [3].gameObject.SetActive (false);
			break;
		default:
			break;
		}
		commonTipButton [1].gameObject.name = itemId.ToString ();
	}



	public void ShowMountTips(Pet m){
		ShowTipPanel ();
		ClearCommonTipTexts ();
		commonTipText [0].text = m.name;
		commonTipText[1].text = "宠物";
		commonTipText [4].text = "警戒值: " + m.alertness;
		commonTipText [4].text = "速度: " + m.speed;
		commonTipButton [0].gameObject.SetActive (false);
		commonTipButton [1].gameObject.SetActive (false);
		commonTipButton [2].gameObject.SetActive (false);
		commonTipButton [3].gameObject.SetActive (false);
	}
		
	void ShowTipPanel(){
		if (commonTipPanel.gameObject.activeSelf && commonTipPanel.gameObject.transform.localPosition == Vector3.zero)
			return;
		commonTipPanel.gameObject.SetActive (true);
		commonTipPanel.gameObject.transform.localPosition = Vector3.zero;
		commonTipPanel.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		commonTipPanel.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), tipPanelEnlarge);
	}

	public void OnNormalTipCover(){
		if (commonTipPanel.gameObject.transform.localPosition.x > 10 || commonTipPanel.gameObject.transform.localPosition.x < -10)
			return;
		MoveCommonTipPanel ();
	}

	void MoveCommonTipPanel(){
		commonTipPanel.gameObject.transform.DOBlendableScaleBy (new Vector3 (-1f, -1f, 0f), tipPanelEnlarge);
		commonTipPanel.gameObject.SetActive (false);
	}
		
	bool CheckReq(Dictionary<int,int> dic){
		foreach (int key in dic.Keys) {
			if (_gameData.CountInBp (key) < dic [key])
				return false;
		}
		return true;
	}

	void ClearCommonTipTexts(){
		for (int i = 0; i < commonTipText.Length; i++) {
			commonTipText [i].text = "";
		}
	}
		
	public void DropItem(){
		string[] s = commonTipButton [0].gameObject.name.Split('|');
		string place = s [0];
		int itemId = int.Parse (s [1]);
		if (place == "warehouse_warehouse") {
			_gameData.DeleteItemInWh (itemId);
			_warehouseActions.UpdatePanel ();
		} else if (place == "warehouse_backpack") {
			_gameData.DeleteItemInBp (itemId);
			_warehouseActions.UpdatePanel ();
		} else if (place == "backpack_backpack") {
			_gameData.DeleteItemInBp (itemId);
			_backpackActions.UpdataPanel ();
		}
		OnNormalTipCover ();
	}

	public void OnActionButton1(){
		string actionType = commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text;
		int itemId = int.Parse (commonTipButton [1].gameObject.name);
		Debug.Log (itemId);
		int equipType = LoadTxt.MatDic [(int)(itemId / 10000)].type;
		switch (actionType) {
		case "取出":
			_gameData.WithdrawItem (itemId);
			_warehouseActions.UpdatePanel ();
			MoveCommonTipPanel ();
			break;
		case "存放":
			_gameData.StoreItem (itemId);
			_warehouseActions.UpdatePanel ();
			MoveCommonTipPanel ();
			break;
		case "卸下":
			TakeOffEquip (equipType);
			MoveCommonTipPanel ();
			break;
		case "使用":
			UseItem (itemId);
			break;
		default:
			Debug.Log ("Wrong actionType + " + actionType);
			break;
		}
	}

	void TakeOffEquip(int equipType){
		_gameData.TakeOffEquip (equipType);
		_backpackActions.UpdataPanel ();
	}
		
	void UseItem(int itemId){
		int id = (int)(itemId / 10000);
		Mats m = LoadTxt.MatDic [id];

		switch (m.type) {
		case 0:
			_floating.CallInFloating("该物品无法直接使用!",1);
			break;
		case 2:
			_gameData.EatFood (itemId);
			_backpackActions.UpdataPanel ();
			break;
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
			_gameData.ChangeEquip (itemId);
			OnNormalTipCover ();
			_backpackActions.UpdataPanel ();
			break;
		default:
			break;
		}
	}
		
	public void SetHotkey(int index){
		int itemId = int.Parse (commonTipButton [1].gameObject.name);
		_gameData.SetHotkey (index, itemId);
		_floating.CallInFloating ("成功设置快捷键！", 0);
	}

	public void OnHotkey0(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		int itemId = GameData._playerData.Hotkey0;
//		Debug.Log ("Hotkey0" + itemId);
		UseItem (itemId);
	}

	public void OnHotkey1(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		int itemId = GameData._playerData.Hotkey1;
//		Debug.Log ("Hotkey1" + itemId);
		UseItem (itemId);
	}
}
