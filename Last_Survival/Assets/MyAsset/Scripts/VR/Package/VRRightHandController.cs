using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRightHandController : MonoBehaviour {

	public VRInput vrinput;

	public bool isHold = false;

	private Animation anim;

	public float test;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		test = vrinput.rightTriggerAxis.x;
		if (vrinput.rightTriggerAxis.x > 0.5f) {
			if (!isHold) {
				isHold = true;
				anim.Play ("HandHold");
			}
		} else if (vrinput.rightTriggerAxis.x < 0.1f) {
			if (isHold) {
				isHold = false;
				anim.Play ("HandUnhold");
			}
		} else
			return;
	}
}
