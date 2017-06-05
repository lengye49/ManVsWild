using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeadUiManager : MonoBehaviour {
	public Text hpNow;
	public Text hpMax;
	public Text spiritNow;
	public Text spiritMax;
	public Text foodNow;
	public Text foodMax;
	public Text waterNow;
	public Text waterMax;
	public Text strengthNow;
	public Text strengthMax;
	public Text tempNow;

	public Text dateNow;
	public Text timeNow;

	public Button hotkey0;
	public Button hotkey1;

	public void UpdateHeadUI(){
		hpNow.text = GameData._playerData.hpNow.ToString ();
		hpMax.text = "/" + GameData._playerData.property[1];
		spiritNow.text = GameData._playerData.spiritNow.ToString ();
		spiritMax.text = "/" + GameData._playerData.property[3];
		foodNow.text = GameData._playerData.foodNow.ToString ();
		foodMax.text = "/" + GameData._playerData.property[5];
		waterNow.text = GameData._playerData.waterNow.ToString ();
		waterMax.text = "/" + GameData._playerData.property[7];
		strengthNow.text = GameData._playerData.strengthNow.ToString ();
		strengthMax.text = "/" + GameData._playerData.property[9];
		tempNow.text = GameData._playerData.tempNow.ToString ();

		dateNow.text = GetDate ();
		timeNow.text = GetTime ();
	}

	public void UpdateHeadUI(string propName){
		switch (propName) {
		case "hpNow":
			hpNow.text = GameData._playerData.hpNow.ToString ();
			break;
		case "hpMax":
			hpMax.text = "/" + GameData._playerData.property[1];
			break;
		case "spiritNow":
			spiritNow.text = GameData._playerData.spiritNow.ToString ();
			break;
		case "spiritMax":
			spiritMax.text = "/" + GameData._playerData.property[3];
			break;
		case "foodNow":
			foodNow.text = GameData._playerData.foodNow.ToString ();
			break;
		case "foodMax":
			foodMax.text = "/" + GameData._playerData.property[5];
			break;
		case "waterNow":
			waterNow.text = GameData._playerData.waterNow.ToString ();
			break;
		case "waterMax":
			waterMax.text = "/" + GameData._playerData.property[7];
			break;
		case "strengthNow":
			strengthNow.text = GameData._playerData.strengthNow.ToString ();
			break;
		case "strengthMax":
			strengthMax.text = "/" + GameData._playerData.property[9];
			break;
		case "tempNow":
			tempNow.text = GameData._playerData.tempNow.ToString ();
			break;
		case "dateNow":
			dateNow.text = GetDate ();
			break;
		case "timeNow":
			timeNow.text = GetTime ();
			break;
		default:
			Debug.Log ("Wrong propName with " + propName);
			break;
		}
	}

	public void UpdateHotkeys(){
		if (GameData._playerData.Hotkey0 != 0) {
			hotkey0.gameObject.SetActive (true);
			hotkey0.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey0 / 10000)].name;
			hotkey0.interactable = true;
		} else {
			hotkey0.gameObject.SetActive (false);
//			hotkey0.gameObject.GetComponentInChildren<Text> ().text = "";
//			hotkey0.interactable = false;
		}

		if (GameData._playerData.Hotkey1 != 0) {
			hotkey1.gameObject.SetActive (true);
			hotkey1.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey1 / 10000)].name;
			hotkey1.interactable = true;
		} else {
			hotkey1.gameObject.SetActive (false);
//			hotkey1.gameObject.GetComponentInChildren<Text> ().text = "";
//			hotkey1.interactable = false;
		}
	}

	string GetDate(){
		string s = "";
		s += (GameData._playerData.yearNow > 9 ? "" : "0") + GameData._playerData.yearNow.ToString () + "/";
		s += (GameData._playerData.monthNow > 9 ? "" : "0") + GameData._playerData.monthNow.ToString () + "/";
		s += (GameData._playerData.dayNow > 9 ? "" : "0") + GameData._playerData.dayNow.ToString ();
		return s;
	}

	string GetTime(){
		string s = "";
		s += GameData._playerData.hourNow >= 12 ? "PM " : "AM ";
		s += (GameData._playerData.hourNow > 9 ? "" : "0") + GameData._playerData.hourNow.ToString () + ":";
		s += (GameData._playerData.minuteNow > 9 ? "" : "0") + GameData._playerData.minuteNow.ToString ();
		return s;
	}


}
