using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeManager : MonoBehaviour {

	public Button BedRoom;
	public Button Warehouse;
	public Button Kitchen;
	public Button Workshop;
	public Button Study;
	public Button Farm;
	public Button Pets;
	public Button Well;
	public Button MailBox;
	public Button Altar;

	public void UpdateContent(){

		BedRoom.gameObject.GetComponent<Image> ().color = GameData._playerData.BedRoomOpen > 0 ? Color.white : Color.gray;
		BedRoom.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.BedRoomOpen > 0 ? "BedRoom" : "BedRoom";

		Warehouse.gameObject.GetComponent<Image> ().color = GameData._playerData.WarehouseOpen > 0 ? Color.white : Color.gray;
		Warehouse.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.WarehouseOpen > 0 ? "Warehouse" : "Warehouse";

		Kitchen.gameObject.GetComponent<Image> ().color = GameData._playerData.KitchenOpen > 0 ? Color.white : Color.gray;
		Kitchen.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.KitchenOpen > 0 ? "Kitchen" : "Kitchen";

		Workshop.gameObject.GetComponent<Image> ().color = GameData._playerData.WorkshopOpen > 0 ? Color.white : Color.gray;
		Workshop.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.WorkshopOpen > 0 ? "Workshop" : "Workshop";

		Study.gameObject.GetComponent<Image> ().color = GameData._playerData.StudyOpen > 0 ? Color.white : Color.gray;
		Study.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.StudyOpen > 0 ? "Study" : "Study";

		Farm.gameObject.GetComponent<Image> ().color = GameData._playerData.FarmOpen > 0 ? Color.white : Color.gray;
		Farm.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.FarmOpen > 0 ? "Farm" : "Farm";

		Pets.gameObject.GetComponent<Image> ().color = GameData._playerData.PetsOpen > 0 ? Color.white : Color.gray;
		Pets.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.PetsOpen > 0 ? "Pets" : "Pets";

		Well.gameObject.GetComponent<Image> ().color = GameData._playerData.WellOpen > 0 ? Color.white : Color.gray;
		Well.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.WellOpen > 0 ? "Well" : "Well";

		MailBox.gameObject.GetComponent<Image> ().color = GameData._playerData.MailBoxOpen > 0 ? Color.white : Color.gray;
		MailBox.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.MailBoxOpen > 0 ? "MailBox" : "MailBox";

		Altar.gameObject.GetComponent<Image> ().color = GameData._playerData.AltarOpen > 0 ? Color.white : Color.gray;
		Altar.gameObject.GetComponentInChildren<Text> ().text = GameData._playerData.AltarOpen > 0 ? "Altar" : "Altar";
	}
}
