using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AlterActions : MonoBehaviour {

	public Text ssNum;
	public Text memoryPoolState;
	public Button storeButton;
	public Button recoverButton;
	public Button changeButton;
	public RectTransform memories;
	public RectTransform achievements;
	public GameObject ContentA;

	private GameData _gameData;
	private AchieveActions _achieveActions;
	private float tweenerTime = 0.5f;
	private GameObject achievementCell;
	private ArrayList achievementCells;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_achieveActions = this.gameObject.GetComponentInParent<AchieveActions> ();
		achievementCell = Instantiate (Resources.Load ("achievementCell")) as GameObject;
		achievementCells = new ArrayList ();
	}

	public void UpdateAltar(){
		memories.DOLocalMoveX (0, tweenerTime);
		achievements.DOLocalMoveX (-2000, tweenerTime);
		changeButton.gameObject.GetComponentInChildren<Text> ().text = "Achievements";

		ssNum.text = GameData._playerData.SoulStone.ToString ();
		ssNum.color = (GameData._playerData.SoulStone >= GameConfigs.SoulStoneForStoreMem) ? Color.green : Color.red;
		storeButton.interactable = (GameData._playerData.SoulStone >= GameConfigs.SoulStoneForStoreMem);

		bool s = GameData._playerData.HasMemmory > 0;
		memoryPoolState.text = s ? "Full" : "Empty";
		memoryPoolState.color = s ? Color.green : Color.red;
		recoverButton.interactable = s;
	}

	public void UpdateAchievement(){
		memories.DOLocalMoveX (2000, tweenerTime);
		achievements.DOLocalMoveX (0, tweenerTime);

		for (int i = achievementCells.Count; i < LoadTxt.AchievementDic.Count; i++) {
			GameObject o = Instantiate (achievementCell) as GameObject;
			o.transform.SetParent (ContentA.transform);
			o.transform.localPosition = Vector3.zero;
			o.transform.localScale = Vector3.one;
			achievementCells.Add (o);
		}

		int index = 0;
		foreach(int key in LoadTxt.AchievementDic.Keys) {
			GameObject o = achievementCells [index] as GameObject;
			SetAchievement (o, LoadTxt.AchievementDic [key]);
			index++;
		}
		ContentA.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,110 * achievementCells.Count);
	}

	void SetAchievement(GameObject o,Achievement a){
		Text[] t = o.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = a.name;
		t [1].text = a.desc + "(" + _achieveActions.GetProgress (a.id) + ")";
	}

	public void StoreMemory(){
		if (GameData._playerData.SoulStone < GameConfigs.SoulStoneForStoreMem)
			return;
		_gameData.StoreMemmory ();
		UpdateAltar ();
	}

	public void RecoverMemory(){
		_gameData.RebirthLoad ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void RestartGame(){
		_gameData.ReStartLoad ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void Exchange(){
		if (memories.localPosition.x == 0) {
			changeButton.gameObject.GetComponentInChildren<Text> ().text = "Memories";
			UpdateAchievement ();
		} else {
			changeButton.gameObject.GetComponentInChildren<Text> ().text = "Achievements";
			UpdateAltar ();
		}
	}
}
