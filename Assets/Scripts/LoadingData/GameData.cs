//Load and Store Player Data
//Application.LoadLevel(Application.loadedLevel); // 重新调用当前场景
using UnityEngine;
using UnityEngine.UI;//测试用
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public static PlayerData _playerData;
	private HeadUiManager _headUiManager;
	private LoadTxt _loadTxt;

	public int meleeIdUsedData;
	public int rangedIdUsedData;

	void Awake () {

		/*测试代码，删除所有数据
		PlayerPrefs.DeleteAll ();
		*******************/

		_playerData = new PlayerData ();
		LoadAchievements ();
		LoadAllData (false);
		LoadStaticData ();
		_headUiManager = GameObject.Find ("HeadUI").GetComponent<HeadUiManager> ();
		_loadTxt = this.gameObject.GetComponent<LoadTxt> ();
		_headUiManager.UpdateHeadUI ();
		_headUiManager.UpdateHotkeys ();
		_loadTxt.LoadPlaces (false);
		_loadTxt.LoadShop (false);
	}

	/// <summary>
	/// Data that will not be changed when dying or restart.
	/// </summary>
	void LoadStaticData(){
		meleeIdUsedData = PlayerPrefs.GetInt ("MeleeIdUsed", 1);
		rangedIdUsedData= PlayerPrefs.GetInt ("RangedIdUsed", 1);
	}

	void LoadAchievements(){
		_playerData.Achievements = GetTechList(PlayerPrefs.GetString("Achievements","1|0;2|0;3|0;4|0;5|0;6|0;7|0;8|0;9|0;10|0;11|0;12|0;13|0;14|0;15|0;16|0;17|0;18|0;19|0;20|0;21|0;22|0;23|0;24|0;25|0;26|0;27|0;28|0;29|0;30|0;31|0;32|0;33|0;34|0;35|0;36|0;37|0;38|0;39|0;40|0"));
	}

	int[] GetIntFromStr(string str){
		int[] i;
		if (str == "")
			return new int[0];
		else {
			string[] s = str.Split ('|');
			i = new int[s.Length];
			for (int j = 0; j < i.Length; j++)
				i [j] = int.Parse (s [j]);
		}
		return i;
	}

	public string GetStrFromInt(int[] i){
		string s = "";
		for(int j=0;j<i.Length;j++)
			s+=i[j]+"|";
		s = s.Substring (0, s.Length - 1);
		return s;
	}

	/// <summary>
	/// Load Player Data From Local File.
	/// </summary>
	void LoadAllData(bool isMemory){
		string s = isMemory ? "_Memory" : "";

		_playerData.techLevels = GetTechList (PlayerPrefs.GetString ("TechList", "1|0;2|0;3|0;4|0;5|0;6|0;7|0;8|0;9|0;10|0;11|0;12|0;13|0;14|0;15|0;16|0;17|0"));

		_playerData.hpNow = PlayerPrefs.GetInt ("hpNow" + s, 100);
		_playerData.spiritNow = PlayerPrefs.GetInt ("spiritNow" + s, 100);
		_playerData.foodNow = PlayerPrefs.GetInt ("foodNow" + s, 100);
		_playerData.waterNow = PlayerPrefs.GetInt ("waterNow" + s, 100);
		_playerData.strengthNow = PlayerPrefs.GetInt ("strengthNow" + s, 100);
		_playerData.tempNow = PlayerPrefs.GetInt ("tempNow" + s, 20);

		_playerData.HpMax = PlayerPrefs.GetInt ("HpMax" + s, 100);
		_playerData.SpiritMax = PlayerPrefs.GetInt ("SpiritMax" + s, 100);
		_playerData.FoodMax = PlayerPrefs.GetInt ("FoodMax" + s, 100);
		_playerData.WaterMax = PlayerPrefs.GetInt ("Watermax" + s, 100);
		_playerData.StrengthMax = PlayerPrefs.GetInt ("StrengthMax" + s, 100);
		_playerData.TempMax = PlayerPrefs.GetInt ("TempMax" + s, 60);
		_playerData.TempMin = PlayerPrefs.GetInt ("TempMin" + s, -60);

		_playerData.minutesPassed = PlayerPrefs.GetInt ("minutesPassed" + s, 0);

		_playerData.BedRoomOpen = PlayerPrefs.GetInt ("BedRoomOpen" + s, 1);
		_playerData.WarehouseOpen = PlayerPrefs.GetInt ("WarehouseOpen" + s, 1);
		_playerData.KitchenOpen = PlayerPrefs.GetInt ("KitchenOpen" + s, 1);
		_playerData.WorkshopOpen = PlayerPrefs.GetInt ("WorkshopOpen" + s, 1);
		_playerData.StudyOpen = PlayerPrefs.GetInt ("StudyOpen" + s, 1);
		_playerData.FarmOpen = PlayerPrefs.GetInt ("FarmOpen" + s, 0);
		_playerData.PetsOpen = PlayerPrefs.GetInt ("PetsOpen" + s, 1);
		_playerData.WellOpen = PlayerPrefs.GetInt ("WellOpen" + s, 0);
		_playerData.MailBoxOpen = PlayerPrefs.GetInt ("MailBoxOpen" + s, 0);
		_playerData.AltarOpen = PlayerPrefs.GetInt ("AltarOpen" + s, 0);

		_playerData.bp = GetDicFormStr (PlayerPrefs.GetString ("bp" + s, "34020000|100"));
		_playerData.wh = GetDicFormStr (PlayerPrefs.GetString ("wh" + s, ""));

		_playerData.HasMemmory = PlayerPrefs.GetInt ("HasMemmory" + s, 0);

		_playerData.LearnedBlueprints = GetDicFormStr (PlayerPrefs.GetString ("LearnedBlueprints" + s, ""));

		_playerData.MeleeId = PlayerPrefs.GetInt ("MeleeId" + s, 1000000);
		_playerData.RangedId = PlayerPrefs.GetInt ("RangedId" + s, 0);
		_playerData.MagicId = PlayerPrefs.GetInt ("MagicId" + s, 0);
		_playerData.HeadId = PlayerPrefs.GetInt ("HeadId" + s, 0);
		_playerData.BodyId = PlayerPrefs.GetInt ("BodyId" + s, 0);
		_playerData.ShoeId = PlayerPrefs.GetInt ("ShoeId" + s, 0);
		_playerData.AccessoryId = PlayerPrefs.GetInt ("AccessoryId" + s, 0);
		_playerData.AmmoId = PlayerPrefs.GetInt ("AmmoId" + s, 0);
		_playerData.AmmoNum = PlayerPrefs.GetInt ("AmmoNum" + s, 0);
		_playerData.Mount = GetMount (PlayerPrefs.GetString ("Mount" + s, ""));

		_playerData.Hotkey0 = PlayerPrefs.GetInt ("Hotkey0" + s, 0);
		_playerData.Hotkey1 = PlayerPrefs.GetInt ("Hotkey1" + s, 0);

		_playerData.LastWithdrawWaterTime = PlayerPrefs.GetInt ("LastWithdrawWaterTime" + s, 0);

		_playerData.SoulStone = PlayerPrefs.GetInt ("SoulStone" + s, 0);
		_playerData.Gold = PlayerPrefs.GetInt ("Gold" + s, 0);

		_playerData.Farms = GetFarmStateFromStr (PlayerPrefs.GetString ("Farms" + s, "0|0|1|0;1|0|2|0;2|0|3|0;3|0|0|0;4|0|0|0;5|0|0|0;6|0|0|0;7|0|0|0"));

		_playerData.Mails = GetMailsFromStr (PlayerPrefs.GetString ("Mails" + s, "")); //"0|Hi,Glad to meet you!|Li Shujuan|I Love You|1100|10|0"));

		_playerData.Pets = GetPetListFromStr (PlayerPrefs.GetString ("Pets" + s, ""));//"100|1|50|15|Hello;100|0|20|10|Kitty"));
		_playerData.PetRecord = PlayerPrefs.GetInt("PetRecord"+s,0);

		_playerData.MapOpenState = GetMapOpenStateFromStr (PlayerPrefs.GetString ("MapOpenState" + s, "1|1|1|1|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0"));
		_playerData.mapNow = PlayerPrefs.GetInt ("mapNow" + s, 0);
		_playerData.dungeonLevelMax = PlayerPrefs.GetInt ("DungeonLevelMax" + s, 0);

		//科技只需要读取，不需要存储
		_playerData.bpNum = _playerData.techLevels [1] * GameConfigs.IncBpNum + GameConfigs.BasicBpNum; 
		_playerData.ConstructTimeDiscount = GameConfigs.ConstructionDiscount [_playerData.techLevels [2]];
		_playerData.TheftLossDiscount = GameConfigs.TheftDiscount [_playerData.techLevels [3]];
		_playerData.HarvestIncrease = GameConfigs.HarvestIncrease [_playerData.techLevels [4]];
		_playerData.OenologyIncrease = GameConfigs.OenologyIncrease [_playerData.techLevels [5]];
		_playerData.MagicPower = GameConfigs.WitchcraftPower [_playerData.techLevels [8]];
		_playerData.MagicCostRate = GameConfigs.WitchcraftCostRate [_playerData.techLevels [10]];
		_playerData.CaptureRate = GameConfigs.CaptureRate [_playerData.techLevels [11]];
		_playerData.SpotRate = GameConfigs.SpotRate [_playerData.techLevels [12]];
		_playerData.SearchRate = GameConfigs.SearchRate [_playerData.techLevels [13]];
		_playerData.BlackSmithTimeDiscount = GameConfigs.BlackSmithTimeDiscount [_playerData.techLevels [15]];
		_playerData.CookingTimeDiscount = GameConfigs.CookingTimeDiscount [_playerData.techLevels [16]];
		_playerData.CookingIncreaseRate = GameConfigs.CookingIncreaseRate [_playerData.techLevels [16]];
		_playerData.WaterCollectingRate = GameConfigs.WaterCollectingRate [_playerData.techLevels [17]];
		_playerData.GhostComingProp = GameConfigs.GhostComingProp [0];
		_playerData.ThiefDefence = 0.3f;
		//科技结束

		//成就读取
		_playerData.thiefCaught = PlayerPrefs.GetInt ("ThiefCaught", 0);
		_playerData.ghostKill = PlayerPrefs.GetInt ("GhostKill", 0);
		_playerData.ghostBossKill = PlayerPrefs.GetInt ("GhostBossKill", 0);
		_playerData.ghostKingKill = PlayerPrefs.GetInt ("GhostKingKill", 0);
		_playerData.wineTasted = GetIntFromStr (PlayerPrefs.GetString ("WineTasted", ""));
		_playerData.foodCooked = GetIntFromStr (PlayerPrefs.GetString ("FoodCooked", ""));
		_playerData.meleeCollected = GetIntFromStr (PlayerPrefs.GetString ("MeleeCollected", ""));
		_playerData.rangedCollected = GetIntFromStr (PlayerPrefs.GetString ("RangedCollected", ""));
		_playerData.magicCollected = GetIntFromStr (PlayerPrefs.GetString ("MagicCollected", ""));
		_playerData.monsterKilled = PlayerPrefs.GetInt ("MonsterKilled", 0);
		_playerData.sleepTime = PlayerPrefs.GetInt ("SleepTime", 0);
		_playerData.dragonKilled = PlayerPrefs.GetInt ("DragonKilled", 0);
		_playerData.legendThiefCaught = PlayerPrefs.GetInt ("LegendThiefCaught", 0);
		_playerData.meleeAttackCount = PlayerPrefs.GetInt ("MeleeAttackCount", 0);
		_playerData.rangedAttackCount = PlayerPrefs.GetInt ("RangedAttackCount", 0);
		_playerData.magicAttackCount = PlayerPrefs.GetInt ("MagicAttackCount", 0);
		//成就结束

		_playerData.lastThiefTime = PlayerPrefs.GetInt ("LastThiefTime", 0);

		_playerData.goldConsume = PlayerPrefs.GetInt ("GoldConsume", 0);
		_playerData.demonPoint = PlayerPrefs.GetInt ("DemonPoint", 0);
		_playerData.renown = PlayerPrefs.GetInt ("Renown", 0);
		_playerData.petsCaptured = PlayerPrefs.GetInt ("PetsCaptured", 0);
		_playerData.wineDrinked = PlayerPrefs.GetInt ("WineDrinked", 0);

		_playerData.placeNowId = PlayerPrefs.GetInt ("PlaceNowId", 0);

		if (isMemory) {
			_loadTxt.LoadPlaces (isMemory);
			_loadTxt.LoadShop (isMemory);
		}
			
		UpdateProperty ();

	}
		
	void StoreData(bool isRebirth)
	{
		string s = isRebirth ? "" : "_Memory";

		PlayerPrefs.SetString ("TechList" + s, GetStrFromTechList (_playerData.techLevels));

		PlayerPrefs.SetInt ("hpNow" + s, _playerData.hpNow);
		PlayerPrefs.SetInt ("spiritNow" + s, _playerData.spiritNow);
		PlayerPrefs.SetInt ("foodNow" + s, _playerData.foodNow);
		PlayerPrefs.SetInt ("waterNow" + s, _playerData.waterNow);
		PlayerPrefs.SetInt ("strengthNow" + s, _playerData.strengthNow);
		PlayerPrefs.SetInt ("tempNow" + s, _playerData.tempNow);
		PlayerPrefs.SetInt ("HpMax" + s, _playerData.HpMax);
		PlayerPrefs.SetInt ("SpiritMax" + s, _playerData.SpiritMax);
		PlayerPrefs.SetInt ("Watermax" + s, _playerData.WaterMax);
		PlayerPrefs.SetInt ("FoodMax" + s, _playerData.FoodMax);
		PlayerPrefs.SetInt ("StrengthMax" + s, _playerData.StrengthMax);
		PlayerPrefs.SetInt ("TempMin" + s, _playerData.TempMin);
		PlayerPrefs.SetInt ("TempMax" + s, _playerData.TempMax);

		PlayerPrefs.SetInt ("minutesPassed" + s, _playerData.minutesPassed);

		PlayerPrefs.SetInt ("BedRoomOpen" + s, _playerData.BedRoomOpen);
		PlayerPrefs.SetInt ("WarehouseOpen" + s, _playerData.WarehouseOpen);
		PlayerPrefs.SetInt ("KitchenOpen" + s, _playerData.KitchenOpen);
		PlayerPrefs.SetInt ("WorkshopOpen" + s, _playerData.WorkshopOpen);
		PlayerPrefs.SetInt ("StudyOpen" + s, _playerData.StudyOpen);
		PlayerPrefs.SetInt ("FarmOpen" + s, _playerData.FarmOpen);
		PlayerPrefs.SetInt ("PetsOpen" + s, _playerData.PetsOpen);
		PlayerPrefs.SetInt ("WellOpen" + s, _playerData.WellOpen);
		PlayerPrefs.SetInt ("MailBoxOpen" + s, _playerData.MailBoxOpen);
		PlayerPrefs.SetInt ("AltarOpen" + s, _playerData.AltarOpen);

		PlayerPrefs.SetString ("bp" + s, GetStrFromDic (_playerData.bp));
		PlayerPrefs.SetString ("wh" + s, GetStrFromDic (_playerData.wh));

		PlayerPrefs.SetInt ("HasMemmory" + s, _playerData.HasMemmory);

		PlayerPrefs.SetString ("LearnedBlueprints" + s, GetStrFromDic (_playerData.LearnedBlueprints));

		PlayerPrefs.SetInt ("MeleeId" + s, _playerData.MeleeId);
		PlayerPrefs.SetInt ("RangedId" + s, _playerData.RangedId);
		PlayerPrefs.SetInt ("MagicId" + s, _playerData.MagicId);
		PlayerPrefs.SetInt ("HeadId" + s, _playerData.HeadId);
		PlayerPrefs.SetInt ("BodyId" + s, _playerData.BodyId);
		PlayerPrefs.SetInt ("ShoeId" + s, _playerData.ShoeId);
		PlayerPrefs.SetInt ("AccessoryId" + s, _playerData.AccessoryId);
		PlayerPrefs.SetInt ("AmmoId" + s, _playerData.AmmoId);
		PlayerPrefs.SetInt ("AmmoNum" + s, _playerData.AmmoNum);
		PlayerPrefs.SetString ("Mount" + s, GetStrFromMount (_playerData.Mount));

		PlayerPrefs.SetInt ("Hotkey0" + s, _playerData.Hotkey0);
		PlayerPrefs.SetInt ("Hotkey1" + s, _playerData.Hotkey1);

		PlayerPrefs.SetInt ("LastWithdrawWaterTime" + s, _playerData.LastWithdrawWaterTime);

		PlayerPrefs.SetInt ("SoulStone" + s, _playerData.SoulStone);
		PlayerPrefs.SetInt ("Gold" + s, _playerData.Gold);

		PlayerPrefs.SetString ("Farms" + s, GetStrFromFarmState (_playerData.Farms));

		PlayerPrefs.SetString ("Mails" + s, GetStrFromMails (_playerData.Mails));

		PlayerPrefs.SetString ("Pets" + s, GetstrFromPets (_playerData.Pets));
		PlayerPrefs.SetInt ("PetRecord" + s, _playerData.PetRecord);

		PlayerPrefs.SetString ("MapOpenState" + s, GetStrFromMapOpenState (_playerData.MapOpenState));
		PlayerPrefs.SetInt ("mapNow" + s, _playerData.mapNow);
		PlayerPrefs.SetInt ("DungeonLevelMax" + s, _playerData.dungeonLevelMax);

		PlayerPrefs.SetInt ("ThiefCaught" + s, _playerData.thiefCaught);
		PlayerPrefs.SetInt ("GhostKill" + s, _playerData.ghostKill);
		PlayerPrefs.SetInt ("GhostBossKill" + s, _playerData.ghostBossKill);
		PlayerPrefs.SetInt ("GhostKingKill" + s, _playerData.ghostKingKill);
		PlayerPrefs.SetString ("WineTasted" + s, GetStrFromInt (_playerData.wineTasted));
		PlayerPrefs.SetString ("FoodCooked" + s, GetStrFromInt (_playerData.foodCooked));
		PlayerPrefs.SetString ("MeleeCollected" + s, GetStrFromInt (_playerData.meleeCollected));
		PlayerPrefs.SetString ("RangedCollected" + s, GetStrFromInt (_playerData.rangedCollected));
		PlayerPrefs.SetString ("MagicCollected" + s, GetStrFromInt (_playerData.magicCollected));
		PlayerPrefs.SetInt ("MonsterKilled" + s, _playerData.monsterKilled);
		PlayerPrefs.SetInt ("SleepTime" + s, _playerData.sleepTime);
		PlayerPrefs.SetInt ("DragonKilled" + s, _playerData.dragonKilled);
		PlayerPrefs.SetInt ("LegendThiefCaught" + s, _playerData.legendThiefCaught);
		PlayerPrefs.SetInt ("MeleeAttackCount" + s, _playerData.meleeAttackCount);
		PlayerPrefs.SetInt ("RangedAttackCount" + s, _playerData.rangedAttackCount);
		PlayerPrefs.SetInt ("MagicAttackCount" + s, _playerData.magicAttackCount);

		PlayerPrefs.SetInt ("LastThiefTime" + s, _playerData.lastThiefTime);
		PlayerPrefs.SetInt ("GoldConsume" + s, _playerData.goldConsume);
		PlayerPrefs.SetInt ("DemonPoint" + s, _playerData.demonPoint);
		PlayerPrefs.SetInt ("Renown" + s, _playerData.renown);
		PlayerPrefs.SetInt ("PetsCaptured" + s, _playerData.petsCaptured);
		PlayerPrefs.SetInt ("WineDrinked" + s, _playerData.wineDrinked);

		PlayerPrefs.SetInt ("PlaceNowId" + s, _playerData.placeNowId);

		_loadTxt.StorePlaceMemmory (isRebirth);
		_loadTxt.StoreShopMemmory (isRebirth);
	}

	public void StoreMemmory(){
		_playerData.HasMemmory = 1;
		StoreData ("HasMemmory", _playerData.HasMemmory);
		StoreData (false);
	}

	public void RebirthLoad(){
		//游戏内加载存档
		LoadAllData (true);
		StoreData (true);
	}

	public void ReStartLoad(){
		PlayerPrefs.DeleteAll ();
		LoadAllData (false);
		_loadTxt.LoadPlaces (false);
		_loadTxt.LoadShop (false);
		StoreData (true);
	}

	/// <summary>
	/// Change Time by Minutes
	/// </summary>
	/// <param name="minutes">Minutes.</param>
	public void ChangeTime(int minutes){
		int lastHour = _playerData.hourNow;

		_playerData.minutesPassed += minutes;
		StoreData ("minutesPassed", _playerData.minutesPassed);
		_headUiManager.UpdateHeadUI ("dateNow");
		_headUiManager.UpdateHeadUI ("timeNow");

		if (_playerData.hourNow > lastHour) {

			_playerData.foodNow -= (int)Mathf.Max (0, GameConfigs.FoodCostPerHour * (_playerData.hourNow - lastHour));
			StoreData ("foodNow", _playerData.foodNow);
			UpdateProperty (4, _playerData.foodNow);
			_headUiManager.UpdateHeadUI ("foodNow");

			_playerData.waterNow -= (int)Mathf.Max (0, GameConfigs.WaterCostPerHour * (_playerData.hourNow - lastHour));
			StoreData ("waterNow", _playerData.waterNow);
			UpdateProperty (6, _playerData.waterNow);
			_headUiManager.UpdateHeadUI ("waterNow");

			if (_playerData.foodNow <= 0)
				Debug.Log ("Death of Food");
			if (_playerData.waterNow <= 0)
				Debug.Log ("Death of Water");
		}

		//Achievement
		this.gameObject.GetComponent<AchieveActions>().TimeChange();
	}

	/// <summary>
	/// 食物、卧室等可以用来恢复的属性.
	/// </summary>
	/// <param name="propName">Property name.</param>
	/// <param name="value">Value.</param>
	public void ChangeProperty(int propId, int value){
		switch (propId) {
		case 0:
			_playerData.hpNow = (int)((_playerData.hpNow + value) > _playerData.property[1] ? _playerData.property[1]: (_playerData.hpNow + value));
			StoreData ("hpNow", _playerData.hpNow);
			UpdateProperty (0, _playerData.hpNow);
			_headUiManager.UpdateHeadUI ("hpNow");
			if (_playerData.hpNow <= 0)
				Debug.Log ("Die of Hp");
			break;
		case 1:
			_playerData.HpMax += value;
			StoreData ("HpMax", _playerData.HpMax);
			UpdateProperty (1, _playerData.HpMax);
			_headUiManager.UpdateHeadUI ("hpMax");
			break;
		case 2:
			_playerData.spiritNow = (int)((_playerData.spiritNow + value) >_playerData.property[3]? _playerData.property[3] : (_playerData.spiritNow + value));
			StoreData ("spiritNow", _playerData.spiritNow);
			_headUiManager.UpdateHeadUI ("spiritNow");
			UpdateProperty (2, _playerData.spiritNow);
			if (_playerData.spiritNow <= 0)
				Debug.Log ("Die of Spirit");
			break;
		case 3:
			_playerData.SpiritMax += value;
			StoreData ("SpiritMax", _playerData.SpiritMax);
			UpdateProperty (3, _playerData.SpiritMax);
			_headUiManager.UpdateHeadUI ("SpiritMax");
			break;
		case 4:
			_playerData.foodNow = (int)((_playerData.foodNow + value) > _playerData.property[5] ? _playerData.property[5] : (_playerData.foodNow + value));
			StoreData ("foodNow", _playerData.foodNow);
			_headUiManager.UpdateHeadUI ("foodNow");
			UpdateProperty (4, _playerData.foodNow);
			if (_playerData.foodNow <= 0)
				Debug.Log ("Die of Food");
			break;
		case 5:
			_playerData.FoodMax += value;
			StoreData ("FoodMax", _playerData.FoodMax);
			UpdateProperty (5, _playerData.FoodMax);
			_headUiManager.UpdateHeadUI ("FoodMax");
			break;
		case 6:
			_playerData.waterNow = (int)((_playerData.waterNow + value) > _playerData.property[7] ? _playerData.property[7]: (_playerData.waterNow + value));
			StoreData ("waterNow", _playerData.waterNow);
			_headUiManager.UpdateHeadUI ("waterNow");
			UpdateProperty (6, _playerData.waterNow);
			if (_playerData.waterNow <= 0)
				Debug.Log ("Die of Water");
			break;
		case 7:
			_playerData.WaterMax += value;
			StoreData ("WaterMax", _playerData.WaterMax);
			UpdateProperty (7, _playerData.WaterMax);
			_headUiManager.UpdateHeadUI ("WaterMax");
			break;
		case 8:
			_playerData.strengthNow = (int)((_playerData.strengthNow + value) > _playerData.property[9]?_playerData.property[9] : (_playerData.strengthNow + value));
			StoreData ("strengthNow", _playerData.strengthNow);
			_headUiManager.UpdateHeadUI ("strengthNow");
			UpdateProperty (8, _playerData.strengthNow);
			break;
		case 9:
			_playerData.StrengthMax += value;
			StoreData ("StrengthMax", _playerData.StrengthMax);
			UpdateProperty (9, _playerData.StrengthMax);
			_headUiManager.UpdateHeadUI ("StrengthMax");
			break;
		case 10:
			_playerData.tempNow += value;
			StoreData ("tempNow", _playerData.tempNow);
			_headUiManager.UpdateHeadUI ("tempNow");
			UpdateProperty (10, _playerData.tempNow);
			if (_playerData.tempNow > _playerData.property[12])
				Debug.Log ("Die of Hot");
			if (_playerData.tempNow < _playerData.property[11])
				Debug.Log ("Die of Cold");
			break;
		case 11:
			_playerData.TempMin += value;
			StoreData ("TempMin", _playerData.TempMin);
			UpdateProperty (11, _playerData.TempMin);
			break;
		case 12:
			_playerData.TempMax += value;
			StoreData ("TempMax", _playerData.TempMax);
			UpdateProperty (12, _playerData.TempMax);
			break;
		default:
			Debug.Log ("Wrong propName");
			break;
		}
	}


	/// <summary>
	/// Add item.
	/// </summary>
	/// <param name="itemId">Item identifier*10000.</param>
	/// <param name="num">Number.</param>
	public void AddItem(int itemId,int num){
		if (_playerData.bp.ContainsKey (itemId))
			_playerData.bp [itemId] += num;
		else
			_playerData.bp.Add (itemId, num);
		//要判断背包是否满了
		StoreData ("bp", GetStrFromDic (_playerData.bp));

		//Achievement
		this.gameObject.GetComponent<AchieveActions> ().GoldGet ();
	}

	/// <summary>
	/// Consume item.
	/// </summary>
	/// <param name="Id">SHORT Item identifier.</param>
	/// <param name="num">Number.</param>
	public void ConsumeItem(int Id,int num){
		int i = num;
		foreach (int key in _playerData.bp.Keys) {
			
			if ((int)(key / 10000) != Id)
				continue;
			
			if (_playerData.bp [key] > i) {
				_playerData.bp [key] -= i;
				break;
			} else if (_playerData.bp [key] == i) {
				_playerData.bp.Remove (key);
				break;
			} else {
				i -= _playerData.bp [key];
				_playerData.bp.Remove (key);
			}
		}
		StoreData ("bp", GetStrFromDic (_playerData.bp));

		if ((int)(_playerData.Hotkey0 / 10000) == Id || (int)(_playerData.Hotkey1 / 10000) == Id)
			CheckHotKeyState ();

		//Achievement
		if (Id == 3100)
			this.gameObject.GetComponent<AchieveActions> ().GoldConsume (num);
	}



	public void DeleteItemInBp(int itemId){
		_playerData.bp.Remove (itemId);
		StoreData ("bp", GetStrFromDic (_playerData.bp));

		if (_playerData.Hotkey0 == itemId || _playerData.Hotkey1 == itemId)
			CheckHotKeyState ();
	}
		

	public void DeleteItemInWh(int itemId){
		_playerData.wh.Remove (itemId);
		StoreData ("wh", GetStrFromDic (_playerData.wh));
	}

	public void DeleteItemInWh(Dictionary<int,int> d){
		foreach (int key in d.Keys) {
			if (_playerData.wh [key] == d [key])
				_playerData.wh.Remove (key);
			else
				_playerData.wh [key] -= d [key];
		}
		StoreData ("wh", GetStrFromDic (_playerData.wh));
	}

	/// <summary>
	/// Store item to warehouse.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="num">Number.</param>
	public void StoreItem(int itemId){
		if (_playerData.wh.ContainsKey (itemId))
			_playerData.wh [itemId] += _playerData.bp[itemId];
		else
			_playerData.wh.Add (itemId, _playerData.bp[itemId]);
		DeleteItemInBp (itemId);
		StoreData ("wh", GetStrFromDic (_playerData.wh));
	}

	/// <summary>
	/// Withdraw item from warehouse.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="num">Number.</param>
	public void WithdrawItem(int itemId){
		Debug.Log (itemId + "|" + _playerData.wh.ContainsKey (itemId));
		AddItem (itemId, _playerData.wh [itemId]);
		_playerData.wh.Remove (itemId);
		StoreData ("wh", GetStrFromDic (_playerData.wh));
	}

	public void StoreData(string dataName,int data){
		PlayerPrefs.SetInt (dataName, data);
	}
	public void StoreData(string dataName,float data){
		PlayerPrefs.SetFloat (dataName, data);
	}
	public void StoreData(string dataName,string data){
		PlayerPrefs.SetString (dataName, data);
	}
		
	/// <summary>
	/// Counts the in bp.
	/// </summary>
	/// <returns>The in bp.</returns>
	/// <param name="itemId">Item identifier,4 size.</param>
	public int CountInBp(int itemId){
		int i = 0;
		foreach (int key in _playerData.bp.Keys) {
			if ((int)(key / 10000) == itemId)
				i += _playerData.bp [key];
		}
		return i;
	}


	Dictionary<int,int> GetDicFormStr(string originStr)
	{
		Dictionary<int,int> dic = new Dictionary<int, int> ();
		string[] strs = originStr.Split(';');
		for (int i = 0; i < strs.Length; i++) {
			if (strs [i].Contains ("|")) {
				string[] s = strs [i].Split ('|');
				dic.Add (int.Parse (s [0]), int.Parse (s [1]));
			}
		}
		return dic;
	}

	string GetStrFromDic(Dictionary<int,int> dic){
		if (dic.Count <= 0)
			return "";
		string s = "";
		foreach (int key in dic.Keys) {
			s += key.ToString () + "|" + dic [key] + ";";
		}
		if (s != "")
			s = s.Substring (0, s.Length - 1);
		return s;
	}

	public Dictionary<int,FarmState> GetFarmStateFromStr(string str){
		Dictionary<int,FarmState> d = new Dictionary<int, FarmState> ();
		string[] strs = str.Split (';');
		for (int i = 0; i < strs.Length; i++) {
			if (strs [i].Contains ("|")) {
				string[] s = strs [i].Split ('|');
				int j = int.Parse(s [0]);
				FarmState f = new FarmState ();
				f.open = int.Parse(s [1]);
				f.plantType = int.Parse(s [2]);
				f.plantTime = int.Parse(s [3]);
				d.Add (j, f);
			}
		}
		return d;
	}

	public string GetStrFromFarmState(Dictionary<int,FarmState> f){
		if (f.Count <= 0)
			return "";
		string s = "";
		foreach (int key in f.Keys) {
			s += key + "|" + f [key].open + "|" + f [key].plantType + "|" + f [key].plantTime + ";";
		}
		if (s != "")
			s = s.Substring (0, s.Length - 1);
		return s;
	}

	public Pet GetMount(string str){
		Pet p = new Pet ();
		if (str.Contains ("|")) {
			string[] ss = str.Split ('|');
			p.monsterId = int.Parse (ss [0]);
			p.state = int.Parse (ss [1]);
			p.alertness = int.Parse (ss [2]);
			p.speed = int.Parse (ss [3]);
			p.name = ss [4];
		}
		return p;
	}

	public string GetStrFromMount(Pet p){
		string s = "";
		if (p.monsterId > 0)
			s += p.monsterId + "|" + p.state + "|" + p.alertness + "|" + p.speed + "|" + p.name;
		return s;
	}

	public Dictionary<int,Pet> GetPetListFromStr(string str){
		Dictionary<int,Pet> ps = new Dictionary<int, Pet> ();
		string[] s = str.Split(';');
		for (int i = 0; i < s.Length; i++) {
			if (s [i].Contains ("|")) {
				string[] ss = s [i].Split ('|');
				Pet p = new Pet ();
				p.monsterId = int.Parse (ss [0]);
				p.state = int.Parse (ss [1]);
				p.alertness = int.Parse (ss [2]);
				p.speed = int.Parse (ss [3]);
				p.name = ss [4];
				ps.Add (i, p);
			}
		}
		return ps;
	}

	public string GetstrFromPets(Dictionary<int,Pet> p){
		if (p.Count <= 0)
			return "";
		string s = "";
		foreach (int key in p.Keys) {
			s += p [key].monsterId + "|" + p [key].state + "|" + p [key].alertness + "|" + p [key].speed + "|" + p [key].name + ";";
		}
		s = s.Substring (0, s.Length - 1);
		return s;
	}

	Dictionary<int,int> GetTechList(string str){
		Dictionary<int,int> l = new Dictionary<int, int> ();
		string[] s = str.Split (';');
		for (int i = 0; i < s.Length; i++) {
			if (s [i].Contains ("|")) {
				string[] ss = s [i].Split ('|');
				l.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			}
		}
		return l;
	}

	public string GetStrFromTechList(Dictionary<int,int> l){
		if (l.Count <= 0)
			return "";
		string s = "";
		foreach (int key in l.Keys) {
			s += key + "|" + l [key] + ";";
		}
		s = s.Substring (0, s.Length - 1);
		return s;
	}

	public Dictionary<int,Mails> GetMailsFromStr(string str){
		Dictionary<int,Mails> d = new Dictionary<int,Mails> ();
		string[] strs = str.Split (';');
		int j = 0;
		for (int i = 0; i < strs.Length; i++) {
			if (strs [i].Contains ("|")) {
				string[] s = strs [i].Split ('|');
				Mails m = new Mails ();
				m.type = int.Parse (s [0]);
				m.subject = s [1];
				m.addresser = s [2];
				m.mainText = s [3];
				m.attachmentId = int.Parse (s [4]);
				m.attachmentNum = int.Parse (s [5]);
				m.isRead = int.Parse (s [6]);
				d.Add (j, m);
				j++;
			}
		}
		return d;
	}

	public string GetStrFromMails(Dictionary<int,Mails> m){
		if (m.Count <= 0)
			return "";
		string s = "";
		foreach (int key in m.Keys) {
			s += m [key].type + "|"+ m [key].subject + "|"+ m [key].addresser + "|"+ m [key].mainText + "|"+ m [key].attachmentId + "|"+ m [key].attachmentNum + "|"+ m [key].isRead + ";";
		}
		if (s != "")
			s = s.Substring (0, s.Length - 1);
		return s;
	}
		
	public Dictionary<int,int> GetMapOpenStateFromStr(string str){
		Dictionary<int,int> d = new Dictionary<int, int> ();
		string[] s = str.Split ('|');
		for (int i = 0; i < s.Length; i++) {
			d.Add (i, int.Parse (s [i]));
		}
		return d;
	}

	public string GetStrFromMapOpenState(Dictionary<int,int> d){
		if (d.Count <= 0)
			return "";
		string s = "";
		for (int i = 0; i < d.Count; i++) {
			s += d [i].ToString () + "|";
		}
		if (s != "")
			s = s.Substring (0, s.Length - 1);
		return s;
	}

	/// <summary>
	/// Updates the properties.
	/// </summary>
	public void UpdateProperty(){
		_playerData.property = new float[GameConfigs.BasicProperty.Length];
		for (int i = 0; i < _playerData.property.Length; i++) {
			_playerData.property[i] += GameConfigs.BasicProperty [i];
		}
		UpdateStoreProperty ();
		UpdateEquipProperty (GameData._playerData.MeleeId);
		UpdateEquipProperty (GameData._playerData.RangedId);
		UpdateEquipProperty (GameData._playerData.MagicId);
		UpdateEquipProperty (GameData._playerData.HeadId);
		UpdateEquipProperty (GameData._playerData.BodyId);
		UpdateEquipProperty (GameData._playerData.ShoeId);
		UpdateEquipProperty (GameData._playerData.AccessoryId);
		UpdateEquipProperty (GameData._playerData.AmmoId);
		UpdateMountProperty ();
		UpdateTechniqueProperty ();
	}

	void UpdateProperty(int propId,int newValue){
		_playerData.property [propId] = newValue;
	}

	/// <summary>
	/// Gets the 6_now property.
	/// </summary>
	/// <returns>The store property.</returns>
	/// <param name="p">P.</param>
	void UpdateStoreProperty(){
		_playerData.property [0] = _playerData.hpNow;
		_playerData.property [1] = _playerData.HpMax;
		_playerData.property [2] = _playerData.spiritNow;
		_playerData.property [3] = _playerData.SpiritMax;
		_playerData.property [4] = _playerData.foodNow;
		_playerData.property [5] = _playerData.FoodMax;
		_playerData.property [6] = _playerData.waterNow;
		_playerData.property [7] = _playerData.WaterMax;
		_playerData.property [8] = _playerData.strengthNow;
		_playerData.property [9] = _playerData.StrengthMax;
		_playerData.property [10] = _playerData.tempNow;
		_playerData.property [11] = _playerData.TempMin;
		_playerData.property [12] = _playerData.TempMax;
	}

	void UpdateEquipProperty(int id){
		if (id == 0)
			return;
		foreach (int key in LoadTxt.MatDic[(int)(id/10000)].property.Keys) {
			_playerData.property [key] += LoadTxt.MatDic [(int)(id / 10000)].property [key];
		}
		int extra = (int)((id % 10000) / 1000);
		if (LoadTxt.MatDic [(int)(id / 10000)].type == 3) {
			Extra_Weapon ew = LoadTxt.ExtraMelee [extra];
			foreach (int key in ew.property.Keys) {
				_playerData.property [key] += ew.property [key];
			}
		}
		if (LoadTxt.MatDic [(int)(id / 10000)].type != 4) {
			Extra_Weapon ew = LoadTxt.ExtraRanged [extra];
			foreach (int key in ew.property.Keys) {
				_playerData.property [key] += ew.property [key];
			}
		}
	}

	void UpdateMountProperty(){
		if (GameData._playerData.Mount.monsterId > 0)
			_playerData.property [23] = GameData._playerData.Mount.speed;
	}

	void UpdateTechniqueProperty(){
		_playerData.property [14] += GameConfigs.ArcheryRangedDamage [_playerData.techLevels [6]];
		_playerData.property [17] += GameConfigs.ArcheryRangedPrecise [_playerData.techLevels [6]];
		_playerData.property [13] += GameConfigs.ArcheryMeleeDamage [_playerData.techLevels [7]];
		_playerData.property [16] += GameConfigs.ArcheryMeleePrecise [_playerData.techLevels [7]];
	}

	public void SetHotkey(int index,int itemId){
		if (index == 0) {
			_playerData.Hotkey0 = itemId;
			StoreData ("Hotkey0", itemId);
		} else if (index == 1) {
			_playerData.Hotkey1 = itemId;
			StoreData ("Hotkey1", itemId);
		}
		_headUiManager.UpdateHotkeys ();
		this.gameObject.GetComponentInChildren<BackpackActions> ().UpdateHotkeys ();
	}

	void CheckHotKeyState(){
		if (!_playerData.bp.ContainsKey (_playerData.Hotkey0)) {
			_playerData.Hotkey0 = 0;
			StoreData ("Hotkey0", 0);
			_headUiManager.UpdateHotkeys ();
			this.gameObject.GetComponentInChildren<BackpackActions> ().UpdateHotkeys ();
		}
		if (!_playerData.bp.ContainsKey (_playerData.Hotkey1)) {
			_playerData.Hotkey1 = 0;
			StoreData ("Hotkey1", 0);
			_headUiManager.UpdateHotkeys ();
			this.gameObject.GetComponentInChildren<BackpackActions> ().UpdateHotkeys ();
		}
	}



	public void EatFood(int itemId){
		int id = (int)(itemId / 10000);
		foreach (int key in LoadTxt.MatDic[id].property.Keys) {
			ChangeProperty (key, (int)LoadTxt.MatDic [id].property [key]);
		}
		if (_playerData.bp [itemId] > 1)
			_playerData.bp [itemId]--;
		else
			_playerData.bp.Remove (itemId);
		StoreData ("bp", GetStrFromDic (_playerData.bp));

		if (LoadTxt.MatDic [id].tags.Contains ("Wine"))	//Achievement
			this.gameObject.GetComponent<AchieveActions> ().TasteWine (id);

		if (_playerData.Hotkey0 == itemId || _playerData.Hotkey1 == itemId)
			CheckHotKeyState ();
	}

	/// <summary>
	/// id:119,120...
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void LearnBlueprint(int itemId){
		if (_playerData.LearnedBlueprints.ContainsKey (itemId)) {
			return;
		}
		_playerData.LearnedBlueprints.Add (itemId, 1);
		StoreData ("LearnedBlueprints", GetStrFromDic (_playerData.LearnedBlueprints));
	}

	public void WithDrawWater(int waterStoredNow){
		AddItem (GameConfigs.WaterId, waterStoredNow);
		_playerData.LastWithdrawWaterTime = _playerData.minutesPassed;
		StoreData ("LastWithdrawWaterTime", _playerData.LastWithdrawWaterTime);
	}
		
	public void TakeOffEquip(int equipType){
		switch (equipType) {
		case 3:
			if (_playerData.MeleeId == 0)
				return;
			AddItem (_playerData.MeleeId, 1);
			_playerData.MeleeId = 0;
			StoreData ("MeleeId", 0);
			break;
		case 4:
			if (_playerData.RangedId == 0)
				return;
			AddItem (_playerData.RangedId, 1);
			_playerData.RangedId = 0;
			StoreData ("RangedId", 0);
			break;
		case 5:
			if (_playerData.MagicId == 0)
				return;
			AddItem (_playerData.MagicId, 1);
			_playerData.MagicId = 0;
			StoreData ("MagicId", 0);
			break;
		case 6:
			if (_playerData.HeadId == 0)
				return;
			AddItem (_playerData.HeadId, 1);
			_playerData.HeadId = 0;
			StoreData ("HeadId", 0);
			break;
		case 7:
			if (_playerData.BodyId == 0)
				return;
			AddItem (_playerData.BodyId, 1);
			_playerData.BodyId = 0;
			StoreData ("BodyId", 0);
			break;
		case 8:
			if (_playerData.ShoeId == 0)
				return;
			AddItem (_playerData.ShoeId, 1);
			_playerData.ShoeId = 0;
			StoreData ("ShoeId", 0);
			break;
		case 9:
			if (_playerData.AccessoryId == 0)
				return;
			AddItem (_playerData.AccessoryId, 1);
			_playerData.AccessoryId = 0;
			StoreData ("AccessoryId", 0);
			break;
		case 10:
			if (_playerData.AmmoId == 0 || _playerData.AmmoNum == 0)
				return;
			AddItem (_playerData.AmmoId, _playerData.AmmoNum);
			_playerData.AmmoId = 0;
			StoreData ("AmmoId", 0);
			_playerData.AmmoNum = 0;
			StoreData ("AmmoNum", 0);
			break;
		default:
			break;
		}
		UpdateProperty ();
	}

	public void ChangeEquip(int equipId){
		int equipType = LoadTxt.MatDic [(int)(equipId / 10000)].type;
		TakeOffEquip (equipType);
		switch (equipType) {
		case 3:
			_playerData.MeleeId = equipId;
			StoreData ("MeleeId", equipId);
			break;
		case 4:
			_playerData.RangedId = equipId;
			StoreData ("RangedId", equipId);
			break;
		case 5:
			_playerData.MagicId = equipId;
			StoreData ("MagicId", equipId);
			break;
		case 6:
			_playerData.HeadId = equipId;
			StoreData ("HeadId", equipId);
			break;
		case 7:
			_playerData.BodyId = equipId;
			StoreData ("BodyId", equipId);
			break;
		case 8:
			_playerData.ShoeId = equipId;
			StoreData ("ShoeId", equipId);
			break;
		case 9:
			_playerData.AccessoryId = equipId;
			StoreData ("AccessoryId", equipId);
			break;
		case 10:
			if (equipId == _playerData.AmmoId) {
				_playerData.AmmoNum += _playerData.bp [_playerData.AmmoId];
				StoreData ("AmmoNum", _playerData.bp [_playerData.AmmoId]);
			} else {
				_playerData.AmmoId = equipId;
				StoreData ("AmmoId", equipId);
				_playerData.AmmoNum = _playerData.bp [_playerData.AmmoId];
				StoreData ("AmmoNum", _playerData.bp [_playerData.AmmoId]);
			}
			break;
		default:
			break;
		}
		UpdateProperty ();
		DeleteItemInBp (equipId);
	}

	public int GetUsedPetSpace(){
		int usedPetSpace = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			usedPetSpace += LoadTxt.MonsterDic [GameData._playerData.Pets [key].monsterId].canCapture;
		}
		return usedPetSpace;
	}

	public void AddPet(Pet p){
		_playerData.Pets.Add (_playerData.PetRecord, p);
		StoreData ("Pets", GetstrFromPets (GameData._playerData.Pets));
		_playerData.PetRecord++;
		StoreData ("PetRecord", _playerData.PetRecord);
	}


	public Text inputWords;
	public void AddItemById(){
		int id = int.Parse (inputWords.text);
		AddItem (id, 100);
	}

}
