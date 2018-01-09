using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRecorderController : MonoBehaviour {

	public VRInput vrinput;
	public VRAudioController audioController;

	public GameObject screen;
	public Material originMat;
	public Material targetMat;

	private bool isPlaying = false;

	public float speed = 1.0f;
	private Vector2 speedVec;

	// Use this for initialization
	void Start () {
		speedVec = speed * new Vector2 (1, 0);//only to change x
	}
	
	// Update is called once per frame
	void Update () {
		if (vrinput.rightControllerTouchDown.trigger) {
			if (isPlaying) {
				//stop the audio
				audioController.StopAudio ("VoiceRecorder");

				screen.GetComponent<MeshRenderer> ().material = originMat;
				targetMat.SetTextureOffset ("_MainTex", new Vector2 (0, 0));//reset offset of the texture

				isPlaying = false;
			} else {
				//play the audio
				audioController.PlayAudio ("VoiceRecorder");

				screen.GetComponent<MeshRenderer> ().material = targetMat;

				isPlaying = true;
			}
		}

		if (isPlaying && !audioController.IsAudioPlaying ("VoiceRecorder")) {
			//stop the audio
			audioController.StopAudio ("VoiceRecorder");

			screen.GetComponent<MeshRenderer> ().material = originMat;
			targetMat.SetTextureOffset ("_MainTex", new Vector2 (0, 0));//reset offset of the texture

			isPlaying = false;
		}

		if (isPlaying) {			
			Vector2 tempOff = targetMat.GetTextureOffset ("_MainTex");
			tempOff += Time.deltaTime * speedVec;
			if (tempOff.x >= 1.0f)
				tempOff.x = 0.0f;
			targetMat.SetTextureOffset ("_MainTex", tempOff);//change x
		}
	}
}
