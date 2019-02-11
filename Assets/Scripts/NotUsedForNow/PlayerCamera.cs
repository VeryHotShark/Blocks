using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	Vector3 offset;
	public float smoothSpeed;
	Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindObjectOfType<PlayerMovement>().transform;
		offset = target.position - transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 desiredPos = target.position - offset;
		transform.position = Vector3.Lerp(transform.position,desiredPos,smoothSpeed * Time.deltaTime);
	}
}
