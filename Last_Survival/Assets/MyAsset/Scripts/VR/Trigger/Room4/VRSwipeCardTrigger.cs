using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSwipeCardTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	public bool isTrigger = false;
	public bool isEnd = false;
	public Material chooseMat;
	private Material originMat;

	[Header("LockInfo")]
	public Transform startPointZ;//start of forward dir
	public Transform endPointZ;//end of forward dir
	public Vector3 forwardDir;
	public Transform startPointY;//start of up dir
	public Transform endPointY;//end of up dir
	public Vector3 upDir;
	public float swipeHeight;//swipe height
	public GameObject lockMesh;

	private float prevY = 0.0f;

	// Use this for initialization
	void Start () {
		originMat = lockMesh.GetComponent<MeshRenderer> ().material;
		forwardDir = Vector3.Normalize(endPointZ.position - startPointZ.position);//world
		upDir = Vector3.Normalize(endPointY.position - startPointY.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (Vector3.Angle (vrinput.right.transform.forward, forwardDir) < 40.0f &&
			    Vector3.Angle (vrinput.right.transform.up, upDir) < 40.0f) {
				float deltaY = prevY - vrinput.rightControllerPosition.y;
				if (deltaY >= swipeHeight) {
					isEnd = true;
					gameController.Interact ("Door_4");
					clearStatus ();
				}
			} else {
				clearStatus ();
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "DoorCard_3" && packageController.prevItem == "DoorCard_3" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
            print(Vector3.Angle(vrinput.right.transform.forward, forwardDir));
            print(vrinput.right.transform.forward);
            print(forwardDir);
			if (Vector3.Angle(vrinput.right.transform.forward,forwardDir) < 40.0f &&
				Vector3.Angle(vrinput.right.transform.up,upDir) < 40.0f) {//forward must point to the lock
				isTrigger = true;
				lockMesh.GetComponent<MeshRenderer> ().material = chooseMat;

				prevY = vrinput.rightControllerPosition.y;
			}
		} else {
			clearStatus ();
		}
	}

	void OnTriggerExit(Collider collider){
		clearStatus ();
	}

	void clearStatus(){
		isTrigger = false;
		lockMesh.GetComponent<MeshRenderer> ().material = originMat;
		prevY = 0.0f;
	}
}
