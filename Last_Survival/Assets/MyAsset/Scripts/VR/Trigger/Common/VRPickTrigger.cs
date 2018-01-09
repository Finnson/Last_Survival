using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPickTrigger : MonoBehaviour {

	public VRInput vrinput;
	public VRPackageController packageController;
	public VRAudioController audioController;
	public VRGameController gameController;
    public VRMovieController movieController;

	public VRMouseController mouseController;

	private bool isWatch = false;
	private bool isWatchEnd = false;

	private bool isMoneyOrder = false;
	private bool isMoneyOrderEnd = false;

	private bool isLetter = false;
	private bool isLetterEnd = false;

	private bool isDiary = false;
	private bool isDiaryEnd = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isWatch && !isWatchEnd) {
			//we need play video here
			if (!movieController.IsMoviePlayed("Get_Watch")) {
				movieController.PlayMovie ("Get_Watch");
			}

			//after we played video
			if (movieController.IsPlayEnd ()) {
				gameController.hasWatch_1 = true;

				audioController.PlayAudio ("Pickup");
				packageController.Pick (transform.gameObject);

				isWatchEnd = true;
			}
		}

		if (isMoneyOrder && !isMoneyOrderEnd) {
			//we need play video here
			if (!movieController.IsMoviePlayed("Get_MoneyOrder")) {
				movieController.PlayMovie ("Get_MoneyOrder");
			}

			//after we played video
			if (movieController.IsPlayEnd ()) {
				gameController.hasMoneyOrder_3 = true;

				audioController.PlayAudio ("Pickup");
				packageController.Pick (transform.gameObject);

				isMoneyOrderEnd = true;
			}
		}

		if (isLetter && !isLetterEnd) {
			//we need play video here
			if (!movieController.IsMoviePlayed("Get_Letter")) {
				movieController.PlayMovie ("Get_Letter");
			}

			//after we played video
			if (movieController.IsPlayEnd ()) {
				gameController.hasLetter_4 = true;

				audioController.PlayAudio ("Pickup");
				packageController.Pick (transform.gameObject);

				isLetterEnd = true;
			}
		}

		if (isDiary && !isDiaryEnd) {
			//we need play video here
			if (!movieController.IsMoviePlayed("Get_Diary")) {
				movieController.PlayMovie ("Get_Diary");
			}

			//after we played video
			if (movieController.IsPlayEnd ()) {
				audioController.PlayAudio ("Pickup");
				packageController.Pick (transform.gameObject);

				isDiaryEnd = true;
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (packageController.curItem == "Hand" && packageController.prevItem == "Hand" && vrinput.rightTriggerAxis.x > 0.5f
                && movieController.IsPlayEnd()) {
			if (transform.gameObject.name == "Watch_1") {
				if (gameController.isDrawerOpen_1) {
					isWatch = true;
				}
				return;
			} else if (transform.gameObject.name == "VoiceRecorder_1") {
				if (!gameController.isDrawerOpen_1) {
					return;
				}

				gameController.hasVoiceRecorder_1 = true;
			} else if (transform.gameObject.name == "MoneyOrder_3") {
				isMoneyOrder = true;
				return;
			} else if (transform.gameObject.name == "Letter_4") {
				isLetter = true;
				return;
			} else if (transform.gameObject.name == "Acid_2") {
				//a mouse wiil run out forwarding player
				mouseController.SetTrigger ();
			} else if (transform.gameObject.name == "Diary_4") {
				isDiary = true;
				return;
			}
			audioController.PlayAudio ("Pickup");
			packageController.Pick (transform.gameObject);
		}
	}
}
