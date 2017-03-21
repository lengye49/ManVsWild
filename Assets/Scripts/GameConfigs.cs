﻿using System.Collections.Generic;

public class GameConfigs{
	
	public static int FoodCostPerHour = 1;
	public static int WaterCostPerHour = 1;
	public static int SpiritCostPerBattle = 1;

	public static int[] StrengthRecoverPerRestHour = new int[4]{6,10,10,10};
	public static int SpiritRecoverPerRestHour = 1;

	public static int TempRecoverPerNormalBath = -10;
	public static int TempRecoverPerHotBath = 10;
	public static int SpiritRecoverPerBath = 3;
	public static int TimeForBath = 1;
	public static int WaterId = 41000000;
	public static int WaterForBath = 5;
	public static int WoodId = 42140000;
	public static int WoodForHotBath = 5;

	public static int bpNumMax = 50;
	public static int bpNumMin = 20;
	public static int bpNumAdd = 3;

	public static int warehouseMin = 20;
	public static int warehouseAdd = 5;

	public static int MaxLv_BedRoom = 3;
	public static int MaxLv_Warehouse = 3;
	public static int MaxLv_Kitchen = 3;
	public static int MaxLv_Workshop = 3;
	public static int MaxLv_Study = 1;
	public static int MaxLv_Farm = 3;
	public static int MaxLv_Pets = 3;
	public static int MaxLv_Well = 3;
	public static int MaxLv_MailBox = 1;
	public static int MaxLv_Altar = 1;

	public static int WaterInWellPerDay = 5;
	public static int WaterStoreMax = 100;

	public static int hpMax = 100;
	public static int spiritMax=100;
	public static int foodMax=100;
	public static int waterMax=100;
	public static int strengthMax=100;
	public static int tempMin=-60;
	public static int tempMax=60;

									//   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6  7  8  9 0 1 2 3 4 5 6 7 8
	public static int[] BasicProperty = {0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,95,80,20,2,0,2,0,5,0,0,0,0,0};
	//0hpNow 1hpMax 2spiritNow 3spiritMax 4foodNow 5foodMax 6waterNow 7waterMax 8strengthNow 9strengthMax 10-12 tempNow/Min/Max
	//13MeleeDamage 14RangedDamage 15Def 16MeleePrecise 17RangePrecise 18Dodge 19MeleeDistance 20RangedDistance
	//21MeleeAttackSpeed 22RangedAttackSpeed 23MoveSpeed 24MagicDamage 25MeleePercent 26RangePercent 27MeleeSpeedPercent 28RangedSpeedPercent

	public static int SoulStoneForStoreMem = 10;

	public static int TreeGrowSpeed = 2;
	public static int CuttingStrength = 5;
	public static int CuttingTime = 60;
	public static int DiggingStrength = 5;
	public static int DiggingTime = 60;
	public static int FetchingStrength = 5;
	public static int FetchingTime = 60;
	public static int SearchTime = 60;
	public static int SearchRewardsNum = 8;
	public static int CollectTime = 60;
	public static int CollectStrength=5;
	public static int HuntTime = 60;

	public static int SetTrapTime = 60;
	public static Dictionary<int,int> AnimalTrapReq = new Dictionary<int, int>{ { 1000,5 }, { 1001,2 }, { 1002,1 } };
	public static Dictionary<int,int> BirdTrapReq = new Dictionary<int, int>{ { 1000,5 }, { 1001,2 }, { 1002,1 } };
	public static Dictionary<int,int> FishTrapReq = new Dictionary<int, int>{ { 1000,5 }, { 1001,2 }, { 1002,1 } };
	public static Dictionary<int,int> AnimalTrapAc = new Dictionary<int, int>{ { 1000,50 }, { 1001,20 }, { 0,50 } };
	public static Dictionary<int,int> BirdTrapAc = new Dictionary<int, int>{ { 1000,50 }, { 1001,20 }, { 0,50 } };
	public static Dictionary<int,int> FishTrapAc = new Dictionary<int, int>{ { 1000,50 }, { 1001,20 }, { 0,50 } };

	public static int defParam = 33;
	public static int luck = 100;//影响掉落概率

	public static int BasicBpNum = 20;//背包基础数量
	public static int IncBpNum=10;//每级增加背包数量
	public static float[] ConstructionDiscount = {1f,0.8f,0.6f,0.4f};
	public static float[] TheftDiscount={1f,0.8f,0.6f,0.4f};
	public static float[] HarvestIncrease={1f,1.3f,1.6f,2f};
	public static float[] OenologyIncrease={1f,1.3f,1.6f,2f};
	public static int[] ArcheryRangedDamage = {10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200};
	public static int[] ArcheryRangedPrecise = {0,0,0,0,5,0,0,0,0,10,0,0,0,0,15,0,0,0,0,20};
	public static int[] ArcheryMeleeDamage = {10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180,190,200};
	public static int[] ArcheryMeleePrecise = {0,0,0,0,5,0,0,0,0,10,0,0,0,0,15,0,0,0,0,20};
	public static float[] WitchcraftPower = {1f,1.1f,1.25f,1.5f};
	public static float[] WitchcraftCostRate={1f,0.8f,0.5f};
	public static float[] CaptureRate = {1f,1.2f,1.5f,2f};
	public static float[] SpotRate = {0.5f,0.4f,0.25f};
	public static float[] SearchRate = {1f,1.5f};
	public static float[] BlackSmithTimeDiscount={1f,0.9f,0.7f,0.5f};
	public static float[] CookingTimeDiscount = {1f,0.8f,0.5f};
	public static float[] CookingIncreaseRate = {1f,1.5f,2.5f};
	public static float[] WaterCollectingRate = {1f,1.2f,1.5f};
	public static float[] GhostComingProp = {0.2f,0.1f,0.05f};

	public static int StartThiefEvent = 0;
	public static int StartGhostEvent = 0;
																		//Ghost     Boss     King
	public static Dictionary<int,int> GhostDic = new Dictionary<int, int>{{100,50},{101,20},{102,10}};
}
