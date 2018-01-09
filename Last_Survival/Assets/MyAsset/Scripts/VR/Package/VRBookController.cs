using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBookController : MonoBehaviour {

	public MegaBook book;
	public VRInput vrinput;

	public int curIndex = 0;
	public bool isFliping = false;

	public float forwardSpeed = 10.0f;
	public float targetAlpha = 0.0f;

	public float backwardSpeed = 50.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (vrinput.rightControllerTouchDown.trigger && !isFliping) {
			curIndex = ++curIndex;
			if (curIndex == 5)
				targetAlpha = 100.0f;
			else if (curIndex == 6)
				targetAlpha = 0.0f;
			else
				targetAlpha = curIndex * 20.0f + 0.00001f;
			isFliping = true;
		}

		if (isFliping) {
			if (targetAlpha > 0.0f) {//forward
				if (book.bookalpha < targetAlpha) {
					book.bookalpha += forwardSpeed * Time.deltaTime;
				} else {
					book.bookalpha = targetAlpha;
					isFliping = false;
				}
			} else {//backward
				if (book.bookalpha > targetAlpha) {
					book.bookalpha -= backwardSpeed * Time.deltaTime;
				} else {
					book.bookalpha = targetAlpha;//0.0f
					isFliping = false;
				}
			}
		}
	}
}
