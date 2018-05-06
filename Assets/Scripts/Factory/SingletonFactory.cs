using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonFactory : MonoBehaviour {

	private static Dictionary<System.Type, object> instances = new Dictionary<System.Type, object>();

	public static T GetInstance<T>() {
		if (!instances.ContainsKey(typeof(T))) {
			if (typeof(T).IsSubclassOf(typeof(MonoBehaviour))) {
				instances.Add(typeof(T), GameObject.FindObjectOfType(typeof(T)));
			} else {
				instances.Add(typeof(T), System.Activator.CreateInstance<T>());
			}
		}

		return (T)instances[typeof(T)];
	}

	public static void ClearInstances() {
		System.Type[] keys = instances.Keys.ToArray();

		foreach (System.Type key in keys) {
			instances.Remove(key);
		}
	}
}
