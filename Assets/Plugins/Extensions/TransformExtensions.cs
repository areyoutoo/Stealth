using UnityEngine;
using System.Collections.Generic;

public static class TransformExtensions {
	public static void AttachChildren(this Transform root, IEnumerable<Transform> children) {
		foreach(var child in children) {
			child.parent = root;
		}
	}
	
	public static void ClearLocalPosition(this Transform root) {
		List<Transform> children = new List<Transform>(root.childCount);
		foreach(Transform child in root) {
			children.Add(child);
		}
		
		root.DetachChildren();
		root.localPosition = Vector3.zero;
		root.AttachChildren(children);
	}
	
	public static void ClearLocalPositionRecursively(this Transform root) {
		root.ClearLocalPosition();
		
		foreach(Transform child in root) {
			if (child.childCount > 0) {
				child.ClearLocalPositionRecursively();
			}
		}
	}
	
	public static Transform GetTopTransform(this Transform root) {
		while (root.parent != null) {
			root = root.parent;
		}
		return root;
	}
	
	public static List<Transform> GetBottomTransforms(this Transform root) {
		List<Transform> bottoms = new List<Transform>();
		GetBottomTransformsRecursively(root, ref bottoms);
		return bottoms;
	}
	
	private static void GetBottomTransformsRecursively(Transform root, ref List<Transform> bottoms) {
		if (root.childCount == 0) {
			bottoms.Add(root);
		} else {
			foreach(Transform child in root) {
				GetBottomTransformsRecursively(child, ref bottoms);
			}
		}
	}
}
