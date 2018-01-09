using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour {
	public GameObject Smoke;
	ParticleSystem Smk;

    bool burn = false;
    bool burnFin = true;
    float amount = 0.0f;

	// Use this for initialization
	void Start () {
		Smk = Smoke.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if ((amount < 0.0001f) || (amount >= 1.0f)) {
				burn = true;
				amount = 0.0f;

				Smk.Play();
			}
		}

		if (burn) {
			amount += Time.deltaTime / 3;
			gameObject.GetComponent<Renderer> ().material.SetFloat ("_Amount", amount);

			if (amount > .9) {
				burn = false;

				Smk.Stop();
			}
		}
	}
}
