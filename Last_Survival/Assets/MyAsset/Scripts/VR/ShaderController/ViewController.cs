using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public float speed = 10000;
    public float MouseSpeed = 300;
    public float rotateSpeed = 40.0f;

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");

        //transform.Translate(new Vector3(h * speed, -mouse * MouseSpeed, v * speed) * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(0, - h * rotateSpeed,0) * Time.deltaTime,Space.World);
    }
}
