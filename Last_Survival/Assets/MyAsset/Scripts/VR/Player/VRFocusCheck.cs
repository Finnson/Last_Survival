using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFocusCheck : MonoBehaviour {

	public VRCrossController EyeFocus;//change the focus point to crosshair
	public VRHintTextController HintText;//display hint text
	public VRPackageController packageController;//pick something
	public VRGameController gameController;//game controller
	public VRAudioController audioController;
	public VRInput vrinput;

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
				if (hit.transform.gameObject.tag == "ConstantTag") {
					//set crosshair
					hitName = hit.transform.gameObject.name;
					EyeFocus.setPickChecking (true);
					EyeFocus.setShootChecking (false);
					//set hint text
					setHintContentForConstant (hitName);
					/*if (vrinput.leftControllerTouchDown.trigger) {
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
					}*/
					return;
				}
				//shoot
				else if (hit.transform.gameObject.tag == "MaskTag") {
					if (hit.distance <= validDistance) {
						//set crosshair
						hitName = hit.transform.gameObject.name;
						EyeFocus.setPickChecking (false);
						EyeFocus.setShootChecking (false);
						return;
					}
				}
				//interact
				else if (hit.transform.gameObject.tag == "VariableTag") {
					//set crosshair
					hitName = hit.transform.gameObject.name;
					EyeFocus.setPickChecking (true);
					EyeFocus.setShootChecking (false);
					//set hint text
					setHintContentForVariable (hitName);
					return;
				}
			}
		}
	}

	public void setHintContentForConstant(string objName){//Pick thing
		//room 1
		if (objName == "Key_1") {
			HintText.setHintContent ("This is a key.\nMaybe I can use it to open some door.");
		} else if (objName == "Watch_1") {
			HintText.setHintContent ("This is a watch.\nMaybe I have seen it berfore.");
		} else if (objName == "VoiceRecorder_1") {
			HintText.setHintContent ("This is a voice recorder.");
		}

		//room 2
		else if (objName == "Acid_2") {
			HintText.setHintContent ("This is an acid.\nMaybe I can corrode something with it.");
		} else if (objName == "CabinetKey_2") {
			HintText.setHintContent ("This is a cabinet key.\nMaybe I can use it to open some object.");
		} else if (objName == "Medicine_2") {
			HintText.setHintContent ("This is a hypnotic agent.");
		}

		//room 3
		else if (objName == "IronBar_3") {
			HintText.setHintContent ("This is an iron bar.\nMaybe I can use it to break something.");
		} else if (objName == "DoorCard_3") {
			HintText.setHintContent ("This is a door card.");
		} else if (objName == "MoneyOrder_3") {
			HintText.setHintContent ("This is a money order.");
		}

		//room 4
		else if (objName == "Letter_4") {
			HintText.setHintContent ("This is a letter.");
		} else if (objName == "Diary_4") {
			HintText.setHintContent ("This is a diary.");
		}

		//hall
		else if (objName == "Gun") {
			HintText.setHintContent ("This is a gun.");
		} else if (objName == "ClownFace") {
			HintText.setHintContent ("A large clown face.");
		} else if (objName == "Table") {
			HintText.setHintContent ("This is a lousy table.");
		} else if (objName == "ShootLight") {
			HintText.setHintContent ("This is a shoot light.");
		} else if (objName == "Nose") {
			HintText.setHintContent ("A strange nose.\nMaybe I can tap it.");
		}
		else {
			HintText.setHintContent ("");
		}
	}

	public void setHintContentForVariable(string objName){//interact with thing to trigger something
		//room 1
		if (objName == "Door_1") {
			if (gameController.isDoorOpen_1) {//Door_1 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_1 is close
				if (packageController.Search ("Key_1")) {//have Key_1
					if (packageController.curItem == "Key_1" && packageController.prevItem == "Key_1") {//carry Key_1
						HintText.setHintContent ("This is a locked door.\nMaybe I can rotate the key in the lock.");
					} else {//have but don't carry Key_1
						HintText.setHintContent ("This is a locked door.\nMaybe I should carry the key I have.");
					}
				} else {//don't have Key_1
					HintText.setHintContent ("This is a locked door.\nMaybe I should find a key first.");
				}
			}
		} else if (objName == "Shield_1") {
			if (gameController.isShieldDown_1) {//Shield_1 is down
				HintText.setHintContent ("I have triggered this shield.\nNothing can do now.");
			} else {//Shield_1 isn't down
				HintText.setHintContent ("This is a shield.\nMaybe I should find some gadget to trigger it.");
			}
		} else if (objName == "Drawer_1") {
			if (gameController.isDrawerOpen_1) {
				HintText.setHintContent ("I have opened this drawer.\nNothing can do now.");
			} else {
				HintText.setHintContent ("This is a drawer.\nMaybe I can pull it out.");
			}
		} else if (objName == "RotateHandle_1") {
			if (gameController.isShieldDown_1) {
				HintText.setHintContent ("I have rotated this handle.\nNothing can do now.");
			} else {
				if (packageController.curItem == "Hand" && packageController.prevItem == "Hand") {
					HintText.setHintContent ("This is a handle.\nMaybe I can rotate it now.");
				} else {
					HintText.setHintContent ("This is a handle.\nMaybe I should use my hand first.");	
				}
			}
		}

		//room 2
		else if (objName == "Door_2") {
			if (gameController.isDoorOpen_2) {//Door_2 is open
				HintText.setHintContent ("The door has been opened.");
			} else {//Door_2 is close
				if (packageController.Search ("Acid_2")) {//have Acid_2
					if (packageController.curItem == "Acid_2" && packageController.prevItem == "Acid_2") {//Carry Acid_2
						HintText.setHintContent ("This is a rusty door.\nMaybe I can use the acid to corrode the lock.");
					} else {//have but don't carry Acid_2
						HintText.setHintContent ("This is a rusty door.\nMaybe I should carry the acid I have.");
					}
				} else {//don't have Acid_2
					HintText.setHintContent ("This is a rusty door.\nMaybe I should find an acid first.");
				}
			}
		} else if (objName == "Torch_2") {
			if (!gameController.isTrochSet) {
				if (packageController.curItem == "Hand" && packageController.prevItem == "Hand") {
					HintText.setHintContent ("This is a torch.\nMaybe I can move it now.");
				} else {
					HintText.setHintContent ("This is a torch.\nMaybe I should use my hand first.");	
				}
			} else {
				HintText.setHintContent ("The torch is fixed.\nNothing can do now.");
			}
		}

		//room 3
		else if (objName == "Door_3") {
			if (gameController.isDoorOpen_3) {//Door_3 is open
				HintText.setHintContent ("The glass door has been broken.");
			} else {//Door_3 is close
				if (packageController.Search ("IronBar_3")) {//have IronBar_3
					//Carry IronBar_3
					if (packageController.curItem == "IronBar_3" && packageController.prevItem == "IronBar_3") {
						HintText.setHintContent ("This is a glass door that can be broken.\nMaybe I can use the ironbar.");
					} else {//have but don't carry IronBar_3
						HintText.setHintContent ("This is a glass door that can be broken.\nMaybe I should carry the ironbar I have.");
					}
				} else {//don't have IronBar_3
					HintText.setHintContent ("This is a glass door that can be broken.\nMaybe I should find something hard first.");
				}
			}
		} else if (objName == "Cabinet_3") {
			if (gameController.isCabinetOpen_3) {//Cabinet_3 is open
				HintText.setHintContent ("The cabinet has been opened.");
			} else {//Cabinet_3 is close
				if (packageController.Search ("CabinetKey_2")) {//have CabinetKey_2
					//Carry CabinetKey_2
					if (packageController.curItem == "CabinetKey_2" && packageController.prevItem == "CabinetKey_2") {
						HintText.setHintContent ("This is a locked cabinet.\nMaybe I can rotate the cabinet key in the lock.");
					} else {//have but don't carry CabinetKey_2
						HintText.setHintContent ("This is a locked cabinet.\nMaybe I should carry the cabinet key I have.");
					}
				} else {//don't have CabinetKey_2
					HintText.setHintContent ("This is a locked cabinet.\nMaybe I should find a cabinet key first.");
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
						HintText.setHintContent ("This is a door that can be opened by a card.\nMaybe I can swipe the door card.");
					} else {//have but don't carry DoorCard_3
						HintText.setHintContent ("This is a door that can be opened by a card.\nMaybe I should carry the door card I have.");
					}
				} else {//don't have DoorCard_3
					HintText.setHintContent ("This is a door that can be opened by a card.\nMaybe I should find a door card first.");
				}
			}
		}

		//hall
		else if (objName == "LightSwitch") {
			if (!gameController.isLightStart) {
				if (packageController.curItem == "Hand" && packageController.prevItem == "Hand") {
					HintText.setHintContent ("This is a light switch.\nMaybe I can turn it on now.");
				} else {
					HintText.setHintContent ("This is a light switch.\nMaybe I should use my hand first.");	
				}
			} else {
				HintText.setHintContent ("The light switch is ON.\nNothing can do now.");
			}
		} else if (objName == "Board") {
			if (!gameController.isBoardOpen) {
				if (packageController.curItem == "Hand" && packageController.prevItem == "Hand") {
					HintText.setHintContent ("This is a board.\nMaybe I can pull the handle to open it.");
				} else {
					HintText.setHintContent ("This is a board.\nMaybe I should use my hand first.");	
				}
			} else {
				HintText.setHintContent ("The board has been opened.\nNothing can do now.");
			}
		}
		else {
			HintText.setHintContent ("");
		}
	}
}
