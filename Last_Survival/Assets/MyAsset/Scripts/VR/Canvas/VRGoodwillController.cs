using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRGoodwillController : MonoBehaviour {

	/*

	public int Green_Goodwill = 80;
	public int Yellow_Goodwill = 60;
	public int Red_Goodwill = 30;

	public GameObject B_CurrentDegree;
	private RectTransform B_RectTransform;
	private RawImage B_RawImage;

	public GameObject C_CurrentDegree;
	private RectTransform C_RectTransform;
	private RawImage C_RawImage;

	public GameObject D_CurrentDegree;
	private RectTransform D_RectTransform;
	private RawImage D_RawImage;

	public VRGameController gameController;

	// Use this for initialization
	void Start () {
		if (gameController == null || B_CurrentDegree == null || C_CurrentDegree == null || D_CurrentDegree == null) {
			Debug.LogError ("GoodwillController params are null!");
		}

		B_RectTransform = B_CurrentDegree.GetComponent<RectTransform> ();
		B_RawImage = B_CurrentDegree.GetComponent<RawImage> ();

		C_RectTransform = C_CurrentDegree.GetComponent<RectTransform> ();
		C_RawImage = C_CurrentDegree.GetComponent<RawImage> ();

		D_RectTransform = D_CurrentDegree.GetComponent<RectTransform> ();
		D_RawImage = D_CurrentDegree.GetComponent<RawImage> ();
	}

	// Update is called once per frame
	void Update () {
		B_RectTransform.sizeDelta = new Vector2((float)gameController.B_Goodwill, B_RectTransform.rect.height);
		if (gameController.B_Goodwill >= Green_Goodwill)
			B_RawImage.color = Color.green;
		else if(gameController.B_Goodwill >= Yellow_Goodwill)
			B_RawImage.color = Color.yellow;
		else if(gameController.B_Goodwill >= Red_Goodwill)
			B_RawImage.color = Color.red;
		else
			B_RawImage.color = Color.gray;

		C_RectTransform.sizeDelta = new Vector2((float)gameController.C_Goodwill, C_RectTransform.rect.height);
		if (gameController.C_Goodwill >= Green_Goodwill)
			C_RawImage.color = Color.green;
		else if(gameController.C_Goodwill >= Yellow_Goodwill)
			C_RawImage.color = Color.yellow;
		else if(gameController.C_Goodwill >= Red_Goodwill)
			C_RawImage.color = Color.red;
		else
			C_RawImage.color = Color.gray;

		D_RectTransform.sizeDelta = new Vector2((float)gameController.D_Goodwill, D_RectTransform.rect.height);
		if (gameController.D_Goodwill >= Green_Goodwill)
			D_RawImage.color = Color.green;
		else if(gameController.D_Goodwill >= Yellow_Goodwill)
			D_RawImage.color = Color.yellow;
		else if(gameController.D_Goodwill >= Red_Goodwill)
			D_RawImage.color = Color.red;
		else
			D_RawImage.color = Color.gray;
	}

*/
}
