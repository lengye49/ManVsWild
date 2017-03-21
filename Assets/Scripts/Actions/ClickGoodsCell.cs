using UnityEngine;
using System.Collections;

public class ClickGoodsCell : MonoBehaviour {

	private PlaceActions _placeActions;
	void Start () {
		_placeActions = this.gameObject.GetComponentInParent<PlaceActions> ();
	}
	
	public void OnClickGoods(){
		int itemId = int.Parse (this.gameObject.name);
		_placeActions.CallInGoodsDetail (itemId);
	}
}
