using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class ListEditor {
	static List<System.Object> _list = new List<object>();
	static System.Object[] _array;

	
	public static bool EditList<T>(UnityEngine.Object subject, ref List<T> list, System.Func<T,T> editFunc) {
		list.Clear();
		list.AddRange(list);
		return EditListOrArray<T>(
			subject,
			ListLength,
			ListGet<T>,
			ListSet<T>,
			ListRemove,
			ListAdd<T>,
			t => editFunc(t)
		);
	}
	
	static int ListLength() {
		return _list.Count;
	}
	
	static T ListGet<T>(int i) {
		return (T)_list[i];
	}
	
	static void ListSet<T>(int i, T item) {
		_list[i] = item;
	}
	
	static void ListAdd<T>(T item) {
		_list.Add(item);
	}
	
	static void ListRemove(int i) {
		_list.RemoveAt(i);
	}
	
	
	public static bool EditArray<T>(UnityEngine.Object subject, ref T[] list, System.Func<T,T> editFunc) {
		_array = new System.Object[list.Length];
		for (int i=0; i<list.Length; i++) {
			_array[i] = list[i];
		}
		return EditListOrArray<T>(
			subject,
			ArrayLength,
			ArrayGet<T>,
			ArraySet<T>,
			ArrayRemove,
			ArrayAdd<T>,
			(t) => editFunc(t)
		);
	}
	
	static int ArrayLength() {
		return _array.Length;
	}
	
	static T ArrayGet<T>(int i) {
		return (T)_array[i];
	}
	
	static void ArraySet<T>(int i, T item) {
		_array[i] = item;
	}
	
	static void ArrayAdd<T>(T item) {
		ArrayUtility.Add<System.Object>(ref _array, (System.Object)item);
	}
	
	static void ArrayRemove(int i) {
		ArrayUtility.RemoveAt<System.Object>(ref _array, i);
	}
	
	
	public static bool EditListOrArray<T>(
		UnityEngine.Object subject,
		System.Func<int> lenFunc,
		System.Func<int,T> getFunc,
		System.Action<int,T> setFunc,
		System.Action<int> delFunc,
		System.Action<T> appendFunc,
		System.Func<T,T> editFunc
	) {
		
		bool dirty = false;
		
		EditorGUILayout.BeginVertical();
		
		int len = lenFunc();
		for (int i=0; i<len; i++) {
			EditorGUILayout.BeginHorizontal();
			T itemA = getFunc(i);
			T itemB = editFunc(itemA);
//			if (itemA != itemB) {
//				setFunc(i, itemB);
//				dirty = true;
//			}
			//TODO
			
			if (GUILayout.Button("-", GUILayout.Width(10f)) && i > 0) {
				setFunc(i, getFunc(i-1));
				setFunc(i-1, itemB);
				dirty = true;
			}
			
			if (GUILayout.Button("+", GUILayout.Width(10f)) && i < len-1) {
				setFunc(i, getFunc(i+1));
				setFunc(i+1, itemB);
				dirty = true;
			}
			
			if (GUILayout.Button("x", GUILayout.Width(10f))) {
				delFunc(i);
				dirty = true;
			}
			
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUILayout.Button("Add new")) {
			appendFunc(default(T));
			dirty = true;
		}
		
		EditorGUILayout.EndVertical();
		
		if (dirty) {
			EditorUtility.SetDirty(subject);
		}
		return dirty;
	}
	
}
