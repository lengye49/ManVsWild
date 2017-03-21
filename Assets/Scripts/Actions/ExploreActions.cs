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
	private Maps mapGoing;
	private GameData _gameData;
	private PanelManager _panelManager;

	void Start () {
		mapCell = Instantiate (Resources.Load ("mapCell")) as GameObject;
		mapCells = new ArrayList ();
		mapGoing = new Maps ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
	}
	
	public void UpdateExplore(){
		detail.localPosition = new Vector3 (150, 2000, 0);
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
//		t [1].text = LoadTxt.MapDic [mapId].desc;
		int m = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [mapId]);
		string s = GetTimeFormat (m);
		t [2].text = s;
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	public void CallInDetail(Maps m){
		if(detail.localPosition.y<-10 || detail.localPosition.y>10)
			detail.DOLocalMoveY (0, 0.3f);
		mapGoing = m;
		Text[] t = detail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = m.name;
//		t [1].text = "    " + m.desc;
		int min = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [m.id]);
		string s = GetTimeFormat (min);
		t [2].text = s;
	}

	public void CallOutDetail(){
		detail.DOLocalMoveY (2000, 0.3f);
	}

	public void GoToPlace(){
		if (GameData._playerData.MapOpenState [mapGoing.id] == 0) {
			Debug.Log ("This map is not open yet!");
			return;
		}
		int min = TravelTime (LoadTxt.MapDic [GameData._playerData.mapNow].distances [mapGoing.id]);
		_gameData.ChangeTime (min);
		GameData._playerData.mapNow = mapGoing.id;
		_gameData.StoreData ("mapNow", mapGoing.id);

		_panelManager.MapGoing = mapGoing;
		_panelManager.GoToPanel ("Place");
	}

	string GetTimeFormat(int m){
		string s = "";
		if (m < 60)
			s = m + "m";
		else if (m % 60 == 0)
			s = (int)(m / 60) + "h";
		else
			s = (int)(m / 60) + "h" + (m % 60) + "m";

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
