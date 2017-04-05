using UnityEngine;
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

	public Image autoOn;
	public Image autoOff;

	private Unit enemy;
	private float myNextTurn;
	private float enemyNextTurn;
	private float distance;
	private GameData _gameData;
	private PanelManager _panelManager;
	private FloatingActions _floating;
	private ArrayList logs;
	private int logIndex;
	private GameObject battleLog;
	private float enemyMaxHp;
	private bool isAuto;
	private int captureFailTime;
	private int thisEnemyIndex;
	private Monster[] thisMonsters;

	private AchieveActions _achieveActions;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		this.gameObject.transform.localPosition = new Vector3 (-2000, 0, 0);
		logs = new ArrayList ();
		battleLog = Instantiate (Resources.Load ("battleLog")) as GameObject;
		battleLog.SetActive (false);
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
				AddLog ("Caution,Surrounded by " + monsters.Length + " enemies!", 2);
			else
				AddLog ("Caution,Attacked by " + monsters [0].name, 2);
		} else {
			if (monsters.Length > 1)
				AddLog (monsters.Length + " targets found!", 0);
			else
				AddLog (monsters [0].name + " found!", 0);
		}

		SetEnemy ();
//		if (isAuto)
//			AutoFight ();
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
		for (int i = 0; i < logs.Count; i++) {
			Text t = logs [i] as Text;
			t.gameObject.SetActive (false);
			t.text = "";
		}
		logIndex = 0;
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
		logIndex++;
		Text t;
		if (logs.Count > logIndex) {
			t = logs [logIndex] as Text;
			t.gameObject.SetActive (true);
		} else {
			GameObject o = Instantiate (battleLog) as GameObject;
			o.SetActive (true);
			t = o.GetComponent<Text> ();
			o.gameObject.transform.SetParent (battleLogContainer.transform);
			o.gameObject.transform.localPosition = Vector3.zero;
			o.gameObject.transform.localScale = Vector3.one;
			logs.Add (t);
		}
		t.text = "→" + s;

		if (isGood == 1)
			t.color = Color.green;
		else if (isGood == 2)
			t.color = Color.red;
		else
			t.color = Color.black;

		battleLogContainer.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,50 * logIndex);
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
					CastSkill ();
				}
			}
		}
	}

	void Move(bool forward,float speed,bool isEnemyMove){

		int f = forward ? -1 : 1;
		distance = Mathf.Max (0, distance + speed * f);
		string s = forward ? "forward" : "backward";
		if (isEnemyMove) {
			enemyNextTurn += 1;
			AddLog (enemy.name + " moved " + s, 0);
		} else {
			myNextTurn += 1;
			AddLog ("I moved " + s, 0);
		}

		enemyDistance.text = distance + "m";
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
			AddLog ((isMyAtk ? "" : enemy.name) + " Tried to hit " + (isMyAtk ? enemy.name : "you") + "but missed!",0);
			return;
		} else if (hitRate == 1) {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Body) : "body";
		} else {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Vital) : "face";
		}

		int dam = Algorithms.CalculateDamage (atk, def, skillId, hitRate,isMyAtk);

		if (isMyAtk) {
			enemy.hp -= dam;
			SetEnemyHpSlider ();
			if(skillId>0)
				AddEffect (LoadTxt.skillDic [skillId], isMyAtk, dam);
			AddLog ("Hit " + enemy.name + " int the " + hitPart + ".", 0);
		} else {
			_gameData.ChangeProperty (0, -dam);
			SetMyHpSlider ();
			AddEffect (LoadTxt.skillDic [skillId], isMyAtk, dam);
			AddLog (enemy.name + " hit you in the " + hitPart + ".", 0);
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
						AddLog ("I'm slowed down for 2s.", 0);
					}
					break;
				case 101:
					if (isMyAtk) {
						enemyNextTurn += 3;
						AddLog (enemy.name + " is confused for 3s.", 0);
					} else {
						myNextTurn += 3;
						AddLog ("I'm confused for 3s.", 0);
					}
					break;
				case 102:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is deluded for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("I'm deluded for 5s.", 0);
					}
					break;
				case 103:
					if (isMyAtk) {
						enemyNextTurn += 7;
						AddLog (enemy.name + " is dizzied for 7s.", 0);
					} else {
						myNextTurn += 7;
						AddLog ("I'm dizzied for 7s.", 0);
					}
					break;
				case 104:
					if (isMyAtk) {
						enemyNextTurn += 7;
						AddLog (enemy.name + " is frozen for 7s.", 0);
					} else {
						myNextTurn += 7;
						_gameData.ChangeProperty (10, -5);
						AddLog ("I'm frozen for 7s.", 0);
						AddLog ("I feel a bit hot, temperature +5", 0);
					}
					break;
				case 105:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is petrified for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("I'm petrified for 5s.", 0);
					}
					break;
				case 106:
					if (isMyAtk) {
						_gameData.ChangeProperty (2, 5);
						enemy.spirit -= 5;
						AddLog ("I recovered 5 spirit, " + enemy.name + " lost 5 spirit.", 0);
					} else {
						_gameData.ChangeProperty (2, -5);
						enemy.spirit += 5;
						AddLog ("I lost 5 spirit, " + enemy.name + " recovered 5 spirit.", 0);
					}
					break;
				case 107:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + " is chained for 5s.", 0);
					} else {
						myNextTurn += 5;
						AddLog ("I'm chained for 5s.", 0);
					}
					break;
				case 109:
					int hpPlus = (int)(dam * 0.3f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus);
						AddLog ("I recovered " + hpPlus + " hp", 0);
					} else {
						enemy.hp += hpPlus;
						AddLog (enemy.name + " recovered " + hpPlus + " hp", 0);
					}
					break;
				case 110:
					int hpPlus1 = (int)(dam * 0.5f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus1);
						AddLog ("I recovered " + hpPlus1 + " hp", 0);
					} else {
						enemy.hp += hpPlus1;
						AddLog (enemy.name + " recovered " + hpPlus1 + " hp", 0);
					}
					break;
				case 111:
					if (!isMyAtk) {
						_gameData.ChangeProperty (10, 5);
						AddLog ("I feel a bit hot, temperature +5", 0);
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
		if (enemy.hp > 0)
			return;

		Dictionary<int,int> drop = Algorithms.GetReward (enemy.drop);
		string s = "";
		if (drop.Count > 0) {
			s="I killed "+enemy.name+" and found ";
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
			s="I killed "+enemy.name+" but found nothing.";
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
			c = new Color ((1f - value) * 300f / 255f, 150f / 255F, 0F);
		else
			c = new Color (150f / 255f, value * 300f / 255f, 0f);

		return c;
	}

	public void MeleeFight(){
		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.MeleeId/10000)].skillId;
		Fight (GameData._playerData.property [16], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], GameData._playerData.property [13], enemy.def, skillId, true);
		myNextTurn += GameData._playerData.property [21];
		SetPoint ();
		//Achievement
		_achieveActions.Fight ("Melee");
	}
	public void RangedFight(){
		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.RangedId/10000)].skillId;
		Fight (GameData._playerData.property [17], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], GameData._playerData.property [14], enemy.def, skillId, true);
		myNextTurn += GameData._playerData.property [22];
		SetPoint ();
		//Achievement
		_achieveActions.Fight ("Ranged");
	}
	public void MagicFight(){
		_gameData.ChangeProperty (2, -(int)(LoadTxt.MatDic [GameData._playerData.MagicId].castSpirit * GameData._playerData.MagicCostRate));
		int dam = (int)(GameData._playerData.property [24] * GameData._playerData.MagicPower * Algorithms.GetIndexByRange (80, 120) / 100);
		enemy.hp -= dam;
		CheckBattleEnd ();
		myNextTurn += 1;
		SetPoint ();
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
			_floating.CallInFloating ("No more space for this pet!", 1);
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
			AddLog ("New pet captured: " + enemy.name, 0);

			//Achievement
			this.gameObject.GetComponentInParent<AchieveActions> ().CapturePet ();
			//战斗结束。。

		}else{
			captureFailTime++;
			AddLog ("Tried to capture " + enemy.name + " but failed.", 0);
			CheckEnemyAction ();
		}
	}

	void CheckEnemyAction(){
		while (enemyNextTurn < myNextTurn) {
			CastSkill ();
		}
	}

	void CastSkill(){
		int index = Algorithms.GetIndexByRange (0, enemy.skills.Length);
		Debug.Log ("enemy => castSkill => skillID => " + enemy.skills [index]);
		Fight (enemy.hit, GameData._playerData.property [18], 90, enemy.spirit, enemy.atk, GameData._playerData.property [15], enemy.skills [index], false);
		enemyNextTurn += LoadTxt.skillDic [enemy.skills [index]].castSpeed * (1 - enemy.castSpeedBonus);
		SetPoint ();
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
