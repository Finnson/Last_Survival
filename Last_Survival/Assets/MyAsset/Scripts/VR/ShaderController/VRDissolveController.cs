using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDissolveController : MonoBehaviour {
	public GameObject Smoke;
	ParticleSystem Smk;

    public bool burn = false;
    float amount = 0.0f;
    public bool end = false;

	// Use this for initialization
	void Start () {
		Smk = Smoke.GetComponent<ParticleSystem> ();
	}

	public void Play(){
		Smk.Play();
	}

	public void Stop(){
		Smk.Stop();
	}

	// Update is called once per frame
	void Update () {
        if (burn)
        {
            amount += Time.deltaTime / 3;
            gameObject.GetComponent<Renderer>().material.SetFloat("_Amount", amount);

            if (amount > 0.8f)
            {
                amount = 0.8f;
                burn = false;
                end = true;
            }
        }
	}
}
