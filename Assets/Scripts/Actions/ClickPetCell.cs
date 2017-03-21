using UnityEngine;
using System.Collections;

public class ClickPetCell : MonoBehaviour {

	public void OnClick(){
		int i = int.Parse (this.gameObject.name);
		Debug.Log (i);
		foreach (int key in GameData._playerData.Pets.Keys) {
			Debug.Log (key);
		}
		Pet p = GameData._playerData.Pets [i];
		this.gameObject.GetComponentInParent<PetsActions> ().CallInDetail (p,i);
	}
}
