using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

	protected AudioSource audioSource;
	public AudioClip damageClip;

	public float speed = 1.0f;
	public int maxHp = 10;
	protected int curHp = 0;

	protected List<Verb> verbStack = new List<Verb>();
	protected List<Item> itemStack = new List<Item>();
	protected List<Direction> directionStack = new List<Direction>();

	protected float verbCooldown = 0.0f;

	private Vector3 targetPosition = Vector3.zero;

	public Item recommendedTool;
	public Item givenItem;
	public float dropRate;

	// Use this for initialization
	public virtual void Start () {
		curHp = maxHp;

		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		verbCooldown -= Time.deltaTime;

		if (CanDoVerb()) {
			switch (verbStack[0]) {
				case Verb.MOVE:
					PerformMove();
					break;
			}
		}
	}

	public void Move(Direction direction, int distance) {
		for(int i = 0; i < distance; i++) {
			verbStack.Add(Verb.MOVE);
			directionStack.Add(direction);
		}
	}

	private void PerformMove() {
		if (targetPosition == Vector3.zero) {
			targetPosition = transform.position;

			if (IsPathBlocked(directionStack[0])) {
				CompleteDirectionVerb();
				return;
			}

			targetPosition = transform.position + GetVectorFromDirection(directionStack[0]);
		}

		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

		if (transform.position == targetPosition) {
			CompleteDirectionVerb();
		}
	}

	protected void CompleteVerb() {
		verbStack.RemoveAt(0);
	}

	protected void CompleteItemVerb() {
		CompleteVerb();
		itemStack.RemoveAt(0);
	}
		
	protected void CompleteDirectionVerb() {
		CompleteVerb();
		directionStack.RemoveAt(0);

		targetPosition = Vector3.zero;
	}

	protected bool IsPathBlocked(Direction direction) {
		return GetBlockingObject(direction);
	}

	protected RaycastHit2D GetBlockingObject(Direction direction) {
		Vector3 directionVector = GetVectorFromDirection(direction);

		return Physics2D.Raycast(transform.position, directionVector, 1.0f);
	}

	protected Vector3 GetVectorFromDirection(Direction direction) {
		Vector3 vector = Vector3.zero;

		switch (direction) {
			case Direction.NORTH:
				vector = Vector3.up;
				break;
			case Direction.EAST:
				vector = Vector3.right;
				break;
			case Direction.SOUTH:
				vector = Vector3.down;
				break;
			case Direction.WEST:
				vector = Vector3.left;
				break;
		}

		return vector;
	}

	protected bool CanDoVerb() {
		return (verbStack.Count > 0 && verbCooldown <= 0);
	}

	public AudioSource GetAudioSource() {
		return audioSource;
	}

	public void Damage(Item? equippedItem, Player player) {
		if (equippedItem != null && equippedItem == recommendedTool) {
			curHp -= maxHp;
		} else {
			curHp--;
		}

		audioSource.Play();

		if (curHp <= 0 && player != null) {
			if (Random.Range(0.0f, 1.0f) < dropRate) {
				player.AddItem(givenItem);
				SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Added " + givenItem + " to inventory");
				player.GetAudioSource().clip = SingletonFactory.GetInstance<PrefabUtil>().pickupClip;
				player.GetAudioSource().Play();
			}
			
			HideObject();
			Invoke("DestroySelf", 1.0f);
		}
	}

	private void HideObject() {
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = false;
		}
	}
}
