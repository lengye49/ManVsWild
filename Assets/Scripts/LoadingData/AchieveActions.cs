using UnityEngine;
using System.Collections;

public class AchieveActions : MonoBehaviour {

	private GameData _gameData;
	private FloatingActions _floating;
	private LogManager _log;
	void Start(){
		_gameData = GetComponent<GameData> ();
		_floating = GetComponentInChildren<FloatingActions> ();
		_log = GetComponentInChildren<LogManager> ();
	}

	public string GetProgress(int id){
		int openCount = 0;
		string s = "";
		switch (id) {
		case 1:
			s += GameData._playerData.thiefCaught + "/" + LoadTxt.AchievementDic [1].req;
			break;
		case 2:
			s += GameData._playerData.ghostKill + "/" + LoadTxt.AchievementDic [2].req;
			break;
		case 3:
			s += GameData._playerData.ghostBossKill + "/" + LoadTxt.AchievementDic [3].req;
			break;
		case 4:
			s += GameData._playerData.ghostKingKill + "/" + LoadTxt.AchievementDic [4].req;
			break;
		case 5:
			s += GameData._playerData.wineTasted.Length + "/" + LoadTxt.AchievementDic [5].req;
			break;
		case 6:
			s += GameData._playerData.foodCooked.Length + "/" + LoadTxt.AchievementDic [6].req;
			break;
		case 7:
			s += GameData._playerData.Renown + "/" + LoadTxt.AchievementDic [7].req;
			break;
		case 8:
			s += GameData._playerData.Renown + "/" + LoadTxt.AchievementDic [8].req;
			break;
		case 9:
			s += GameData._playerData.Renown + "/" + LoadTxt.AchievementDic [9].req;
			break;
		case 10:
			s += GameData._playerData.Renown + "/" + LoadTxt.AchievementDic [10].req;
			break;
		case 11:
			s += GameData._playerData.meleeCollected.Length + "/" + LoadTxt.AchievementDic [11].req;
			break;
		case 12:
			s += GameData._playerData.rangedCollected.Length + "/" + LoadTxt.AchievementDic [12].req;
			break;
		case 13:
			s += GameData._playerData.magicCollected.Length + "/" + LoadTxt.AchievementDic [13].req;
			break;
		case 14:
			s += GameData._playerData.petsCaptured + "/" + LoadTxt.AchievementDic [14].req;
			break;
		case 15:
			if (GameData._playerData.Achievements [15] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 16:
			s += GameData._playerData.monsterKilled + "/" + LoadTxt.AchievementDic [16].req;
			break;
		case 17:
			if (GameData._playerData.Achievements [17] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 18:
			if (GameData._playerData.Achievements [18] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 19:
			foreach (int key in GameData._playerData.MapOpenState.Keys) {
				openCount += GameData._playerData.MapOpenState [key];
			}
			s += openCount + "/" + LoadTxt.AchievementDic [19].req;
			break;
		case 20:
			s += GameData._playerData.dayNow + "/" + LoadTxt.AchievementDic [20].req;
			break;
		case 21:
			s += GameData._playerData.sleepTime + "/" + LoadTxt.AchievementDic [21].req;
			break;
		case 22:
			s += GameData._playerData.wineDrinked + "/" + LoadTxt.AchievementDic [22].req;
			break;
		case 23:
			if (GameData._playerData.Achievements [23] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 24:
			s += GameData._playerData.dungeonLevelMax + "/" + LoadTxt.AchievementDic [24].req;
			break;
		case 25:
			s += GameData._playerData.meleeAttackCount + "/" + LoadTxt.AchievementDic [25].req;
			break;
		case 26:
			s += GameData._playerData.rangedAttackCount + "/" + LoadTxt.AchievementDic [26].req;
			break;
		case 27:
			s += GameData._playerData.magicAttackCount + "/" + LoadTxt.AchievementDic [27].req;
			break;
		case 28:
			break;
		case 29:
			break;
		case 30:
			break;
		case 31:
			foreach (int key in GameData._playerData.MapOpenState.Keys) {
				openCount += GameData._playerData.MapOpenState [key];
			}
			s += openCount + "/" + LoadTxt.AchievementDic [31].req;
			break;
		case 32:
			s += GameData._playerData.dragonKilled + "/" + LoadTxt.AchievementDic [32].req;
			break;
		case 33:
			if (GameData._playerData.Achievements [23] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 34:
			s += GameData._playerData.dayNow + "/" + LoadTxt.AchievementDic [34].req;
			break;
		case 35:
            if (GameData._playerData.Achievements [35] == 0)
                s = "0/1";
            else
                s = "1/1";
			break;
		case 36:
			if (GameData._playerData.Achievements [36] == 0)
				s = "0/1";
			else
				s = "1/1";
			break;
		case 37:
            s += s += GameData._playerData.dungeonLevelMax + "/" + LoadTxt.AchievementDic [35].req;
			break;
		case 38:
			s += GameData._playerData.legendThiefCaught + "/" + LoadTxt.AchievementDic [38].req;
			break;
		case 39:
			break;
		case 40:
			break;
		default:
			break;
		}
		return s;
	}

	public void DefeatEnemy(int monsterId){
		
		if (monsterId == 1) {
			//Ghost
			GameData._playerData.ghostKill++;
			_gameData.StoreData ("GhostKill", GameData._playerData.ghostKill);
			if (GameData._playerData.Achievements [2] == 0) {
				if (GameData._playerData.ghostKill >= LoadTxt.AchievementDic [2].req) {
					StoreAchievement (2);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [2].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [2].name,true);
				}
			}
		} else if (monsterId == 2) {
			//GhostBoss
			GameData._playerData.ghostBossKill++;
			_gameData.StoreData ("GhostBossKill", GameData._playerData.ghostBossKill);
			if (GameData._playerData.Achievements [3] == 0) {
				if (GameData._playerData.ghostBossKill >= LoadTxt.AchievementDic [3].req) {
					StoreAchievement (3);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [3].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [3].name,true);
				}
			}
		} else if (monsterId == 3) {
			//GhostKing
			GameData._playerData.ghostKingKill++;
			_gameData.StoreData ("GhostKingKill", GameData._playerData.ghostKingKill);
			if (GameData._playerData.Achievements [4] == 0) {
				if (GameData._playerData.ghostKingKill >= LoadTxt.AchievementDic [4].req) {
					StoreAchievement (4);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [4].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [4].name,true);
				}
			}
		} else if (monsterId == 1801 || monsterId == 1802 || monsterId == 1803) {
			//Dragon
			GameData._playerData.dragonKilled++;
			_gameData.StoreData ("DragonKilled", GameData._playerData.dragonKilled);
			if(GameData._playerData.Achievements[32]==0){
				if (GameData._playerData.dragonKilled >= LoadTxt.AchievementDic [32].req) {
					StoreAchievement (32);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [32].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [32].name,true);
				}
			}
		}

		GameData._playerData.monsterKilled++;
		_gameData.StoreData ("MonsterKilled", GameData._playerData.monsterKilled);
		if (GameData._playerData.Achievements [16] == 0) {
			if (GameData._playerData.monsterKilled >= LoadTxt.AchievementDic [16].req) {
				StoreAchievement (16);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [16].name, 0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [16].name,true);
			}
		}
	}

	/// <summary>
	/// Tastes the wine,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void TasteWine(int itemId){

		GameData._playerData.wineDrinked++;
		_gameData.StoreData ("WineDrinked", GameData._playerData.wineDrinked);
		if (GameData._playerData.Achievements [22] == 0) {
			if (GameData._playerData.wineDrinked >= LoadTxt.AchievementDic [22].req) {
				StoreAchievement (22);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [22].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [22].name,true);
			}
		}

		int[] w = GameData._playerData.wineTasted;
		int[] wNew = new int[w.Length + 1];
		for (int i = 0; i < wNew.Length-1; i++) {
			if (w [i] == itemId)
				return;
			wNew [i] = w [i];
		}
		wNew [wNew.Length - 1] = itemId;
		GameData._playerData.wineTasted = wNew;
		_gameData.StoreData ("WineTasted", _gameData.GetStrFromInt (wNew));
		if (GameData._playerData.Achievements [5] == 0) {
			if (wNew.Length >= LoadTxt.AchievementDic[5].req) {
				StoreAchievement (5);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [5].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [5].name,true);
			}
		}
	}

	public void LoadMemmory(){
		if (GameData._playerData.Achievements [23] == 0) {
			StoreAchievement (23);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [23].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [23].name,true);
		}
	}

	/// <summary>
	/// Cooks the food,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CookFood(int itemId){
		
		int[] f = GameData._playerData.foodCooked;
		int[] fNew = new int[f.Length + 1];

		for (int i = 0; i < fNew.Length-1; i++) {
			if (f [i] == itemId)
				return;
			fNew [i] = f [i];
		}
		fNew [fNew.Length - 1] = itemId;
		GameData._playerData.foodCooked = fNew;
		_gameData.StoreData ("FoodCooked", _gameData.GetStrFromInt (fNew));

		if (GameData._playerData.Achievements [6] == 0) {
			if (fNew.Length >= LoadTxt.AchievementDic [6].req) {
				StoreAchievement (6);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [6].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [6].name,true);
			}
		}
	}

	/// <summary>
	/// Collects the melee weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectMeleeWeapon(int itemId){

		int[] m = GameData._playerData.meleeCollected;
		int[] mNew = new int[m.Length + 1];


		for (int i = 0; i < m.Length; i++) {
			if (m [i] == itemId)
				return;
			else
				mNew [i] = m [i];		
		}
		mNew [mNew.Length - 1] = itemId;

		GameData._playerData.meleeCollected = mNew;
		_gameData.StoreData ("MeleeCollected", _gameData.GetStrFromInt (mNew));

		if (GameData._playerData.Achievements [11] == 0) {
			if (mNew.Length >= LoadTxt.AchievementDic [11].req) {
				StoreAchievement (11);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [11].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [11].name,true);
			}
		}
	}

	/// <summary>
	/// Collects the ranged weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectRangedWeapon(int itemId){

		int[] r = GameData._playerData.rangedCollected;
		int[] rNew = new int[r.Length + 1];

		for (int i = 0; i < rNew.Length-1; i++) {
			if (r [i] == itemId)
				return;
			rNew [i] = r [i];
		}
		rNew [rNew.Length-1] = itemId;
		GameData._playerData.rangedCollected = rNew;
		_gameData.StoreData ("RangedCollected", _gameData.GetStrFromInt (rNew));

		if (GameData._playerData.Achievements [12] == 0) {
			if (rNew.Length >= LoadTxt.AchievementDic [12].req) {
				StoreAchievement (12);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [12].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [12].name,true);
			}
		}
	}

	/// <summary>
	/// Collects the magic weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectMagicWeapon(int itemId){

		int[] m = GameData._playerData.magicCollected;
		int[] mNew = new int[m.Length + 1];

		for (int i = 0; i < mNew.Length-1; i++) {
			if (m [i] == itemId)
				return;
			mNew [i] = m [i];
		}
		mNew [mNew.Length - 1] = itemId;
		GameData._playerData.magicCollected = mNew;
		_gameData.StoreData ("MagicCollected", _gameData.GetStrFromInt (mNew));

		if (GameData._playerData.Achievements [13] == 0) {
			if (mNew.Length >= LoadTxt.AchievementDic [13].req) {
				StoreAchievement (13);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [13].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [13].name,true);
			}
		}
	}

	public void CapturePet(){

		GameData._playerData.petsCaptured++;
		_gameData.StoreData ("PetsCaptured", GameData._playerData.petsCaptured);
		if (GameData._playerData.Achievements [14] == 0) {
			if (GameData._playerData.petsCaptured >= LoadTxt.AchievementDic [14].req) {
				StoreAchievement (14);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [14].name, 0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [14].name,true);
			}
		}
	}

	public void MountPet(int monsterId){
		if (GameData._playerData.Achievements [15] == 0) {
			StoreAchievement (15);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [15].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [15].name,true);
		}

		//Dragon
		if (GameData._playerData.Achievements [36] == 0) {
			if(monsterId==1801 || monsterId==1802 || monsterId==1803) {
				StoreAchievement (36);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [36].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [36].name,true);
			}
		}
	}

	public void NewPlaceFind(){

		int openCount = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			openCount += GameData._playerData.MapOpenState [key];
		}

		if (openCount >= LoadTxt.AchievementDic [19].req && (GameData._playerData.Achievements [19] == 0)) {
			StoreAchievement (19);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [19].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [19].name,true);
		}
		if (openCount >= LoadTxt.AchievementDic [31].req && (GameData._playerData.Achievements [31] == 0)) {
			StoreAchievement (31);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [31].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [31].name,true);
		}
	}

	public void TimeChange(){
		if (GameData._playerData.Achievements [20] == 0) {
			if (GameData._playerData.dayNow >= LoadTxt.AchievementDic [20].req) {
				StoreAchievement (20);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [20].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [20].name,true);
			}
		}
		if (GameData._playerData.Achievements [34] == 0) {
			if (GameData._playerData.dayNow >= LoadTxt.AchievementDic [34].req) {
				StoreAchievement (34);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [34].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [34].name,true);
			}
		}
	}

	public void TotalSearch(){
		if (GameData._playerData.Achievements [18] == 0) {
			StoreAchievement (18);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [18].name, 0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [18].name,true);
		}

	}

	public void Sleep(int t){

		GameData._playerData.sleepTime += t;
		_gameData.StoreData ("SleepTime", GameData._playerData.sleepTime);

		if (GameData._playerData.Achievements [21] == 1) {
			if (GameData._playerData.sleepTime >= LoadTxt.AchievementDic [21].req) {
				StoreAchievement (21);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [21].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [21].name,true);
			}
		}
	}

	public void TechUpgrade(){
		foreach (int key in GameData._playerData.techLevels.Keys) {
			foreach (int k in LoadTxt.TechDic.Keys) {
				if ((key == LoadTxt.TechDic [k].type) && (GameData._playerData.techLevels [key]< LoadTxt.TechDic [k].maxLv)) {
					return;
				}
			}
		}
		StoreAchievement (35);
		_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [35].name,0);
		_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [35].name,true);
	}

	public void ConstructBulding(){
		if (GameData._playerData.Achievements [17] == 0) {
			if(GameData._playerData.BedRoomOpen == 0)
				return;
				if(GameData._playerData.WarehouseOpen ==0)
				return;
			if (GameData._playerData.KitchenOpen==0)
				return;
			if (GameData._playerData.WorkshopOpen == 0)
				return;
			if (GameData._playerData.StudyOpen == 0)
				return;
			if (GameData._playerData.FarmOpen == 0)
				return;
			if (GameData._playerData.PetsOpen == 0)
				return;
			if (GameData._playerData.WellOpen == 0)
				return;
			if (GameData._playerData.AchievementOpen == 0)
				return;
			if (GameData._playerData.AltarOpen == 0)
				return;
			StoreAchievement (17);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [17].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [17].name,true);
		}

		if (GameData._playerData.Achievements [33] == 0) {
			if(GameData._playerData.BedRoomOpen <GameConfigs.MaxLv_BedRoom)
				return;
			if(GameData._playerData.WarehouseOpen <GameConfigs.MaxLv_Warehouse)
				return;
			if (GameData._playerData.KitchenOpen<GameConfigs.MaxLv_Kitchen)
				return;
			if (GameData._playerData.WorkshopOpen <GameConfigs.MaxLv_Workshop)
				return;
			if (GameData._playerData.StudyOpen<GameConfigs.MaxLv_Study)
				return;
			if (GameData._playerData.FarmOpen <GameConfigs.MaxLv_Farm)
				return;
			if (GameData._playerData.PetsOpen <GameConfigs.MaxLv_Pets)
				return;
			if (GameData._playerData.WellOpen <GameConfigs.MaxLv_Well)
				return;
			if (GameData._playerData.AchievementOpen <GameConfigs.MaxLv_Achievement)
				return;
			if (GameData._playerData.AltarOpen <GameConfigs.MaxLv_Altar)
				return;
			StoreAchievement (33);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [33].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [33].name,true);
		}
	}

	public void Fight(string kind){
		if (kind == "Melee") {
			GameData._playerData.meleeAttackCount++;
			_gameData.StoreData ("MeleeAttackCount", GameData._playerData.meleeAttackCount);
			if (GameData._playerData.Achievements [25] == 0) {
				if (GameData._playerData.meleeAttackCount >= LoadTxt.AchievementDic [26].req) {
					StoreAchievement (25);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [25].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [25].name,true);
				}
			}
		}
		if (kind == "Ranged") {
			GameData._playerData.rangedAttackCount++;
			_gameData.StoreData ("RangedAttackCount", GameData._playerData.rangedAttackCount);
			if (GameData._playerData.Achievements [26] == 0) {
				if (GameData._playerData.rangedAttackCount >= LoadTxt.AchievementDic [26].req) {
					StoreAchievement (26);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [26].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [26].name,true);
				}
			}
		}
		if (kind == "Magic" ) {
			GameData._playerData.magicAttackCount++;
			_gameData.StoreData ("MagicAttackCount", GameData._playerData.magicAttackCount);
			if (GameData._playerData.Achievements [27] == 0) {
				if (GameData._playerData.magicAttackCount >= LoadTxt.AchievementDic [27].req) {
					StoreAchievement (27);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [27].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [27].name,true);
				}
			}
		}
	}

	public void CatchThief(int thiefId){
		
		GameData._playerData.thiefCaught++;
		_gameData.StoreData ("ThiefCaught", GameData._playerData.thiefCaught);

		if (GameData._playerData.Achievements [1] == 0) {
			if (GameData._playerData.thiefCaught >= LoadTxt.AchievementDic[1].req) {
				StoreAchievement (1);
				_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [1].name,0);
				_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [1].name,true);
			}
		}

		if (thiefId >= 200) {
			GameData._playerData.legendThiefCaught++;
			_gameData.StoreData ("LegendThiefCaught", GameData._playerData.legendThiefCaught);
			if (GameData._playerData.Achievements [38] == 0) {
				if (GameData._playerData.legendThiefCaught >= LoadTxt.AchievementDic [38].req) {
					StoreAchievement (38);
					_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [38].name,0);
					_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [38].name,true);
				}
			}
		}
	}

	public void RenownChange(){
		if (GameData._playerData.Achievements [10] == 1)
			return;
		if (GameData._playerData.Renown >= LoadTxt.AchievementDic [10].req) {
			StoreAchievement (10);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [10].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [10].name,true);
		}

		if (GameData._playerData.Achievements [9] == 1)
			return;
		if (GameData._playerData.Renown >= LoadTxt.AchievementDic [9].req) {
			StoreAchievement (9);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [9].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [9].name,true);
		}

		if (GameData._playerData.Achievements [8] == 1)
			return;
		if (GameData._playerData.Renown >= LoadTxt.AchievementDic [8].req) {
			StoreAchievement (8);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [8].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [8].name,true);
		}
		
		if (GameData._playerData.Achievements [7] == 1)
			return;
		if (GameData._playerData.Renown >= LoadTxt.AchievementDic [7].req) {
			StoreAchievement (7);
			_floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [7].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [7].name,true);
		}
		
	}

	public void GetToNewDungeon(int lv){
        if (GameData._playerData.Achievements[37] == 1)
            return;
        if (GameData._playerData.dungeonLevelMax >= LoadTxt.AchievementDic[37].req)
        {
            StoreAchievement (37);
            _floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [37].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [37].name,true);
        }
        if (GameData._playerData.Achievements[24] == 1)
            return;
        if (GameData._playerData.dungeonLevelMax >= LoadTxt.AchievementDic[24].req)
        {
            StoreAchievement (24);
            _floating.CallInFloating ("达成新成就:" + LoadTxt.AchievementDic [24].name,0);
			_log.AddLog ("达成新成就:" + LoadTxt.AchievementDic [24].name,true);
        }
	}

	void StoreAchievement(int index){
		GameData._playerData.Achievements [index] = 1;
		_gameData.StoreData ("Achievements", _gameData.GetStrFromTechList (GameData._playerData.Achievements));
	}

}
