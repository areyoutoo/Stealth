using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicLooper : MonoBehaviour {
	[SerializeField] bool persistBetweenLevels;
	
	static MusicLooper instance;
	
	protected void Awake() {
		if (persistBetweenLevels) {
			if (instance == null) {
				instance = this;
				Object.DontDestroyOnLoad(gameObject);
			} else {
				Destroy(gameObject);
			}
		}
		
		audio.ignoreListenerPause = true;
		audio.ignoreListenerVolume = true;
		audio.loop = true;
		
		audio.bypassEffects = true;
		audio.bypassListenerEffects = true;
		
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.maxDistance = float.MaxValue;
		
		audio.dopplerLevel = 0f;		
		audio.panLevel = 0f;		
	}
	
	protected void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}
	
	public void SwitchTracks(AudioClip clip) {
		audio.Stop();
		audio.clip = clip;
		audio.Play();
	}
}
