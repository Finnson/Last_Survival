using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAudioController : MonoBehaviour {

	[System.Serializable]
	public class AudioInfo{
		public string audioName;
		public AudioSource audioSource;
	};
		
	public List<AudioInfo> audios;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	public void PlayAudio(string name) {
		for (int i = 0; i < audios.Count; i++) {
			if (audios [i].audioName == name) {
				audios [i].audioSource.Play ();
				return;
			}
		}
		Debug.Log ("Can't find audio:" + name);
		return;
	}

	public void StopAudio(string name){
		for (int i = 0; i < audios.Count; i++) {
			if (audios [i].audioName == name) {
				audios [i].audioSource.Stop();
				return;
			}
		}
		Debug.Log ("Can't find audio:" + name);
		return;
	}

	public bool IsAudioPlaying(string name){
		for (int i = 0; i < audios.Count; i++) {
			if (audios [i].audioName == name) {
				return audios [i].audioSource.isPlaying;
			}
		}
		Debug.Log ("Can't find audio:" + name);
		return false;
	}
}
