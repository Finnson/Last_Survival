using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBrokenController : MonoBehaviour {
	public GameObject Door;
	public GameObject Glass;
	public GameObject DoorBroken;
	public GameObject GlassBroken;
	public GameObject[] DoorPieces;
	public GameObject[] GlassPieces;

	public VRGameController gameController;

	bool isBreak = false;
	float time = 0.0f;

	public bool isEnd = false;

	// Use this for initialization
	void Start () {
	}

	public void setBreak() {
		isBreak = true;

		Door.SetActive (false);
		Glass.SetActive (false);
		DoorBroken.SetActive (true);
		GlassBroken.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isEnd) {
			if (isBreak) {
				time += Time.deltaTime;

				if (time > 1f) {
					for (int i = 0; i < DoorPieces.Length; i++)
						DoorPieces [i].GetComponent<MeshCollider> ().isTrigger = true;
					for (int i = 0; i < GlassPieces.Length; i++)
						GlassPieces [i].GetComponent<MeshCollider> ().isTrigger = true;
				
					gameController.SetCSave ();
					GetComponent<BoxCollider> ().enabled = false;
				}
				if (time > 2f) {
					for (int i = 0; i < DoorPieces.Length; i++)
						Destroy (DoorPieces [i]);
					for (int i = 0; i < GlassPieces.Length; i++)
						Destroy (GlassPieces [i]);
					isEnd = true;
				}
			}
		}
	}
}
