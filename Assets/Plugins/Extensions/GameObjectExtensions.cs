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
}
