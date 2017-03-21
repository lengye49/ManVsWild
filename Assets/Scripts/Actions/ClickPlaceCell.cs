using UnityEngine;
using System.Collections;

public class ClickPlaceCell : MonoBehaviour {

	public void OnClick(){
		int unitId = int.Parse (this.gameObject.name);
		this.gameObject.GetComponentInParent<PlaceActions> ().CallInDetail (unitId);
	}
}
