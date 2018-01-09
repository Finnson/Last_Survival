using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMouseController : MonoBehaviour {

	private bool isTrigger = false;
	private bool isStart = false;
	private bool isArrive = false;

	private Vector3 eyePos;

	public GameObject cameraEye;
	public Transform farPoint;

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrigger) {
			if (!isStart) {
				eyePos = cameraEye.transform.position;
				isStart = true;
			} else {
				if (!isArrive) {
					transform.position += speed * Time.deltaTime * Vector3.Normalize ((eyePos - transform.position));
					if (Vector3.Distance (transform.position, eyePos) < 0.1f) {
						isArrive = true;
					}
				} else {
					transform.position += speed * Time.deltaTime * Vector3.Normalize ((farPoint.position - transform.position));
					if (Vector3.Distance (transform.position, farPoint.position) < 0.1f) {
						Destroy (this.gameObject);
					}
				}
			}
		}
	}

	public void SetTrigger(){
		isTrigger = true;
	}
}
