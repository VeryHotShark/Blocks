using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
	public Transform playerToFollow;
	
	Vector3 offset;
	Vector3 position;
	Quaternion rotation;
	Quaternion localRot;

	// Use this for initialization
	void Awake ()
	{
		//offset = transform.position - playerToFollow.position;
		offset = new Vector3(0f, transform.position.y,1.5f);
		/*position = transform.localPosition;
		rotation = transform.rotation;
		localRot = transform.localRotation;*/
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		/*transform.localPosition = position;
		transform.rotation = rotation;
		transform.localRotation = localRot;*/
		transform.position = playerToFollow.position + offset;
	}
}
