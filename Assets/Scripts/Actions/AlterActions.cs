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
	private GameData _gameData;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
	}

	public void UpdateAltar(){
		ssNum.text = GameData._playerData.SoulStone.ToString ();
		ssNum.color = (GameData._playerData.SoulStone >= GameConfigs.SoulStoneForStoreMem) ? Color.green : Color.red;
		storeButton.interactable = (GameData._playerData.SoulStone >= GameConfigs.SoulStoneForStoreMem);

		bool s = GameData._playerData.HasMemmory > 0;
		memoryPoolState.text = s ? "已有存档" : "当前无存档";
		memoryPoolState.color = s ? Color.green : Color.red;
		recoverButton.interactable = s;
	}

	public void StoreMemory(){
		if (GameData._playerData.SoulStone < GameConfigs.SoulStoneForStoreMem)
			return;
		_gameData.StoreMemmory ();
		UpdateAltar ();
	}

	public void RecoverMemory(){
		this.gameObject.GetComponentInParent<AchieveActions> ().LoadMemmory (); //Achievement
		_gameData.RebirthLoad ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void RestartGame(){
		_gameData.ReStartLoad ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
		
}
