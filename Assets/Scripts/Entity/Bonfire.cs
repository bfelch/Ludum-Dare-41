using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonfire : MonoBehaviour {

	private float healCooldown = 0.0f;
	public SpriteRenderer fireGlow;

	// Use this for initialization
	void Start () {
		Invoke("DimFire", 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (healCooldown > 0.0f) {
			healCooldown -= Time.deltaTime;
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (healCooldown <= 0.0f) {
			Player player = collider.gameObject.GetComponent<Player>();
			if (player != null) {
				player.Heal(1);
			}

			healCooldown = 4.0f;
		}
	}

	private void DimFire() {
		fireGlow.color -= new Color(0, 0, 0, 0.02f);

		if (fireGlow.color.a <= 0) {
			AudioSource audioSource = GetComponent<AudioSource>();
			if (audioSource != null) {
				audioSource.Play();
			}

			GetComponent<Obstacle>().HideObject();
			Invoke("DestroySelf", 2.0f);
		} else {
			Invoke("DimFire", 5.0f);
		}
	}

	private void DestroySelf() {
		Destroy(gameObject);
	}
}
