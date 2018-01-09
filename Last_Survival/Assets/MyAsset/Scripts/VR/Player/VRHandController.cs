using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandController : MonoBehaviour {

	public VRInput vrinput;

	public Material lightMat;
	public Material originMat;
	public GameObject glass;

	public GameObject spotLight;

	private bool isOn = false;

	// Use this for initialization
	void Start () {
		spotLight.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (vrinput.leftControllerTouchDown.trigger) {
			if (!isOn) {
				spotLight.SetActive (true);
				glass.GetComponent<MeshRenderer> ().material = lightMat;
				isOn = true;
			} else {
				spotLight.SetActive (false);
				glass.GetComponent<MeshRenderer> ().material = originMat;
				isOn = false;
			}
		}
	}
}
