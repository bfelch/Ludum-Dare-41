using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

	public float speed = 1.0f;
	public int maxHp = 100;
	private int curHp = 0;

	protected List<Verb> verbStack = new List<Verb>();
	protected List<Item> itemStack = new List<Item>();
	protected List<Direction> directionStack = new List<Direction>();

	protected float verbCooldown = 0.0f;

	private Vector3 targetPosition = Vector3.zero;

	// Use this for initialization
	public virtual void Start () {
		curHp = maxHp;
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
}
