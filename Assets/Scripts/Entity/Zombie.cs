using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Creature {

	// Use this for initialization
	public override void Start () {
		base.Start();

		Reset();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if (verbStack.Count <= 0) {
			GetNextAction();
		}
	}

	public void Reset() {
		isHidden = false;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = true;
		}

		curHp = maxHp;
		ClearVerbStack();

		float x = Random.value * (GenerationUtil.maxMapBound * 2 + 5) - GenerationUtil.maxMapBound;
		float y = Random.value * (GenerationUtil.maxMapBound * 2 + 5) - GenerationUtil.maxMapBound;

		if (x < y) {
			x = Mathf.Sign(x) * 100;
		} else {
			y = Mathf.Sign(y) * 100;
		}

		this.transform.position = new Vector3(x, y, 0);
	}

	private void GetNextAction() {
		Vector3 ray = SingletonFactory.GetInstance<Player>().transform.position - transform.position;
		Direction direction = GetDirectionFromVector(ray);

		RaycastHit2D hit = GetBlockingObject(direction);
		if (hit.collider != null) {
			Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
			if (obstacle != null) {
				verbStack.Add(Verb.USE);
				directionStack.Add(direction);
			} else {
				Player player = hit.collider.GetComponent<Player>();
				if (player != null) {
					verbStack.Add(Verb.USE);
					directionStack.Add(direction);
				}
			}
		} else {
			verbStack.Add(Verb.MOVE);
			directionStack.Add(direction);
		}
	}
}
