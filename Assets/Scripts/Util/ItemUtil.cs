using System.Collections;
using System.Collections.Generic;

public enum Item {
	WALL,
	FENCE,
	BONFIRE,
	AXE,
	PICKAXE,
	SWORD,
	POTION
}

public class ItemUtil {

	public static readonly Dictionary<Item, Dictionary<Resource, int>> ItemResources = new Dictionary<Item, Dictionary<Resource, int>> {
		{Item.WALL,		new Dictionary<Resource, int> {
			{Resource.STONE, 5}
		}},
		{Item.FENCE,	new Dictionary<Resource, int> {
			{Resource.WOOD, 5}
		}},
		{Item.BONFIRE,		new Dictionary<Resource, int> {
			{Resource.WOOD, 5},
			{Resource.STONE, 1},
			{Resource.IRON, 1}
		}},
		{Item.AXE,		new Dictionary<Resource, int> {
			{Resource.WOOD, 1},
			{Resource.IRON, 3}
		}},
		{Item.PICKAXE,	new Dictionary<Resource, int> {
			{Resource.WOOD, 1},
			{Resource.IRON, 3}
		}},
		{Item.SWORD,	new Dictionary<Resource, int> {
			{Resource.WOOD, 1},
			{Resource.IRON, 3}
		}},
		{Item.POTION,	new Dictionary<Resource, int>()}
	};

	public string Help(Item item) {
		string response = "Item '" + item.ToString() + "' does not have a defined resource list!";
		if (item == Item.POTION) {
			response = "Potion cannot be crafted";
		} else if (ItemResources.ContainsKey(item)) {
			response = item.ToString() + ResourcesToString(ItemResources[item]);
		}

		return response;
	}

	private string ResourcesToString(Dictionary<Resource, int> resources) {
		string resourceString = ":";

		if (resources.Count == 0) {
			resourceString = " cannot be crafted";
		}

		int i = 0;
		foreach(KeyValuePair<Resource, int> resource in resources) {
			if (i > 0) {
				resourceString += ",";
			}

			resourceString += " " + resource.Key.ToString() + "(" + resource.Value + ")";

			i++;
		}

		return resourceString;
	}

	public Dictionary<Resource, int> GetRequiredResources(Item item) {
		return ItemResources[item];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
