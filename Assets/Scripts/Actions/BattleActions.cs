﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BattleActions : MonoBehaviour {

	public Text enemyName;
	public Text enemyDistance;
	public GameObject battleLogContainer;
	public Slider enemyHpSlider;
	public Image enemyHpFill;
	public Slider myHpSlider;
	public Image myHpFill;
	public Text timeBarEnd;
	public GameObject myPoint;
	public GameObject enemyPoint;

	public Button bp;
	public Button map;
	public Button meleeAttack;
	public Button rangedAttack;
	public Button magicAttack;
	public Button jumpForward;
	public Button jumpBackward;
	public Button capture;
	public Button autoButton;
	public Button returnButton;
	public Text[] battleLogs = new Text[9];

	public Image autoOn;
	public Image autoOff;

	private Unit enemy;
	private float myNextTurn;
	private float enemyNextTurn;
	private float distance;
	private GameData _gameData;
	private PanelManager _panelManager;
	private FloatingActions _floating;
//	private ArrayList logs;
//	private int logIndex;
//	private GameObject battleLog;
	private float enemyMaxHp;
	private bool isAuto;
	private int captureFailTime;
	private int thisEnemyIndex;
	private Monster[] thisMonsters;

	private AchieveActions _achieveActions;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		this.gameObject.transform.localPosition = new Vector3 (-2000, 0, 0);
//		logs = new ArrayList ();
//		battleLog = Instantiate (Resources.Load ("battleLog")) as GameObject;
//		battleLog.SetActive (false);
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
		_achieveActions = this.gameObject.GetComponentInParent<AchieveActions> ();
	}

	public void InitializeBattleField(Monster[] monsters,bool isAttacked){
		thisMonsters = monsters;
		bp.interactable = false;
		map.interactable = false;
		thisEnemyIndex = 0;
		autoButton.gameObject.SetActive (true);
		returnButton.gameObject.SetActive (false);
		ClearLog ();

		if (isAttacked) {
			if (monsters.Length > 1)
				AddLog ("注意，你被" + monsters.Length + "名敌人围攻了!", 0);
			else
				AddLog ("注意，你被" + monsters [0].name + "偷袭了！", 0);
		} else {
			if (monsters.Length > 1)
				AddLog ("你发现了" + monsters.Length + "名敌人!", 0);
			else
				AddLog ("你发现了" + monsters [0].name + "!", 0);
		}

		SetEnemy ();

		if (isAttacked)
			EnemyCastSkill ();
	}

	void SetEnemy(){
		SetState ();
		int titleIndex = Algorithms.GetIndexByRange (0, LoadTxt.MonsterTitleDic.Count);
		GetEnemyProperty (thisMonsters [thisEnemyIndex-1], titleIndex);
		ResetPanel ();
	}

	void SetState(){
		thisEnemyIndex++;
		myNextTurn = 0;
		enemyNextTurn = 0;
		captureFailTime = 0;
		isAuto = false;
	}

	void CallOutBattlePanel(){
		bp.interactable = true;
		map.interactable = true;
		this.gameObject.transform.localPosition = new Vector3 (-2000, 0, 0);
	}
		
	void ClearLog(){
		for (int i = 0; i < battleLogs.Length; i++) {
			battleLogs [i].text = "";
		}
	}

	void SetActions(){
		if (isAuto) {
			meleeAttack.interactable = false;
			rangedAttack.interactable = false;
			magicAttack.interactable = false;
			jumpForward.interactable = false;
			jumpBackward.interactable = false;
			capture.interactable = false;
		} else {
			meleeAttack.interactable = (GameData._playerData.property [19] >= distance);
			rangedAttack.interactable = (GameData._playerData.RangedId>0 && GameData._playerData.property [20] >= distance);
			magicAttack.interactable = (GameData._playerData.MagicId > 0);
			jumpForward.interactable = true;
			jumpBackward.interactable = true;
			capture.interactable = ((enemy.hp <= enemyMaxHp * 0.3f) && enemy.canCapture > 0 && captureFailTime < 3);
		}
	}

	void GetEnemyProperty(Monster m,int titleIndex){
		enemy = new Unit ();
		enemy.monsterId = m.id;
		enemy.name = m.name;
		enemy.level = m.level;
		enemy.hp = LoadTxt.MonsterModelDic [m.model].hp + LoadTxt.MonsterModelDic [m.model].hp_inc * (m.level - 1);
		enemy.hp *= 1 + LoadTxt.MonsterTitleDic [titleIndex].hpBonus ;
		enemyMaxHp = enemy.hp;

		enemy.spirit = m.spirit;

		enemy.atk = LoadTxt.MonsterModelDic [m.model].atk + LoadTxt.MonsterModelDic [m.model].atk_inc * (m.level - 1);
		enemy.atk *= 1 + LoadTxt.MonsterTitleDic [titleIndex].atkBonus ;

		enemy.def = LoadTxt.MonsterModelDic [m.model].def + LoadTxt.MonsterModelDic [m.model].def_inc * (m.level - 1);
		enemy.def *= 1 + LoadTxt.MonsterTitleDic [titleIndex].defBonus ;

		enemy.hit = LoadTxt.MonsterModelDic [m.model].hit;

		enemy.dodge = LoadTxt.MonsterModelDic [m.model].dodge;
		enemy.dodge *= 1 + LoadTxt.MonsterTitleDic [titleIndex].dodgeBonus ;

		enemy.speed = m.speed * (1 + LoadTxt.MonsterTitleDic [titleIndex].speedBonus );
		enemy.range = m.range;
		enemy.castSpeedBonus = LoadTxt.MonsterTitleDic [titleIndex].attSpeedBonus ;
		enemy.skills = m.skills;
		enemy.drop = m.drop;
		enemy.vitalSensibility = m.vitalSensibility;
		enemy.hit_Body = m.bodyPart[0];
		enemy.hit_Vital = m.bodyPart[1];
		enemy.hit_Move = m.bodyPart [2];
		enemy.canCapture = m.canCapture;
		Debug.Log ("Hp:" + enemy.hp + " Spirit:" + enemy.spirit + " hit:" + enemy.hit);
	}

	void ResetPanel(){
		int maxdistance = Mathf.Max ((int)(enemy.range), (int)GameData._playerData.property [19], (int)GameData._playerData.property [20]);
		distance = Algorithms.GetIndexByRange (1, maxdistance + 1);

		enemyName.text = enemy.name;
		enemyDistance.text = distance.ToString() +"m";
		SetMyHpSlider ();
		SetEnemyHpSlider ();
		SetPoint ();
		SetActions ();
	}

	private float maxTime = 0f;
	void SetPoint(){
		if (maxTime == 0f)
			maxTime = 20f;
		else if (Mathf.Max (myNextTurn, enemyNextTurn) > maxTime * 0.9f)
			maxTime = maxTime * 3f;
		timeBarEnd.text = maxTime.ToString ();
		float myPointX = 800f * myNextTurn / (float)maxTime - 400f;
		float enemyPointX = 800f * enemyNextTurn / (float)maxTime - 400f;
		myPoint.transform.DOLocalMoveX (myPointX, 0.5f);
		enemyPoint.transform.DOLocalMoveX (enemyPointX, 0.5f);
	}

	/// <summary>
	/// Add log.需要处理新加一行，显示被遮挡的问题
	/// </summary>
	/// <param name="s">S.</param>
	/// <param name="isGood">Is good:0normal 1good 2bad.</param>
	void AddLog(string s,int isGood){
//		logIndex++;
//		Text t;
//		if (logs.Count > logIndex) {
//			t = logs [logIndex] as Text;
//			t.gameObject.SetActive (true);
//		} else {
//			GameObject o = Instantiate (battleLog) as GameObject;
//			o.SetActive (true);
//			t = o.GetComponent<Text> ();
//			o.gameObject.transform.SetParent (battleLogContainer.transform);
//			o.gameObject.transform.localPosition = Vector3.zero;
//			o.gameObject.transform.localScale = Vector3.one;
//			logs.Add (t);
//		}


		for (int i = 0; i < 8; i++) {
			battleLogs [i].text = battleLogs [i + 1].text;
			battleLogs [i].color = new Color (battleLogs [i + 1].color.r, battleLogs [i + 1].color.g, battleLogs [i + 1].color.b, battleLogs [i + 1].color.a - 0.1f);
		}

		battleLogs[8].text =  "→" + s;
		if (isGood == 1)
			battleLogs[8].color = new Color(0f,1f,0f,1f);
		else if (isGood == 2)
			battleLogs[8].color = new Color(1f,0f,0f,1f);
		else
			battleLogs[8].color = new Color(1f,1f,1f,1f);

//		battleLogContainer.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,50 * logIndex);
	}

	void AutoFight(){
		while (GameData._playerData.hpNow > 0 && enemy.hp > 0) {
			if (myNextTurn < enemyNextTurn) {
				//如果近战攻击高，则向前移动后再攻击；如果远程攻击高，直接用远程攻击
				if (GameData._playerData.property [13] > GameData._playerData.property [14]) {
					if (distance > GameData._playerData.property [19]) {
						Move (true, GameData._playerData.property [23], false);
					} else {
						MeleeFight ();
					}
				} else{
					if (distance > GameData._playerData.property [20]) {
						Move (true, GameData._playerData.property [23],false);
					} else {
						RangedFight ();
					}
				}
			} else {
				if (distance > enemy.range) {
					Move (true, enemy.speed,true);
				} else {
					EnemyCastSkill ();
				}
			}
		}
	}

	void Move(bool forward,float speed,bool isEnemyMove){

		int f = forward ? -1 : 1;
		distance = Mathf.Max (0, distance + speed * f);
		string s = forward ? "前进" : "后退";
		if (isEnemyMove) {
			enemyNextTurn += 1;
			AddLog (enemy.name  + s + "了", 0);
		} else {
			myNextTurn += 1;
			AddLog ("You moved " + s + "了" , 0);
		}

		enemyDistance.text = distance + "米";
		SetPoint ();
		SetActions ();
	}

	void Fight(int skillId){
		AddEffect (LoadTxt.skillDic [skillId], true,0);
	}

	void Fight(float hit,float dodge,float vitalSensibility,float spirit,float atk,float def,int skillId,bool isMyAtk){
		int hitRate = Algorithms.IsDodgeOrCrit (hit, dodge, vitalSensibility, spirit);
		string hitPart = "";
		if (hitRate == 0) {
			Debug.Log ("Missed!");
			AddLog ((isMyAtk ? "你" : enemy.name) + "发起攻击，但是" + (isMyAtk ? enemy.name : "你") + "灵巧地躲开了!",0);
			return;
		} else if (hitRate == 1) {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Body) : "身体";
		} else {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Vital) : "头部";
		}

		int dam = Algorithms.CalculateDamage (atk, def, skillId, hitRate,isMyAtk);
		Debug.Log ("Atk = " + atk + ", Dam = " + dam);

		if (isMyAtk) {
			enemy.hp -= dam;
			SetEnemyHpSlider ();
			if(skillId>0)
				AddEffect (LoadTxt.skillDic [skillId], isMyAtk, dam);
			AddLog ("你击中了" + enemy.name + "的" + hitPart + "。", 0);
		} else {
			_gameData.ChangeProperty (0, -dam);
			SetMyHpSlider ();
			AddEffect (LoadTxt.skillDic [skillId], isMyAtk, dam);
			AddLog (enemy.name + "击中了你的" + hitPart + "。", 0);
		}

		CheckBattleEnd ();
	}

	string GetHitPart(string s){
		string[] ss = s.Split ('|');
		return ss [Algorithms.GetIndexByRange (0, ss.Length)];
	}

	void AddEffect(Skill s,bool isMyAtk,int dam){
		if (s.effectId == 0)
			return;
		if (s.effectId > 0) {
			int r = Algorithms.GetIndexByRange (0, 100);
			if (r < s.effectProp) {
				switch (s.effectId) {
				case 100:
					if (isMyAtk) {
						enemyNextTurn += 2;
						AddLog (enemy.name + " is slowed down for 2s.", 0);
					} else {
						myNextTurn += 2;
						AddLog ("You're slowed down for 2s.", 0);
					}
					break;
				case 101:
					if (isMyAtk) {
						enemyNextTurn += 3;
						AddLog (enemy.name + " is confused for 3s.", 0);
					} else {
						myNextTurn += 3;
						AddLog ("You're confused for 3s.", 0);
					}
					break;
				case 102:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is deluded for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("You're deluded for 5s.", 0);
					}
					break;
				case 103:
					if (isMyAtk) {
						enemyNextTurn += 7;
						AddLog (enemy.name + " is dizzied for 7s.", 0);
					} else {
						myNextTurn += 7;
						AddLog ("You're dizzied for 7s.", 0);
					}
					break;
				case 104:
					if (isMyAtk) {
						enemyNextTurn += 7;
						AddLog (enemy.name + " is frozen for 7s.", 0);
					} else {
						myNextTurn += 7;
						_gameData.ChangeProperty (10, -5);
						AddLog ("You're frozen for 7s.", 0);
						AddLog ("You feel a bit hot, temperature +5", 0);
					}
					break;
				case 105:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is petrified for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("You're petrified for 5s.", 0);
					}
					break;
				case 106:
					if (isMyAtk) {
						_gameData.ChangeProperty (2, 5);
						enemy.spirit -= 5;
						AddLog ("You recovered 5 spirit, " + enemy.name + " lost 5 spirit.", 0);
					} else {
						_gameData.ChangeProperty (2, -5);
						enemy.spirit += 5;
						AddLog ("You lost 5 spirit, " + enemy.name + " recovered 5 spirit.", 0);
					}
					break;
				case 107:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is chained for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("You're chained for 5s.", 0);
					}
					break;
				case 109:
					int hpPlus = (int)(dam * 0.3f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus);
						AddLog ("You recovered " + hpPlus + " hp", 0);
					} else {
						enemy.hp += hpPlus;
						AddLog (enemy.name + " recovered " + hpPlus + " hp", 0);
					}
					break;
				case 110:
					int hpPlus1 = (int)(dam * 0.5f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus1);
						AddLog ("You recovered " + hpPlus1 + " hp", 0);
					} else {
						enemy.hp += hpPlus1;
						AddLog (enemy.name + " recovered " + hpPlus1 + " hp", 0);
					}
					break;
				case 111:
					if (!isMyAtk) {
						_gameData.ChangeProperty (10, 5);
						AddLog ("You feel a bit hot, temperature +5", 0);
					}
					break;
				default:
					Debug.Log ("Incorrect effectId: " + s.effectId);
					break;
				}
			}
			SetMyHpSlider ();
			SetEnemyHpSlider ();
		}
	}

	void CheckBattleEnd(){
		if (enemy.hp > 0) {
			CheckEnemyAction ();
			return;
		}

		Dictionary<int,int> drop = Algorithms.GetReward (enemy.drop);
		string s = "";
		if (drop.Count > 0) {
			s="你击败了"+enemy.name+"，获得";
			foreach (int key in drop.Keys) {
				int itemId = GenerateItemId (key);
				_gameData.AddItem (itemId, drop [key]);
				s += LoadTxt.MatDic [key].name + " ×" + drop [key] + ",";

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
			s = s.Substring (0, s.Length - 1) + ".";
		} else {
			s="你击败了"+enemy.name+"，但什么也没找到。";
		}
		AddLog (s,1);
		_achieveActions.DefeatEnemy (enemy.monsterId);
		StartCoroutine (WaitAndCheck ());
	}

	IEnumerator WaitAndCheck(){
		meleeAttack.interactable = false;
		rangedAttack.interactable = false;
		magicAttack.interactable = false;
		jumpForward.interactable = false;
		jumpBackward.interactable = false;
		capture.interactable = false;
		yield return new WaitForSeconds (1f);
		CheckNextEnemy ();

	}

	void CheckNextEnemy(){
		if (thisEnemyIndex < thisMonsters.Length) {
			SetEnemy ();
		} else {
			autoButton.gameObject.SetActive (false);
			returnButton.gameObject.SetActive (true);
		}
	}

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


	void SetEnemyHpSlider(){
		enemyHpSlider.value = enemy.hp / enemyMaxHp;
		enemyHpFill.color = GetColor (enemy.hp / enemyMaxHp);
	}

	void SetMyHpSlider(){
		myHpSlider.value = GameData._playerData.property [0] / GameData._playerData.property [1];
		myHpFill.color = GetColor (GameData._playerData.property [0] / GameData._playerData.property [1]);
	}

	Color GetColor(float value){
		Color c = new Color ();

		if (value > 0.5)
			c = new Color ((1f - value) * 300f / 255f, 150f / 255F, 0F,1f);
		else if (value <= 0)
			c = new Color (1f, 1f, 1f, 0f);
		else
			c = new Color (150f / 255f, value * 300f / 255f, 0f,1f);



		return c;
	}

	public void MeleeFight(){
		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.MeleeId/10000)].skillId;
		myNextTurn += GameData._playerData.property [21];
		SetPoint ();
		Fight (GameData._playerData.property [16], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], GameData._playerData.property [13], enemy.def, skillId, true);
		//Achievement
		_achieveActions.Fight ("Melee");
	}
	public void RangedFight(){
		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.RangedId/10000)].skillId;
		myNextTurn += GameData._playerData.property [22];
		SetPoint ();
		Fight (GameData._playerData.property [17], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], GameData._playerData.property [14], enemy.def, skillId, true);

		//Achievement
		_achieveActions.Fight ("Ranged");
	}
	public void MagicFight(){
		myNextTurn += 1;
		SetPoint ();
		_gameData.ChangeProperty (2, -(int)(LoadTxt.MatDic [GameData._playerData.MagicId].castSpirit * GameData._playerData.MagicCostRate));
		int dam = (int)(GameData._playerData.property [24] * GameData._playerData.MagicPower * Algorithms.GetIndexByRange (80, 120) / 100);
		enemy.hp -= dam;
		CheckBattleEnd ();

		//Achievement
		_achieveActions.Fight ("Magic");
	}
	public void JumpForward(){
		Move (true, GameData._playerData.property [23], false);
		CheckEnemyAction ();
	}
	public void JumpBackward(){
		Move (false, GameData._playerData.property [23], false);
		CheckEnemyAction ();
	}
	public void Capture(){
		if (enemy.canCapture == 0 || (enemy.hp / enemyMaxHp > 0.3f) || captureFailTime >= 3)
			return;
		if (_gameData.GetUsedPetSpace () + enemy.canCapture > GameData._playerData.PetsOpen * 10) {
			_floating.CallInFloating ("宠物笼空间不足!", 1);
			return;
		}
		float rate = 0.1f - enemy.level / 100 * 0.5f;
		rate *= GameData._playerData.CaptureRate;
		int i = Algorithms.GetIndexByRange (0, 10000);
		if (i < (int)(rate * 10000)) {
			Pet p = new Pet ();
			p.monsterId = enemy.monsterId;
			p.state = 0;
			p.alertness = enemy.level;
			p.speed = (int)enemy.speed;
			p.name = enemy.name;
			GameData._playerData.Pets.Add (p.monsterId, p);
			_gameData.StoreData ("Pets", _gameData.GetstrFromPets (GameData._playerData.Pets));
			AddLog ("你捕获了新宠物: " + enemy.name, 0);

			//Achievement
			this.gameObject.GetComponentInParent<AchieveActions> ().CapturePet ();
			//战斗结束。。

		}else{
			captureFailTime++;
			AddLog ("你试图抓捕" + enemy.name + "，但是失败了。", 0);
			CheckEnemyAction ();
		}
	}

	void CheckEnemyAction(){
		while (enemyNextTurn < myNextTurn) {
			EnemyCastSkill ();
		}
	}

	void EnemyCastSkill(){
		int index = Algorithms.GetIndexByRange (0, enemy.skills.Length);
		enemyNextTurn += LoadTxt.skillDic [enemy.skills [index]].castSpeed * (1 - enemy.castSpeedBonus);
		SetPoint ();
		Fight (enemy.hit, GameData._playerData.property [18], 90, enemy.spirit, enemy.atk, GameData._playerData.property [15], enemy.skills [index], false);
	}

	public void OnAuto(){
		isAuto = !isAuto;
		autoOn.gameObject.SetActive (isAuto);
		autoOff.gameObject.SetActive (!isAuto);
		if (isAuto)
			AutoFight ();
	}

	public void OnReturn(){
		CallOutBattlePanel ();
		_panelManager.GoToPanel ("Father");
	}
}
