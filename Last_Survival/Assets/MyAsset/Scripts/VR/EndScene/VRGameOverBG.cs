using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRGameOverBG : MonoBehaviour {

	public Image gameOverBG;

	public float speed = 0.5f;
	public float curTime = 0.0f;

	public bool isEnd = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isEnd) {
			curTime += Time.deltaTime;

			if (curTime >= 2.0f) {
				gameOverBG.color = new Color (gameOverBG.color.r, gameOverBG.color.g, gameOverBG.color.b, 1.0f);

				isEnd = true;
			} else {
				gameOverBG.color = new Color (gameOverBG.color.r, gameOverBG.color.g, gameOverBG.color.b, speed * curTime);
			}
		}
	}
}
