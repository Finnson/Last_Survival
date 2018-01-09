using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintTextController : MonoBehaviour {

	public Text HintText;

	private string HintContent;

	// Use this for initialization
	void Start () {
		if (HintText == null) {
			Debug.LogError ("HintTextController params are null!");
		}

		HintText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		HintText.text = HintContent;
	}

	public void setHintContent(string hintContent)
	{
		HintContent = hintContent;
	}

}
