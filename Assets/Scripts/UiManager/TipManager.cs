using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TipManager : MonoBehaviour {

	public GameObject headTipPanel;
	public GameObject commonTipPanel;
	public GameObject buildTip;
	public GameObject MakingTip;
	public GameObject TechTip;

	public StudyActions _studyActions;

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

	private Vector3 hpPos;
	private Vector3 foodPos;
	private Vector3 strengthPos;
	private Vector3 spiritPos;
	private Vector3 waterPos;
	private Vector3 tempPos;
	private Vector3 dayPos;
	private Vector3 timePos;

	//tipPanelEnlargeTimePeriod
	private float tipPanelEnlarge = 0.3f;

	// Use this for initialization
	void Start () {
		tipText = headTipPanel.GetComponentsInChildren<Text> ();
		_gameData = this.gameObject.GetComponent<GameData> ();
		_homeManager = this.gameObject.GetComponentInChildren<HomeManager> ();
		_warehouseActions = this.gameObject.GetComponentInChildren<WarehouseActions> ();
		_floating = this.gameObject.GetComponentInChildren<FloatingActions> ();
		_backpackActions = this.gameObject.GetComponentInChildren<BackpackActions> ();

		hpPos = new Vector3(-232,702,0);
		foodPos = new Vector3(-232,635,0);
		strengthPos = new Vector3(-232,574,0);
		spiritPos = new Vector3(103,702,0);
		waterPos = new Vector3(103,635,0);
		tempPos = new Vector3(103,574,0);
		dayPos =new Vector3(-295,783,0);
		timePos =new Vector3(-295,783,0);

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
	/// Shows head tip.
	/// </summary>
	/// <param name="p">P.</param>
	/// <param name="left">If set to <c>true</c> left.</param>
	/// <param name="tipHead">Tip head.</param>
	/// <param name="tipContent">Tip content.</param>
	public void ShowTip(Vector3 p,bool left,string tipHead,string tipContent){
		bool isStartCo = false;
		if (!headTipPanel.gameObject.activeSelf) {
			headTipPanel.gameObject.SetActive (true);
			isStartCo = true;
		}
		headTipPanel.transform.localPosition = p;
		tipText [0].text = tipHead;
		tipText [1].text = tipContent;
		if (isStartCo) {
			StartCoroutine (WaitAndDisappearTips ());
			isStartCo = false;
		}
	}
		
	IEnumerator WaitAndDisappearTips(){
		yield return new WaitForSeconds (10.0f);
		headTipPanel.gameObject.SetActive (false);
	}

	/// <summary>
	/// Shows HeadTips.
	/// </summary>
	/// <param name="tipName">Tip name.</param>
	public void ShowTip(string tipName){
		switch (tipName) {
		case "Hp":
			ShowTip (hpPos, true, "Hp","Be careful with the monsters!");
			break;
		case "Food":
			ShowTip (foodPos, true, "Food","Never Starve!");
			break;
		case "Strength":
			ShowTip (strengthPos, false, "Strength","Ability to cut,dig,and fish.");
			break;
		case "Spirit":
			ShowTip (spiritPos, true, "Spirit","Affect your accuracy and magic weapon.");
			break;
		case "Water":
			ShowTip (waterPos, true, "Water","Vital for all forms of life.");
			break;
		case "Temp":
			ShowTip (tempPos, false, "Temperature","Keep your body temp. from "+GameData._playerData.property[11]+"℃ to "+GameData._playerData.property[12]+"℃");
			break;
		case "Day":
			int season = GameData._playerData.seasonNow;
			string s = "";
			switch (season) {
			case 0:
				s = "It's Spring now.\nEnjoy your adventure!";
				break;
			case 1:
				s = "It's Summer now.\nKeep cool!";
				break;
			case 2:
				s = "It's Autumn now.\nWinter is coming!";
				break;
			case 3:
				s = "It's Winter now.\nStay warm, stay alive!";
				break;
			default:
				Debug.Log ("wrong season!");
				break;	
			}
			string daysPassed = (GameData._playerData.dayNow - 1) + " days have passed. \n" + s;
			ShowTip (dayPos, true, "Days Passed",daysPassed);
			break;
		case "Time":
			ShowTip (timePos, false, "Time Now","Beware of ghosts at night!");
			break;
		default:
			Debug.Log ("tipName is Wrong with " + tipName);
			break;
		}
	}

	/// <summary>
	/// Shows the building construct (Tip).
	/// </summary>
	/// <param name="buildingName">Building name.</param>
	public void ShowBuildingConstruct(string buildingName){
		ShowBuidTip ();
		ClearBuildTip ();

		buildTipText [0].text = buildingName;
		buildTipButton [0].gameObject.SetActive (true);
		buildTipButton[0].gameObject.GetComponentInChildren<Text>().text = "Cancel";
//		commonTipButton [1].gameObject.SetActive (false);
		buildTipButton [1].gameObject.SetActive (true);
		buildTipButton[1].gameObject.GetComponentInChildren<Text>().text = "Build"; 
		buildTipButton [1].gameObject.name = buildingName;

		switch (buildingName) {
		case "BedRoom":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.BedRoomOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Warehouse":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.WarehouseOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Kitchen":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.KitchenOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Workshop":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.WorkshopOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Well":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.WellOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Study":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.StudyOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Farm":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.FarmOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Pets":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.PetsOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "MailBox":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.MailBoxOpen + 1) {
					SetTipDesc (b);
					buildTipButton [1].interactable = CheckReq (b.combReq);
					break;
				}
			}
			break;
		case "Altar":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == buildingName && b.id == GameData._playerData.AltarOpen + 1) {
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
		buildTipText [i].text = "Time: " + GetTime (b.timeCost * GameData._playerData.ConstructTimeDiscount);
		buildTipText [i].color = Color.white;
	}

	string GetTime(float hFloat){
		int hInt = (int)hFloat;
		if (hFloat - hInt == 0)
			return hInt + "h";
		else {
			return hInt + "h" + (int)(60 * hFloat - 60 * hInt) + "m";
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
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Bedroom is built.", 0);
		} else {
			_floating.CallInFloating ("Bedroom has upgraded to Lv." + GameData._playerData.BedRoomOpen, 0);
		}

		_homeManager.UpdateContent ();
		this.gameObject.GetComponentInChildren<RoomActions> ().UpdateRoomStates ();
	}

	IEnumerator WaitAndBuildWarehouse(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Warehouse is built.", 0);
		} else {
			_floating.CallInFloating ("Warehouse has upgraded to Lv." + GameData._playerData.WarehouseOpen, 0);
		}

		_homeManager.UpdateContent ();
		this.gameObject.GetComponentInChildren<WarehouseActions> ().UpdatePanel ();
	}

	IEnumerator WaitAndBuildKitchen(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Kitchen is built.", 0);
		} else {
			_floating.CallInFloating ("Kitchen has upgraded to Lv." + GameData._playerData.KitchenOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildWorkshop(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Workshop is built.", 0);
		} else {
			_floating.CallInFloating ("Workshop has upgraded to Lv." + GameData._playerData.WorkshopOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildStudy(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Study is built.", 0);
		} else {
			_floating.CallInFloating ("Study has upgraded to Lv." + GameData._playerData.StudyOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildFarm(Building b){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		BuildFarm (b);
	}
	void BuildFarm(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.FarmOpen++;
		_gameData.StoreData ("FarmOpen", GameData._playerData.FarmOpen);

		//farm state starts from 3 to 6
		if (GameData._playerData.FarmOpen == 1) {
			GameData._playerData.Farms [3].open = 1;
			_gameData.StoreData ("FarmOpen", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		} else if (GameData._playerData.FarmOpen <= 4) {
			GameData._playerData.Farms [GameData._playerData.FarmOpen + 2].open = 1;
			_gameData.StoreData ("FarmOpen", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		}

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.FarmOpen == 1) {
			_floating.CallInFloating ("Farm is built.", 0);
		} else {
			_floating.CallInFloating ("Farm has upgraded to Lv." + GameData._playerData.FarmOpen, 0);
		}
		this.gameObject.GetComponentInChildren<FarmActions> ().UpdateFarm ();
		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildPets(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Pets is built.", 0);
		} else {
			_floating.CallInFloating ("Pets has upgraded to Lv." + GameData._playerData.PetsOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildWell(Building b){
		int t = _loadingBar.CallInLoadingBar ();
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
			_floating.CallInFloating ("Well is built.", 0);
		} else {
			_floating.CallInFloating ("Well has upgraded to Lv." + GameData._playerData.WellOpen, 0);
		}

		_homeManager.UpdateContent ();

		GameData._playerData.LastWithdrawWaterTime = GameData._playerData.minutesPassed;
		_gameData.StoreData ("LastWithdrawWaterTime", GameData._playerData.LastWithdrawWaterTime);

	}

	IEnumerator WaitAndBuildMail(Building b){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		BuildMail (b);
	}
	void BuildMail(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.MailBoxOpen++;
		_gameData.StoreData ("MailBoxOpen", GameData._playerData.MailBoxOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.MailBoxOpen == 1) {
			_floating.CallInFloating ("MailBox is built.", 0);
		} else {
			_floating.CallInFloating ("MailBox has upgraded to Lv." + GameData._playerData.MailBoxOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	IEnumerator WaitAndBuildAltar(Building b){
		int t = _loadingBar.CallInLoadingBar ();
		yield return new WaitForSeconds (t);
		BuildAltar (b);
	}
	void BuildAltar(Building b){
		foreach (int key in b.combReq.Keys)
			_gameData.ConsumeItem (key, b.combReq [key]);

		GameData._playerData.AltarOpen++;
		_gameData.StoreData ("AltarOpen", GameData._playerData.AltarOpen);

		_gameData.ChangeTime ((int)(b.timeCost * GameData._playerData.ConstructTimeDiscount * 60));

		if (GameData._playerData.AltarOpen == 1) {
			_floating.CallInFloating ("Altar is built.", 0);
		} else {
			_floating.CallInFloating ("Altar has upgraded to Lv." + GameData._playerData.AltarOpen, 0);
		}

		_homeManager.UpdateContent ();
	}

	public void ConstructBuilding(){
		string s = buildTipButton [1].gameObject.name;
		switch (s) {
		case "BedRoom":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "BedRoom" && b.id == GameData._playerData.BedRoomOpen + 1) {
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
				if (b.name == "Warehouse" && b.id == GameData._playerData.WarehouseOpen + 1) {
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
				if (b.name == "Kitchen" && b.id == GameData._playerData.KitchenOpen + 1) {
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
				if (b.name == "Workshop" && b.id == GameData._playerData.WorkshopOpen + 1) {
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
				if (b.name == "Study" && b.id == GameData._playerData.StudyOpen + 1) {
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
				if (b.name == "Farm" && b.id == GameData._playerData.FarmOpen + 1) {
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
				if (b.name == "Pets" && b.id == GameData._playerData.PetsOpen + 1) {
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
				if (b.name == "Well" && b.id == GameData._playerData.WellOpen + 1) {
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
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "MailBox" && b.id == GameData._playerData.MailBoxOpen + 1) {
					if (!CheckReq (b.combReq))
						break;
					else {
						StartCoroutine (WaitAndBuildMail (b));
						break;
					}
				}
			}
			break;
		case "Altar":
			foreach (Building b in LoadTxt.buildings) {
				if (b.name == "Altar" && b.id == GameData._playerData.AltarOpen + 1) {
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
		makingTipButton[0].gameObject.GetComponentInChildren<Text>().text ="Cancel"; 
		makingTipButton[1].gameObject.GetComponentInChildren<Text>().text ="Make"; 
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
		for (int i = 0; i < makingTipText.Length; i++) {
			makingTipText [i].text = "";
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
		string[] tags = m.tags.Split (',');
		for (int j = 0; j < tags.Length; j++) {
			makingTipText [j + 1].text = tags [j];
		}
		int i = 4;
		if (m.property != null) {
			foreach (int key in m.property.Keys) {
				makingTipText [i].text = PlayerData.GetPropName (key) + " " + (m.property [key] > 0 ? "+" : "") + m.property [key];
				i++;
			}
		} else {
			makingTipText[i].text = "Can not be used directly.";
		}
		i = 8;
		foreach (int key in m.combReq.Keys) {
			if (_gameData.CountInBp (key) < m.combReq [key]) {
				makingTipText [i].text = LoadTxt.MatDic [key].name + " × " + m.combReq [key] + " /" + _gameData.CountInBp (key);
				makingTipText [i].color = Color.red;
			} else {
				makingTipText [i].text = LoadTxt.MatDic [key].name + " × " + m.combReq [key] + " /" + _gameData.CountInBp (key);
				makingTipText [i].color = Color.green;
			}
			i++;
		}
		float discount = (m.makingType == "Kitchen") ? GameData._playerData.CookingTimeDiscount : GameData._playerData.BlackSmithTimeDiscount;
		makingTipText [i].text = GetTime (m.makingTime * discount);
		makingTipText [i].color = Color.white;
	}

	public void OnMakingItem(){
		int targetId = int.Parse (makingTipButton [1].gameObject.name);
		foreach (int key in LoadTxt.MatDic[targetId].combReq.Keys) {
			_gameData.ConsumeItem (key, LoadTxt.MatDic [targetId].combReq [key]);
		}

		bool isKitchen = LoadTxt.MatDic [targetId].makingType == "Kitchen";
		float discount = isKitchen ? GameData._playerData.CookingTimeDiscount : GameData._playerData.BlackSmithTimeDiscount;
		_gameData.ChangeTime ((int)(LoadTxt.MatDic [targetId].makingTime * 60 *discount));
		MoveMakingTipPanel ();

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
			PlayerPrefs.SetInt ("MeleeIdUsed", j++);
		} else if (LoadTxt.MatDic [orgId].type == 4) {
			i = Algorithms.GetIndexByRange (0, 10);
			j = _gameData.rangedIdUsedData;
			PlayerPrefs.SetInt ("RangedIdUsed", j++);
		} 

		return orgId * 10000 + i * 1000 + j;
	}


	public void ShowTechTips(int techId){
		ShowTechTip ();
		ClearTechTipTexts ();
		techTipText [0].text = LoadTxt.TechDic[techId].name;
		techTipButton[0].gameObject.GetComponentInChildren<Text>().text ="Cancel"; 
		techTipButton[1].gameObject.GetComponentInChildren<Text>().text ="Study"; 
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
			Debug.Log ("Wrong techId = " + techId);
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

		Mats m = LoadTxt.MatDic [(int)(itemId / 10000)];
		commonTipText [0].text = m.name + ((type == 4) ? (" ×" + GameData._playerData.AmmoNum) : "");
		string[] tags = m.tags.Split (',');
		for (int j = 0; j < tags.Length; j++) {
			commonTipText [j + 1].text = tags [j];
		}
		int i = 4;
		if (m.property != null) {
			foreach (int key in m.property.Keys) {
				commonTipText [i].text = PlayerData.GetPropName (key) + " " + (m.property [key] > 0 ? "+" : "") + m.property [key];
				i++;
			}
		} else {
			commonTipText[i].text = "Can not be used directly.";
		}
		//Extra....

		commonTipButton [0].gameObject.GetComponentInChildren<Text> ().text = "Drop";
		commonTipButton [2].gameObject.GetComponentInChildren<Text> ().text = "Hotkey1";
		commonTipButton [3].gameObject.GetComponentInChildren<Text> ().text = "Hotkey2";
		commonTipButton [1].gameObject.SetActive (true);
		switch (type) {
		case 0:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "warehouse_warehouse|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "Withdraw";
			commonTipButton [2].gameObject.SetActive (false);
			commonTipButton [3].gameObject.SetActive (false);
			break;
		case 1:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "warehouse_backpack|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "Store";
			commonTipButton [2].gameObject.SetActive (false);
			commonTipButton [3].gameObject.SetActive (false);
			break;
		case 2:
			commonTipButton [0].gameObject.SetActive (true);
			commonTipButton[0].gameObject.name = "backpack_backpack|"+itemId;
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "Use";
			bool canUse = !(m.property == null);
			commonTipButton [1].interactable = canUse;
			commonTipButton [2].gameObject.SetActive (canUse);
			commonTipButton [3].gameObject.SetActive (canUse);
			break;
		case 3:
		case 4:
			commonTipButton [0].gameObject.SetActive (false);
			commonTipButton [1].gameObject.GetComponentInChildren<Text> ().text = "Take Off";
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
		commonTipText[1].text = "Pet";
		commonTipText [4].text = "Alertness: " + m.alertness;
		commonTipText [4].text = "Speed: " + m.speed;
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
		case "Withdraw":
			_gameData.WithdrawItem (itemId);
			_warehouseActions.UpdatePanel ();
			MoveCommonTipPanel ();
			break;
		case "Store":
			_gameData.StoreItem (itemId);
			_warehouseActions.UpdatePanel ();
			MoveCommonTipPanel ();
			break;
		case "Take Off":
			TakeOffEquip (equipType);
			MoveCommonTipPanel ();
			break;
		case "Use":
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
			_floating.CallInFloating("Can not use directly!",1);
			break;
//		case 1:
//			//blueprint&recipe,learn a new blueprint if not learnt yet
//			int i = (int)m.property [99];
//			if (!GameData._playerData.LearnedBlueprints.ContainsKey (i)) {
//				_gameData.LearnBlueprint (i);
//				_gameData.ConsumeItem (m.id, 1);
//				_backpackActions.UpdataPanel ();
//				_floating.CallInFloating ("Learnt how to make " + LoadTxt.MatDic [i].name, 0);
//			} else {
//				_floating.CallInFloating ("Can not learn again!", 1);
//			}
//			break;
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
			break;
		default:
			break;
		}
	}
		
	public void SetHotkey(int index){
		int itemId = int.Parse (commonTipButton [1].gameObject.name);
		_gameData.SetHotkey (index, itemId);
		_floating.CallInFloating ("Hotkey Set", 0);
	}

	public void OnHotkey0(){
		int itemId = GameData._playerData.Hotkey0;
		Debug.Log ("Hotkey0" + itemId);
		UseItem (itemId);
	}

	public void OnHotkey1(){
		int itemId = GameData._playerData.Hotkey1;
		Debug.Log ("Hotkey1" + itemId);
		UseItem (itemId);
	}
}
