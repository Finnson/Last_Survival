using UnityEngine;
using System.Collections;

public class VolShaft_1 : MonoBehaviour {
    public Transform mainCamera;

	public Light[] shadowLight;

    public Material[] mats;


    private void Start()
    { }

    private void Update()
    {
		for (int i = 0; i < shadowLight.Length; i++)
		{
			Vector4 lightPos;

			if (shadowLight[i].type == LightType.Directional)
	        {
				Vector3 dir = -shadowLight[i].transform.forward;
	            //dir = mainCamera.transform.InverseTransformDirection(dir);
	            lightPos = new Vector4(dir.x, dir.y, -dir.z, 0.0f);
	        }
	        else
	        {
				Vector3 pos = shadowLight[i].transform.position;
	            //pos = mainCamera.transform.InverseTransformPoint(pos);
	            lightPos = new Vector4(pos.x, pos.y, -pos.z, 1.0f);
	        }
	        
			mats[i].SetVector("litPos", lightPos);
		}

        /*
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetVector("litPos", lightPos);
        }
        */
    }
}
