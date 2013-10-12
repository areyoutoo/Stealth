using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EdTransformExtensions {
	[MenuItem("Stealth/Transforms/Clear Local Positions", true)]
	static bool ValidateClearLocalPositions() {
		var transforms = Selection.transforms;
		return transforms != null && transforms.Length > 0;
	}
	
	[MenuItem("Stealth/Transforms/Clear Local Positions")]
	static void ClearLocalPositions() {
		Undo.RegisterSceneUndo("Clear Local Positions");
		var transforms = EdSelect.TopTransforms();
		foreach(Transform transform in transforms) {
			transform.ClearLocalPositionRecursively();
		}
	}
}
