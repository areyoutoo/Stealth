using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EdTransformExtensions {
	const string CLEAR_RECURSIVE_POSITION = "Stealth/Transforms/Clear Recursive Position";
	const string CLEAR_RECURSIVE_ROTATION = "Stealth/Transforms/Clear Recursive Rotation";
	const string CLEAR_RECURSIVE_SCALE    = "Stealth/Transforms/Clear Recursive Scale";
	const string CLEAR_LOCAL_POSITION     = "Stealth/Transforms/Clear Local Position";
	const string CLEAR_LOCAL_ROTATION     = "Stealth/Transforms/Clear Local Rotation";
	const string CLEAR_LOCAL_SCALE        = "Stealth/Transforms/Clear Local Scale";
	
	[MenuItem(CLEAR_RECURSIVE_POSITION, true, 1000)]
	[MenuItem(CLEAR_RECURSIVE_ROTATION, true, 1001)]
	[MenuItem(CLEAR_RECURSIVE_SCALE   , true, 1002)]
	[MenuItem(CLEAR_LOCAL_POSITION    , true, 1101)]
	[MenuItem(CLEAR_LOCAL_ROTATION    , true, 1101)]
	[MenuItem(CLEAR_LOCAL_SCALE       , true, 1102)]
	static bool ValidateClearLocalPositions() {
		var transforms = Selection.transforms;
		return transforms != null && transforms.Length > 0;
	}
	
	[MenuItem(CLEAR_RECURSIVE_POSITION)]
	static void ClearLocalPositions() {
		SelectedTransformsAction("Clear Recursive Position", t => t.ClearLocalPositionRecursively());
	}
	
	[MenuItem(CLEAR_RECURSIVE_ROTATION)]
	static void ClearLocalRotations() {
		SelectedTransformsAction("Clear Recursive Rotation", t => t.ClearLocalRotationRecursively());
	}
	
	[MenuItem(CLEAR_RECURSIVE_SCALE)]
	static void ClearLocalScales() {
		SelectedTransformsAction("Clear Recursive Scale", t => t.ClearLocalScaleRecursively());
	}
	
	[MenuItem(CLEAR_LOCAL_POSITION)]
	static void ClearLocalPosition() {
		SelectedTransformsAction("Clear Local Position", t => t.ClearLocalPosition());
	}
	
	[MenuItem(CLEAR_LOCAL_ROTATION)]
	static void ClearLocalRotation() {
		SelectedTransformsAction("Clear Local Rotation", t => t.ClearLocalRotation());
	}
	
	[MenuItem(CLEAR_LOCAL_SCALE)]
	static void ClearLocalScale() {
		SelectedTransformsAction("Clear Local Scale", t => t.ClearLocalScale());
	}
	
	static void SelectedTransformsAction(string title, System.Action<Transform> action) {
		Undo.RegisterSceneUndo(title);
		var transforms = EdSelect.TopTransforms();
		foreach(Transform transform in transforms) {
			action(transform);
		}
	}
}
