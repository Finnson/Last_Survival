using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBreakTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;
	public VRMovieController movieController;

	public bool isEnd = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "IronBar_3" && packageController.prevItem == "IronBar_3" && vrinput.rightTriggerAxis.x > 0.5f
			&& !isEnd) {
			gameController.Interact ("Door_3");
			isEnd = true;
		}
	}
}
