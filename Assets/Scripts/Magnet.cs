using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

	public LayerMask myLayerMask;
	public float radius;
	public float magnetPower;
	public float magnetTime = 5f;
	Collider[] col;

	// Use this for initialization
	void Start () {
		col = Physics.OverlapSphere(transform.position, radius, myLayerMask);
	}
	
	// Update is called once per frame
	void Update ( )
	{
		col = Physics.OverlapSphere(transform.position, radius, myLayerMask);
		if ( col != null )
		{
				foreach ( Collider c in col )
				{
					Rigidbody player = c.GetComponent<Rigidbody> ( ) ;
					player.AddForce ( ( transform.position - c.transform.position ) * magnetPower / (Vector3.Distance(transform.position,c.transform.position) / 10f), ForceMode.Force ) ;
				}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position,radius);
	}
}
