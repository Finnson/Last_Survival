using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossController : MonoBehaviour {

	public GameObject PickCrossHair;
	public GameObject ShootCrossHair;

	private bool isPickChecking = false;
	private bool isShootChecking = false;

	// Use this for initialization
	void Start () {
		if (PickCrossHair == null || ShootCrossHair == null) {
			Debug.LogError ("CrossController params are null!");
		}

		PickCrossHair.SetActive (false);
		ShootCrossHair.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		PickCrossHair.SetActive (isPickChecking);
		ShootCrossHair.SetActive (isShootChecking);
	}

	public void setPickChecking(bool isPickChecking)
	{
		this.isPickChecking = isPickChecking;
	}

	public void setShootChecking(bool isShootChecking)
	{
		this.isShootChecking = isShootChecking;
	}
}
