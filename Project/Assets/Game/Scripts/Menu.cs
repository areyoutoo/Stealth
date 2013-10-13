using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(40f);
		GUILayout.BeginVertical();
		GUILayout.Space(40f);
	
		GUI.contentColor = Color.grey;
		GUILayout.Label("STEALTH");
		
		GUI.contentColor = Color.white;
		GUILayout.Label("");
		GUILayout.Label("A 3-hour game by Robert Utter");
		GUILayout.Label("");
		GUILayout.Label("");
		
		if (GUILayout.Button("Play!")) {
			Application.LoadLevel(1);
		}
		
		if (GUILayout.Button("Quit :(")) {
			Application.Quit();
		}
		
		GUILayout.EndVertical();
		
		GUILayout.Space(60f);
		
		GUILayout.BeginVertical();
		
		GUILayout.Space(90f);
		GUILayout.Label("Instructions");
		GUILayout.Label("");
		GUILayout.Label("WASD moves");
		GUILayout.Label("SPACE jumps");
		GUILayout.Label("Dodge guards; collect gold");
		GUILayout.Label("");
		GUILayout.Label("Hint: you can jump through platforms");

		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
	}
}
