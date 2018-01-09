using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBeatNoseTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	public int count = 0;
	private bool isEnd = false;

	public GameObject leftEye;
	public GameObject rightEye;
	public GameObject tongue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
			//change texture in the wall
			count++;

			//left eye
			if (count == 1) {
				leftEye.GetComponent<Animation> ().Play ("Left_Eye_Open");
			} 
			//right eye
			else if (count == 2) {
				rightEye.GetComponent<Animation> ().Play ("Right_Eye_Open");
			}
			//mouse
			else if (count == 3) {
				tongue.GetComponent<Animation> ().Play ("Tongue_Open");
				isEnd = true;//done
			} else {
				print ("VRBeatNoseTrigger error!");
			}
		}
	}
}
