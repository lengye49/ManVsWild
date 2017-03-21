using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PetsActions : MonoBehaviour {

	public GameObject contentP;
	public RectTransform Detail;
	public FloatingActions _floating;
	public Text spaceText;

	private GameObject petCell;
	private ArrayList petCells;
	private int openPetCell;

	private int petSpace;
	private int usedSpace;

	private GameData _gameData;
	private int _localIndex;
	private Pet _localPet;

	void Start(){
		petCell = Instantiate (Resources.Load ("petCell")) as GameObject;
		petCells = new ArrayList ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
	}

	public void UpdatePets(){
		Detail.localPosition = new Vector3 (150, -2000, 0);
		SetPetCells ();
	}

	void SetPetCells(){
		petSpace = GameData._playerData.PetsOpen * 10;

		openPetCell = 0;
		usedSpace = 0;

		for (int i = 0; i < petCells.Count; i++) {
			GameObject o = petCells [i] as GameObject;
			ClearContents (o);
		}
			
		foreach (int key in GameData._playerData.Pets.Keys) {
			openPetCell++;
			usedSpace += LoadTxt.MonsterDic [GameData._playerData.Pets [key].monsterId].canCapture;
		}
		spaceText.text = "Space(" + usedSpace + "/" + petSpace + ")";

		if (openPetCell > petCells.Count) {
			for (int i = petCells.Count; i < openPetCell; i++) {
				GameObject o = Instantiate (petCell) as GameObject;
				o.transform.SetParent (contentP.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				petCells.Add (o);
			}
		}
		if (openPetCell < petCells.Count) {
			for (int i = openPetCell; i < petCells.Count; i++) {
				GameObject o = petCells [i] as GameObject;
				petCells.RemoveAt (i);
				GameObject.Destroy (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
				GameObject o = petCells [j] as GameObject;
				o.SetActive (true);
				o.gameObject.name = j.ToString ();
				SetPetCellState (o, GameData._playerData.Pets [key]);
				j++;
		}

		contentP.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,100 * petCells.Count);
	}

	void SetPetCellState(GameObject o,Pet p){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = LoadTxt.MonsterDic[p.monsterId].name;
		switch (p.state) {
		case 0:
			t [1].text = "Free Range";
			break;
		case 1:
			t [1].text = "Riding";
			break;
		case 2:
			t [1].text = "Patrolling";
			break;
		default:
			t [1].text = "Free Range";
			break;
		}
		t[2].text = LoadTxt.MonsterDic[p.monsterId].canCapture.ToString();
	}

	public void CallInDetail(Pet p,int index){
		_localPet = p;
		_localIndex = index;
		Detail.DOLocalMoveY (0, 0.3f);
		UpdateDetail ();
	}

	void UpdateDetail(){
		Pet p = _localPet;
		Text[] t = Detail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = LoadTxt.MonsterDic [p.monsterId].name;
		t [1].text = "DESC";

		switch (p.state) {
		case 0:
			t [2].text = "Free Range";
			break;
		case 1:
			t [2].text = "Riding";
			break;
		case 2:
			t [2].text = "Patrolling";
			break;
		default:
			break;
		}
		t [3].text = LoadTxt.MonsterDic [p.monsterId].canCapture.ToString ();
		t [4].text = LoadTxt.MonsterDic [p.monsterId].name;
		t [5].text = p.speed.ToString ();
		t [6].text = p.alertness.ToString ();
	}

	public void CallOutDetail(){
		if (Detail.localPosition.y > -10 && Detail.localPosition.y < 10)
			Detail.DOLocalMoveY (-2000f, 0.3f);
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	public void Guard(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}
		_localPet.state = (_localPet.state == 2) ? 0 : 2;
		GameData._playerData.Pets [_localIndex] = _localPet;
		StorePetState ();
		UpdateDetail ();
		SetPetCells();
	}

	public void Ride(){
		if (_localPet.state == 1) {
			RemoveMount ();
			_localPet.state = 0;
		} else {
			_localPet.state = 1;
			AddMount (_localPet);
		}
		GameData._playerData.Pets [_localIndex] = _localPet;
		StorePetState ();
		UpdateDetail ();
		SetPetCells();
	}

	public void SetFree(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}

		GameData._playerData.Pets.Remove (_localIndex);
		_floating.CallInFloating (_localPet.name + " has left.", 1);
		StorePetState ();
		CallOutDetail ();
		SetPetCells();
	}

	public void Kill(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}
		Monster m = LoadTxt.MonsterDic [_localPet.monsterId];
		foreach (int key in m.drop.Keys) {
			if (m.drop [key] >= 1) {
				_gameData.AddItem (key * 10000, (int)(m.drop [key]));
				_floating.CallInFloating (LoadTxt.MatDic [key].name + " ×" + (int)(m.drop [key]), 0);
			}
			else {
				float f = Algorithms.GetIndexByRange (0, 10000)/10000;
				if (f <= m.drop [key]) {
					_gameData.AddItem (key * 10000, 1);
					_floating.CallInFloating (LoadTxt.MatDic [key].name + " ×1", 0);
				}
			}
		}
		GameData._playerData.Pets.Remove (_localIndex);
		StorePetState ();
		CallOutDetail ();
		SetPetCells();
	}

	void RemoveMount(){
		GameData._playerData.Mount = new Pet ();
		_gameData.StoreData ("Mount", _gameData.GetStrFromMount (GameData._playerData.Mount));
		_gameData.UpdateProperty ();
	}

	void AddMount(Pet p){
		GameData._playerData.Mount = p;
		_gameData.StoreData ("Mount", _gameData.GetStrFromMount (GameData._playerData.Mount));
		_gameData.UpdateProperty ();
	}

	void StorePetState(){
		_gameData.StoreData ("Pets", _gameData.GetstrFromPets (GameData._playerData.Pets));
		GameData._playerData.Pets = _gameData.GetPetListFromStr (PlayerPrefs.GetString ("Pets", "100|1|50|Hello;100|0|20|Kitty"));
	}
}
