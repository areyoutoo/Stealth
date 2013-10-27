using UnityEngine;
using System.Collections.Generic;

public static class TransformExtensions {
	
	/// <summary>
	/// Gets all direct children.
	/// </summary>
	/// <returns>
	/// The children.
	/// </returns>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static Transform[] GetChildren(this Transform root) {
		Transform[] children = new Transform[root.childCount];
		int i=0;
		foreach(Transform child in root) {
			children[i++] = child;
		}
		return children;
	}

	/// <summary>
	/// Attaches a list of transforms to this one.
	/// </summary>
	/// <param name='root'>
	/// Parent transform.
	/// </param>
	/// <param name='children'>
	/// Array, list, or other collection of transforms to add.
	/// </param>
	public static void AttachChildren(this Transform root, IEnumerable<Transform> children) {
		foreach(var child in children) {
			child.parent = root;
		}
	}
	
	public static void AttachChild(this Transform root, Transform child) {
		child.parent = root;
	}
	
	/// <summary>
	/// Clears the position of this transform without affecting its children.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalPosition(this Transform root) {
		var children = root.GetChildren();
		root.DetachChildren();
		root.localPosition = Vector3.zero;
		root.AttachChildren(children);
	}
	
	/// <summary>
	/// Clears the position of this transform and its children without affecting leaves.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalPositionRecursively(this Transform root) {
		root.ClearLocalPosition();		
		foreach(Transform child in root) {
			if (child.childCount > 0) {
				child.ClearLocalPositionRecursively();
			}
		}
	}
	
	/// <summary>
	/// Clears rotation of this transform without affecting its children.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalRotation(this Transform root) {
		var children = root.GetChildren();
		root.DetachChildren();
		root.localRotation = Quaternion.identity;
		root.AttachChildren(children);
	}
	
	/// <summary>
	/// Clears rotation of this transform and its children without affecting leaves.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalRotationRecursively(this Transform root) {
		root.ClearLocalRotation();
		foreach(Transform child in root) {
			if (child.childCount > 0) {
				child.ClearLocalRotationRecursively();
			}
		}
	}
	
	/// <summary>
	/// Clears scale of this transform without affecting children.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalScale(this Transform root) {
		var children = root.GetChildren();
		root.DetachChildren();
		root.localScale = Vector3.one;
		root.AttachChildren(children);
	}
	
	/// <summary>
	/// Clears scale of this transform and its children without affecting leaves.
	/// </summary>
	/// <param name='root'>
	/// Root.
	/// </param>
	public static void ClearLocalScaleRecursively(this Transform root) {
		root.ClearLocalScale();
		foreach(Transform child in root) {
			if (child.childCount > 0) {
				child.ClearLocalScaleRecursively();
			}
		}
	}
	
	/// <summary>
	/// Gets topmost transform from this one.
	/// </summary>
	/// <returns>
	/// The top transform.
	/// </returns>
	/// <param name='root'>
	/// Transform to search from.
	/// </param>
	public static Transform GetTopTransform(this Transform root) {
		while (root.parent != null) {
			root = root.parent;
		}
		return root;
	}
	
	/// <summary>
	/// Gets the bottom transforms.
	/// </summary>
	/// <returns>
	/// All transforms at the bottom of the hierarchy.
	/// </returns>
	/// <param name='root'>
	/// Top of tree.
	/// </param>
	public static List<Transform> GetBottomTransforms(this Transform root) {
		List<Transform> bottoms = new List<Transform>();
		GetBottomTransformsRecursively(root, ref bottoms);
		return bottoms;
	}
	
	public static void MultiplyScale(this Transform root, float multiplier) {
		root.transform.localScale = root.transform.localScale * multiplier;
	}
	
	public static void MultiplyScale(this Transform root, Vector3 multiplier) {
		root.transform.localScale = root.transform.localScale.WithScale(multiplier);
	}
	
	/// <summary>
	/// Helper function for GetBottomTransforms.
	/// </summary>
	/// <param name='root'>
	/// Current search root.
	/// </param>
	/// <param name='bottoms'>
	/// Cumulative list of leaf nodes.
	/// </param>
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
