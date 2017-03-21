using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathActions : MonoBehaviour {

	public Text deathMsg;
	public Button rebirthButton;

	public void UpdateDeath(string cause){

		switch (cause) {
		case "Spirit":
			deathMsg.text = "You Die of Madness! \nTake it easy man, Go get some rest!";
			break;
		case "Food":
			deathMsg.text = "You Die of Hunger! \nNever starve, Eat anything you can!";
			break;
		case "Water":
			deathMsg.text = "You Die of Thirsty! \nWater means a lot!";
			break;
		case "Cold":
			deathMsg.text = "You Die of Cold! \nKeep your self warm, Winter is coming!";
			break;
		case "Hot":
			deathMsg.text = "You Die of Hot! \nHow can you survive in Africa?";
			break;
		default:
			deathMsg.text = "You Are Killed By " + cause + "! \nRebirth and make it pay!";
			break;
		}
			
		rebirthButton.interactable = (GameData._playerData.HasMemmory > 0);
	}
}
