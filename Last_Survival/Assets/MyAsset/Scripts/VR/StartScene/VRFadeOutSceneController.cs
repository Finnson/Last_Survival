using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRFadeOutSceneController : MonoBehaviour {

	public GameObject ForeGround;

	public bool isStart = false;
	public bool isSwitch = false;

	public float speed = 0.5f;
	public float curTime = 0.0f;

	// Use this for initialization
	void Start () {
		ForeGround.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isStart) {
			if (!ForeGround.activeSelf)
				ForeGround.SetActive (true);

			if (!isSwitch) {
				curTime += Time.deltaTime;

				if (curTime >= 2.0f) {
					ForeGround.GetComponent<Image>().color = new Color(ForeGround.GetComponent<Image>().color.r,ForeGround.GetComponent<Image>().color.g,ForeGround.GetComponent<Image>().color.b,1.0f);

					isSwitch = true;
				} else {
					ForeGround.GetComponent<Image>().color = new Color(ForeGround.GetComponent<Image>().color.r,ForeGround.GetComponent<Image>().color.g,ForeGround.GetComponent<Image>().color.b, speed * curTime);;
				}
			} else {
				SceneManager.LoadScene ("mainVRtest");
			}
		}
	}

	public void SwitchScene(){
		isStart = true;
	}
}
