using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
	public Mesh mesh;

	public float moveSpeed = 10.0f;
	public float rotateSpeed = 40.0f;
	public float jumpVelocity = 10.0f;

	private Animator animator;

	private float h;
	private float v;

	void Start () {
		animator= GetComponent<Animator> ();
	}

	void Update () {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		MoveAndRotate(h, v);
	}

	void MoveAndRotate(float h, float v)
	{
		if (v > 0)
		{
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			animator.SetInteger("isMove", 1);
		}
		else if (v < 0)
		{
			transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
			animator.SetInteger("isMove", -1);
		}
		else animator.SetInteger("isMove", 0);

		transform.Rotate(Vector3.up * h * rotateSpeed * Time.deltaTime);
	}
}
