using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarehouseActions : MonoBehaviour {

	public GameObject contentW;
	public GameObject contentB;
	public Text stateW;
	public Text stateB;
	public Button upgradeWarehouse;
	private GameObject bpCell;
	private GameObject whCell;
//	private GameData _gameData;

	private int _bpNum;
	private int _bpUsed;
	private int _warehouseNum;
	private int _warehouseUsed;

	private ArrayList whCells;
	private ArrayList bpCells;

	void Start(){
//		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		whCells = new ArrayList ();
		bpCells = new ArrayList ();
	}
		

	public void UpdatePanel(){
		_bpNum = GameData._playerData.bpNum;
		_bpUsed = GameData._playerData.bp.Count;
		_warehouseNum = GameConfigs.warehouseMin + GameConfigs.warehouseAdd * (GameData._playerData.WarehouseOpen - 1);
		_warehouseUsed = GameData._playerData.wh.Count;
		SetState ();
		upgradeWarehouse.gameObject.SetActive (GameData._playerData.WarehouseOpen < GameConfigs.MaxLv_Warehouse);
		UpdateBpContent ();
		UpdateWhContent ();
	}

	void SetState(){
		stateW.text="("+_warehouseUsed+"/"+_warehouseNum+")";
		stateW.color = (_warehouseUsed >= _warehouseNum) ? Color.yellow : Color.black;
		stateB.text="("+_bpUsed+"/"+_bpNum+")";
		stateB.color = (_bpUsed >= _bpNum) ? Color.yellow : Color.black;
	}

	void UpdateWhContent(){
		if (whCells.Count<_warehouseNum) {
			whCell = Instantiate (Resources.Load ("whCell")) as GameObject;
			int n = whCells.Count;
			for (int i = n; i < _warehouseNum; i++) {
				GameObject o = Instantiate (whCell) as GameObject;
				o.transform.SetParent (contentW.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				whCells.Add (o);
				ClearContent (o);
			}
			contentW.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,100 * whCells.Count);
		}

		for (int i = 0; i < whCells.Count; i++) {
			ClearContent (whCells [i] as GameObject);
		}

		int j = 0;
		foreach (int key in GameData._playerData.wh.Keys) {
			GameObject o = whCells [j] as GameObject;
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.wh [key].ToString();
			j++;
		}
	}
		

	void UpdateBpContent(){
		if (bpCells.Count<_bpNum) {
			bpCell = Instantiate (Resources.Load ("bpCell")) as GameObject;
			int n = bpCells.Count;
			for (int i = n; i < _bpNum; i++) {
				GameObject o = Instantiate (bpCell) as GameObject;
				o.transform.SetParent (contentB.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				bpCells.Add (o);
				ClearContent (o);
			}
			contentB.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,100 * bpCells.Count);
		}

		for (int i = 0; i < bpCells.Count; i++) {
			ClearContent (bpCells [i] as GameObject);
		}

		int j = 0;
		foreach (int key in GameData._playerData.bp.Keys) {
			GameObject o = bpCells [j] as GameObject;
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.bp [key].ToString();
			j++;
		}
	}

	void ClearContent(GameObject o){
		Text[] ts = o.gameObject.GetComponentsInChildren<Text> ();
		for (int i = 0; i < ts.Length; i++)
			ts [i].text = "";
		o.GetComponent<Button> ().interactable = false;
	}
}
