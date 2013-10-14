using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class EdSelect {
	
	public static List<T> Components<T>() where T : Component {
		List<T> components = new List<T>();
		foreach (var go in Selection.gameObjects) {
			foreach (var c in go.GetComponents<T>()) {
				if (!components.Contains(c)) {
					components.Add(c);
				}
			}
		}
		return components;
	}
	
	public static List<T> ComponentsInChildren<T>() where T : Component {
		List<T> components = new List<T>();
		foreach (var go in Selection.gameObjects) {
			foreach (var c in go.GetComponentsInChildren<T>()) {
				if (!components.Contains(c)) {
					components.Add(c);
				}
			}
		}
		return components;
	}
	
	public static Transform[] TopTransforms() {
		return GetFiltered<Transform>(SelectionMode.TopLevel);
	}
	
	public static T[] GetFiltered<T>(SelectionMode selectionMode) where T : UnityEngine.Object {
//		return (T[])(Selection.GetFiltered(typeof(T), selectionMode));
		
		UnityEngine.Object[] objects = Selection.GetFiltered(typeof(T), selectionMode);
		T[] casts = new T[objects.Length];
		for (int i=0; i<objects.Length; i++) {
			casts[i] = (T)objects[i];
		}
		return casts;
		
//		var objects = Selection.GetFiltered(typeof(T), selectionMode);
//		return (T[])objects;
	}
}

