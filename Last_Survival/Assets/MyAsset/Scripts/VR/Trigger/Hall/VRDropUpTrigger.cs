using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDropUpTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	private bool isTrigger = false;
	public bool isEnd = false;
	public Material chooseMat;
	private Material originMat;

	[Header("BoardInfo")]
	public float totalHeight;
	private float curAngle;//end when curAngle >= 90
	private float ratio;
	public GameObject board;
	public float dropSpeed;
    private Quaternion originQua;

	private float prevY = 0.0f;
	private float curTime = 0.0f;
	private float tempAngle = 0.0f;
    private float deltaY;

	// Use this for initialization
	void Start () {
		ratio = 90.0f / totalHeight;
		originMat = GetComponent<MeshRenderer> ().material;

        originQua = board.transform.localRotation;
    }
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (vrinput.rightTriggerAxis.x > 0.5f) {
				deltaY = vrinput.rightControllerPosition.y - prevY;
				if (deltaY > 0) {
					float deltaAngle = deltaY * ratio;
					curAngle += deltaAngle;

					//play audio
					if (!audioController.IsAudioPlaying ("BoardDropUp")) {
						audioController.PlayAudio ("BoardDropUp");
					}

					if (curAngle >= 90.0f) {
						isEnd = true;
						clearStatus ();
                        tempAngle = curAngle;

						gameController.isBoardOpen = true;
					} else {
						board.transform.localRotation = originQua * Quaternion.Euler (0, 0, curAngle);//rotate the board
					}
				}
			} else {
				clearStatus ();
                tempAngle = curAngle;
			}
		} else {
			if (curAngle <= 0.0f || curAngle >= 180.0f) {
				return;
			}

			//drop down because of gravity
			curTime += Time.deltaTime;
			if (curAngle < 90.0f) {
				curAngle = tempAngle - dropSpeed * curTime * curTime;
				if (curAngle <= 0.0f) {
					curTime = 0.0f;
					curAngle = 0.0f;
					tempAngle = 0.0f;
				}
                board.transform.localRotation = originQua * Quaternion.Euler(0, 0, curAngle);
            } else {
				curAngle = tempAngle + dropSpeed * curTime * curTime;
				if (curAngle >= 180.0f) {
					curTime = 0.0f;
					curAngle = 180.0f;
					tempAngle = 0.0f;
				}
                board.transform.localRotation = originQua * Quaternion.Euler(0, 0, curAngle);
            }
		}
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f && !isEnd) {
			isTrigger = true;
			GetComponent<MeshRenderer> ().material = chooseMat;

			prevY = vrinput.rightControllerPosition.y;
            curTime = 0.0f;
            tempAngle = 0.0f;
		} else {
			clearStatus ();
		}
	}

	void OnTriggerExit(Collider collider){
		//clearStatus ();
	}

	void clearStatus(){
		isTrigger = false;
		prevY = 0.0f;
		GetComponent<MeshRenderer> ().material = originMat;

		if (audioController.IsAudioPlaying ("BoardDropUp")) {
			audioController.StopAudio ("BoardDropUp");
		}
	}

}
