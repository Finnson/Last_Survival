using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRTriggerTextController : MonoBehaviour {

	public Text QuesText;
	public Text LeftAnsText;
	public Text RightAnsText;

	// Use this for initialization
	void Start () {
		if (QuesText == null || LeftAnsText == null || RightAnsText == null) {
			Debug.LogError ("TriggerTextController params are null!");
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetTriggerText(string ques,string leftAns,string rightAns)
	{
		QuesText.text = ques;
		LeftAnsText.text = leftAns;
		RightAnsText.text = rightAns;
	}

	public void ClearTriggerText()
	{
		QuesText.text = "";
		LeftAnsText.text = "";
		RightAnsText.text = "";
	}
}
