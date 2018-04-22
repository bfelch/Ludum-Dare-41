using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public int maxHp = 3;
	private int curHp = 0;

	public Item recommendedTool;
	public Resource givenResource;
	public int resourceQuantity;

	private GameObject damageSprite;

	// Use this for initialization
	void Start () {
		curHp = maxHp;

		damageSprite = new GameObject();
		damageSprite.AddComponent<SpriteRenderer>();

		damageSprite.transform.SetParent(transform);
		damageSprite.transform.localPosition = Vector3.back;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(Item? itemUsed, Player player) {
		if (itemUsed != null && itemUsed == recommendedTool) {
			curHp -= maxHp;
		} else {
			curHp--;
		}

		if (curHp <= 0 && player != null) {
			player.AddResource(givenResource, resourceQuantity);
			SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Added " + resourceQuantity + " " + givenResource + " to inventory");
			Destroy(this.gameObject);
		} else {
			damageSprite.GetComponent<SpriteRenderer>().sprite = SingletonFactory.GetInstance<PrefabUtil>().damageSprite[maxHp - curHp - 1];
		}
	}
}
