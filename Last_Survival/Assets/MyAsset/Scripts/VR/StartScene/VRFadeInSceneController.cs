using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFadeInSceneController : MonoBehaviour {

	public Material FadeIn;

	public VRMovieController movieController;

	private float speed = 0.2f;
	private float curTime = 0.0f;

	public AudioSource backGround;

	public bool isReset = false;

	// Use this for initialization
	void Start () {
		FadeIn.SetColor ("_Color", new Color (0.0f, 0.0f, 0.0f, 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (!movieController.IsMoviePlayed("Start")) {
			//movieController.PlayMovie ("Start");
		}

		if (movieController.IsPlayEnd ()) {

			if (!isReset) {
				isReset = true;
				FadeIn.SetColor ("_Color", new Color (0.0f, 0.0f, 0.0f, 1.0f));
			}

			curTime += Time.deltaTime;

			if (FadeIn.GetColor ("_Color").a <= 0.0f) {
				FadeIn.SetColor ("_Color", new Color (0.0f, 0.0f, 0.0f, 0.0f));
				backGround.enabled = true;
				Destroy (this.gameObject);
			} else {
				FadeIn.SetColor ("_Color", new Color (0.0f, 0.0f, 0.0f, 1.0f - speed * curTime));
			}
		}
	}
}
