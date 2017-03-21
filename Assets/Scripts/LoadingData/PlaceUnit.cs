using System.Collections;
using System.Collections.Generic;

public class PlaceUnit  {
	public int unitId;
	public int actionType;
	public string actionParam;
	public string name;
}

public class Places{
	public int id; // = mapId
	public ArrayList placeUnits = new ArrayList();
}