using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(EdUtilsDemo))]
public class EdUtilsDemoInspector : Editor {
	Transform root;
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		
		Action(100);
		Action(1000);
		Action(3000);
	}
	
	void Action(int count) {
		root = null;
		
		string text = string.Format("Create {0} GameObjects", count);
		if (GUILayout.Button(text)) {
			List<int> numbers = new List<int>(count);
			for (int i=0; i<count; i++) {
				numbers.Add(i);
			}
			EdUtils.SceneAction<int>(text, numbers, SpawnGameObject);
		}
	}
	
	bool SpawnGameObject(int id) {
		var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.name = id.ToString();
		go.transform.position = Vector3.right * (id * 1.5f);
		
		if (root == null) {
			root = new GameObject("DemoObjects").transform;
		}
		go.transform.parent = root.transform;
		
		return go != null;
	}
}