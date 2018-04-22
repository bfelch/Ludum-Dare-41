using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {

	private Dictionary<Resource, int> resourceInventory = new Dictionary<Resource, int>();
	private Dictionary<Item, int> itemInventory = new Dictionary<Item, int>();

	private Item? equippedItem = null;

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if (CanDoVerb()) {
			switch (verbStack[0]) {
				case Verb.USE:
					PerformUse();
					break;
			}
		}
	}

	public void Use(Direction direction, int times) {
		for (int i = 0; i < times; i++) {
			verbStack.Add(Verb.USE);
			directionStack.Add(direction);
		}

	}

	private void PerformUse() {
		verbCooldown = 0.8f;

		RaycastHit2D hit = GetBlockingObject(directionStack[0]);

		if (hit.collider != null) {
			if (UsingTool()) {
				Obstacle obstacle = hit.collider.gameObject.GetComponent<Obstacle>();
				if (obstacle != null) {
					obstacle.Damage(equippedItem, this);
				}
			} else if (UsingObstacle()) {
				SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Cannot place '" + equippedItem + "' to the " + directionStack[0]);
			}
		} else {
			if (UsingObstacle()) {
				GameObject obstacle = SingletonFactory.GetInstance<PrefabUtil>().GetPrefab(equippedItem.Value);

				if (obstacle != null) {
					Instantiate(obstacle, transform.position + GetVectorFromDirection(directionStack[0]), Quaternion.identity);

					SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Placed '" + equippedItem + "', " + (itemInventory[equippedItem.Value] - 1) + " remaining");
					RemoveItem(equippedItem.Value);
				}
			}
		}

		CompleteDirectionVerb();
	}

	public void AddResource(Resource resource, int quantity) {
		if (resourceInventory.ContainsKey(resource)) {
			resourceInventory[resource] += quantity;
		} else {
			resourceInventory[resource] = quantity;
		}
	}

	private int GetResourceCount(Resource resource) {
		if (!resourceInventory.ContainsKey(resource)) {
			resourceInventory[resource] = 0;
		}

		return resourceInventory[resource];
	}

	private void AddItem(Item item) {
		if (itemInventory.ContainsKey(item)) {
			itemInventory[item]++;
		} else {
			itemInventory[item] = 1;
		}
	}

	private void RemoveItem(Item item) {
		if (itemInventory.ContainsKey(item)) {
			itemInventory[item]--;
		} else {
			itemInventory[item] = 0;
		}

		if (itemInventory[item] <= 0) {
			itemInventory[item] = 0;
			equippedItem = null;
		}
	}

	public void Craft(Item item, int quantity) {
		ParserUtil parserUtil = SingletonFactory.GetInstance<ParserUtil>();
		ItemUtil itemUtil = SingletonFactory.GetInstance<ItemUtil>();
		Dictionary<Resource, int> requiredResources = itemUtil.GetRequiredResources(item);

		int numCrafted = 0;
		for (int i = 0; i < quantity; i++) {
			if (HasRequiredResources(requiredResources)) {
				foreach(KeyValuePair<Resource, int> requiredResource in requiredResources) {
					resourceInventory[requiredResource.Key] -= requiredResource.Value;
				}

				AddItem(item);
				numCrafted++;
			} else {
				parserUtil.PrintResponse("Not enough resources to craft '" + item.ToString() + "'");
				break;
			}
		}

		if (numCrafted > 0) {
			parserUtil.PrintResponse("Crafted " + numCrafted + " '" + item.ToString() + (numCrafted > 1 ? "s" : "") + "'");
		}
	}

	public void Equip(Item item) {
		if (HasSpecifiedItem(item)) {
			equippedItem = item;
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Equipped '" + item.ToString() + "'");
		} else {
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("No '" + item.ToString() + "' in inventory");
		}
	}

	private bool HasRequiredResources(Dictionary<Resource, int> requiredResources) {
		bool hasResources = true;

		foreach(KeyValuePair<Resource, int> requiredResource in requiredResources) {
			if (GetResourceCount(requiredResource.Key) < requiredResource.Value) {
				hasResources = false;
				break;
			}
		}

		return hasResources;
	}

	private bool HasSpecifiedItem(Item item) {
		if (itemInventory.ContainsKey(item)) {
			return (itemInventory[item] > 0);
		}

		return false;
	}

	public void DisplayInventory() {
		ParserUtil parserUtil = SingletonFactory.GetInstance<ParserUtil>();

		string inventory = ResourceInventoryToString() + "\n\n" + ItemInventoryToString();
		parserUtil.PrintResponse(inventory);
	}

	private string ResourceInventoryToString() {
		string resources = "";

		if (resourceInventory.Count == 0) {
			resources = "no resources";
		}

		int i = 0;
		foreach(KeyValuePair<Resource, int> resource in resourceInventory) {
			if (i > 0) {
				resources += "\n";
			}

			resources += resource.Key.ToString().PadRight(10) + "(" + resource.Value + ")";

			i++;
		}

		return resources;
	}

	private string ItemInventoryToString() {
		string items = "";

		if (itemInventory.Count == 0) {
			items = "no items";
		}

		int i = 0;
		foreach(KeyValuePair<Item, int> item in itemInventory) {
			if (i > 0) {
				items += "\n";
			}

			items += item.Key.ToString().PadRight(10) + "(" + item.Value + ")";

			if (equippedItem == item.Key) {
				items += "    equipped";
			}

			i++;
		}

		return items;
	}

	private bool UsingTool() {
		if (equippedItem == null
		 || equippedItem == Item.PICKAXE
		 || equippedItem == Item.AXE
		 || equippedItem == Item.SWORD) {
			 return true;
		 }

		 return false;
	}

	private bool UsingObstacle() {
		if (equippedItem == Item.WALL
		 || equippedItem == Item.FENCE
		 || equippedItem == Item.BONFIRE) {
			return true;
		}

		return false;
	}
}
