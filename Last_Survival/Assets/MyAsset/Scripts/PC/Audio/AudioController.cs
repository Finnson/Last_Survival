using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

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

	public void PlayAudio(string name){
		for (int i = 0; i < audios.Count; i++) {
			if (audios [i].audioName == name) {
				audios [i].audioSource.Play ();
				return;
			}
		}
		Debug.Log ("Can't find audio:" + name);
		return;
	}
}
