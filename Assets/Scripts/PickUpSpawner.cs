using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
	public bool generateRandomPos;
	public float pickUpLifetime;

	public float minDistanceBetweenPickUpAndPlayer = 2f;
	public Vector2 xMinMax;
	public Vector2 zMinMax;

	public float timeBetweenSpawns;
	public GameObject[] pickUps;
	public float firstSpawnTime;

	float fixedYPos = -0.9f;
	CameraController cam;
	bool isGood = false;

	// Use this for initialization
	void Start ()
	{
		cam = GameObject.FindObjectOfType<CameraController>();
		InvokeRepeating("spawnPickUp",firstSpawnTime,timeBetweenSpawns);
	}
	
	void spawnPickUp ( )
	{
		Vector3 randomPos = new Vector3 ( ) ;
		int randomPickUp = Random.Range ( 0 , pickUps.Length ) ;

		if ( generateRandomPos )
		{
			isGood = false;
			Vector3 tempRandomPos = returnRandomPos ( randomPos ) ;
			while ( !isGood )
			{
				float distanceBetweenPlayerOne = Vector3.Distance ( tempRandomPos , cam.m_Targets [ 0 ].position );
				float distanceBetweenPlayerTwo = Vector3.Distance ( tempRandomPos , cam.m_Targets [ 1 ].position);
				if ( distanceBetweenPlayerOne < minDistanceBetweenPickUpAndPlayer || distanceBetweenPlayerTwo < minDistanceBetweenPickUpAndPlayer )
				{
					isGood = false ;
					tempRandomPos = returnRandomPos ( randomPos ) ;
				}
				else
				{
					isGood = true ;
					randomPos = tempRandomPos ;
				}
			}
		}
		else
		{
			Vector3 camPos = cam.m_DesiredPosition ;
			randomPos = new Vector3 ( camPos.x , fixedYPos , camPos.z ) ;
		}

		GameObject pickUp = Instantiate(pickUps[randomPickUp],randomPos,Quaternion.identity);
		Destroy(pickUp, pickUpLifetime);
	}

	Vector3 returnRandomPos(Vector3 randomPos)
	{
		float randomXcoordinate = Random.Range ( xMinMax.x , xMinMax.y ) ;
		float randomZcoordinate = Random.Range ( zMinMax.x , zMinMax.y ) ;
		randomPos = new Vector3 ( randomXcoordinate , fixedYPos , randomZcoordinate ) ;
		return randomPos;
	}
}
