using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadTxt : MonoBehaviour {

	public static Building[] buildings;
	public static Mats[] mats;

	public static Dictionary<int,Mats> MatDic;
	public static Dictionary<int,Extra_Weapon> ExtraMelee;
	public static Dictionary<int,Extra_Weapon> ExtraRanged;
	public static Dictionary<int,Plants> PlantsDic;
	public static Dictionary<int,Maps> MapDic;
	public static Dictionary<int,Places> PlaceDic;
	public static Dictionary<int,Monster> MonsterDic;
	public static Dictionary<int,MonsterModel> MonsterModelDic;
	public static Dictionary<int,MonsterTitle> MonsterTitleDic;
	public static Dictionary<int,Skill> skillDic;//3
	public static Dictionary<int,SkillEffect> SkillEffectDic;//4
	public static Dictionary<int,Technique> TechDic;
	public static Dictionary<int,Thief> ThiefDic;
	public static Dictionary<int,Achievement> AchievementDic;
	public static Dictionary<int,DungeonTreasure> DungeonTreasureList;

	void Awake () {
		MatDic = new Dictionary<int, Mats> ();
		ExtraMelee = new Dictionary<int, Extra_Weapon> ();
		ExtraRanged = new Dictionary<int, Extra_Weapon> ();
		PlantsDic = new Dictionary<int, Plants> ();
		MapDic = new Dictionary<int, Maps> ();
		PlaceDic = new Dictionary<int, Places> ();
		MonsterDic = new Dictionary<int, Monster> ();
		MonsterModelDic = new Dictionary<int, MonsterModel> ();
		MonsterTitleDic = new Dictionary<int, MonsterTitle> ();
		skillDic = new Dictionary<int, Skill> ();
		SkillEffectDic = new Dictionary<int, SkillEffect> ();
		TechDic = new Dictionary<int, Technique> ();
		ThiefDic = new Dictionary<int, Thief> ();
		AchievementDic = new Dictionary<int, Achievement> ();
		DungeonTreasureList = new Dictionary<int, DungeonTreasure> ();

		LoadBuildings ();
		LoadMats ();
		LoadPlants ();
		LoadMaps ();
		LoadTech ();
		LoadMonster ();
		LoadExtra ();
		LoadThief ();
		LoadAchievement ();
		LoadDungeonTreasure ();
	}
		

	void LoadBuildings()
	{
		string[][] strs = ReadTxt.ReadText("buildings");
		buildings = new Building[strs.Length-1];
		for (int i=0; i<buildings.Length; i++) {
			buildings [i] = new Building ();
			buildings [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0)); //i+1 means to remove the first line
			buildings [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			buildings [i].maxLv = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			SetMaxLv (buildings [i].name, buildings [i].maxLv);
			string rs = ReadTxt.GetDataByRowAndCol (strs, i + 1, 3);
			if (rs != "") {	
				string[][] req = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
				for (int j = 0; j < req.Length; j++)
					buildings [i].combReq.Add (int.Parse (req [j] [0]), int.Parse (req [j] [1]));
			}
			buildings [i].tips = ReadTxt.GetDataByRowAndCol (strs, i + 1, 4);
			buildings [i].timeCost = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
		}
	}

	void SetMaxLv(string buildingName,int maxLv){
		switch (buildingName) {
		case "休息室":
			GameConfigs.MaxLv_BedRoom = maxLv;
			break;
		case "仓库":
			GameConfigs.MaxLv_Warehouse = maxLv;
			break;
		case "厨房":
			GameConfigs.MaxLv_Kitchen = maxLv;
			break;
		case "工作台":
			GameConfigs.MaxLv_Workshop = maxLv;
			break;
		case "研究室":
			GameConfigs.MaxLv_Study = maxLv;
			break;
		case "农田":
			GameConfigs.MaxLv_Farm = maxLv;
			break;
		case "宠物笼":
			GameConfigs.MaxLv_Pets = maxLv;
			break;
		case "水井":
			GameConfigs.MaxLv_Well = maxLv;
			break;
		case "成就":
			GameConfigs.MaxLv_Achievement = maxLv;
			break;
		case "祭坛":
			GameConfigs.MaxLv_Altar = maxLv;
			break;
		default:
			Debug.Log ("Wrong Building Name" + buildingName);
			break;
		}
	}

	void LoadMats(){
		string[][] strs = ReadTxt.ReadText ("mats");
		mats = new Mats[strs.Length - 1];
		for (int i = 0; i < mats.Length; i++) {
			mats [i] = new Mats ();
			mats [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			mats [i].type = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			mats [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 2);
			mats [i].desc = ReadTxt.GetDataByRowAndCol (strs, i + 1, 3);
			mats [i].price = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			string[][] prop = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			if (prop != null) {
				mats[i].property = new Dictionary<int,float>();
				for (int j = 0; j < prop.Length; j++) 
					mats [i].property.Add (int.Parse(prop[j][0]), float.Parse(prop[j][1]));
			}
			string[][] req = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			if (req != null) {
				mats [i].combGet = int.Parse (req [0] [0]);
				mats[i].combReq = new Dictionary<int,int>();
				for (int j = 1; j < req.Length; j++)
					mats [i].combReq.Add (int.Parse (req [j] [0]), int.Parse (req [j] [1]));
			}
			mats [i].needBlueprint = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));
			mats [i].makingType = ReadTxt.GetDataByRowAndCol (strs, i + 1, 8);
			mats [i].makingTime = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 9));
			mats [i].castSpirit = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 10));
			mats [i].tags = ReadTxt.GetDataByRowAndCol (strs, i + 1, 11);
			mats [i].skillId = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 12));
			mats [i].quality = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 13));
			MatDic.Add (mats [i].id, mats [i]);
		}
	}

	void LoadMonster(){
		string[][] strs = ReadTxt.ReadText("monster");
		Monster[] m = new Monster[strs.Length-1];
		for (int i = 0; i < m.Length; i++) {
			m [i] = new Monster ();
			m [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			m [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			m [i].level = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			m [i].model = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			m [i].spirit = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			m [i].speed = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			m [i].range = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			m [i].vitalSensibility = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));
//			Debug.Log ("This monster id = " + m [i].id);

			string s = ReadTxt.GetDataByRowAndCol (strs, i + 1, 8);
			if (s.Contains ("|")) {
				string[] ss = s.Split ('|');
				m [i].skills = new int[ss.Length];
				for (int j = 0; j < ss.Length; j++)
					m [i].skills [j] = int.Parse (ss [j]);
			} else {
				m [i].skills = new int[1];
				m [i].skills [0] = int.Parse (s);
			}

			s=ReadTxt.GetDataByRowAndCol (strs, i + 1, 9);
			string[] s1 = s.Split('|');
			m [i].bodyPart = new string[3];
			for (int j = 0; j < m [i].bodyPart.Length; j++)
				m [i].bodyPart [j] = s1 [j];

			string[][] re = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 10));
			if (re != null) {
				m[i].drop = new Dictionary<int,float>();
				for (int j = 0; j < re.Length; j++)
					m [i].drop.Add (int.Parse (re [j] [0]), float.Parse (re [j] [1]));
			}

			m [i].canCapture = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 11));
			m [i].groupNum = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 12));
			m [i].mapOpen = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 13));
			m [i].renown = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 14));
			MonsterDic.Add (m [i].id, m [i]);
		}

		strs = ReadTxt.ReadText ("monster_title");
		MonsterTitle[] mt = new MonsterTitle[strs.Length - 1];
		for (int i = 0; i < mt.Length; i++) {
			mt [i] = new MonsterTitle ();
			mt [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			mt [i].title = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			mt [i].hpBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			mt [i].atkBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			mt [i].defBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			mt [i].attSpeedBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			mt [i].speedBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			mt [i].dodgeBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));

			MonsterTitleDic.Add (mt [i].id, mt [i]);
		}

		strs = ReadTxt.ReadText ("monster_model");
		MonsterModel[] mm = new MonsterModel[strs.Length - 1];
		for (int i = 0; i < mm.Length; i++) {
			mm [i] = new MonsterModel ();
			mm [i].modelType = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			mm [i].hp = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			mm [i].hp_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			mm [i].atk = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			mm [i].atk_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			mm [i].def = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			mm [i].def_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			mm [i].hit = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));
			mm [i].dodge = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 8));
			MonsterModelDic.Add (mm [i].modelType, mm [i]);
		}

		strs = ReadTxt.ReadText ("skill");
		Skill[] skills = new Skill[strs.Length - 1];
		for (int i = 0; i < skills.Length; i++) {
			skills [i] = new Skill ();
			skills[i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			skills[i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			skills[i].target = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			skills[i].power = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			skills[i].effectId = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			skills[i].effectProp = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			skills[i].castSpeed = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));

			skillDic.Add (skills [i].id, skills [i]);
		}
	}

	void LoadTech(){
		string[][] strs = ReadTxt.ReadText ("techniques");
		Technique[] techs = new Technique[strs.Length - 1];
		for (int i = 0; i < techs.Length; i++) {
			techs [i] = new Technique ();
			techs[i].id=int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			techs[i].name=ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			techs[i].type=int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			techs[i].lv=int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			techs[i].maxLv=int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			techs [i].req = new Dictionary<int, int> ();
			string[][] ss = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			if (ss != null) {
				for (int j = 0; j < ss.Length; j++) {
					techs [i].req.Add (int.Parse (ss [j] [0]), int.Parse (ss [j] [1]));
				}
			}
			techs[i].timeCost = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			techs[i].desc = ReadTxt.GetDataByRowAndCol (strs, i + 1, 7);
//			Debug.Log (techs [i].id + "," + techs [i].name);
			TechDic.Add (techs [i].id, techs [i]);
		}
	}

	void LoadThief(){
		string[][] strs = ReadTxt.ReadText("thief");
		Thief[] t = new Thief[strs.Length - 1];
		for (int i = 0; i < t.Length; i++) {
			t [i] = new Thief ();
			t [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			t [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			t [i].monsterId = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			t [i].weight = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));

			ThiefDic.Add (t [i].id, t [i]);
		}
	}

	void LoadAchievement(){
		string[][] strs = ReadTxt.ReadText ("achievement");
		Achievement[] a = new Achievement[strs.Length - 1];
		for (int i = 0; i < a.Length; i++) {
			a [i] = new Achievement ();
			a [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			a [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			a [i].desc = ReadTxt.GetDataByRowAndCol (strs, i + 1, 2);
			a [i].req = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));

//			Debug.Log ("id = " + a [i].id + ", name = " + a [i].name + ", req = " + a [i].req);
			AchievementDic.Add (a [i].id, a [i]);
		}
	}

	void LoadExtra(){
		string[][] strs = ReadTxt.ReadText ("extra_melee");
		Extra_Weapon[] ex = new Extra_Weapon[strs.Length - 1];
		for (int i = 0; i < ex.Length; i++) {
			ex [i] = new Extra_Weapon ();
			ex [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			ex [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			string[][] prop = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			if (prop != null) {
				ex [i].property = new Dictionary<int, float> ();
				for (int j = 0; j < prop.Length; j++)
					ex [i].property.Add (int.Parse (prop [j] [0]), float.Parse (prop [j] [1]));
			}

			ExtraMelee.Add (ex [i].id, ex [i]);
		}

		strs = ReadTxt.ReadText ("extra_ranged");
		ex = new Extra_Weapon[strs.Length - 1];
		for (int i = 0; i < ex.Length; i++) {
			ex [i] = new Extra_Weapon ();
			ex [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			ex [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			string[][] prop = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			if (prop != null) {
				ex [i].property = new Dictionary<int, float> ();
				for (int j = 0; j < prop.Length; j++)
					ex [i].property.Add (int.Parse (prop [j] [0]), float.Parse (prop [j] [1]));
			}

			ExtraRanged.Add (ex [i].id, ex [i]);
		}
	}

	void LoadDungeonTreasure(){
		string[][] strs = ReadTxt.ReadText("dungeon_treasure");
		DungeonTreasure[] dt = new DungeonTreasure[strs.Length - 1];
		for (int i = 0; i < dt.Length; i++) {
			dt [i] = new DungeonTreasure ();
			dt [i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));

			string[][] p = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			dt [i].reward = new Dictionary<int, int> ();
			for (int j = 0; j < p.Length; j++) {
				dt [i].reward.Add (int.Parse (p [j] [0]), int.Parse (p [j] [1]));
			}

			DungeonTreasureList.Add (dt [i].id, dt [i]);
		}
	}

	void LoadPlants(){
		string[][] strs = ReadTxt.ReadText ("plants");
		Plants[] p = new Plants[strs.Length - 1];
		for (int i = 0; i < p.Length; i++) {
			p [i] = new Plants ();
			p [i].plantType = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			string[][] s= ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			if (s != null) {
				p [i].plantReq = new Dictionary<int, int> ();
				for (int j = 0; j < s.Length; j++)
					p [i].plantReq.Add (int.Parse (s [j] [0]), int.Parse (s [j] [1]));
			}
			p [i].plantTime = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			p [i].plantGrowCycle = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));

			string[][] ss = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			if (ss != null) {
				p [i].plantObtain = new Dictionary<int, int> ();
				for (int j = 0; j < ss.Length; j++)
					p [i].plantObtain.Add (int.Parse (ss [j] [0]), int.Parse (ss [j] [1]));
			}

			PlantsDic.Add (p [i].plantType, p [i]);
		}
	}		
		
	void LoadMaps(){
		string[][] strs = ReadTxt.ReadText ("maps");
		Maps[] m = new Maps[strs.Length - 1];
		for (int i = 0; i < m.Length; i++) {
			m [i] = new Maps ();
			m[i].id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			m[i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			string[][] ss = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			if (ss != null) {
				m [i].distances = new Dictionary<int, int> ();
				for (int j = 0; j < ss.Length; j++) {
					m [i].distances.Add (int.Parse (ss [j] [0]), int.Parse (ss [j] [1]));
				}
			}
			m[i].desc = ReadTxt.GetDataByRowAndCol (strs, i + 1, 3);

			//探索本地图可能开启哪个地图
			m [i].mapNext = new ArrayList ();
			string str = ReadTxt.GetDataByRowAndCol (strs, i + 1, 4);
			if (str != "0") {
				string[] s = str.Split ('|');
				for (int j = 0; j < s.Length; j++) {
					m [i].mapNext.Add (int.Parse (s [j]));	
				}
			}

			MapDic.Add (m [i].id, m [i]);
		}
	}

	private PlaceUnit[] p;

	public void LoadPlaces(bool isRebirth){
		string[][] strs = ReadTxt.ReadText ("places");
		p = new PlaceUnit[strs.Length-1];
		for (int i = 0; i < p.Length; i++) {
			p [i] = new PlaceUnit ();
			p [i].unitId = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			p [i].actionType = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));

			string s = "PlaceUnit" + p [i].unitId.ToString () + "Para" + (isRebirth ? "_Memmory" : "");
			if (PlayerPrefs.GetString (s, "") == "") {
				p [i].actionParam = 1 + ";" + ReadTxt.GetDataByRowAndCol (strs, i + 1, 2);
			} else {
				p [i].actionParam = PlayerPrefs.GetString (s, "");
			}
			p [i].name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 3);
			p [i].desc = ReadTxt.GetDataByRowAndCol (strs, i + 1, 4);
		}

		for (int i = 0; i < MapDic.Count; i++) {
			Places ps = new Places ();
			ps.id = MapDic [i].id;
			for (int j = 0; j < p.Length; j++) {
				if ((int)(p [j].unitId / 100) == ps.id) {
					ps.placeUnits.Add (p [j]);
				}	
			}
			PlaceDic.Add (ps.id, ps);
		}
	}

	public void StorePlaceMemmory(bool isRebirth){
		//如果不是rebirth，就存储当前数据到memmory；如果是rebirth，就用memmory覆盖当前数据
		string s = isRebirth?"":"_Memory";
		for (int i = 0; i < p.Length; i++) {
			PlayerPrefs.SetString ("PlaceUnit" + p [i].unitId.ToString() + "Para" + s, p [i].actionParam);
		}
	}

	public void StorePlaceUnit(PlaceUnit pu){
//		p [pu.unitId] = pu;
		PlayerPrefs.SetString ("PlaceUnit" + pu.unitId + "Para", pu.actionParam);
	}
}
