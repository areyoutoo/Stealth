using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

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
	
	public static T[] GetFiltered<T>(SelectionMode selectionMode = SelectionMode.Unfiltered) where T : UnityEngine.Object {
		return Selection.GetFiltered(typeof(T), selectionMode).OfType<T>().ToArray();
	}
	
	public static void SelectComponentsInScene<T>() where T : Component {
		var objects = Object.FindObjectsOfType(typeof(T));
		SetSelection(objects);
	}
	
	public static void SelectComponentsInSelection<T>() where T : Component {
		var objects = GetFiltered<T>();
		SetSelection(objects);
	}
	
	public static void SetSelection(IEnumerable<Object> objects) {		
		Selection.objects = objects.ToArray();
	}
}

