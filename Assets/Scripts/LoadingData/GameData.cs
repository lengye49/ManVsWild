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
	public int battleCount;
	public LogManager _logManager;

	void Awake () {

//		//*测试代码，删除所有数据
//		PlayerPrefs.DeleteAll ();
//		/*******************/

		_playerData = new PlayerData ();
		LoadAchievements ();
		LoadAllData (false);
		LoadStaticData ();
		_headUiManager = gameObject.GetComponentInChildren<HeadUiManager> ();
		_loadTxt = this.gameObject.GetComponent<LoadTxt> ();
		_headUiManager.UpdateHeadUI ();
		_loadTxt.LoadPlaces (false);
	}

	/// <summary>
	/// 重新开始和读取记忆时不会重置的数据
	/// </summary>
	void LoadStaticData(){
		meleeIdUsedData = PlayerPrefs.GetInt ("MeleeIdUsed", 1);
		rangedIdUsedData= PlayerPrefs.GetInt ("RangedIdUsed", 1);
		battleCount = PlayerPrefs.GetInt ("BattleCount", 0);
	}

	void DeleteAllData(){
		int meleeUsed = PlayerPrefs.GetInt ("MeleeIdUsed", 1);
		int rangedUsed =  PlayerPrefs.GetInt ("RangedIdUsed", 1);
		int battleCount = PlayerPrefs.GetInt ("BattleCount", 0);
		string techStr = PlayerPrefs.GetString ("Achievements", "1|0;2|0;3|0;4|0;5|0;6|0;7|0;8|0;9|0;10|0;11|0;12|0;13|0;14|0;15|0;16|0;17|0;18|0;19|0;20|0;21|0;22|0;23|0;24|0;25|0;26|0;27|0;28|0;29|0;30|0;31|0;32|0;33|0;34|0;35|0;36|0;37|0;38|0;39|0;40|0");
		int thiefCaught = PlayerPrefs.GetInt ("ThiefCaught", 0);
		int ghostKill = PlayerPrefs.GetInt ("GhostKill", 0);
		int ghostBossKill = PlayerPrefs.GetInt ("GhostBossKill", 0);
		int ghostKingKill = PlayerPrefs.GetInt ("GhostKingKill", 0);
		string wineTasted =  (PlayerPrefs.GetString ("WineTasted", ""));
		string foodCooked =  (PlayerPrefs.GetString ("FoodCooked", ""));
		string meleeCollected =  (PlayerPrefs.GetString ("MeleeCollected", ""));
		string rangedCollected =  (PlayerPrefs.GetString ("RangedCollected", ""));
		string magicCollected =  (PlayerPrefs.GetString ("MagicCollected", ""));
		int monsterKilled = PlayerPrefs.GetInt ("MonsterKilled", 0);
		int sleepTime = PlayerPrefs.GetInt ("SleepTime", 0);
		int dragonKilled = PlayerPrefs.GetInt ("DragonKilled", 0);
		int legendThiefCaught = PlayerPrefs.GetInt ("LegendThiefCaught", 0);
		int meleeAttackCount = PlayerPrefs.GetInt ("MeleeAttackCount", 0);
		int rangedAttackCount = PlayerPrefs.GetInt ("RangedAttackCount", 0);
		int magicAttackCount = PlayerPrefs.GetInt ("MagicAttackCount", 0);

		PlayerPrefs.DeleteAll ();

		PlayerPrefs.SetInt ("MeleeIdUsed", meleeUsed);
		PlayerPrefs.SetInt ("RangedIdUsed", rangedUsed);
		PlayerPrefs.SetInt ("BattleCount", battleCount);
		PlayerPrefs.SetString ("Achievements", techStr);
		PlayerPrefs.SetInt ("ThiefCaught" , thiefCaught);
		PlayerPrefs.SetInt ("GhostKill" , ghostKill);
		PlayerPrefs.SetInt ("GhostBossKill" , ghostBossKill);
		PlayerPrefs.SetInt ("GhostKingKill" , ghostKingKill);
		PlayerPrefs.SetString ("WineTasted" ,  wineTasted);
		PlayerPrefs.SetString ("FoodCooked" ,  foodCooked);
		PlayerPrefs.SetString ("MeleeCollected" ,  meleeCollected);
		PlayerPrefs.SetString ("RangedCollected" , rangedCollected);
		PlayerPrefs.SetString ("MagicCollected" , magicCollected);
		PlayerPrefs.SetInt ("MonsterKilled" , monsterKilled);
		PlayerPrefs.SetInt ("SleepTime" , sleepTime);
		PlayerPrefs.SetInt ("DragonKilled" , dragonKilled);
		PlayerPrefs.SetInt ("LegendThiefCaught" , legendThiefCaught);
		PlayerPrefs.SetInt ("MeleeAttackCount" , meleeAttackCount);
		PlayerPrefs.SetInt ("RangedAttackCount" , rangedAttackCount);
		PlayerPrefs.SetInt ("MagicAttackCount", magicAttackCount);
	
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
		if (s.Length > 0)
			s = s.Substring (0, s.Length - 1);
		return s;
	}

	/// <summary>
	/// Load Player Data From Local File.
	/// </summary>
	void LoadAllData(bool isMemory){
		string s = isMemory ? "_Memory" : "";

		_playerData.firstTimeInGame = PlayerPrefs.GetInt ("FirstTimeInGame", 0);

		_playerData.techLevels = GetTechList (PlayerPrefs.GetString ("TechList", "1|0;2|0;3|0;4|0;5|0;6|0;7|0;8|0;9|0;10|0;11|0;12|0;13|0;14|0;15|0;16|0;17|0"));

		_playerData.hpNow = PlayerPrefs.GetInt ("hpNow" + s, 100);
		_playerData.spiritNow = PlayerPrefs.GetInt ("spiritNow" + s, 100);
		_playerData.foodNow = PlayerPrefs.GetInt ("foodNow" + s, 100);
		_playerData.waterNow = PlayerPrefs.GetInt ("waterNow" + s, 100);
		_playerData.strengthNow = PlayerPrefs.GetInt ("strengthNow" + s, 100);
        _playerData.tempNow = PlayerPrefs.GetFloat ("tempNow" + s, 20);

		_playerData.HpMax = PlayerPrefs.GetInt ("HpMax" + s, 100);
		_playerData.SpiritMax = PlayerPrefs.GetInt ("SpiritMax" + s, 100);
		_playerData.FoodMax = PlayerPrefs.GetInt ("FoodMax" + s, 100);
		_playerData.WaterMax = PlayerPrefs.GetInt ("Watermax" + s, 100);
		_playerData.StrengthMax = PlayerPrefs.GetInt ("StrengthMax" + s, 100);
		_playerData.TempMax = PlayerPrefs.GetFloat ("TempMax" + s, 60);
        _playerData.TempMin = PlayerPrefs.GetFloat ("TempMin" + s, -60);

		_playerData.minutesPassed = PlayerPrefs.GetInt ("minutesPassed" + s, 0);

		_playerData.BedRoomOpen = PlayerPrefs.GetInt ("BedRoomOpen" + s, 0);
		_playerData.WarehouseOpen = PlayerPrefs.GetInt ("WarehouseOpen" + s, 0);
		_playerData.KitchenOpen = PlayerPrefs.GetInt ("KitchenOpen" + s, 0);
		_playerData.WorkshopOpen = PlayerPrefs.GetInt ("WorkshopOpen" + s, 0);
		_playerData.StudyOpen = PlayerPrefs.GetInt ("StudyOpen" + s, 0);
		_playerData.FarmOpen = PlayerPrefs.GetInt ("FarmOpen" + s, 0);
		_playerData.PetsOpen = PlayerPrefs.GetInt ("PetsOpen" + s, 0);
		_playerData.WellOpen = PlayerPrefs.GetInt ("WellOpen" + s, 0);
		_playerData.AchievementOpen = PlayerPrefs.GetInt ("AchievementOpen" + s, 0);
		_playerData.AltarOpen = PlayerPrefs.GetInt ("AltarOpen" + s, 0);

		_playerData.bp = GetDicFormStr (PlayerPrefs.GetString ("bp" + s, "11000000|50;11010000|20;41000000|5;42000000|2;42140000|10"));
		_playerData.wh = GetDicFormStr (PlayerPrefs.GetString ("wh" + s, "41000000|999;42000000|999"));

		_playerData.HasMemmory = PlayerPrefs.GetInt ("HasMemmory" + s, 0);

		_playerData.LearnedBlueprints = GetDicFormStr (PlayerPrefs.GetString ("LearnedBlueprints" + s, ""));

		_playerData.MeleeId = PlayerPrefs.GetInt ("MeleeId" + s, 1000000);
        _playerData.RangedId = PlayerPrefs.GetInt ("RangedId" + s, 0);
        _playerData.MagicId = PlayerPrefs.GetInt ("MagicId" + s, 0);
		_playerData.HeadId = PlayerPrefs.GetInt ("HeadId" + s, 0);
		_playerData.BodyId = PlayerPrefs.GetInt ("BodyId" + s, 0);
		_playerData.ShoeId = PlayerPrefs.GetInt ("ShoeId" + s, 0);
		_playerData.AmmoId = PlayerPrefs.GetInt ("AmmoId" + s, 0);
		_playerData.AmmoNum = PlayerPrefs.GetInt ("AmmoNum" + s, 0);
		_playerData.Mount = GetMount (PlayerPrefs.GetString ("Mount" + s, ""));

		_playerData.LastWithdrawWaterTime = PlayerPrefs.GetInt ("LastWithdrawWaterTime" + s, 0);

		_playerData.Renown = PlayerPrefs.GetInt ("Renown" + s, 0);

		_playerData.Farms = GetFarmStateFromStr (PlayerPrefs.GetString ("Farms" + s, "0|0|1|0;1|0|2|0;2|0|3|0;3|0|0|0;4|0|0|0;5|0|0|0;6|0|0|0;7|0|0|0"));

		_playerData.Pets = GetPetListFromStr (PlayerPrefs.GetString ("Pets" + s, ""));//"100|1|50|15|Hello;100|0|20|10|Kitty"));
		_playerData.PetRecord = PlayerPrefs.GetInt("PetRecord"+s,0);

																									 //0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 
		_playerData.MapOpenState = GetMapOpenStateFromStr (PlayerPrefs.GetString ("MapOpenState" + s, "1|1|1|0|0|0|0|0|0|0|0|1|0|0|0|1|0|0|0|0|0|0|0|0|0|0|0|0|0|0"));
		_playerData.dungeonLevelMax = 99;// PlayerPrefs.GetInt ("DungeonLevelMax" + s, 0);
        _playerData.tunnelLevelMax = PlayerPrefs.GetInt ("TunnelLevelMax" + s, 0);

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
		_playerData.GhostComingProp = GameConfigs.GhostComingProp [0];//这个科技还没做
		_playerData.ThiefDefence = 0.3f;
		//科技结束

		_playerData.lastThiefTime = PlayerPrefs.GetInt ("LastThiefTime", 0);
		_playerData.petsCaptured = PlayerPrefs.GetInt ("PetsCaptured", 0);
		_playerData.wineDrinked = PlayerPrefs.GetInt ("WineDrinked", 0);

		_playerData.placeNowId = PlayerPrefs.GetInt ("PlaceNowId", 0);

		if (isMemory) {
			_loadTxt.LoadPlaces (isMemory);
		}
			
		UpdateProperty ();

	}
		
	void StoreData(bool isRebirth)
	{
		string s = isRebirth ? "" : "_Memory";

		PlayerPrefs.SetInt ("FirstTimeInGame", _playerData.firstTimeInGame);

		PlayerPrefs.SetString ("TechList" + s, GetStrFromTechList (_playerData.techLevels));

		PlayerPrefs.SetInt ("hpNow" + s, _playerData.hpNow);
		PlayerPrefs.SetInt ("spiritNow" + s, _playerData.spiritNow);
		PlayerPrefs.SetInt ("foodNow" + s, _playerData.foodNow);
		PlayerPrefs.SetInt ("waterNow" + s, _playerData.waterNow);
		PlayerPrefs.SetInt ("strengthNow" + s, _playerData.strengthNow);
		PlayerPrefs.SetFloat ("tempNow" + s, _playerData.tempNow);
		PlayerPrefs.SetInt ("HpMax" + s, _playerData.HpMax);
		PlayerPrefs.SetInt ("SpiritMax" + s, _playerData.SpiritMax);
		PlayerPrefs.SetInt ("Watermax" + s, _playerData.WaterMax);
		PlayerPrefs.SetInt ("FoodMax" + s, _playerData.FoodMax);
		PlayerPrefs.SetInt ("StrengthMax" + s, _playerData.StrengthMax);
        PlayerPrefs.SetFloat ("TempMin" + s, _playerData.TempMin);
        PlayerPrefs.SetFloat ("TempMax" + s, _playerData.TempMax);

		PlayerPrefs.SetInt ("minutesPassed" + s, _playerData.minutesPassed);

		PlayerPrefs.SetInt ("BedRoomOpen" + s, _playerData.BedRoomOpen);
		PlayerPrefs.SetInt ("WarehouseOpen" + s, _playerData.WarehouseOpen);
		PlayerPrefs.SetInt ("KitchenOpen" + s, _playerData.KitchenOpen);
		PlayerPrefs.SetInt ("WorkshopOpen" + s, _playerData.WorkshopOpen);
		PlayerPrefs.SetInt ("StudyOpen" + s, _playerData.StudyOpen);
		PlayerPrefs.SetInt ("FarmOpen" + s, _playerData.FarmOpen);
		PlayerPrefs.SetInt ("PetsOpen" + s, _playerData.PetsOpen);
		PlayerPrefs.SetInt ("WellOpen" + s, _playerData.WellOpen);
		PlayerPrefs.SetInt ("AchievementOpen" + s, _playerData.AchievementOpen);
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
		PlayerPrefs.SetInt ("AmmoId" + s, _playerData.AmmoId);
		PlayerPrefs.SetInt ("AmmoNum" + s, _playerData.AmmoNum);
		PlayerPrefs.SetString ("Mount" + s, GetStrFromMount (_playerData.Mount));

		PlayerPrefs.SetInt ("LastWithdrawWaterTime" + s, _playerData.LastWithdrawWaterTime);

		PlayerPrefs.SetString ("Farms" + s, GetStrFromFarmState (_playerData.Farms));

		PlayerPrefs.SetString ("Pets" + s, GetstrFromPets (_playerData.Pets));
		PlayerPrefs.SetInt ("PetRecord" + s, _playerData.PetRecord);

		PlayerPrefs.SetString ("MapOpenState" + s, GetStrFromMapOpenState (_playerData.MapOpenState));
		PlayerPrefs.SetInt ("DungeonLevelMax" + s, _playerData.dungeonLevelMax);
        PlayerPrefs.SetInt ("TunnelLevelMax" + s, _playerData.tunnelLevelMax);

		PlayerPrefs.SetInt ("LastThiefTime" + s, _playerData.lastThiefTime);
		PlayerPrefs.SetInt ("Renown" + s, _playerData.Renown);
		PlayerPrefs.SetInt ("PetsCaptured" + s, _playerData.petsCaptured);
		PlayerPrefs.SetInt ("WineDrinked" + s, _playerData.wineDrinked);

		PlayerPrefs.SetInt ("PlaceNowId" + s, _playerData.placeNowId);

		_loadTxt.StorePlaceMemmory (isRebirth);
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
		DeleteAllData ();
		LoadAllData (false);
		StoreData (true);
	}

	/// <summary>
	/// Change Time by Minutes
	/// </summary>
	/// <param name="minutes">Minutes.</param>
	public void ChangeTime(int minutes){
		int lastHour = _playerData.hourNow;
		int lastDay = _playerData.dayNow;
		int lastMonth = _playerData.monthNow;
		int lastSeason = _playerData.seasonNow;

		_playerData.minutesPassed += minutes;
		StoreData ("minutesPassed", _playerData.minutesPassed);
		_headUiManager.UpdateHeadUI ("dateNow");
		_headUiManager.UpdateHeadUI ("timeNow");

        if (_playerData.hourNow != lastHour || _playerData.dayNow != lastDay) {

			int value;
            int hours = (_playerData.hourNow - lastHour) + (_playerData.dayNow - lastDay) * 24;

            value = -(int)Mathf.Max(0, GameConfigs.FoodCostPerHour * hours);
			ChangeProperty (4, value);

            value = -(int)Mathf.Max (0, GameConfigs.WaterCostPerHour * hours);
			ChangeProperty (6, value);

            ChangeTempByTime(hours);
		}

		if (_playerData.dayNow != lastDay) {
			ChangeDay ();

            if (lastDay < 1000 && _playerData.dayNow >= 1000)
                OpenMap(25);
		}

		if (_playerData.monthNow != lastMonth) {
			ChangeMonth ();
		}

		if (_playerData.seasonNow != lastSeason) {
			ChangeSeason ();
		}

		//Achievement
		this.gameObject.GetComponent<AchieveActions>().TimeChange();
	}

    void ChangeTempByTime(int hours){
		float value;
        if (_playerData.hourNow > 6 && _playerData.hourNow <= 18)
            value = Mathf.Clamp(GameConfigs.TempChangeDay[_playerData.seasonNow] * (float)Random.Range(80, 120) / 100f * hours, -5.0f, 5.0f);
        else
            value = Mathf.Clamp(GameConfigs.TempChangeNight[_playerData.seasonNow] * (float)Random.Range(80, 120) / 100f * hours, -5.0f, 5.0f);

		ChangeProperty (10, value);
    }

	void ChangeDay(){
		string txt = "第" + _playerData.dayNow + "天开始了，加油！";
		_logManager.AddLog (txt);
        if(_playerData.dayNow==20)
            _logManager.AddLog("随着你的居住地的发展，渐渐引起了附近盗贼的注意。请严加防范，安排宠物看守房屋或者升级科技都可以有效减少盗贼的入侵损失。");
	}

	void ChangeMonth(){
	
	}

	void ChangeSeason(){
		string s = "";
		switch (_playerData.seasonNow) {
		case 0:
			s += "春天来了,气候变暖。";
			break;
		case 1:
			s += "夏天来了，天气炎热，请注意防暑降温。";
			break;
		case 2:
			s += "秋天来了，气候转凉。";
			break;
		case 3:
			s += "冬天来了，天气寒冷，请注意取暖。";
			break;
		default:
			break;
		}
		_logManager.AddLog (s);
		GetComponentInChildren<FloatingActions> ().CallInFloating (s, 0);
	}

	/// <summary>
	/// 食物、卧室等可以用来恢复的属性.
	/// </summary>
	/// <param name="propName">Property name.</param>
	/// <param name="value">Value.</param>
	public void ChangeProperty(int propId, int value){
        int lastProp;
		switch (propId) {
		case 0:
			lastProp = _playerData.hpNow;
    			
			_playerData.hpNow = (int)((_playerData.hpNow + value) > _playerData.property [1] ? _playerData.property [1] : (_playerData.hpNow + value));
			StoreData ("hpNow", _playerData.hpNow);
			UpdateProperty (0, _playerData.hpNow);
			_headUiManager.UpdateHeadUI ("hpNow");

			if (lastProp > _playerData.HpMax * 0.3f && _playerData.hpNow <= _playerData.HpMax * 0.3f) {
				_logManager.AddLog ("你当前的[生命值]较低，生命值过低会流血致死。");
				_logManager.AddLog (1f, "可以使用食物增加生命值。");
			}
                
			if (_playerData.hpNow <= 0) {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
				GetComponentInChildren<DeathActions> ().UpdateDeath ("Hp");
				Debug.Log ("Die of Hp");
			}
    				
			break;
		case 1:
			_playerData.HpMax += value;
			StoreData ("HpMax", _playerData.HpMax);
			UpdateProperty (1, _playerData.HpMax);
			_headUiManager.UpdateHeadUI ("hpMax");
			break;
        case 2:
            lastProp = _playerData.spiritNow;

            _playerData.spiritNow = (int)((_playerData.spiritNow + value) > _playerData.property[3] ? _playerData.property[3] : (_playerData.spiritNow + value));
            StoreData("spiritNow", _playerData.spiritNow);
            _headUiManager.UpdateHeadUI("spiritNow");
            UpdateProperty(2, _playerData.spiritNow);

            if (lastProp > _playerData.SpiritMax * 0.3f && _playerData.spiritNow <= _playerData.SpiritMax * 0.3f)
            {
                _logManager.AddLog("你当前的[精神]较低，精神过低时会精神失常。");
                _logManager.AddLog(1f, "可以通过睡觉、饮酒等提高精神。");
            }
            
            if (_playerData.spiritNow <= 0)
            {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
                gameObject.GetComponentInChildren<DeathActions>().UpdateDeath("Spirit");
                Debug.Log("Die of Spirit");
            }
			break;
		case 3:
			_playerData.SpiritMax += value;
			StoreData ("SpiritMax", _playerData.SpiritMax);
			UpdateProperty (3, _playerData.SpiritMax);
			_headUiManager.UpdateHeadUI ("SpiritMax");
			break;
        case 4:
            lastProp = _playerData.foodNow;

            _playerData.foodNow = (int)((_playerData.foodNow + value) > _playerData.property[5] ? _playerData.property[5] : (_playerData.foodNow + value));
            StoreData("foodNow", _playerData.foodNow);
            _headUiManager.UpdateHeadUI("foodNow");
            UpdateProperty(4, _playerData.foodNow);

            if (lastProp > _playerData.FoodMax * 0.3f && _playerData.foodNow <= _playerData.FoodMax * 0.3f)
            {
                _logManager.AddLog("你当前的[饱腹值]较低，饱腹值过低时会饿死。");
                _logManager.AddLog(1f, "可以使用食物增加饱腹值。");
            }

            if (_playerData.foodNow <= 0)
            {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
                gameObject.GetComponentInChildren<DeathActions>().UpdateDeath("Food");
                Debug.Log("Die of Food");
            }
			break;
		case 5:
			_playerData.FoodMax += value;
			StoreData ("FoodMax", _playerData.FoodMax);
			UpdateProperty (5, _playerData.FoodMax);
			_headUiManager.UpdateHeadUI ("FoodMax");
			break;
        case 6:
            lastProp = _playerData.waterNow;

            _playerData.waterNow = (int)((_playerData.waterNow + value) > _playerData.property[7] ? _playerData.property[7] : (_playerData.waterNow + value));
            StoreData("waterNow", _playerData.waterNow);
            _headUiManager.UpdateHeadUI("waterNow");
            UpdateProperty(6, _playerData.waterNow);

            if (lastProp > _playerData.WaterMax * 0.3f && _playerData.waterNow <= _playerData.WaterMax * 0.3f)
            {
                _logManager.AddLog("你当前的[水分]较低，水分过低会导致脱水死亡。");
                _logManager.AddLog(1f, "可以通过喝水、饮料等增加水分。");
            }
                
            if (_playerData.waterNow <= 0)
            {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
                gameObject.GetComponentInChildren<DeathActions>().UpdateDeath("Water");
                Debug.Log("Die of Water");
            }
			break;
		case 7:
			_playerData.WaterMax += value;
			StoreData ("WaterMax", _playerData.WaterMax);
			UpdateProperty (7, _playerData.WaterMax);
			_headUiManager.UpdateHeadUI ("WaterMax");
			break;
        case 8:
            lastProp = _playerData.strengthNow;

            _playerData.strengthNow = (int)((_playerData.strengthNow + value) > _playerData.property[9] ? _playerData.property[9] : (_playerData.strengthNow + value));
            StoreData("strengthNow", _playerData.strengthNow);
            _headUiManager.UpdateHeadUI("strengthNow");
            UpdateProperty(8, _playerData.strengthNow);

            if (lastProp > _playerData.StrengthMax * 0.3f && _playerData.strengthNow <= _playerData.StrengthMax * 0.3f)
            {
                _logManager.AddLog("你当前的[力量]较低，力量过低时无法进行采集活动。");
                _logManager.AddLog(1f, "可以通过休息恢复力量。");
            }
			break;
		case 9:
			_playerData.StrengthMax += value;
			StoreData ("StrengthMax", _playerData.StrengthMax);
			UpdateProperty (9, _playerData.StrengthMax);
			_headUiManager.UpdateHeadUI ("StrengthMax");
			break;
		case 10:
			float v = (float)value;
			ChangeProperty (10, v);
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


	void ChangeProperty(int propId,float value){
		float lastTemp;
		switch (propId) {
		case 10:
			lastTemp = _playerData.tempNow;

			_playerData.tempNow += value;
			StoreData ("tempNow", _playerData.tempNow);
			_headUiManager.UpdateHeadUI ("tempNow");
			UpdateProperty (10, _playerData.tempNow);

			if (lastTemp < _playerData.property [12] * 0.7f && _playerData.tempNow >= _playerData.property [12] * 0.7f) {
				_logManager.AddLog ("你当前体温过高，高温极限为" + _playerData.property [11] + "℃。");
				_logManager.AddLog (1f, "可以通过喝冰水、洗澡等降低体温。");
			}

			if (lastTemp > _playerData.property [11] * 0.7f && _playerData.tempNow <= _playerData.property [11] * 0.7f) {
				_logManager.AddLog ("你当前体温过低，低温极限为" + _playerData.property [11] + "℃。");
				_logManager.AddLog (1f, "可以通过使用火把、喝酒等提高体温。");
			}

			if (_playerData.tempNow > _playerData.property [12]) {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
				gameObject.GetComponentInChildren<DeathActions> ().UpdateDeath ("Cold");
				Debug.Log ("Die of Hot");
			}

			if (_playerData.tempNow < _playerData.property [11]) {
				GetComponentInChildren<PanelManager> ().GoToPanel ("Death");
				gameObject.GetComponentInChildren<DeathActions> ().UpdateDeath ("Hot");
				Debug.Log ("Die of Cold");
			}
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// 向背包中添加物品（8位ID）
	/// </summary>
	/// <param name="itemId">Item identifier*10000.</param>
	/// <param name="num">Number.</param>
	public void AddItem(int itemId,int num){
        if (_playerData.bp.ContainsKey(itemId))
            _playerData.bp[itemId] += num;
        else
        {
            if (_playerData.bpNum <= _playerData.bp.Count)
            {
                GetComponentInChildren<FloatingActions>().CallInFloating("背包已满！", 1);
                _logManager.AddLog("你的背包满了，无法获得新物品。");
            }
            else
            {
                _playerData.bp.Add(itemId, num);
            }
        }
		StoreData ("bp", GetStrFromDic (_playerData.bp));

	}

	/// <summary>
	/// 使用背包中的物品（4位ID）
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
	}


	/// <summary>
	/// 删除背包物品，id是8位.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void DeleteItemInBp(int itemId){
		_playerData.bp.Remove (itemId);
		StoreData ("bp", GetStrFromDic (_playerData.bp));
	}

	/// <summary>
	///  删除背包物品，id是8位.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="num">Number.</param>
	void DeleteItemInBp(int itemId,int num){
		if(_playerData.bp[itemId]>num)
			_playerData.bp[itemId] -= num;
		else
			_playerData.bp.Remove (itemId);

		StoreData ("bp", GetStrFromDic (_playerData.bp));
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
	/// <param name="itemId">8位ID.</param>
	/// <param name="num">Number.</param>
	public void StoreItem(int itemId){
		StoreItem(itemId,_playerData.bp[itemId]);
		DeleteItemInBp (itemId);
	}

	public void StoreItem(int itemId,int num){
		if (_playerData.wh.ContainsKey (itemId))
			_playerData.wh [itemId] += num;
		else
			_playerData.wh.Add (itemId, num);
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
	/// 计数背包中某类型的物品数量（4位ID）
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
		UpdateEquipProperty (GameData._playerData.AmmoId);
		UpdateMountProperty ();
		UpdateTechniqueProperty ();
	}

	void UpdateProperty(int propId,int newValue){
		_playerData.property [propId] = newValue;
	}

    void UpdateProperty(int propId,float newValue){
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
			Extra_Weapon ew = LoadTxt.GetExtraMelee(extra);
			foreach (int key in ew.property.Keys) {
				_playerData.property [key] += ew.property [key];
			}
		}
		if (LoadTxt.MatDic [(int)(id / 10000)].type != 4) {
			Extra_Weapon ew = LoadTxt.GetExtraRanged(extra);
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
        int equipType = LoadTxt.MatDic[(int)(equipId / 10000)].type;
        int num = 1;
        int oldId = 0;
        switch (equipType)
        {
            case 3:
                oldId = _playerData.MeleeId;
                _playerData.MeleeId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("MeleeId", equipId);
                break;
            case 4:
                oldId = _playerData.RangedId;
                _playerData.RangedId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("RangedId", equipId);
                break;
            case 5:
                oldId = _playerData.MagicId;
                _playerData.MagicId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("MagicId", equipId);
                break;
            case 6:
                oldId = _playerData.HeadId;
                _playerData.HeadId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("HeadId", equipId);
                break;
            case 7:
                oldId = _playerData.BodyId;
                _playerData.BodyId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("BodyId", equipId);
                break;
            case 8:
                oldId = _playerData.ShoeId;
                _playerData.ShoeId = equipId;
                DeleteItemInBp(equipId, num);
                AddItem(oldId, 1);
                StoreData("ShoeId", equipId);
                break;
            case 9:
                break;
            case 10:
                oldId = _playerData.AmmoId;
                num = _playerData.AmmoNum;
                DeleteItemInBp(oldId, num);
                if (equipId == _playerData.AmmoId)
                {
                    _playerData.AmmoNum += _playerData.bp[_playerData.AmmoId];
                    StoreData("AmmoNum", _playerData.bp[_playerData.AmmoId]);
                }
                else
                {
                    _playerData.AmmoId = equipId;
                    StoreData("AmmoId", equipId);
                    int n = _playerData.bp[equipId];
                    _playerData.AmmoNum = n;
                    StoreData("AmmoNum", n);
                    DeleteItemInBp(equipId, n);
                    AddItem(oldId, num);
                }
                break;
            default:
                break;
        }
        UpdateProperty();
    }

	public int GetUsedPetSpace(){
		int usedPetSpace = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			usedPetSpace += LoadTxt.GetMonster(GameData._playerData.Pets [key].monsterId).canCapture;
		}
		return usedPetSpace;
	}

	public void AddPet(Pet p){
		_playerData.Pets.Add (_playerData.PetRecord, p);
		StoreData ("Pets", GetstrFromPets (GameData._playerData.Pets));
		_playerData.PetRecord++;
		StoreData ("PetRecord", _playerData.PetRecord);
	}

    /// <summary>
    /// 在探索的过程中是否发现了新地图，根据当前进度来调整新地图出现的概率
    /// </summary>
    /// <param name="isMission">If set to <c>true</c> is mission.</param>
	public void SearchNewPlace(int thisMapId,int searchProcess){
        if (LoadTxt.MapDic[thisMapId].mapNext.Count<=0)
            return;
        int r;
        for (int i = 0; i < LoadTxt.MapDic[thisMapId].mapNext.Count; i++)
        {
			int m = (int)LoadTxt.MapDic[thisMapId].mapNext[i];
			if (_playerData.MapOpenState [m] > 0)
				continue;

            r = Random.Range(0, 100);
			int t = 0;
			if (searchProcess < 25)
				t = 5;
			else if (searchProcess < 50)
				t = 10;
			else if (searchProcess < 75)
				t = 15;
			else
				t = 20;

			if (r < t) {
				OpenMap (m);
				break;
			}
        }
    }

	/// <summary>
	/// 开启新地图
	/// </summary>
	/// <param name="mapId">Map identifier.</param>
    public void OpenMap(int mapId){
        if (GameData._playerData.MapOpenState[mapId] == 1)
            return;

        GameData._playerData.MapOpenState[mapId] = 1;
        StoreData("MapOpenState", GetStrFromMapOpenState(_playerData.MapOpenState));

		_logManager.AddLog("你发现了新地点：" + LoadTxt.MapDic[mapId].name,true);
        GetComponentInChildren<FloatingActions>().CallInFloating("你发现了新地点：" + LoadTxt.MapDic[mapId].name, 0);

        //Achievement
        this.gameObject.GetComponentInParent<AchieveActions>().NewPlaceFind();

    }

	public void AddRenown(int num){
		_playerData.Renown += num;
		StoreData ("Renown", _playerData.Renown);
		this.gameObject.GetComponent<AchieveActions> ().RenownChange ();
	}

	/// <summary>
	/// 获得每日的奖励和提示信息
	/// </summary>
	void GetDailyTribute(){;
		
		int d = _playerData.dayNow;

		if (d == 3 && _playerData.WarehouseOpen==1) {
			StoreItem (34020000, 5);//空间宝石
			_logManager.AddLog( "镇上居民送来了一些材料。");
			_logManager.AddLog( "空间宝石+5 已放入仓库。");
		}
		if (d == 5 && _playerData.WarehouseOpen==1) {
			StoreItem (41000000, 10);
			StoreItem (42000000, 10);
			_logManager.AddLog( "镇长希望你能够照顾好自己后，多去闯荡。镇子很小，世界很大。");
		}
		if (d == 10 && _playerData.WarehouseOpen==1) {
			StoreItem (34020000, 5);//空间宝石
			_logManager.AddLog( "镇上居民送来了一些材料。");
			_logManager.AddLog( "空间宝石+5 已放入仓库。");
		}
		if (d == 7 && _playerData.WarehouseOpen==1) {
			StoreItem (33020000, 5);//空间宝石
			_logManager.AddLog( "镇长派人送来了一些战斗记录。");
			_logManager.AddLog( "战士训练手册+5 已放入仓库。");
		}
		if (d == 12 && _playerData.WarehouseOpen==1) {
			StoreItem (33040000, 5);//空间宝石
			_logManager.AddLog( "镇长派人送来了一些战斗记录。");
			_logManager.AddLog( "射术精要+5 已放入仓库。");
		}

		//进贡前的提醒
		if (d % 30 == 27)
			_logManager.AddLog ("镇上的居民将在3天后送来补给，请保证仓库有>5空位。");
	}

	/// <summary>
	/// 获得每月的进贡
	/// </summary>
	void GetMonthlyTribute(){
		int r = _playerData.Renown;
		Dictionary<int,int> dic = new Dictionary<int, int> ();
		string s;
		if (r < 9) {
			dic.Add (41000000, 20);//水
			dic.Add (42000000, 20);//面包
			s = "希望你能够更加勇敢，闯荡出一些名声，居民给你送来了一些补给。";
		} else if (r < 99) {
			dic.Add (41000000, 20);//水
			dic.Add (42000000, 20);//面包
			dic.Add (33020000, 10);//战士训练手册
			dic.Add (33040000, 10);//射术精要
			s = "你的大名已经传遍小镇，居民给你送来了一些补给。";
		} else {
			//根据季节进行区分
			if (_playerData.seasonNow == 0 || _playerData.seasonNow == 2) {
				dic.Add (42040000, 20);//烤肉
				dic.Add (43000000, 30);//鸡尾酒
				dic.Add (41000000, 50);//水
			} else if (_playerData.seasonNow == 1) {
				dic.Add (42050000, 20);//烤鱼
				dic.Add (41090000, 30);//冰块
				dic.Add (43010000, 20);//冰镇酒
			} else {
				dic.Add (42060000, 20);//烤鱼
				dic.Add (42020000, 30);//冰块
				dic.Add (43000000, 20);//鸡尾酒
			}
			s = "居民对你非常仰慕，希望你能够继续保护他们。";
		}
		_logManager.AddLog (s);
		s = "";
		foreach (int key in dic.Keys) {
			StoreItem (key, dic [key]);
			s += LoadTxt.MatDic [key / 10000].name + "+" + dic [key] + ",";
		}
		s = s.Substring (0, s.Length - 1);
		s += " 已放入仓库";
		_logManager.AddLog (s);
	}

	/// <summary>
	/// 获得第一次进贡
	/// </summary>
	public void GetFirstTribute(){
		StoreItem (41000000, 30);
		StoreItem (41010000, 100);
		string s = "附近的居民非常友善，他们热情的欢迎你，";
		_logManager.AddLog (1f,s);
		s = "送来 水+30，小麦+100 已放入你的仓库。";
		_logManager.AddLog (2f,s);
	}

	public Text inputWords;
	public void AddItemById(){
		int id = int.Parse (inputWords.text);
		AddItem (id, 100);
	}

}
