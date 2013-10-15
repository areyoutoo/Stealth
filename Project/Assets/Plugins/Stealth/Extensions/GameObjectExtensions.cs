using UnityEngine;
using System.Collections;

public static class GameObjectExtensions {
	public static void SetLayerRecursively(this GameObject root, int layer) {
		root.layer = layer;
		foreach (Transform child in root.transform) {
			child.gameObject.SetLayerRecursively(layer);
		}
	}
	
	public static void SetLayerRecursively(this GameObject root, string layerName) {
		int layer = LayerMask.NameToLayer(layerName);
		root.SetLayerRecursively(layer);
	}
	
	public static bool RemoveComponent<T>(this GameObject root, bool immediate=false) where T : Component {
		T component = root.GetComponent<T>();
		
		bool found = component != null;
		if (found) {
			if (immediate) {
				GameObject.DestroyImmediate(component);
			} else {
				GameObject.Destroy(component);
			}
		}
		
		return found;
	}
	
	public static bool RemoveComponents<T>(this GameObject root, bool immediate=false) where T : Component {
		T[] components = root.GetComponents<T>();
		
		bool found = components.Length > 0;
		if (found) {
			for (int i=0; i<components.Length; i++) {
				if (immediate) {
					GameObject.DestroyImmediate(components[i], false);
				} else {
					GameObject.Destroy(components[i]);
				}
			}
		}
		
		return found;
	}
	
	public static bool RemoveComponentsInChildren<T>(this GameObject root, bool immediate=false) where T : Component {
		T[] components = root.GetComponentsInChildren<T>();
		
		bool found = components.Length > 0;
		if (found) {
			for (int i=0; i<components.Length; i++) {
				if (immediate) {
					GameObject.DestroyImmediate(components[i], false);
				} else {
					GameObject.Destroy(components[i]);
				}
			}
		}
		
		return found;
	}
}
