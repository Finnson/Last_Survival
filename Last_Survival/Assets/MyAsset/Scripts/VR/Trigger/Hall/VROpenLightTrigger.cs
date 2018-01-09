using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VROpenLightTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;

	private bool isEnd = false;

	public Material LightSwitchOn;

	public GameObject ShootLight;

	public GameObject LightScreen;
	public Material LightOn;

	public GameObject Clown;

	// Use this for initialization
	void Start () {
		ShootLight.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f
			&& !isEnd) {
			//lighting switch turns to be red
			GetComponent<MeshRenderer>().material = LightSwitchOn;

			//open the light to make wall of Room B lighter
			ShootLight.SetActive(true);

			//play audio of open light
			audioController.PlayAudio("OpenLight");

			//change material of light screen
			LightScreen.GetComponent<MeshRenderer>().material = LightOn;

			//show up clown 
			Clown.SetActive(true);

			gameController.isLightStart = true;

			isEnd = true;
		}
	}
}
