using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadTxt : MonoBehaviour {

	public static Building[] buildings;
	public static Mats[] mats;

	public static Dictionary<int,Mats> MatDic;
	public static Dictionary<int,Maps> MapDic;
	public static Dictionary<int,Places> PlaceDic;
	public static Dictionary<int,Technique> TechDic;
	public static Dictionary<int,Thief> ThiefDic;
	public static Dictionary<int,Achievement> AchievementDic;

	public  static DungeonTreasure GetDungeonTreasure(int dtId){
		string[][] strs = ReadTxt.ReadText("dungeon_treasure");
		DungeonTreasure dt = new DungeonTreasure();
		for (int i = 0; i < strs.Length-1; i++) {
			dt.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (dt.id != dtId)
				continue;
			string[][] p = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			dt.reward = new Dictionary<int, int> ();
			for (int j = 0; j < p.Length; j++) {
				dt.reward.Add (int.Parse (p [j] [0]), int.Parse (p [j] [1]));
			}
			return dt;
		}
		Debug.Log ("没有找到地牢宝藏--" + dtId);
		return new DungeonTreasure ();
	}

	public static Plants GetPlant(int pId){
		string[][] strs = ReadTxt.ReadText ("plants");
		Plants p = new Plants();
		for (int i = 0; i < strs.Length-1; i++) {
			p.plantType = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (p.plantType != pId)
				continue;

			string[][] s= ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			if (s != null) {
				p.plantReq = new Dictionary<int, int> ();
				for (int j = 0; j < s.Length; j++)
					p.plantReq.Add (int.Parse (s [j] [0]), int.Parse (s [j] [1]));
			}
			p.plantTime = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			p.plantGrowCycle = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));

			string[][] ss = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			if (ss != null) {
				p.plantObtain = new Dictionary<int, int> ();
				for (int j = 0; j < ss.Length; j++)
					p.plantObtain.Add (int.Parse (ss [j] [0]), int.Parse (ss [j] [1]));
			}
			return p;
		}
		Debug.Log ("没有找到种植类型--" + pId);
		return new Plants ();
	}

	public static Extra_Weapon GetExtraRanged(int exId){
		string[][] strs = ReadTxt.ReadText ("extra_ranged");
		Extra_Weapon ex = new Extra_Weapon();
		for (int i = 0; i < strs.Length-1; i++) {
			ex.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (ex.id != exId)
				continue;

			ex.name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			string[][] prop = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			if (prop != null) {
				ex.property = new Dictionary<int, float> ();
				for (int j = 0; j < prop.Length; j++)
					ex.property.Add (int.Parse (prop [j] [0]), float.Parse (prop [j] [1]));
			}
			return ex;
		}
		Debug.Log ("没有找到远程武器后缀--" + exId);
		return new Extra_Weapon ();
	}

	public static Extra_Weapon GetExtraMelee(int exId){
		string[][] strs = ReadTxt.ReadText ("extra_melee");
		Extra_Weapon ex = new Extra_Weapon();
		for (int i = 0; i < strs.Length-1; i++) {
			ex.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (ex.id != exId)
				continue;
			
			ex.name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			string[][] prop = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			if (prop != null) {
				ex.property = new Dictionary<int, float> ();
				for (int j = 0; j < prop.Length; j++)
					ex.property.Add (int.Parse (prop [j] [0]), float.Parse (prop [j] [1]));
			}
			return ex;
		}
		Debug.Log ("没有找到近战武器后缀--" + exId);
		return new Extra_Weapon ();
	}

	public static Monster GetMonster(int mId){
		string[][] strs = ReadTxt.ReadText("monster");
		Monster m = new Monster();
		for (int i = 0; i < strs.Length-1; i++) {
			m.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (m.id != mId)
				continue;
			m.name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			m.level = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			m.model = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			m.spirit = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			m.speed = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			m.range = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			m.vitalSensibility = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));

			string s = ReadTxt.GetDataByRowAndCol (strs, i + 1, 8);
			int skillId = 0;
			if (s.Contains ("|")) {
				string[] ss = s.Split ('|');
				m.skillList = new Skill[ss.Length];
				for (int j = 0; j < ss.Length; j++) {
					skillId = int.Parse (ss [j]);
					m.skillList [j] = GetSkill (skillId);
				}
			} else {
				m.skillList = new Skill[1];
				skillId = int.Parse (s);
				m.skillList [0] = GetSkill (skillId);
			}

			s=ReadTxt.GetDataByRowAndCol (strs, i + 1, 9);
			string[] s1 = s.Split('|');
			m.bodyPart = new string[3];
			for (int j = 0; j < m.bodyPart.Length; j++)
				m.bodyPart [j] = s1 [j];

			string[][] re = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 10));
			m.drop = new Dictionary<int,float>();
			if (re != null) {
				for (int j = 0; j < re.Length; j++)
					m.drop.Add (int.Parse (re [j] [0]), float.Parse (re [j] [1]));
			}

			m.canCapture = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 11));
			m.groupNum = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 12));
			m.mapOpen = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 13));
			m.renown = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 14));
			m.livePlace = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 15));
			return m;
		}

		Debug.Log ("没有找到怪物--" + mId);
		return new Monster ();
	}

	/// <summary>
	/// 用于获取地牢的怪物，需要提供最小等级和最大等级
	/// </summary>
	/// <returns>The monster.</returns>
	/// <param name="minLv">Minimum lv.</param>
	/// <param name="maxLv">Max lv.</param>
	public static ArrayList GetMonster(int minLv,int maxLv,int livePlace){
		ArrayList mList = new ArrayList ();
		string[][] strs = ReadTxt.ReadText("monster");
		Monster m = new Monster();
		for (int i = 0; i < strs.Length-1; i++) {
			m.level = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			m.livePlace = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 15));
			if (m.level < minLv || m.level > maxLv || m.livePlace != livePlace)
				continue;

			m.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			m.name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			m.model = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			m.spirit = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			m.speed = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			m.range = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			m.vitalSensibility = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));

			string s = ReadTxt.GetDataByRowAndCol (strs, i + 1, 8);
			int skillId = 0;
			if (s.Contains ("|")) {
				string[] ss = s.Split ('|');
				m.skillList = new Skill[ss.Length];
				for (int j = 0; j < ss.Length; j++) {
					skillId = int.Parse (ss [j]);
					m.skillList [j] = GetSkill (skillId);
				}
			} else {
				m.skillList = new Skill[1];
				skillId = int.Parse (s);
				m.skillList [0] = GetSkill (skillId);
			}

			s=ReadTxt.GetDataByRowAndCol (strs, i + 1, 9);
			string[] s1 = s.Split('|');
			m.bodyPart = new string[3];
			for (int j = 0; j < m.bodyPart.Length; j++)
				m.bodyPart [j] = s1 [j];

			string[][] re = ReadTxt.GetRequire (ReadTxt.GetDataByRowAndCol (strs, i + 1, 10));
			m.drop = new Dictionary<int,float>();
			if (re != null) {
				for (int j = 0; j < re.Length; j++)
					m.drop.Add (int.Parse (re [j] [0]), float.Parse (re [j] [1]));
			}

			m.canCapture = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 11));
			m.groupNum = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 12));
			m.mapOpen = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 13));
			m.renown = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 14));
			mList.Add(m);
		}

		return mList;
	
	}

	public static MonsterTitle GetMonsterTitle(int mtId){
		string[][] strs = ReadTxt.ReadText ("monster_title");
		MonsterTitle mt = new MonsterTitle();
		for (int i = 0; i < strs.Length-1; i++) {
			mt.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (mt.id != mtId)
				continue;
			mt.title = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			mt.hpBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			mt.atkBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			mt.defBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			mt.attSpeedBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			mt.speedBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			mt.dodgeBonus = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));
			return mt;
		}
		Debug.Log ("没有找到怪物后缀--" + mtId);
		return new MonsterTitle ();
	}
		
	public static MonsterModel GetMonsterModel(int mdId){
		string[][] strs = ReadTxt.ReadText ("monster_model");
		MonsterModel mm = new MonsterModel();
		for (int i = 0; i < strs.Length -1; i++) {
			mm.modelType = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (mm.modelType != mdId)
				continue;
			mm.hp = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 1));
			mm.hp_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			mm.atk = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			mm.atk_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			mm.def = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			mm.def_inc = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			mm.hit = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7));
			mm.dodge = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 8));
			return mm;
		}
		Debug.Log ("没有找到怪物模板--" + mdId);
		return new MonsterModel ();
	}

	public static Skill GetSkill(int skillId){
		string[][] strs = ReadTxt.ReadText ("skill");
		Skill s = new Skill();
		for (int i = 0; i < strs.Length-1; i++) {
			s.id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
			if (s.id != skillId)
				continue;
			s.name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
			s.target = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 2));
			s.power = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
			s.effectId = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
			s.effectProp = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
			s.castSpeed = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
			return s;
		}
		Debug.Log ("没有找到技能--" + skillId);
		return new Skill ();
	}

	void Awake () {
		MatDic = new Dictionary<int, Mats> ();
		MapDic = new Dictionary<int, Maps> ();
		PlaceDic = new Dictionary<int, Places> ();
		TechDic = new Dictionary<int, Technique> ();
		ThiefDic = new Dictionary<int, Thief> ();
		AchievementDic = new Dictionary<int, Achievement> ();

		LoadBuildings ();
		LoadMats ();
		LoadMaps ();
		LoadTech ();
		LoadThief ();
		LoadAchievement ();
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
			mats [i].desc = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
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
            mats [i].description = ReadTxt.GetDataByRowAndCol (strs, i + 1, 14);
			MatDic.Add (mats [i].id, mats [i]);
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
