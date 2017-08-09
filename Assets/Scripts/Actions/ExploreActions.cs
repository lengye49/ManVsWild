using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ExploreActions : MonoBehaviour {

	public GameObject contentE;
	public RectTransform detail;

	private GameObject mapCell;
	private ArrayList mapCells;
	public Maps mapGoing;
	private GameData _gameData;
	private PanelManager _panelManager;
	public LoadingBar _loadingBar;
	public LogManager _logManager;

	void Start () {
		mapCell = Instantiate (Resources.Load ("mapCell")) as GameObject;
		mapCell.SetActive (false);
		mapCells = new ArrayList ();
		mapGoing = new Maps ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
	}
	
	public void UpdateExplore(){

		CallOutDetail ();
		int openNum = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			if (GameData._playerData.MapOpenState [key] == 1)
				openNum++;
		}

		for (int i = 0; i < mapCells.Count; i++) {
			GameObject o = mapCells [i] as GameObject;
			ClearContents (o);
		}

		if (openNum-1 > mapCells.Count) {
			for (int i = mapCells.Count; i < openNum-1; i++) {
				GameObject o = Instantiate (mapCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentE.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				mapCells.Add (o);
				ClearContents (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			if (GameData._playerData.MapOpenState [key] == 1 && key!=GameData._playerData.mapNow) {
				GameObject o = mapCells [j] as GameObject;
				o.gameObject.name = key.ToString ();
				SetMapCell (o, key);
				j++;
			}
		}

		contentE.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(880,115 * mapCells.Count);
	}

	void SetMapCell(GameObject o,int mapId){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = LoadTxt.MapDic [mapId].name;
		int m = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [mapId]);
		string s = GetTimeFormat (m);
		t [1].text = LoadTxt.MapDic [mapId].desc;
		t [2].text = s;
//		t [3].text = "出发";
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	public void CallInDetail(Maps m){
		if (detail.gameObject.activeSelf == false) {
			detail.gameObject.SetActive (true);
			detail.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
			detail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
		}

		mapGoing = m;
		Text[] t = detail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = m.name;
		int min = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [m.id]);
		string s = GetTimeFormat (min);
		t [2].text = s;
	}

	public void CallOutDetail(){
		detail.localPosition = new Vector3 (150, 0, 0);
		detail.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		detail.gameObject.SetActive (false);
	}

	public void GoToPlace(){
		if (GameData._playerData.MapOpenState [mapGoing.id] == 0) {
			Debug.Log ("未知地域!");
			return;
		}
		int min = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [mapGoing.id]);
		_gameData.ChangeTime (min);
		GameData._playerData.mapNow = mapGoing.id;
		_gameData.StoreData ("mapNow", mapGoing.id);

		StartCoroutine (StartLoading (min));

	}

	IEnumerator StartLoading(int costTime){
		int t = _loadingBar.CallInLoadingBar (costTime);
		yield return new WaitForSeconds (t);
		GoToMap();
	}

	void GoToMap(){
		_panelManager.MapGoing = mapGoing;
		if (mapGoing.id == 0) {
			_logManager.AddLog ("回到家中。");
		} else
			_logManager.AddLog ("你抵达了" + mapGoing.name + "。");
		_panelManager.GoToPanel ("Place");
	}

	string GetTimeFormat(int m){
		string s = "";
		if (m < 60)
			s = m + "分";
		else if (m % 60 == 0)
			s = (int)(m / 60) + "时";
		else
			s = (int)(m / 60) + "时" + (m % 60) + "分";

		return s;
	}

	/// <summary>
	/// Travels the time,minutes.
	/// </summary>
	/// <returns>distance,km.</returns>
	int TravelTime(int distance){
		float speed = GameData._playerData.property [23];
		int min = (int)(distance * 60 / speed);
		return min;
	}
}
