using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDissolveTrigger : MonoBehaviour {

    public VRInput vrinput;
    public VRPackageController packageController;
    public VRAudioController audioController;
    public VRGameController gameController;
    public VRMovieController movieController;
	public VRDissolveController dissolveController1, dissolveController2;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (dissolveController1.end)
        {
            //open door
            gameController.Interact("Door_2");
            gameController.isDoorOpen_2 = true;

			dissolveController1.Stop ();
            dissolveController1.end = false;
			dissolveController2.Stop ();
			dissolveController2.end = false;
        }
    }

    void OnTriggerEnter(Collider collider)
	{ 
		if (packageController.curItem == "Acid_2" && packageController.prevItem == "Acid_2" && vrinput.rightTriggerAxis.x > 0.5f
		          && movieController.IsPlayEnd ()) {
			//enable shader
			dissolveController1.Play ();
			dissolveController1.burn = true;
			dissolveController2.Play ();
			dissolveController2.burn = true;
		}
	}
}
