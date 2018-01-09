using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieController : MonoBehaviour {

	[System.Serializable]
	public class MovieInfo{
		public string movieName;
		public MovieTexture movieTex;
		public bool hasPlayed = false;
	};

	[Header("Movies")]
	public List<MovieInfo> movies;

	[Header("CurMovie")]
	public bool isPlay = false;
	public bool isStart = true;
	public string curMovie = "";

	// Use this for initialization
	void Start () {
		InitStatus ();
	}

	// Update is called once per frame
	void Update () {
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

	void OnGUI()
	{
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

			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movies[index].movieTex, ScaleMode.StretchToFill);

			if (!movies[index].movieTex.isPlaying) {
				if (isStart) {//start play
					movies [index].movieTex.Play ();
					movies [index].hasPlayed = true;
					isStart = false;
				} else {//end play
					InitStatus();
				}
			}

		}
	}
}
