using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VROpenDrawerTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	private bool isTrigger = false;
	private bool isEnd = false;
	public Material chooseMat;
	private Material originMat;

	[Header("DrawerInfo")]
	public Transform startPoint;//start of drag
	public Transform endPoint;//end of drag
	private Vector3 dragDir;
	public float totalLength;
	private Vector3 startDrawerPos;
	private float curLength = 0.0f;
    public GameObject mesh0;
    public GameObject mesh1;
	private float speed = 0.25f;


	private Vector3 prevPos;

	// Use this for initialization
	void Start () {
		dragDir = endPoint.position - startPoint.position;

		startDrawerPos = transform.position;
		originMat = mesh0.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (vrinput.rightTriggerAxis.x > 0.5f) {
				float deltaZ = Vector3.Dot (vrinput.rightControllerPosition - prevPos, dragDir);
				if (deltaZ > 0) {
                    curLength += deltaZ * speed;
					transform.position = startDrawerPos - curLength * transform.up;

					if (curLength >= totalLength) {
						isEnd = true;
						clearStatus ();
                        gameController.isDrawerOpen_1 = true;
					}
				}
			} else {
				clearStatus ();
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
			isTrigger = true;
			mesh0.GetComponent<MeshRenderer> ().material = chooseMat;
            mesh1.GetComponent<MeshRenderer> ().material = chooseMat;

            prevPos = vrinput.rightControllerPosition;
		} else {
			clearStatus ();
		}
	}

	void OnTriggerExit(Collider collider){
		//clearStatus ();
	}

	void clearStatus(){
		isTrigger = false;
		prevPos = Vector3.zero;
        mesh0.GetComponent<MeshRenderer>().material = originMat;
        mesh1.GetComponent<MeshRenderer>().material = originMat;
    }
}
