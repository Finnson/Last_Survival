using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_ControllerManager))]
public class VRInput : MonoBehaviour {

	//left/rigth controller
	[HideInInspector]
	public GameObject left,right;
	[HideInInspector]
	public SteamVR_TrackedObject leftObject,rightObject;
	[HideInInspector]
	public SteamVR_Controller.Device leftDevice,rightDevice;

	//custom class
	[System.Serializable]
	public class ButtonInfo
	{
		public bool applicatonMenu;
		public bool touchpad;
		public bool system;
		public bool trigger;
		public bool grip;
	}

	//Active info
	[Header("Active")]
	public bool leftControllerActive;
	public bool rightControllerActive;

	//transform info
	[Header("Transform")]
	public Vector3 leftControllerPosition;
	public Quaternion leftControllerRotation;
	public Vector3 rightControllerPosition;
	public Quaternion rightControllerRotation;

	//button info
	[Header("TouchDown")]
	public ButtonInfo leftControllerTouchDown;
	public ButtonInfo rightControllerTouchDown;

	//axis info
	[Header("Axis")]
	public Vector2 leftTouchpadAxis;
	public Vector2 leftTriggerAxis;
	public Vector2 rightTouchpadAxis;
	public Vector2 rightTriggerAxis;

	private bool GetTouchDown(SteamVR_Controller.Device device,ulong buttonMask)
	{
		return device.GetTouchDown (buttonMask);

	}

	private Vector2 GetAxis(SteamVR_Controller.Device device,Valve.VR.EVRButtonId buttonId)
	{
		return device.GetAxis (buttonId);
	}

	void Awake()
	{
		left = GetComponent<SteamVR_ControllerManager> ().left;
		right = GetComponent<SteamVR_ControllerManager> ().right;
		if (left != null) {
			leftObject = left.GetComponent<SteamVR_TrackedObject> ();
		}
		if (right != null) {
			rightObject = right.GetComponent <SteamVR_TrackedObject> ();
		}
	}

	void Update()
	{
		if (left != null) {
			leftControllerPosition = left.transform.position;
			//leftControllerPosition = new Vector3(-0.35f,0.94f,0.22f);
			leftControllerRotation = left.transform.rotation;
		}
		if (right != null) {
			rightControllerPosition = right.transform.position;
			//rightControllerPosition = new Vector3(-0.29f,1.01f,0.26f);
			rightControllerRotation = right.transform.rotation;
		}

		if (leftObject != null) {
			leftControllerActive = (leftObject.index != SteamVR_TrackedObject.EIndex.None);
			leftDevice = (leftControllerActive) ? SteamVR_Controller.Input ((int)leftObject.index) : null;
		} else {
			leftControllerActive = false;
			leftDevice = null;
		}
		if (rightObject != null) {
			rightControllerActive = (rightObject.index != SteamVR_TrackedObject.EIndex.None);
			rightDevice = (rightControllerActive) ? SteamVR_Controller.Input ((int)rightObject.index) : null;
		} else {
			rightControllerActive = false;
			rightDevice = null;
		}

		if (leftDevice != null) {
			leftControllerTouchDown.applicatonMenu = GetTouchDown (leftDevice, SteamVR_Controller.ButtonMask.ApplicationMenu);
			leftControllerTouchDown.touchpad = GetTouchDown (leftDevice, SteamVR_Controller.ButtonMask.Touchpad);
			leftControllerTouchDown.system = GetTouchDown (leftDevice, SteamVR_Controller.ButtonMask.System);
			leftControllerTouchDown.trigger = GetTouchDown (leftDevice, SteamVR_Controller.ButtonMask.Trigger);
			leftControllerTouchDown.grip = GetTouchDown (leftDevice, SteamVR_Controller.ButtonMask.Grip);

			leftTouchpadAxis = GetAxis (leftDevice, Valve.VR.EVRButtonId.k_EButton_Axis0);
			leftTriggerAxis = GetAxis (leftDevice, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
		} else {
			leftControllerTouchDown.applicatonMenu = false;
			leftControllerTouchDown.touchpad = false;
			leftControllerTouchDown.system = false;
			leftControllerTouchDown.trigger = false;
			leftControllerTouchDown.grip = false;

			leftTouchpadAxis = new Vector2 (0, 0);
			leftTriggerAxis = new Vector2 (0, 0);
		}

		if (rightDevice != null) {
			rightControllerTouchDown.applicatonMenu = GetTouchDown (rightDevice, SteamVR_Controller.ButtonMask.ApplicationMenu);
			rightControllerTouchDown.touchpad = GetTouchDown (rightDevice, SteamVR_Controller.ButtonMask.Touchpad);
			rightControllerTouchDown.system = GetTouchDown (rightDevice, SteamVR_Controller.ButtonMask.System);
			rightControllerTouchDown.trigger = GetTouchDown (rightDevice, SteamVR_Controller.ButtonMask.Trigger);
			rightControllerTouchDown.grip = GetTouchDown (rightDevice, SteamVR_Controller.ButtonMask.Grip);

			rightTouchpadAxis = GetAxis (rightDevice, Valve.VR.EVRButtonId.k_EButton_Axis0);
			rightTriggerAxis = GetAxis (rightDevice, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
		} else {
			rightControllerTouchDown.applicatonMenu = false;
			rightControllerTouchDown.touchpad = false;
			rightControllerTouchDown.system = false;
			rightControllerTouchDown.trigger = false;
			rightControllerTouchDown.grip = false;

			rightTouchpadAxis = new Vector2 (0, 0);
			rightTriggerAxis = new Vector2 (0, 0);
		}
	}
}

