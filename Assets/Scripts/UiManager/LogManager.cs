using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {
	private Text[] logs;

	void Start(){
		logs = this.gameObject.GetComponentsInChildren<Text> ();
		ClearLogs ();
	}

	void ClearLogs(){
		for (int i = 0; i < logs.Length; i++) {
			logs [i].text = string.Empty;
		}
	}

	public void AddLog(string s){
		for (int i = logs.Length - 1; i > 0; i--) {
			logs [i].text = logs [i - 1].text;
		}
		logs [0].text = ">>" + s;
	}

}
