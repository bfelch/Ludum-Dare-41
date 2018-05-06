using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationUtil : MonoBehaviour {

	public static readonly int maxMapBound = 100;

	// Use this for initialization
	void Start () {
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateMap() {
		for (int x = -maxMapBound; x < maxMapBound + 1; x++) {
			for (int y = -maxMapBound; y < maxMapBound + 1; y++) {
				if ((x != 0 || y != 0) && Random.value < 0.1f) {
					float resourceType = Random.value * 3.0f;
					GameObject resource;
					PrefabUtil prefabUtil = SingletonFactory.GetInstance<PrefabUtil>();

					if (resourceType < 1.0f) {
						resource = prefabUtil.treePre;
					} else if (resourceType < 2.0f) {
						resource = prefabUtil.stonePre;
					} else {
						resource = prefabUtil.ironPre;
					}

					Instantiate(resource, new Vector3(x, y, 0), Quaternion.identity);
				}
			}
		}
	}
}
