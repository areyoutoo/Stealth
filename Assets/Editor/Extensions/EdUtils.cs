using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class EdUtils {
	const int COUNTING_REPORT_INTERVAL = 500;
	
	public static void SceneAction<T>(string title, IEnumerable<T> targets, System.Func<T, bool> action) {				
		try {
			//register undo
			EditorUtility.DisplayProgressBar(title, "Registering undo...", 0f);
			Undo.RegisterSceneUndo(title);
			
			//count targets
			EditorUtility.DisplayProgressBar(title, "Counting...", 0f);
			int reportCounter=COUNTING_REPORT_INTERVAL;
			int count=0;
			foreach (var t in targets) {
				count++;
				reportCounter--;
				if (reportCounter < 0) {
					reportCounter = COUNTING_REPORT_INTERVAL;
					string msg = string.Format("Counting... {0} ({1})", count, t.ToString());
					EditorUtility.DisplayProgressBar(title, msg, 0.1f);
				}
			}
			
			//fire actions
			int successes=0;
			int progress=0;
			foreach(var target in targets) {
				float p = 0.1f + (0.9f * ((1f*progress)/(1f*count)));
				EditorUtility.DisplayProgressBar(title, target.ToString(), p);
				if (action(target)) {
					successes++;
				}
				progress++;
			}
			
			//log completion
			Debug.Log(string.Format("{0} finished. {1}/{2} returned true.", title, successes, count));
			
		//catch-all
		} catch (System.Exception e) {
			Debug.LogError("Encountered exception, recommend undo! Check log for details.\n" + e);
			
		//failsafe: at least clear the progress bar
		} finally {
			EditorUtility.ClearProgressBar();
		}
	}
}
