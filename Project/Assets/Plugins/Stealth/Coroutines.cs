using UnityEngine;
using System.Collections.Generic;

public enum CoroutineState {
	Normal,
	StopRequested,
	PauseRequested,
}

public class Coroutines : MonoBehaviour {
	static Dictionary<int, CoroutineState> marks;
	static int nextId;
	
	public static int GetId(CoroutineState initialState=CoroutineState.Normal) {
		int id = nextId++;
		marks.Add(id, initialState);
		return id;
	}
	
	public static CoroutineState GetState(int id) {
		if (!marks.ContainsKey(id)) {
			throw new System.InvalidOperationException("Create an ID before checking its state");
		}
		return marks[id];
	}
	
	public static void SetState(int id, CoroutineState state) {
		if (!marks.ContainsKey(id)) {
			throw new System.InvalidOperationException("Create an ID before setting its state");
		}
	}
	
	static Coroutines() {
		marks = new Dictionary<int, CoroutineState>();
	}
}
