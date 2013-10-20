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
	
	public static GameObject InstantiateChild(this GameObject root, GameObject prefab) {
		return root.InstantiateChild(prefab, Vector3.zero, Quaternion.identity);
	}
	
	public static GameObject InstantiateChild(this GameObject root, GameObject prefab, Vector3 position, Quaternion rotation, Space space=Space.Self) {
		GameObject child;
		if (space == Space.World) {
			child = (GameObject)GameObject.Instantiate(prefab, position, rotation);
			child.transform.parent = root.transform;
		} else {
			child = (GameObject)GameObject.Instantiate(prefab, root.transform.position, root.transform.rotation);
			child.transform.parent = root.transform;
			child.transform.localPosition = position;
			child.transform.localRotation = rotation;
		}
		
		return child;
	}
}
