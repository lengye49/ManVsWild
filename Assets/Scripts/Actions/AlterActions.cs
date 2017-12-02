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
    public FloatingActions _floating;
	public LoadingBar _loadingBar;

	private GameData _gameData;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
        if (PlayerPrefs.GetInt("IsDead", 0) > 0)
        {
            PlayerPrefs.SetInt("IsDead", 1);
            RestartGame();
        }
	}

	public void UpdateAltar(){
        int num = 0;
        if (GameData._playerData.bp.ContainsKey(22020000))
            num = GameData._playerData.bp[22020000];

		ssNum.text = GameConfigs.SoulStoneForStoreMem + "/" + num;
        ssNum.color = (num >= GameConfigs.SoulStoneForStoreMem) ? Color.green : Color.red;
        storeButton.interactable = (num >= GameConfigs.SoulStoneForStoreMem);

		bool s = GameData._playerData.HasMemmory > 0;
		memoryPoolState.text = s ? "已存档" : "无存档";
		memoryPoolState.color = s ? Color.green : Color.red;
		recoverButton.interactable = s;
	}

	public void StoreMemory(){
        int num = 0;
        if (GameData._playerData.bp.ContainsKey(22020000))
            num = GameData._playerData.bp[22020000];

        if (num < GameConfigs.SoulStoneForStoreMem)
			return;
        _gameData.ConsumeItem(2202, GameConfigs.SoulStoneForStoreMem);
		_gameData.StoreMemmory ();
		UpdateAltar ();
        _floating.CallInFloating("存档成功", 0);
	}

	public void RecoverMemory(){
		this.gameObject.GetComponentInParent<AchieveActions> ().LoadMemmory (); //Achievement
		_gameData.RebirthLoad ();
		StartCoroutine (StartRebirth ());
	}

	IEnumerator StartRebirth(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void RestartGame(){
		_gameData.ReStartLoad ();

		GetComponentInParent<PanelManager>().GoToPanel("Home");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        GameObject head = GameObject.FindGameObjectWithTag("HeadUI");
        head.GetComponent<HeadUiManager>().UpdateHeadUI();
	}
		
}
