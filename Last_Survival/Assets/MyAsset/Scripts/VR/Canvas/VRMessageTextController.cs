using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRMessageTextController : MonoBehaviour {

	public Text MessageText;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void SetMessage(string message,int seconds){
		MessageText.text = message;
		Invoke ("ClearMessage", seconds);
	}

	private void ClearMessage()
	{
		MessageText.text = "";
	}
}
