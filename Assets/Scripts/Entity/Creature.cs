using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

	protected bool isHidden = false;
	public bool IsHidden {
		get { return isHidden; }
	}
	protected AudioSource audioSource;
	public AudioClip damageClip;

	public float speed = 1.0f;
	public int maxHp = 10;
	protected int curHp = 0;
	public int CurHp {
		get { return curHp; }
	}

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
				case Verb.USE:
					PerformUse();
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
				// if (GetComponent<Player>() != null) {
				// 	SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Cannot continue moving " + directionStack[0].ToString());
				// }
				CompleteDirectionVerb();
				return;
			}

			targetPosition = transform.position + GetVectorFromDirection(directionStack[0]);
		}

		// if (GetComponent<Player>() != null) {
		// 	SingletonFactory.GetInstance<ParserUtil>().PrintResponse("Moving " + directionStack[0].ToString());
		// }
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

		if (transform.position == targetPosition) {
			float x = Mathf.Round(transform.position.x);
			float y = Mathf.Round(transform.position.y);
			transform.position = new Vector3(x, y, 0.0f);

			CompleteDirectionVerb();
		}
	}

	protected virtual void PerformUse() {
		verbCooldown = 1.0f;

		RaycastHit2D hit = GetBlockingObject(directionStack[0]);

		if (hit.collider != null) {
			Obstacle obstacle = hit.collider.gameObject.GetComponent<Obstacle>();
			if (obstacle != null) {
				if (!obstacle.IsHidden) {
					obstacle.Damage(null, null);
				} else {
					verbCooldown = 0.0f;
				}
			} else {
				Creature creature = hit.collider.gameObject.GetComponent<Creature>();
				if (creature != null && creature.CurHp > 0) {
					creature.Damage(null, null);
				}
			}
		}

		CompleteDirectionVerb();
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

	protected void ClearVerbStack() {
		verbStack.Clear();
		itemStack.Clear();
		directionStack.Clear();

		verbCooldown = 0.0f;
	}

	protected bool IsPathBlocked(Direction direction) {
		Vector3 targetPosition = GetVectorFromDirection(direction) + transform.position;
		bool blocked = false;
		blocked |= Mathf.Abs(targetPosition.x) > GenerationUtil.maxMapBound;
		blocked |= Mathf.Abs(targetPosition.y) > GenerationUtil.maxMapBound;

		RaycastHit2D hit = GetBlockingObject(direction);
		blocked |= hit;

		if (hit.collider != null) {
			Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
			if (obstacle != null && obstacle.IsHidden) {
				blocked = false;
			}
		}

		return blocked;
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

	protected Direction GetDirectionFromVector(Vector3 vector) {
		Direction direction = Direction.NORTH;

		float angle = Vector3.Angle(vector, Vector3.right + Vector3.up);
		if (angle < 90.0f) {
			if (Mathf.Abs(vector.y) > Mathf.Abs(vector.x)) {
				direction = Direction.NORTH;
			} else {
				direction = Direction.EAST;
			}
		} else if (angle == 90.0f) {
			if (vector.y > 0.0f) {
				direction = Direction.NORTH;
			} else {
				direction = Direction.SOUTH;
			}
		} else {
			if (Mathf.Abs(vector.y) > Mathf.Abs(vector.x)) {
				direction = Direction.SOUTH;
			} else {
				direction = Direction.WEST;
			}
		}

		return direction;
	}

	protected bool CanDoVerb() {
		return (verbStack.Count > 0 && verbCooldown <= 0 && !IsHidden);
	}

	public AudioSource GetAudioSource() {
		return audioSource;
	}

	public virtual void Damage(Item? equippedItem, Player player) {
		if (equippedItem != null && equippedItem == recommendedTool) {
			curHp -= maxHp;
		} else {
			curHp--;
		}

		audioSource.Play();

		if (curHp <= 0) {
			if (player != null && Random.Range(0.0f, 1.0f) < dropRate) {
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
		isHidden = true;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = false;
		}
	}

	private void DestroySelf() {
		Zombie zombie = GetComponent<Zombie>();
		if (zombie != null) {
			zombie.Reset();
		} else {
			Destroy(gameObject);
		}
	}
}
