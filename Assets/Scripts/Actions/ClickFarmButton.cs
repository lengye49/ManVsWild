using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickFarmButton : MonoBehaviour {

	private FarmActions _farmAction;
	private Button[] b;
	void Start(){
		_farmAction = this.gameObject.GetComponentInParent<FarmActions> ();
		b = this.gameObject.GetComponentsInChildren<Button> ();
	}

	public void OnRemoveButton(){
		int i = int.Parse (b [0].name);
		_farmAction.RemoveCrop (i);
	}

	public void OnChargeOrPrepare(){
		int i = int.Parse (b [0].name);
		if (b [1].name == "Prepare") {
			_farmAction.CallInPlantingTip (i);
		} else if (b [1].name == "Charge") {
			_farmAction.ChargeCrop (i);
		} else {
			Debug.Log ("Wrong Type for b[1].name");
		}
	}
}
