using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPackageController : MonoBehaviour {

	[System.Serializable]
	public class ItemInfo{
		public string name;
		public GameObject item;
		public GameObject itemImg;
		public Material mat;
		public bool isPick;
		public float percent;
	};

	public VRAudioController audioController;

	//current item
	public string curItem = "Hand";
	public string prevItem = "";
	public Text itemText;

	//items
	[Header("Items")]
	public List<ItemInfo> itemList;

	[Header("PackageCanvas")]
	public GameObject packageCanvas;
	private bool isShow = false;

	[Header("Switch")]
	public float switchSpeed = 0.1f;
	public float curTime = 0.0f;

	public VRInput vrinput;

	public int handIndex;

	// Use this for initialization
	void Start () {
		if (packageCanvas == null || itemText == null || vrinput == null) {
			Debug.LogError ("PackageController params are null!");
		}

		for (int i = 0; i < itemList.Count; i++) {//init pick list
			itemList[i].item.SetActive(false);
			itemList [i].itemImg.SetActive (false);
			itemList[i].isPick = false;
		}

		//handIndex = itemList.Count - 1;
		curItem = "Hand";
		prevItem = "";
		itemList [itemList.Count - 1].isPick = true;

		for (int i = 0; i < itemList.Count - 1; i++)
			itemList [i].percent = -3.5f;
	}

	// Update is called once per frame
	void Update () {
		//show package in canvas
		if (vrinput.rightControllerTouchDown.grip) {
			ShowPackage ();
		}

		if (curItem != "") {//carry something
			if (curItem != "VoiceRecorder_1") {
				if (audioController.IsAudioPlaying ("VoiceRecorder")) {
					audioController.StopAudio ("VoiceRecorder");
				}
			}

			if (prevItem == "") {//prevItem is null, curItem is the first Item
				CarryItem (curItem, true);
				prevItem = curItem;
				return;
			}

			//switch items to modify curItem
			if (vrinput.rightTouchpadAxis.y < -0.5f) {
				if (curTime >= switchSpeed) {
					SwitchPrev ();
					curTime = 0.0f;
				} else
					curTime += Time.deltaTime;
			} else {
				if (vrinput.rightTouchpadAxis.y > 0.5f) {
					if (curTime >= switchSpeed) {
						SwitchNext ();
						curTime = 0.0f;
					} else
						curTime += Time.deltaTime;
				}
			}

			if (prevItem != curItem) {
				if (prevItem != "") {
					//CarryAnim (prevItem, false);//uncarry the prevItem
					//wait for the end of uncarry animation
					CarryItem (prevItem, false);
					CarryItem (curItem, true);
					prevItem = curItem;
				}
			} else {
				if (curItem == "Key_1")
					itemText.text = "A normal key.";
				else if (curItem == "Acid_2")
					itemText.text = "An acid can corrupt iron.";
				else if (curItem == "Gun")
					itemText.text = "A hand gun.";
				else if (curItem == "IronBar_3")
					itemText.text = "An iron bar.";
				else if (curItem == "CabinetKey_2")
					itemText.text = "A cabinet key.";
				else if (curItem == "DoorCard_3")
					itemText.text = "A door card.";
				else if (curItem == "Medicine_2")
					itemText.text = "A hypnotic medicine.";
				else if (curItem == "Watch_1")
					itemText.text = "A familiar watch.";
				else if (curItem == "VoiceRecorder_1")
					itemText.text = "A voice recorder.";
				else if (curItem == "MoneyOrder_3")
					itemText.text = "A money recorder.";
				else if (curItem == "Letter_4")
					itemText.text = "A letter from John to Hermione.";
				else if (curItem == "Diary_4")
					itemText.text = "A diary writen by John.";
				else if (curItem == "Hand")
					itemText.text = "My hand.";
				else
					itemText.text = "";
			}
		} else
			itemText.text = "";

		for (int i = 0; i < itemList.Count - 1; i++)
			if (itemList [i].isPick)
			if (itemList [i].percent < 3.5f) {
				itemList [i].percent += Time.deltaTime * 7;
				itemList [i].mat.SetFloat ("_percent", itemList [i].percent);
			}
	}

	public void ShowPackage()//show or hide package in canvas
	{
		if (!isShow) {
			packageCanvas.GetComponent<Animation> ().Play ("PackageDown");
			isShow = true;
		} else {
			packageCanvas.GetComponent<Animation> ().Play ("PackageUp");
			isShow = false;
		}
	}

	public bool SwitchPrev(){//set curItem with the first prev item whose isPick is true 
		int prevIndex = -1;
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i].name == curItem)
				break;
			if (itemList [i].isPick == true && i > prevIndex) {
				prevIndex = i;
			}
		}

		if (prevIndex != -1) {//set curItem if we have a prev item
			curItem = itemList [prevIndex].name;
			return true;
		}
		return false;
	}

	public bool SwitchNext(){//set curItem with the first prev item whose isPick is true 
		int nextIndex = itemList.Count;
		for (int i = itemList.Count-1; i >= 0; i--) {
			if (itemList [i].name == curItem)
				break;
			if (itemList [i].isPick == true && i < nextIndex) {
				nextIndex = i;
			}
		}

		if (nextIndex != itemList.Count) {//set curItem if we have a prev item
			curItem = itemList [nextIndex].name;
			return true;
		}
		return false;
	}

	/*public void UncarryEnd()//will be called after uncarry animation ends
	{
		CarryItem (prevItem, false);
		CarryItem (curItem, true);
		CarryAnim (curItem, true);
		prevItem = curItem;
	}

	public void CarryAnim(string itemName,bool isCarry)//play carry or uncarry anim
	{
		for (int i = 0; i < itemList.Count; i++) {
			if (itemName == itemList[i].name) {
				if (isCarry)
					itemList[i].item.GetComponent<Animation> ().Play ("Carry" + itemName);
				else
					itemList[i].item.GetComponent<Animation> ().Play ("UnCarry" + itemName);
				return;
			}
		}
	}*/

	public void CarryItem(string itemName,bool isCarry)//just set the item active
	{
		for (int i = 0; i < itemList.Count; i++) {
			if (itemName == itemList[i].name) {
				itemList [i].item.SetActive (isCarry);
			}
		}
	}

	public void Pick(GameObject obj)//pick the obj and set the isPick of obj with true
	{
		if (curItem == "")
			curItem = obj.name;		

		//set isPick with true
		//mark we have the item now
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i].name == obj.name) {
				itemList [i].isPick = true;
				itemList [i].itemImg.SetActive (true);

				break;
			}
		}

		Destroy(obj);
	}

	public void UnPick(string objName){//drop the obj
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i].name == objName) {
				itemList [i].isPick = false;
				itemList [i].itemImg.SetActive (false);

				//unpick the obj you have carryed now
				if (curItem == objName) {
					itemList [i].item.SetActive (false);
					curItem = "Hand";
					prevItem = "";
				}
				break;
			}
		}
	}

	public bool Search(string objName){
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i].name == objName && itemList[i].isPick == true) {
				return true;
			}
		}
		//Debug.Log (objName + " cant be found!");
		return false;
	}


}
