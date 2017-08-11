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
		if (s.Length >= 15) {
			string s1 = s.Substring (0, 15);
			string s2 = s.Substring (16);
			AddNewLog (s1);
			AddNewLog (s2);
		} else
			AddNewLog (s);

	}

	void AddNewLog(string s){
		for (int i = logs.Length - 1; i > 0; i--) {
			logs [i].text = logs [i - 1].text;
		}
		logs [0].text = ">>" + s;
	}

}
