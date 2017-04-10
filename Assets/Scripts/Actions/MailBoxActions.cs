using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MailBoxActions : MonoBehaviour {

	public GameObject contentM;
	public RectTransform Detail;

	private GameObject mailCell;
	private ArrayList mailCells;
	private Mails localDetail;
	private int localIndex;
	private GameData _gameData;

	void Start(){
		mailCell = Instantiate (Resources.Load ("mailCell")) as GameObject;
		mailCell.SetActive (false);
		Detail.gameObject.SetActive (false);
		mailCells = new ArrayList ();
		localDetail = new Mails ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
	}
	
	public void UpdateMails(){
		if (Detail.localPosition.y != 0)
			Detail.localPosition = new Vector3 (0, -2000, 0);
		int j = 0;
		for (int i = 0; i < GameData._playerData.Mails.Count; i++) {
			GameObject o;
			if (j < mailCells.Count) {
				o = mailCells [i] as GameObject;
				o.SetActive (true);
			} else {
				o = Instantiate (mailCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentM.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = new Vector3 (1, 1, 1);
				mailCells.Add (o);
			}
			o.gameObject.name = j.ToString ();
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = GameData._playerData.Mails [j].addresser;
			t [1].text = GameData._playerData.Mails [j].subject;
			t [1].color = GameData._playerData.Mails [j].isRead == 0 ? Color.green : Color.grey;
			j++;
		}
		if (j < mailCells.Count) {
			for (int i = j; i < mailCells.Count; i++) {
				GameObject o = mailCells [i] as GameObject;
				o.SetActive (false);
			}
		}
		contentM.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,102 * j);
	}

	public void OpenMail(int index){
		if (!Detail.gameObject.activeSelf)
			CallInDetail ();
		Mails m = GameData._playerData.Mails [index];
		localDetail = m;
		localIndex = index;
		Text[] t = Detail.gameObject.GetComponentsInChildren<Text> ();
		Button[] b = Detail.gameObject.GetComponentsInChildren<Button> ();
		t [0].text = m.subject;
		t [1].text = "Dear Lord";
		t [2].text = m.addresser;
		t [3].text = m.mainText;
		if (m.isRead == 0) {
			t [4].text = LoadTxt.MatDic [m.attachmentId].name + " ×" + m.attachmentNum;
			b [0].interactable = true;
		} else {
			t [4].text = "";
			b [0].interactable = false;
		}	
	}

	void CallInDetail(){
		if (Detail.gameObject.activeSelf == true)
			return;
		Detail.gameObject.SetActive (true);
		Detail.gameObject.transform.localPosition = Vector3.zero;
		Detail.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		Detail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	public void CloseDetail(){
		Detail.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1);
		Detail.gameObject.SetActive (false);
		localDetail = new Mails ();
		localIndex = 0;
	}

	public void Collect(){
		if (localDetail.isRead == 0 && localDetail.attachmentId > 0 && localDetail.attachmentNum > 0) {
			_gameData.AddItem (localDetail.attachmentId * 10000, localDetail.attachmentNum);
			localDetail.isRead = 1;
			_gameData.StoreData ("Mails", _gameData.GetStrFromMails (GameData._playerData.Mails));
		}
		UpdateMails ();
		OpenMail (localIndex);
	}

	public void Delete(){
		GameData._playerData.Mails.Remove (localIndex);
		_gameData.StoreData ("Mails", _gameData.GetStrFromMails (GameData._playerData.Mails));
		GameData._playerData.Mails = _gameData.GetMailsFromStr (PlayerPrefs.GetString ("Mails"));
		CloseDetail ();
		UpdateMails ();
	}
}
