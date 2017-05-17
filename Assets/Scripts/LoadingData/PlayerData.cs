﻿//Player Data Class

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

	/// <summary>
	/// Gets the hour now by format 24h.
	/// </summary>
	/// <value>The hour now.</value>
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
		case 0: return "生命";
		case 1: return "生命上限";
		case 2: return "精神";
		case 3: return "精神上限";
		case 4: return "食物";
		case 5: return "食物上限";
		case 6: return "水";
		case 7: return "水上限";
		case 8: return "力量";
		case 9: return "力量上限";
		case 10: return "温度";
		case 11: return "温度下限";
		case 12: return "温度上限";
		case 13: return "近战伤害";
		case 14: return "远程伤害";
		case 15: return "防御";
		case 16: return "近战命中";
		case 17: return "远程命中";
		case 18: return "闪避";
		case 19: return "近战攻击距离";
		case 20: return "远程攻击距离";
		case 21: return "近战攻击速度";
		case 22: return "远程攻击速度";
		case 23: return "速度";
		case 24: return "魔法伤害";
		case 25: return "魔法伤害比例";
		case 26: return "远程伤害比例";
		case 27: return "近战攻击速度比例";
		case 28: return "远程攻击速度比例";
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

	public int placeNowId;
}
