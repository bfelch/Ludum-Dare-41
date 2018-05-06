using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	private AudioSource audioSource;
	private bool isHidden = false;

	public bool IsHidden {
		get{ return isHidden; }
	}

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

		audioSource = GetComponent<AudioSource>();
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

		audioSource.Play();

		if (curHp <= 0) {
			if (player != null) {
				player.AddResource(givenResource, resourceQuantity);
				SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Added " + resourceQuantity + " " + givenResource + " to inventory");
				player.GetAudioSource().clip = SingletonFactory.GetInstance<PrefabUtil>().pickupClip;
				player.GetAudioSource().Play();
			}
			
			HideObject();
			Invoke("DestroySelf", 1.0f);
		} else {
			damageSprite.GetComponent<SpriteRenderer>().sprite = SingletonFactory.GetInstance<PrefabUtil>().damageSprite[maxHp - curHp - 1];
		}
	}

	public void HideObject() {
		isHidden = true;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = false;
		}
	}

	private void DestroySelf() {
		Destroy(gameObject);
	}
}
