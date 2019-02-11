using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightDirectionalMovement : MonoBehaviour {

	public float velocity = 5f;
	public float turnSpeed = 8f;

	Vector2 input;
	float angle;

	Quaternion desiredRotation;
	Transform cam;
	
	void Start () {
		cam = Camera.main.transform;
	}
	

	void Update () {
		GetInput();
		CalculateDirection();

		if(Mathf.Abs(input.x) < 1 &&  Mathf.Abs(input.y) < 1)
		{
		 	return;
		}

		Rotate();
		Move();
	}

	void GetInput()
	{
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");
	}

	void CalculateDirection()
	{
		angle = Mathf.Atan2(input.x,input.y) * Mathf.Rad2Deg;
		angle += cam.eulerAngles.y;
	}

	void Rotate()
	{
		desiredRotation = Quaternion.Euler(0,angle,0);
		transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation, turnSpeed * Time.deltaTime);
	}

	void Move()
	{
		transform.position += transform.forward * velocity * Time.deltaTime;
	}
}
