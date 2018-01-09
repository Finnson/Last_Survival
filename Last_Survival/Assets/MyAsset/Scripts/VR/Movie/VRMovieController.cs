using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRMovieController : MonoBehaviour {

	[System.Serializable]
	public class MovieInfo{
		public string movieName;
		public MovieTexture movieTex;
		public AudioSource movieAudio;
		public bool hasPlayed = false;
	};

	public RawImage canvas;
	public GameObject environment;
	public Rigidbody character_rigidbody;

	public GameObject leftHand;
	public GameObject rightHand;

	[Header("Movies")]
	public List<MovieInfo> movies;

	[Header("CurMovie")]
	private bool isPlay = false;
	private bool isStart = true;
	private string curMovie = "";

	// Use this for initialization
	void Start () {
		InitStatus ();
	}

	// Update is called once per frame
	void Update () {
		if (isPlay) {
			if (curMovie == "") {
				InitStatus ();
				return;
			}

			int index = -1;
			for (int i = 0; i < movies.Count; i++) {
				if (movies [i].movieName == curMovie) {
					index = i;
					break;
				}
			}

			if (index == -1) {
				Debug.Log ("Can't find movie:" + curMovie);
				InitStatus ();
				return;
			}

			canvas.enabled = true;
			canvas.texture = movies [index].movieTex;
			//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movies[index].movieTex, ScaleMode.StretchToFill);

			if (!movies[index].movieTex.isPlaying) {
				if (isStart) {//start play
					movies [index].movieTex.Play ();
					if(movies [index].movieAudio)movies [index].movieAudio.Play ();
					movies [index].hasPlayed = true;
					environment.SetActive (false);
					//character_rigidbody.useGravity = false;
					isStart = false;

					leftHand.SetActive (false);
					rightHand.SetActive (false);
				} else {//end play
					canvas.texture = null;
					canvas.enabled = false;
					environment.SetActive (true);
					movies [index].movieTex.Stop ();
					if(movies [index].movieAudio)movies [index].movieAudio.Stop ();
					//character_rigidbody.useGravity = true;
					InitStatus();

					leftHand.SetActive (true);
					rightHand.SetActive (true);
				}
			}

		}
	}

	public void PlayMovie(string name)
	{
		isPlay = true;
		curMovie = name;
	}

	public bool IsPlayEnd()
	{
		return !isPlay;
	}

	public bool IsMoviePlayed(string name){
		for (int i = 0; i < movies.Count; i++) {
			if (movies [i].movieName == name) {
				return movies [i].hasPlayed;
			}
		}
		Debug.Log ("Can't find movie:" + name);
		return false;
	}

	void InitStatus(){
		isPlay = false;
		isStart = true;
		curMovie = "";
	}
}
