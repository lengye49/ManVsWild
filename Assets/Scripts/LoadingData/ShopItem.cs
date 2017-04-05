using System.Collections;
using System.Collections.Generic;

/// <summary>
/// itemtype: 0once 1always 2timeslimit 3random
/// </summary>
public class ShopItem  {
	public int itemId;
	public int shopId;
	public Dictionary<int,int> cost;
	public Dictionary<int,int> reward;
	public string itemType;//0一次性1永久2次数限制3随机出现
	public int buyTimes;

//	public ShopItem(int id){
//		itemId = id;
//		Mats m = LoadTxt.MatDic [itemId];
//		int num = 1;
//		if (m.price < 100)
//			num = 10;
//		else if (m.price < 400)
//			num = 2;
//
//		cost = new Dictionary<int, int>{ { 3100,m.price * num / 100 } };
//		reward = new Dictionary<int, int>{ { id,num } };
//		itemType = 0;
//		buyTimes = 0;
//	}
}

public class Shop{
	public ArrayList shopItemList;
}