using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabUtil : MonoBehaviour {

	public GameObject playerPre;
	public GameObject zombiePre;

	public GameObject pickaxePre;
	public GameObject axePre;
	public GameObject swordPre;
	public GameObject potionPre;

	public GameObject wallPre;
	public GameObject fencePre;
	public GameObject bonfirePre;

	public GameObject stonePre;
	public GameObject treePre;
	public GameObject ironPre;

	private Dictionary<Item, GameObject> preLibrary = new Dictionary<Item, GameObject>();

	public AudioClip pickupClip;
	public AudioClip humanDeathClip;

	public Sprite[] damageSprite;

	// Use this for initialization
	void Start () {
		preLibrary[Item.PICKAXE] = pickaxePre;
		preLibrary[Item.AXE] = axePre;
		preLibrary[Item.SWORD] = swordPre;
		preLibrary[Item.POTION] = potionPre;

		preLibrary[Item.WALL] = wallPre;
		preLibrary[Item.FENCE] = fencePre;
		preLibrary[Item.BONFIRE] = bonfirePre;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject GetPrefab(Item item) {
		if (preLibrary.ContainsKey(item)) {
			return preLibrary[item];
		}

		return null;
	}
}
