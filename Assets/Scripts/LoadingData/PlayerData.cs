//Player Data Class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData  {

	// Basic Properties
	public int hpNow;
	public int spiritNow;
	public int foodNow;
	public int waterNow;
	public int strengthNow;
	public int tempNow;

	public int HpMax;
	public int SpiritMax;
	public int FoodMax;
	public int WaterMax;
	public int StrengthMax;
	public int TempMax;
	public int TempMin;

	public float[] property;

	//Time Passed
	public int minutesPassed = 0;

	public int minuteNow
	{
		get{ return minutesPassed % 60;}
	}

	public int hourNow {
		get{ return (int)((minutesPassed % (60 * 24)) / 60);}
	}

	public int dayNow {
		get{ return (int)(minutesPassed / 60 / 24 + 1);}
	}

	public int monthNow{
		get{ return (int)(minutesPassed / 60 / 24 / 30);}
	}

	public int seasonNow
	{
		get{return (int)(minutesPassed / 60 / 24 / 90);}
	}

	public int yearNow
	{
		get{return (int)(minutesPassed / 60 / 24 / 360);}
	}

	public int BedRoomOpen;
	public int WarehouseOpen;
	public int KitchenOpen;
	public int WorkshopOpen;
	public int StudyOpen;
	public int FarmOpen;
	public int PetsOpen;
	public int WellOpen;
	public int MailBoxOpen;
	public int AltarOpen;

	public Dictionary<int,int> bp;
	public Dictionary<int,int> wh;
	public int bpNum;

	public int HasMemmory;

	public Dictionary<int,int> LearnedBlueprints;

	public int MeleeId;
	public int RangedId;
	public int MagicId;
	public int HeadId;
	public int BodyId;
	public int ShoeId;
	public int AccessoryId;
	public int AmmoId;
	public int AmmoNum;
	public Pet Mount;

	public int Hotkey0;
	public int Hotkey1;

	public int LastWithdrawWaterTime;

	public int SoulStone;
	public int Gold;

	public Dictionary<int,FarmState> Farms;

	public Dictionary<int,Mails> Mails;

	public Dictionary<int,Pet> Pets;

	public Dictionary<int,int> MapOpenState;
	public int mapNow;
	public int dungeonLevelMax;

	public Dictionary<int,int> techLevels;

	public static string GetPropName(int propId){
		switch (propId) {
		case 0: return "Hp";
		case 1: return "MaxHp";
		case 2: return "Spirit";
		case 3: return "MaxSpirit";
		case 4: return "Food";
		case 5: return "MaxFood";
		case 6: return "Water";
		case 7: return "MaxWater";
		case 8: return "Strength";
		case 9: return "MaxStrength";
		case 10: return "Temp";
		case 11: return "MinTemp";
		case 12: return "MaxTemp";
		case 13: return "Melee Damage";
		case 14: return "Ranged Damage";
		case 15: return "Defence";
		case 16: return "Melee Precise";
		case 17: return "Ranged Precise";
		case 18: return "Dodge";
		case 19: return "Melee Distance";
		case 20: return "Ranged Distance";
		case 21: return "Attack Speed";
		case 22: return "Ranged Attack Speed";
		case 23: return "Speed";
		case 24: return "Magic Damage";
		case 25: return "Melee Damage Percent";
		case 26: return "Ranged Damage Percent";
		case 27: return "Melee Speed Percent";
		case 28: return "Ranged Speed Percent";
		default:
			return "Wrong Prop Id = " + propId;
		}
	}

	public float ConstructTimeDiscount;
	public float TheftLossDiscount;
	public float HarvestIncrease;
	public float OenologyIncrease;
	public float MagicPower;
	public float MagicCostRate;
	public float CaptureRate;
	public float SpotRate;
	public float SearchRate;
	public float BlackSmithTimeDiscount;
	public float CookingTimeDiscount;
	public float CookingIncreaseRate;
	public float WaterCollectingRate;
	public float ThiefDefence;
	public float GhostComingProp;

	public int lastThiefTime;

	public Dictionary<int,int> Achievements;

	public int thiefCaught;
	public int ghostKill;
	public int ghostBossKill;
	public int ghostKingKill;
	public int[] wineTasted;
	public int[] foodCooked;
	public int[] meleeCollected;
	public int[] rangedCollected;
	public int[] magicCollected;
	public int monsterKilled;
	public int sleepTime;
	public int dragonKilled;
	public int legendThiefCaught;
	public int meleeAttackCount;
	public int rangedAttackCount;
	public int magicAttackCount;
	public int goldConsume;
	public int demonPoint;//猎魔点
	public int renown;//善恶值
	public int petsCaptured;
	public int wineDrinked;


}
