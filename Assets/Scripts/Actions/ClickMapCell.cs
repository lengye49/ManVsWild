using UnityEngine;
using System.Collections;

public class ClickMapCell : MonoBehaviour {

	public void OnClick(){
		int i = int.Parse (this.gameObject.name);
		Maps m = LoadTxt.MapDic[i];
		this.gameObject.GetComponentInParent<ExploreActions> ().CallInDetail (m);
	}
}
