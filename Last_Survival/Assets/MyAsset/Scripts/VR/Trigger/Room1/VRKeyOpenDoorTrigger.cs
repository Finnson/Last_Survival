using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeyOpenDoorTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	public bool isTrigger = false;
	public bool isEnd = false;
	public Material chooseMat;
	public Material originMat;

	[Header("LockInfo")]
	public Transform startPoint;//start of lock dir
	public Transform endPoint;//end of lock dir
	public Vector3 lockDir;
    public Quaternion originQua;

    public float startAngle = 0.0f;//angle of z axis when trigger
	public float rotateAngle = 0.0f;//angle of lock

	// Use this for initialization
	void Start () {
		originMat = GetComponent<MeshRenderer> ().material;
        originQua = transform.localRotation;

        lockDir = Vector3.Normalize(endPoint.position - startPoint.position);//world
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (vrinput.rightTriggerAxis.x > 0.5f && Vector3.Angle (vrinput.right.transform.forward, lockDir) < 80.0f) {
                rotateAngle = startAngle - vrinput.right.transform.localRotation.eulerAngles.z;
                if (rotateAngle < -180) rotateAngle += 360.0f;
				if (rotateAngle > 0 && rotateAngle < 180) {//turn on right
					if (rotateAngle >= 75.0f) {
						isEnd = true;
						gameController.Interact ("Door_1");//open the door
						clearStatus ();
					}

					//update rotation of lock
					transform.localRotation = originQua * Quaternion.Euler (0, rotateAngle, 0);
				}
			} else {
				clearStatus ();
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Key_1" && packageController.prevItem == "Key_1" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
			if (Vector3.Angle(vrinput.right.transform.forward,lockDir) < 80.0f) {//forward must point to the lock
				isTrigger = true;
				GetComponent<MeshRenderer> ().material = chooseMat;

				startAngle = vrinput.right.transform.localRotation.eulerAngles.z;
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
		GetComponent<MeshRenderer> ().material = originMat;
		startAngle = 0.0f;
        rotateAngle = 0.0f;
        transform.localRotation = originQua;//reset the lock rotation
    }
}
