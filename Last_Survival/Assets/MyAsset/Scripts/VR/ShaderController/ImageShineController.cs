using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShineController : MonoBehaviour {
    public bool shine = false;
    float percent = -3.5f;

    public Material ImgShn;

	void Start () {
	}

    void Update()
    {
        if (shine)
        {
            percent += Time.deltaTime * 7;
            if (percent > 3.5f)
            {
                percent = -3.5f;
                shine = false;
            }

            ImgShn.SetFloat("_percent", percent);

        }
        else if (Input.GetKeyDown(KeyCode.Z)) shine = true;
    }
}
