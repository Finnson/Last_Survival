using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCheck : MonoBehaviour {

	public CrossController EyeFocus;//change the focus point to crosshair
	public HintTextController HintText;//display hint text
	public PackageController packageController;//pick something
	public GameController gameController;//game controller
	public AudioController audioController;

	public float maxRaycastLength = 1000.0f;
	public float validDistance = 10.0f;

	public string hitName;//for debug, name of hit object

	private int layerMask;

	// Use this for initialization
	void Start () {
		if (EyeFocus == null || HintText == null || packageController == null || gameController == null
			|| audioController == null) {
			Debug.LogError ("FocusCheck params are null!");
		}
		layerMask = LayerMask.GetMask("HitLayer");
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;

		hitName = "";
		EyeFocus.setPickChecking (false);
		EyeFocus.setShootChecking (false);
		HintText.setHintContent ("");

		if (Physics.Raycast (transform.position, transform.forward, out hit,maxRaycastLength,layerMask)) {
			if (hit.distance <= validDistance) {
				//hit
				if (hit.transform.gameObject.tag == "PickCheckable") {
					//set crosshair
					hitName = hit.transform.gameObject.name;
					EyeFocus.setPickChecking (true);
					EyeFocus.setShootChecking (false);
					//set hint text
					setHintContentForPick (hitName);
					if (Input.GetKeyDown (KeyCode.C)) {
						if (hit.transform.gameObject.name == "Watch_1") {
							gameController.hasWatch_1 = true;//we need play video here
						} else if (hit.transform.gameObject.name == "VoiceRecorder_1") {
							gameController.hasVoiceRecorder_1 = true;//we need play video here
						} else if (hit.transform.gameObject.name == "MoneyOrder_3") {
							gameController.hasMoneyOrder_3 = true;//we need play video here
						} else if (hit.transform.gameObject.name == "Letter_4") {
							gameController.hasLetter_4 = true;//we need play video here
						}
						audioController.PlayAudio ("Pickup");
						packageController.Pick (hit.transform.gameObject);
					}
					return;
				}
				//shoot
				else if (hit.transform.gameObject.tag == "MaskCheckable") {
					if (hit.distance <= validDistance) {
						//set crosshair
						hitName = hit.transform.gameObject.name;
						EyeFocus.setPickChecking (false);
						EyeFocus.setShootChecking (false);
						return;
					}
				}
				//interact
				else if (hit.transform.gameObject.tag == "InteractCheckable") {
					//set crosshair
					hitName = hit.transform.gameObject.name;
					EyeFocus.setPickChecking (true);
					EyeFocus.setShootChecking (false);
					//set hint text
					setHintContentForInteract (hitName);
					return;
				}
			}
		}
	}

	public void setHintContentForPick(string objName){//Pick thing
		//room 1
		if (objName == "Key_1") {
			HintText.setHintContent ("This is a key.\nPress c to pick it.");
		} else if (objName == "Watch_1") {
			HintText.setHintContent ("This is a watch.\nMaybe I have seen it berfore.");
		} else if (objName == "VoiceRecorder_1") {
			HintText.setHintContent ("This is a voice recorder.\nPress c to pick it.");
		}

		//room 2
		else if (objName == "Acid_2") {
			HintText.setHintContent ("This is an acid.\nPress c to pick it.");
		} else if (objName == "CabinetKey_2") {
			HintText.setHintContent ("This is a cabinet key.\nPress c to pick it.");
		} else if (objName == "Medicine_2") {
			HintText.setHintContent ("This is a medicine.\nPress c to pick it.");
		}

		//room 3
		else if (objName == "IronBar_3") {
			HintText.setHintContent ("This is an iron bar.\nPress c to pick it.");
		} else if (objName == "DoorCard_3") {
			HintText.setHintContent ("This is a door card.\nPress c to pick it.");
		} else if (objName == "MoneyOrder_3") {
			HintText.setHintContent ("This is a money order.\nPress c to pick it.");
		}

		//room 4
		else if (objName == "Letter_4") {
			HintText.setHintContent ("This is a letter\nPress c to pick it.");
		} else if (objName == "Diary_4") {
			HintText.setHintContent ("This is a diary\nPress c to pick it.");
		}

		//hall
		else if (objName == "Gun") {
			HintText.setHintContent ("This is a gun.\nPress c to pick it.");
		}
		else {
			HintText.setHintContent ("");
		}
	}
		
	public void setHintContentForInteract(string objName){//interact with thing to trigger something
		//room 1
		if (objName == "Door_1") {
			if (gameController.isDoorOpen_1) {//Door_1 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_1 is close
				if (packageController.Search ("Key_1")) {//have Key_1
					if (packageController.curItem == "Key_1" && packageController.prevItem == "Key_1") {//carry Key_1
						HintText.setHintContent ("This is a locked door.\nYou can press c to open it.");
						gameController.Interact (objName);
					} else {//have but don't carry Key_1
						HintText.setHintContent ("This is a locked door.\nPlease carry the key you have.");
					}
				} else {//don't have Key_1
					HintText.setHintContent ("This is a locked door.\nMaybe you should find a key first.");
				}
			}
		} else if (objName == "Shield_1") {
			if (gameController.isShieldDown_1) {//Shield_1 is down
				HintText.setHintContent ("You have triggered this shield.\nNothing can do now.");
			} else {//Shield_1 isn't down
				HintText.setHintContent ("This is a shield.\nYou can press c to trigger it.");
				gameController.Interact (objName);
			}
		} else if (objName == "Drawer_1") {
			if (gameController.isDrawerOpen_1) {
				HintText.setHintContent ("You have opened this drawer.\nNothing can do now.");
			} else {
				HintText.setHintContent ("This is a drawer.\nYou can press c to open it.");
				gameController.Interact (objName);
			}
		}

		//room 2
		else if (objName == "Door_2") {
			if (gameController.isDoorOpen_2) {//Door_2 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_2 is close
				if (packageController.Search ("Acid_2")) {//have Acid_2
					if (packageController.curItem == "Acid_2" && packageController.prevItem == "Acid_2") {//Carry Acid_2
						HintText.setHintContent ("This is a rusty door.\nYou can press c to open it.");
						gameController.Interact (objName);
					} else {//have but don't carry Acid_2
						HintText.setHintContent ("This is a rusty door.\nPlease carry the acid you have.");
					}
				} else {//don't have Acid_2
					HintText.setHintContent ("This is a rusty door.\nMaybe you should find an acid first.");
				}
			}
		} 

		//room 3
		else if (objName == "Door_3") {
			if (gameController.isDoorOpen_3) {//Door_3 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_3 is close
				if (packageController.Search ("IronBar_3")) {//have IronBar_3
					//Carry IronBar_3
					if (packageController.curItem == "IronBar_3" && packageController.prevItem == "IronBar_3") {
						HintText.setHintContent ("This is a door that can be broken.\nYou can press c to open it.");
						gameController.Interact (objName);
					} else {//have but don't carry IronBar_3
						HintText.setHintContent ("This is a door that can be broken.\nPlease carry the ironbar you have.");
					}
				} else {//don't have IronBar_3
					HintText.setHintContent ("This is a door that can be broken.\nMaybe you should find an ironbar first.");
				}
			}
		} else if (objName == "Cabinet_3") {
			if (gameController.isCabinetOpen_3) {//Cabinet_3 is open
				HintText.setHintContent ("The cabinet has been opened.");
			} else {//Cabinet_3 is close
				if (packageController.Search ("CabinetKey_2")) {//have CabinetKey_2
					//Carry CabinetKey_2
					if (packageController.curItem == "CabinetKey_2" && packageController.prevItem == "CabinetKey_2") {
						HintText.setHintContent ("This is a locked cabinet.\nYou can press c to open it.");
						gameController.Interact (objName);
					} else {//have but don't carry CabinetKey_2
						HintText.setHintContent ("This is a locked cabinet.\nPlease carry the cabinet key you have.");
					}
				} else {//don't have CabinetKey_2
					HintText.setHintContent ("This is a locked cabinet.\nMaybe you should find a cabinet key first.");
				}
			}
		}

		//room4
		else if (objName == "Door_4") {
			if (gameController.isDoorOpen_4) {//Door_4 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_4 is close
				if (packageController.Search ("DoorCard_3")) {//have DoorCard_3
					//Carry DoorCard_3
					if (packageController.curItem == "DoorCard_3" && packageController.prevItem == "DoorCard_3") {
						HintText.setHintContent ("This is a door that can be opened by a card.\nYou can press c to open it.");
						gameController.Interact (objName);
					} else {//have but don't carry DoorCard_3
						HintText.setHintContent ("This is a door that can be opened by a card.\nPlease carry the door card you have.");
					}
				} else {//don't have DoorCard_3
					HintText.setHintContent ("This is a door that can be opened by a card.\nMaybe you should find a door card first.");
				}
			}
		}

		//hall
		else {
			HintText.setHintContent ("");
		}
	}
}
