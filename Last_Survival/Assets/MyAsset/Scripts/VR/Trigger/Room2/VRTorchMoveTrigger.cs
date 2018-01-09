using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTorchMoveTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	private bool isTrigger = false;
	public bool isEnd = false;//set by VRTorchPutTrigger
    private bool hasSet = false;
	public Material chooseMat;
	private Material originMat;

    [Header("TorchInfo")]
    public float floorY;//Y value of floor
    public float dropSpeed;
    public float curTime = 0.0f;
    public float tempHeight = 0.0f;

	[Header("CabinetKey")]
	public GameObject cabinetKey;

	private Vector3 prevPos = Vector3.zero;

	private bool isDropingDown = false;

	// Use this for initialization
	void Start () {
		originMat = GetComponent<MeshRenderer> ().material;

		cabinetKey.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnd) {
            if (!hasSet)
            {
                clearStatus();
                cabinetKey.SetActive(true);
                hasSet = true;
            }
            return;
		}

		if (isTrigger) {
			if (vrinput.rightTriggerAxis.x > 0.5f) {
				Vector3 deltaPos = vrinput.rightControllerPosition - prevPos;
				if (deltaPos != Vector3.zero) {
					transform.position += deltaPos;
                    prevPos = vrinput.rightControllerPosition;
                }
                
			} else {
                clearStatus ();
                tempHeight = transform.position.y;
				isDropingDown = true;
            }
		}
        else//drop down to emulate gravity
        {
            float curHeight = tempHeight - dropSpeed * curTime * curTime;
            if (curHeight <= floorY)//hit ground
            {
                tempHeight = 0.0f;
                curTime = 0.0f;

				if (isDropingDown) {
					//play audio
					if (!audioController.IsAudioPlaying ("TorchHitGround")) {
						audioController.PlayAudio ("TorchHitGround");
					}

					isDropingDown = false;
				}
                return;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, curHeight, transform.position.z);
                curTime += Time.deltaTime;
            }
        }
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
			isTrigger = true;
			GetComponent<MeshRenderer> ().material = chooseMat;

			prevPos = vrinput.rightControllerPosition;

            curTime = 0.0f;
            tempHeight = 0.0f;
		} else {
            if (!isEnd)
            {
                clearStatus();
            }
		}
	}

	void OnTriggerExit(Collider collider){
		//clearStatus ();
	}

	void clearStatus(){
        isTrigger = false;
		prevPos = Vector3.zero;
		GetComponent<MeshRenderer> ().material = originMat;

		if (audioController.IsAudioPlaying ("TorchHitGround")) {
			audioController.StopAudio ("TorchHitGround");
		}
	}
}
