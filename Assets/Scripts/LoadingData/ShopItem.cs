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
	public string itemType;
	public int buyTimes;
}

public class Shop{
	public ArrayList shopItemList;
}