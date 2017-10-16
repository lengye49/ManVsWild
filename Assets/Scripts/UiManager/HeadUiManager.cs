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

	public Image HpImage;
	public Image SpiritImage;
	public Image FoodImage;
	public Image WaterImage;
	public Image StrengthImage;
	public Image TempImage;

	public Text dateNow;
	public Text timeNow;

	public Text seasonNow;

	public Button hotkey0;
	public Button hotkey1;

	public void UpdateHeadUI(){
		hpNow.text = GameData._playerData.hpNow.ToString ();
		hpMax.text = "/" + GameData._playerData.property[1];
		hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
		HpImage.color = hpNow.color;

		spiritNow.text = GameData._playerData.spiritNow.ToString ();
		spiritMax.text = "/" + GameData._playerData.property[3];
		spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
		SpiritImage.color = spiritNow.color;

		foodNow.text = GameData._playerData.foodNow.ToString ();
		foodMax.text = "/" + GameData._playerData.property[5];
		foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
		FoodImage.color = foodNow.color;

		waterNow.text = GameData._playerData.waterNow.ToString ();
		waterMax.text = "/" + GameData._playerData.property[7];
		waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
		WaterImage.color = waterNow.color;

		strengthNow.text = GameData._playerData.strengthNow.ToString ();
		strengthMax.text = "/" + GameData._playerData.property[9];
		strengthNow.color = Algorithms.GetDangerColor (GameData._playerData.strengthNow / GameData._playerData.property [9]);
		StrengthImage.color = strengthNow.color;

        tempNow.text = GameData._playerData.tempNow.ToString("#0.0");
		if ((GameData._playerData.tempNow >= (GameData._playerData.property [12] - 20)) || (GameData._playerData.tempNow <= (GameData._playerData.property [11] + 20)))
			tempNow.color = new Color (1f, 1f, 0f, 1f);
		else if ((GameData._playerData.tempNow >= (GameData._playerData.property [12] - 10)) || (GameData._playerData.tempNow <= (GameData._playerData.property [11] + 10)))
			tempNow.color = new Color (1f, 0f, 0f, 1f);
		else
			tempNow.color = new Color (1f, 1f, 1f, 1f);

		TempImage.color = tempNow.color;

		dateNow.text = GetDate ();
		timeNow.text = GetTime ();
		SetSeason ();
	}

	public void UpdateHeadUI(string propName){
		switch (propName) {
		case "hpNow":
			hpNow.text = GameData._playerData.hpNow.ToString ();
			hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
			HpImage.color = hpNow.color;
			break;
		case "hpMax":
			hpMax.text = "/" + GameData._playerData.property[1];
			hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
			HpImage.color = hpNow.color;
			break;
		case "spiritNow":
			spiritNow.text = GameData._playerData.spiritNow.ToString ();
			spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
			SpiritImage.color = spiritNow.color;
			break;
		case "spiritMax":
			spiritMax.text = "/" + GameData._playerData.property[3];
			spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
			SpiritImage.color = spiritNow.color;
			break;
		case "foodNow":
			foodNow.text = GameData._playerData.foodNow.ToString ();
			foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
			FoodImage.color = foodNow.color;
			break;
		case "foodMax":
			foodMax.text = "/" + GameData._playerData.property[5];
			foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
			FoodImage.color = foodNow.color;
			break;
		case "waterNow":
			waterNow.text = GameData._playerData.waterNow.ToString ();
			waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
			WaterImage.color = waterNow.color;
			break;
		case "waterMax":
			waterMax.text = "/" + GameData._playerData.property[7];
			waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
			WaterImage.color = waterNow.color;
			break;
		case "strengthNow":
			strengthNow.text = GameData._playerData.strengthNow.ToString ();
			strengthNow.color = Algorithms.GetDangerColor (GameData._playerData.strengthNow / GameData._playerData.property [9]);
			StrengthImage.color = strengthNow.color;
			break;
		case "strengthMax":
			strengthMax.text = "/" + GameData._playerData.property[9];
			strengthNow.color = Algorithms.GetDangerColor (GameData._playerData.strengthNow / GameData._playerData.property [9]);
			StrengthImage.color = strengthNow.color;
			break;
        case "tempNow":
            tempNow.text = GameData._playerData.tempNow.ToString("#0.0");

            if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] * 0.75f)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] * 0.75f)))
                tempNow.color = new Color(1f, 1f, 0f, 1f);
            else if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] * 0.75f)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] * 0.75f)))
                tempNow.color = new Color(1f, 0f, 0f, 1f);
            else
                tempNow.color = new Color(1f, 1f, 1f, 1f);

            TempImage.color = tempNow.color;
            break;
		case "dateNow":
			dateNow.text = GetDate ();
			break;
		case "timeNow":
			timeNow.text = GetTime ();
			SetSeason ();
			break;
		default:
			Debug.Log ("Wrong propName with " + propName);
			break;
		}
	}

//	public void UpdateHotkeys(){
//		if (GameData._playerData.Hotkey0 != 0) {
//			hotkey0.gameObject.SetActive (true);
//			hotkey0.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey0 / 10000)].name;
//			hotkey0.interactable = true;
//		} else {
//			hotkey0.gameObject.SetActive (false);
////			hotkey0.gameObject.GetComponentInChildren<Text> ().text = "";
////			hotkey0.interactable = false;
//		}
//
//		if (GameData._playerData.Hotkey1 != 0) {
//			hotkey1.gameObject.SetActive (true);
//			hotkey1.gameObject.GetComponentInChildren<Text> ().text = LoadTxt.MatDic [(int)(GameData._playerData.Hotkey1 / 10000)].name;
//			hotkey1.interactable = true;
//		} else {
//			hotkey1.gameObject.SetActive (false);
////			hotkey1.gameObject.GetComponentInChildren<Text> ().text = "";
////			hotkey1.interactable = false;
//		}
//	}

	string GetDate(){
		string s = "";
		s = "第" + GameData._playerData.dayNow + "天";

		return s;
	}

	void SetSeason(){
		string s = "";
		Color c = new Color();
		switch (GameData._playerData.seasonNow) {
		case 0:
			s += "春";
			c = Color.green;
			break;
		case 1:
			s += "夏";
			c = Color.red;
			break;
		case 2:
			s += "秋";
			c = Color.yellow;
			break;
		case 3:
			s += "冬";
			c = Color.white;
			break;
		default:
			break;
		}
		seasonNow.text = s;
		seasonNow.color = c;
	}

	string GetTime(){
		string s = "";
		s += GameData._playerData.hourNow >= 12 ? "下午 " : "上午 ";
		s += (GameData._playerData.hourNow > 9 ? "" : "0") + GameData._playerData.hourNow.ToString () + ":";
		s += (GameData._playerData.minuteNow > 9 ? "" : "0") + GameData._playerData.minuteNow.ToString ();
		return s;
	}


}
