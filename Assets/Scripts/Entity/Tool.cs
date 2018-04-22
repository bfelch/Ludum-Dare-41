using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource != null) {
			audioSource.Play();
		}
		
		Invoke("DestroySelf", (audioSource != null ? audioSource.clip.length : 0.2f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void DestroySelf() {
		Destroy(gameObject);
	}
}
