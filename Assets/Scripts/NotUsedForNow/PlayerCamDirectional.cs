using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamDirectional : MonoBehaviour {

	public  Transform target;
	public Vector3 offsetPos;
	public float moveSpeed = 5f;
	public float turnSpeed = 10f;
	public float smoothSpeed = 0.5f;

	Quaternion targetRotation;
	Vector3 targetPos;
	bool smoothRotating = false;
	
	// Update is called once per frame
	void Update ()
	{
		MoveWithTarget();
		LookAtTarget();

		if(Input.GetKeyDown(KeyCode.Q) && !smoothRotating )
		{
			StartCoroutine(RotateAroundTarget(45f));
		}

		if(Input.GetKeyDown(KeyCode.E) && !smoothRotating)
		{
			StartCoroutine(RotateAroundTarget(-45f));
		}
	}

	void MoveWithTarget()
	{
		targetPos = target.position + offsetPos;
		transform.position = Vector3.Lerp(transform.position,targetPos,moveSpeed * Time.deltaTime);
	}

	void LookAtTarget()
	{
		targetRotation = Quaternion.LookRotation(target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed * Time.deltaTime);
	}

	IEnumerator RotateAroundTarget ( float angle )
	{
		Vector3 velocity = Vector3.zero ;
		Vector3 targetOffsetPos = Quaternion.Euler ( 0 , angle , 0 ) * offsetPos ;
		float distance = Vector3.Distance ( offsetPos , targetOffsetPos ) ;
		smoothRotating = true;
		while ( distance > 0.02f )
		{
			offsetPos = Vector3.SmoothDamp(offsetPos,targetOffsetPos, ref velocity, smoothSpeed);
			distance = Vector3.Distance ( offsetPos , targetOffsetPos ) ;
			yield return null ;
		}

		smoothRotating = false;
		offsetPos = targetOffsetPos;
	}
}
