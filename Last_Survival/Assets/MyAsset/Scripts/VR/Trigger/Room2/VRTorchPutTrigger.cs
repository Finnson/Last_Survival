using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTorchPutTrigger : MonoBehaviour {

	public VRTorchMoveTrigger torchMove;
    public VRGameController gameController;
	public VRAudioController audioController;

	public bool isEnd = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (collider.transform.gameObject.name == "Torch_2" && !isEnd) {
			torchMove.isEnd = true;
			isEnd = true;
            gameController.isTrochSet = true;

			//play audio
			audioController.PlayAudio("TorchPut");
		}
	}
}
