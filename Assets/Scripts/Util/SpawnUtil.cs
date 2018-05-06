using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUtil : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 20; i++) {
			Instantiate(SingletonFactory.GetInstance<PrefabUtil>().zombiePre, new Vector3(105, 0, 0), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
