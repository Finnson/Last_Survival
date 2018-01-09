using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDropDownTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	private bool isTrigger = false;
	private bool isEnd = false;
	public Material chooseMat;
	private Material originMat;

	[Header("BarInfo")]
	public Transform startPoint;//start of tail
	public Transform endPoint;//end of tail
	private float handleRatio = 0.0f;
	private float curAngle = 0.0f;//angle of bar
    private Quaternion originQua;

	[Header("ShieldInfo")]
	public float totalHeight;
	private float shieldRatio = 0.0f;
	private float startHeight = 0.0f;
	public GameObject shield;
    public GameObject smoke;

	private float prevY = 0.0f;

	// Use this for initialization
	void Start () {
		handleRatio = 90.0f / (startPoint.position.y - endPoint.position.y);
		shieldRatio = totalHeight / 90.0f;
		startHeight = shield.transform.position.y;
        originQua = transform.localRotation;

		originMat = GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (vrinput.rightTriggerAxis.x > 0.5f) {//press right trigger
				float deltaY = prevY - vrinput.rightControllerPosition.y;
				if (deltaY > 0) {
					//update bar angle
					curAngle += deltaY * handleRatio;
					transform.localRotation = originQua * Quaternion.Euler (new Vector3 (0, 0, curAngle));
					prevY = vrinput.rightControllerPosition.y;//update prevY

					//update shield height
					float curHeight = curAngle * shieldRatio;
					shield.transform.position = new Vector3 (shield.transform.position.x, startHeight - curHeight, shield.transform.position.z);

					//play audio
					if (!audioController.IsAudioPlaying ("ShieldDown")) {
						audioController.PlayAudio ("ShieldDown");
					}

					smoke.GetComponent<ParticleSystem> ().Play ();

					if (curAngle >= 90.0f) {
						isEnd = true;
						clearStatus ();
                        gameController.isShieldDown_1 = true;
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
			GetComponent<MeshRenderer> ().material = chooseMat;
			prevY = vrinput.rightControllerPosition.y;
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

		smoke.GetComponent<ParticleSystem> ().Stop ();

		if (audioController.IsAudioPlaying ("ShieldDown")) {
			audioController.StopAudio ("ShieldDown");
		}
	}
}
