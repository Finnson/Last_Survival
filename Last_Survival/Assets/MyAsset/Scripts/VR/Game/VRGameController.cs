using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRGameController : MonoBehaviour {

	public int caseNum = 0;
	public VRPackageController packageController;
	public VRMessageTextController messageController;
	public VRTriggerTextController triggerController;
	public VRMovieController movieController;
	public VRAudioController audioController;
	public VRInput vrinput;
	public VRNPCController BController;
	public VRNPCController CController;
	public VRNPCController DController;
	public VRBrokenController BrkController;

	//goodwill degree
	//[Header("Goodwill Degree")]
	//public int B_Goodwill = 100;
	//public int C_Goodwill = 70;
	//public int D_Goodwill = 40;

	public bool gameOver = false;

	//save status
	[Header("Save Status")]
	public bool B_save = false;//room 2
	public bool C_save = false;//room 3
	public bool D_save = false;//room 4

	[Header("Move Status")]
	public bool B_Move = false;
	public bool C_Move = false;
	public bool D_Move = false;

	//survive status
	[Header("Survive Status")]
	public bool B_survive = true;
	public bool C_survive = true;
	public bool D_survive = true;

	//Hall
	[Header("Hall")]
	public bool isLightStart = false;
	public bool isBoardOpen = false;

	//Room 1
	[Header("ROOM 1")]
	public bool isShieldDown_1 = false;
	public bool isDoorOpen_1 = false;
	public GameObject door_1;
	public bool isDrawerOpen_1 = false;

	//Room 2
	[Header("ROOM 2")]
	public bool isDoorOpen_2 = false;
	public GameObject door_2;
    public bool isTrochSet = false;

	//Room 3
	[Header("ROOM 3")]
	public bool isDoorOpen_3 = false;
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

	//Trigger speed
	public float triggerSpeed = 0.4f;
	public float curTime = 0.0f;

	public int endingType = 0;

	// Use this for initialization
	void Start () {
		if (packageController == null || door_1 == null || messageController == null || door_2 == null
			|| triggerController == null || cabinet_3 == null || door_4 == null || movieController == null
			|| audioController == null) {
			Debug.LogError ("GameController params are null!");
		}
	}

	// Update is called once per frame
	void Update () {
		if (saveOrderChecking) {//save order about B and C
			if (C_save) {
				saveOrderChecking = false;
			} else if (B_save && !C_save && packageController.Search ("Gun")) {
				SaveOrderTrigger ();
			} else if (B_save && !C_save && !packageController.Search ("Gun")) {
				saveOrderChecking = false;
				BController.NPCMove ();
			}
		}
		if (!saveOrderChecking && B_save && !B_Move) {
			B_Move = true;
			BController.NPCMove ();
		}
		
		if(C_save&& !C_Move){
			C_Move = true;
			CController.NPCMove();
		}
		
		if(D_save&& !D_Move){
			D_Move = true;
			DController.NPCMove();
		}
		
		if (C_survive && B_has_gun) {//B has gun and C is survive
			if (B_save && C_save && CController.isOut) {
				BKillCTrigger ();
			}
		}
		if (endingChecking) {
			if (hasWatch_1 && hasVoiceRecorder_1 && hasMoneyOrder_3 && hasLetter_4) {
				EndingTrigger ();
			}
		}

		if (endingType == 1) {
			if (!movieController.IsMoviePlayed ("A_Die_B_Got_D")) {
				movieController.PlayMovie ("A_Die_B_Got_D");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You die and John got Hermione!\nGame Over!", 3);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 2) {
			if (!movieController.IsMoviePlayed ("B_Surrender_ACD_Survive")) {
				movieController.PlayMovie ("B_Surrender_ACD_Survive");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You rehabilitate John and go out with William and Hermione!\nJohn turned himself in at last!\nGame Over!", 3);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 3) {
			if (!movieController.IsMoviePlayed ("B_Imprison_ACD_Survive")) {
				movieController.PlayMovie ("B_Imprison_ACD_Survive");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You hypnotize John and go out with William and Hermione!\nJohn is left there forever!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 4) {
			print (111);
			if (!movieController.IsMoviePlayed ("B_Surrender_AD_Survive")) {
				movieController.PlayMovie ("B_Surrender_AD_Survive");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You rehabilitate John and go out with Hermione!\nJohn turned himself in at last!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 5) {
			if (!movieController.IsMoviePlayed ("B_Imprison_AD_Survive")) {
				movieController.PlayMovie ("B_Imprison_AD_Survive");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You hypnotize John and go out with Hermione!\nJohn is left there forever!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 6) {
			if (!movieController.IsMoviePlayed ("A_Die_B_Got_D2")) {
				movieController.PlayMovie ("A_Die_B_Got_D2");
			}
			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You die and John got Hermione! Game Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 7) {
			if (!movieController.IsMoviePlayed ("A_Join_B_Got_D")) {
				movieController.PlayMovie ("A_Join_B_Got_D");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You killed William and get Hermione together with John!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 8) {
			if (!movieController.IsMoviePlayed ("B_Surrender_ACD_Survive_Unknow")) {
				movieController.PlayMovie ("B_Surrender_ACD_Survive_Unknow");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You rehabilitate John and go out with William and Hermione!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		} else if (endingType == 9) {
			if (!movieController.IsMoviePlayed("B_Imprison_ACD_Survive_Unknow")) {
				movieController.PlayMovie ("B_Imprison_ACD_Survive_Unknow");
			}

			if (movieController.IsPlayEnd ()) {
				messageController.SetMessage ("You stun John and go out with William and Hermione!\nJohn is left there forever!\nGame Over!", 2);
				Invoke ("SetGameOver", 3);
			}
		}

		if (gameOver) {
			SceneManager.LoadScene ("End");
		}
	}

	public void Interact(string objName)
	{
		//room 1
		if (objName == "Shield_1") {//open shiled to get Key_1
			/*if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("ShieldDown");
				shield_1.GetComponent<Animation> ().Play ("Shield_1_Down");
				isShieldDown_1 = true;
			}*/
		} else if (objName == "Door_1") {//Use Key_1 to open Door_1
			//if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("OpenDoor");
				door_1.GetComponent<Animation> ().Play ("Door_1_Open");
				isDoorOpen_1 = true;
				packageController.UnPick ("Key_1");
				messageController.SetMessage ("Now you can go out to save your friends!", 1);
			//}
		} else if (objName == "Drawer_1") {
			/*if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("OpenDoor");
				drawer_1.GetComponent<Animation> ().Play ("Drawer_1_Open");
				isDrawerOpen_1 = true;
			}*/
		} 

		//room 2
		else if (objName == "Door_2") {//Use Acid_2 to open Door_2
			//if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("OpenDoor");
				door_2.GetComponent<Animation> ().Play ("Door_2_Open");
				isDoorOpen_2 = true;
				packageController.UnPick ("Acid_2");

				messageController.SetMessage ("Save John Smith success!", 3);
				//when Door_2_Open ends, it will call SetBSave()
			//}
		}

		//room 3
		else if (objName == "Door_3") {//Use Acid_2 to open Door_2
			//if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("GlassDoorBreak");
				BrkController.setBreak ();
				isDoorOpen_3 = true;
				packageController.UnPick ("IronBar_3");
				messageController.SetMessage ("Save Williams Duke success!", 3);
				//when Door_3 broke, it will call SetCSave()
			//}
		} else if (objName == "Cabinet_3") {//Use CabinetKey_2 to open Cabinet_3
			//if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("OpenCabinet");
				cabinet_3.GetComponent<Animation> ().Play ("Cabinet_3_Open");//skeleton will drop down at the same time
				isCabinetOpen_3 = true;
				packageController.UnPick ("CabinetKey_2");
			//}
		}

		//room4
		else if (objName == "Door_4") {//Use DoorCard_3 to open Door_4
			//if (vrinput.rightControllerTouchDown.trigger) {
				audioController.PlayAudio ("OpenDoor");
				door_4.GetComponent<Animation> ().Play ("Door_4_Open");
				isDoorOpen_4 = true;
				packageController.UnPick ("DoorCard_3");
				messageController.SetMessage ("Save Hermione Grander success!", 3);
				//when Door_4_Open, it will call SetCSave()
			//}
		}
	}

	//Triggers
	private bool chooseLeft(){
		if (vrinput.rightTouchpadAxis.x < -0.5f) {
			print (vrinput.rightTouchpadAxis.x);
			if (curTime >= triggerSpeed) {
				curTime = 0.0f;
				return true;
			}
		}
		curTime += Time.deltaTime;
		return false;
	}
	private bool chooseRight(){
		if (vrinput.rightTouchpadAxis.x > 0.5f) {
			print (vrinput.rightTouchpadAxis.x);
			if (curTime >= triggerSpeed) {
				curTime = 0.0f;
				return true;
			}
		}
		curTime += Time.deltaTime;
		return false;
	}

	private void SaveOrderTrigger(){
		if (!movieController.IsMoviePlayed("B_Require_Gun")) {
			movieController.PlayMovie ("B_Require_Gun");
		}

		if (movieController.IsPlayEnd ()) {
			BController.NPCTalking();

			triggerController.SetTriggerText (
				"John Smith:Could you please give your gun to me?\nI feel scared and need something to protect myself.",
				"Sorry, I can't trust you.",
				"OK, here you are."
			);

			if (chooseLeft()) {//按下left button
				triggerController.ClearTriggerText ();

				messageController.SetMessage ("You reject the requirement of John!", 1);
				B_has_gun = false;
				saveOrderChecking = false;
				BController.NPCMove ();

				return;
			} else if (chooseRight()) {//按下right button
				triggerController.ClearTriggerText ();

				messageController.SetMessage ("You give your gun to John!", 1);
				B_has_gun = true;
				saveOrderChecking = false;
				BController.NPCMove ();

				packageController.UnPick ("Gun");
				return;
			}
		}
	}

	private void BKillCTrigger(){
		if (!movieController.IsMoviePlayed("B_Kill_C")) {
			movieController.PlayMovie ("B_Kill_C");
		}

		if (movieController.IsPlayEnd ()) {
			messageController.SetMessage ("Opps!\nB killed C with the gun you give him!", 3);

			C_survive = false;
			//C_Goodwill = 0;

			CController.gameObject.SetActive (false);

			C_save = false;
		}
	}

	private void EndingTrigger(){
		if (!secondEnding) {
			triggerController.SetTriggerText (
				"You have found the evil personality of John.\nWhat will you do now?",
				"Go to chat with John.",
				"Tell others about the fact."
			);
			if (chooseLeft()) {//chat with B by yourself
				if (C_survive) {//ABCD
					secondEnding = true;
				} else {//ABD,A死,B得到了D
					triggerController.ClearTriggerText ();
					endingChecking = false;
					endingType = 1;
				}
			} else if (chooseRight()) {//tell others
				if (C_survive) {//ABCD
					if (packageController.Search ("Diary_4")) {//感化B,B自首,ACD出去
						endingType = 2;
					} else {//ACD出去,B关在里面forever
						endingType = 3;
					}
					triggerController.ClearTriggerText ();
					endingChecking = false;
				} else {//ABD
					if (packageController.Search ("Diary_4")) {//感化B,B自首,AD出去
						endingType = 4;
					} else if (packageController.Search ("Medicine_2")) {//催眠B,AD出去,B关在里面forever
						endingType = 5;
					} else {//A死,B得到了D,同上
						endingType = 6;
					}
					triggerController.ClearTriggerText ();
					endingChecking = false;
				}
			}
		} else {//ABCD->B
			if (!movieController.IsMoviePlayed("Chat_With_B")) {
				movieController.PlayMovie ("Chat_With_B");
			}

			if (movieController.IsPlayEnd ()) {
				string rightText;
				if (!packageController.Search ("Diary_4"))
					rightText = "Stun John and leave him alone.";
				else
					rightText = "Rehabilitate him by using his diary.";
				triggerController.SetTriggerText (
					"John is scared because you have a hand gun.\nWhat will you do now?",
					"Join John to get Hermione together.",
					rightText
				);

				if (chooseLeft ()) {//join B
					endingType = 7;
					triggerController.ClearTriggerText ();
					endingChecking = false;
				} else if (chooseRight ()) {
					if (packageController.Search ("Diary_4")) {//感化B,ABCD一起出去
						triggerController.ClearTriggerText ();
						endingChecking = false;
						endingType = 8;
					} else {//把B关起来,ACD一起出去(CD不知道B去哪里了)
						endingType = 9;	
						triggerController.ClearTriggerText ();
						endingChecking = false;
					}
				}
			}
		}
	}

	public void SetBSave(){
		B_save = true;
	}

	public void SetCSave(){
		C_save = true;
	}
	
	public void SetDSave(){
		D_save = true;
	}

	void SetGameOver(){
		gameOver = true;
	}

}
