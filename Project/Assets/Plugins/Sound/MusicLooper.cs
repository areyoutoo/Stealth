using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicLooper : MonoBehaviour {
	[SerializeField] bool persistBetweenLevels;
	
	protected void Awake() {
		audio.ignoreListenerPause = true;
		audio.ignoreListenerVolume = true;
		audio.loop = true;
		
		audio.bypassEffects = true;
		audio.bypassListenerEffects = true;
		
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.maxDistance = float.MaxValue;
		
		audio.dopplerLevel = 0f;		
		audio.panLevel = 0f;		
		
		if (persistBetweenLevels) {
			Object.DontDestroyOnLoad(gameObject);
		}
	}
	
	public void SwitchTracks(AudioClip clip, float fadeTime) {
		if (fadeTime == 0f) {
			audio.Stop();
			audio.clip = clip;
			audio.Play();
		}
	}
}
