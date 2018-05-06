using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	private int id = 0;

	// Use this for initialization
	void Start () {
		if (id == 0) {
			id = GameObject.FindGameObjectsWithTag(this.tag).Length;

			if (id > 1) {
				Destroy(gameObject);
			}
		}
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
