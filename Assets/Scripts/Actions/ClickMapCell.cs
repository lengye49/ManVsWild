using UnityEngine;
using System.Collections;

public class ClickMapCell : MonoBehaviour {

	public void OnClick(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		int i = int.Parse (this.gameObject.name);
		Maps m = LoadTxt.MapDic[i];
		this.gameObject.GetComponentInParent<ExploreActions> ().CallInDetail (m);
	}

	public void GoToPlace(){		
		int i = int.Parse (this.gameObject.name);
		Maps m = LoadTxt.MapDic [i];
		ExploreActions e = this.gameObject.GetComponentInParent<ExploreActions> ();
		e.mapGoing = m;
		e.GoToPlace ();
	}
}
