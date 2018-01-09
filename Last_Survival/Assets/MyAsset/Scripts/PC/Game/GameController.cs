using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public int caseNum = 0;
	public PackageController packageController;
	public MessageTextController messageController;
	public TriggerTextController triggerController;
	public MovieController movieController;
	public AudioController audioController;

	//goodwill degree
	[Header("Goodwill Degree")]
	public int B_Goodwill = 100;
	public int C_Goodwill = 70;
	public int D_Goodwill = 40;

	//save status
	[Header("Save Status")]
	public bool B_save = false;//room 2
	public bool C_save = false;//room 3
	public bool D_save = false;//room 4

	//survive status
	[Header("Survive Status")]
	public bool B_survive = true;
	public bool C_survive = true;
	public bool D_survive = true;

	//Room 1
	[Header("ROOM 1")]
	public bool isShieldDown_1 = false;
	public GameObject shield_1;
	public bool isDoorOpen_1 = false;
	public GameObject door_1;
	public bool isDrawerOpen_1 = false;
	public GameObject drawer_1;

	//Room 2
	[Header("ROOM 2")]
	public bool isDoorOpen_2 = false;
	public GameObject door_2;

	//Room 3
	[Header("ROOM 3")]
	public bool isDoorOpen_3 = false;
	public GameObject door_3;
	public bool isCabinetOpen_3 = false;
	public GameObject cabinet_3;

	//Room4
	[Header("ROOM 4")]
	public bool isDoorOpen_4 = false;
	public GameObject door_4;

	//Triggers
	[Header("Triggers")]
	//happen while opening Door_2
	//private void SaveOrderTrigger();
	public bool B_has_gun = false;
	public bool saveOrderChecking = true;//if we need to check this trigger
	public bool B_save_text = false;

	//happen while opening Door_3
	//private void BKillCTrigger();

	//happen while getting all of infos
	//private void EndingTrigger();
	public bool hasWatch_1 = false;
	public bool hasVoiceRecorder_1 = false;
	public bool hasMoneyOrder_3 = false;
	public bool hasLetter_4 = false;//these will be set after corresponding videos
	public bool endingChecking = true;
	public bool secondEnding = false;//ABCD->B

	// Use this for initialization
	void Start () {
		if (packageController == null || shield_1 == null || door_1 == null || messageController == null || door_2 == null
			|| door_3 == null || triggerController == null || cabinet_3 == null || door_4 == null || movieController == null
			|| audioController == null || drawer_1 == null) {
			Debug.LogError ("GameController params are null!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (saveOrderChecking) {//save order about B and C
			if(C_save)saveOrderChecking=false;
			if (B_save && !C_save && packageController.Search("Gun")) {
				SaveOrderTrigger ();
			}
		}
		if (C_survive && B_has_gun) {//B has gun and C is survive
			if (B_save && C_save) {
				BKillCTrigger ();
			}
		}
		if (endingChecking) {
			if (hasWatch_1 && hasVoiceRecorder_1 && hasMoneyOrder_3 && hasLetter_4) {
				EndingTrigger ();
			}
		}
	}

	public void Interact(string objName)
	{
		//room 1
		if (objName == "Shield_1") {//open shiled to get Key_1
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("ShieldDown");
				shield_1.GetComponent<Animation> ().Play ("Shield_1_Down");
				isShieldDown_1 = true;
			}
		} else if (objName == "Door_1") {//Use Key_1 to open Door_1
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenDoor");
				door_1.GetComponent<Animation> ().Play ("Door_1_Open");
				isDoorOpen_1 = true;
				packageController.UnPick ("Key_1");
				messageController.SetMessage ("Now you can go out to save your friends!", 1);
			}
		} else if (objName == "Drawer_1") {
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenDoor");
				drawer_1.GetComponent<Animation> ().Play ("Drawer_1_Open");
				isDrawerOpen_1 = true;
			}
		}

		//room 2
		else if (objName == "Door_2") {//Use Acid_2 to open Door_2
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenDoor");
				door_2.GetComponent<Animation> ().Play ("Door_2_Open");
				isDoorOpen_2 = true;
				packageController.UnPick ("Acid_2");
				//when Door_2_Open ends, it will call SetBSave()
			}
		}

		//room 3
		else if (objName == "Door_3") {//Use Acid_2 to open Door_2
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenDoor");
				door_3.GetComponent<Animation> ().Play ("Door_3_Open");
				isDoorOpen_3 = true;
				packageController.UnPick ("IronBar_3");
				messageController.SetMessage ("Save C success!", 1);
				C_save = true;
			}
		} else if (objName == "Cabinet_3") {//Use CabinetKey_2 to open Cabinet_3
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenCabinet");
				cabinet_3.GetComponent<Animation> ().Play ("Cabinet_3_Open");
				isCabinetOpen_3 = true;
				packageController.UnPick ("CabinetKey_2");
			}
		}

		//room4
		else if (objName == "Door_4") {//Use DoorCard_3 to open Door_4
			if (Input.GetKeyDown (KeyCode.C)) {
				audioController.PlayAudio ("OpenDoor");
				door_4.GetComponent<Animation> ().Play ("Door_4_Open");
				isDoorOpen_4 = true;
				packageController.UnPick ("DoorCard_3");
				messageController.SetMessage ("Save D success!", 1);
				D_save = true;
			}
		}
	}

	//Triggers
	private void SaveOrderTrigger(){
		if (!movieController.IsMoviePlayed("B_Require_Gun")) {
			movieController.PlayMovie ("B_Require_Gun");
		}

		if (movieController.IsPlayEnd ()) {
			triggerController.SetTriggerText (
				"B:Could you please give your gun to me?\nI feel scared and need something to protect myself.",
				"Sorry, I can't trust you.",
				"OK, here you are."
			);
			if (!B_save_text) {
				messageController.SetMessage ("Save B success!", 1);
				B_save_text = true;
			}
			if (Input.GetKeyDown (KeyCode.N)) {
				triggerController.ClearTriggerText ();

				messageController.SetMessage ("You reject the requirement of B!", 1);
				B_has_gun = false;
				saveOrderChecking = false;
				return;
			} else if (Input.GetKeyDown (KeyCode.M)) {
				triggerController.ClearTriggerText ();

				messageController.SetMessage ("You give your gun to B!", 1);
				B_has_gun = true;
				saveOrderChecking = false;

				packageController.UnPick ("Gun");
				return;
			}
		}
	}

	private void BKillCTrigger(){
		messageController.SetMessage ("Opps!\nB killed C with the gun you give him!", 1);

		C_survive = false;
		C_Goodwill = 0;
		C_save = false;
	}

	private void EndingTrigger(){
		if (!secondEnding) {
			triggerController.SetTriggerText (
				"You have found the evil personality of B.\nWhat will you do now?",
				"Go to chat with B.",
				"Tell others about the fact."
			);
			if (Input.GetKeyDown (KeyCode.N)) {//chat with B by yourself
				if (C_survive) {//ABCD
					secondEnding = true;
				} else {//ABD,A死,B得到了D
					messageController.SetMessage ("You die and B got D! Game Over!", 2);
					triggerController.ClearTriggerText ();
					endingChecking = false;
				}
			} else if (Input.GetKeyDown (KeyCode.M)) {//tell others
				if (C_survive) {//ABCD
					if (packageController.Search ("Diary_4")) {//感化B,B自首,ACD出去
						messageController.SetMessage ("You rehabilitate B and go out with CD!\nB turned himself in at last!\nGame Over!", 2);
					} else {//ACD出去,B关在里面forever
						messageController.SetMessage ("You stun B and go out with CD!\nB is left there forever!\nGame Over!", 2);
					}
					triggerController.ClearTriggerText ();
					endingChecking = false;
				} else {//ABD
					if (packageController.Search ("Diary_4")) {//感化B,B自首,AD出去
						messageController.SetMessage ("You rehabilitate B and go out with D!\nB turned himself in at last!\nGame Over!", 2);
					} else if (packageController.Search ("Medicine_2")) {//催眠B,AD出去,B关在里面forever
						messageController.SetMessage ("You hypnotize B and go out with D!\nB is left there forever!\nGame Over!", 2);
					} else {//A死,B得到了D,同上
						messageController.SetMessage ("You die and B got D! Game Over!", 2);
					}
					triggerController.ClearTriggerText ();
					endingChecking = false;
				}
			}
		} else {//ABCD->B
			string rightText;
			if (packageController.Search ("Diary_4"))
				rightText = "Stun B and leave him alone.";
			else
				rightText = "Rehabilitate him by using his diary.";
			triggerController.SetTriggerText (
				"B is scared because you have a hand gun.\nWhat will you do now?",
				"Join B to get D together.",
				rightText
			);

			if (Input.GetKeyDown (KeyCode.N)) {//join B
				messageController.SetMessage ("You killed C and get D together with B!\nGame Over!", 2);
				triggerController.ClearTriggerText ();
				endingChecking = false;
			} else if (Input.GetKeyDown (KeyCode.M)) {
				if (packageController.Search ("Diary_4")) {//感化B,ABCD一起出去
					messageController.SetMessage ("You rehabilitate B and go out with CD!\nGame Over!", 2);
				} else {//把B关起来,ACD一起出去(CD不知道B去哪里了)
					messageController.SetMessage ("You stun B and go out with CD!\nB is left there forever!\nGame Over!", 2);
				}
				triggerController.ClearTriggerText ();
				endingChecking = false;
			}
		}
	}

	public void SetBSave(){
		B_save = true;
	}

}
