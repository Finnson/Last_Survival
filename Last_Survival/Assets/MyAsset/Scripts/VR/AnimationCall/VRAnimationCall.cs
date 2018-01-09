using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimationCall : MonoBehaviour {

	public VRGameController gameController;
	public VRAudioController audioController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBSave(){
		gameController.SetBSave ();
	}

	public void SetDSave(){
		gameController.SetDSave ();
	}

	public void PlayHitGroundAudio(){
		audioController.PlayAudio ("HitGround");
	}

	public void PlayCrashAudio(){
		audioController.PlayAudio ("Crash");
	}
}
