using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject blackHole;
	public GameObject chargeHitVFX;
	public GameObject hitVFX;
	public LayerMask collideWith;
	public float speed = 30f;
	public float throwbackAmount = 15f;
	public int damage = 20;

	float moveAmountInNextFrame;
	//float skinWidth = 0.01f;
	Collider[] colliders ;
	Vector3 initialPos;
	public bool spawnMagnet { get; set;}
	PlayerMovement ourPlayer;

	// Use this for initialization
	void Awake ( )
	{
		initialPos = transform.position;
		colliders = Physics.OverlapSphere ( transform.position , 0.1f , collideWith, QueryTriggerInteraction.Ignore ) ;
		if ( colliders.Length > 1 )
		{
			OnHitObject(colliders);
		}
		//Debug.Log(colliders.Length);
		Destroy(gameObject,3f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		moveAmountInNextFrame = speed * Time.fixedDeltaTime;
		transform.Translate(Vector3.forward * moveAmountInNextFrame);
		CheckCollision(moveAmountInNextFrame);
	}

	void CheckCollision ( float moveAmount )
	{
		Ray ray = new Ray ( transform.position - transform.forward * transform.lossyScale.z / 2f , transform.forward ) ; //middle Ray;
		Debug.DrawRay ( transform.position - transform.forward * transform.lossyScale.z / 2f , transform.forward ) ;

		Ray rightRay = new Ray ( ( transform.position - transform.forward * transform.lossyScale.z / 2f ) + transform.right * transform.lossyScale.x / 2f , transform.forward ) ; //right Ray;
		Debug.DrawRay ( ( transform.position - transform.forward * transform.lossyScale.z / 2f ) + transform.right * transform.lossyScale.x / 2f , transform.forward ) ;

		Ray leftRay = new Ray ( ( transform.position - transform.forward * transform.lossyScale.z / 2f ) - transform.right * transform.lossyScale.x / 2f , transform.forward ) ; //left Ray;
		Debug.DrawRay ( ( transform.position - transform.forward * transform.lossyScale.z / 2f ) - transform.right * transform.lossyScale.x / 2f , transform.forward ) ;

		RaycastHit hit ;

		if ( Physics.Raycast ( ray , out hit , moveAmount , collideWith , QueryTriggerInteraction.Ignore ) )
		{
			if ( hit.collider.gameObject.tag == "Player" )
			{
				if ( colliders.Length == 0 )
				{
					OnHitObject ( hit ) ;
					return ;
				}

				if ( hit.collider != colliders [ 0 ] )
				{
					OnHitObject ( hit ) ;
					return ;
				}
			}
			else if(hit.collider.gameObject.tag == "Obstacle")
			{
				OnHitObstacle(hit);
				return;
			}
		}
		else if ( Physics.Raycast ( rightRay , out hit , moveAmount , collideWith , QueryTriggerInteraction.Ignore ) )
		{
			if ( hit.collider.gameObject.tag == "Player" )
			{
				if ( colliders.Length == 0 )
				{
					OnHitObject ( hit ) ;
					return ;
				}

				if ( hit.collider != colliders [ 0 ] )
				{
					OnHitObject ( hit ) ;
					return ;
				}
			}
			else if(hit.collider.gameObject.tag == "Obstacle")
			{
				OnHitObstacle(hit);
				return;
			}
		}
		else if(Physics.Raycast(leftRay,out hit,moveAmount, collideWith,QueryTriggerInteraction.Ignore))
		{
			if ( hit.collider.gameObject.tag == "Player" )
			{
				if ( colliders.Length == 0 )
				{
					OnHitObject ( hit ) ;
					return ;
				}

				if ( hit.collider != colliders [ 0 ] )
				{
					OnHitObject ( hit ) ;
					return ;
				}
			}
			else if(hit.collider.gameObject.tag == "Obstacle")
			{
				OnHitObstacle(hit);
				return;
			}
		}
	}

	void OnHitObject ( RaycastHit hitObject )
	{
		Vector3 rotDir = ( hitObject.point - hitObject.collider.gameObject.transform.position ).normalized ;
		Quaternion rotation = Quaternion.LookRotation ( rotDir ) ;
		if ( transform.localScale == Vector3.one )
		{
			GameObject vfx = ( GameObject ) Instantiate ( chargeHitVFX , hitObject.point , rotation ) ;
			Destroy ( vfx , chargeHitVFX.GetComponent<ParticleSystem>().main.duration ) ;
		}
		else
		{
			GameObject vfx = ( GameObject ) Instantiate ( hitVFX , hitObject.point , rotation ) ;
			Destroy ( vfx , 2f ) ;
		}

		Vector3 dir = (transform.position - initialPos).normalized;
		PlayerMovement player = hitObject.collider.GetComponent<PlayerMovement>();
		player.TakeHit(throwbackAmount,dir);
		player.TakeDamage(damage);
		Destroy(gameObject);
	}

	void OnHitObject(Collider[] hitObject)
	{
		Debug.Log("collider");
		//Vector3 dir = (transform.position - initialPos).normalized;
		Vector3 dir = (hitObject[1].gameObject.transform.position - hitObject[0].gameObject.transform.position).normalized;
		PlayerMovement player = hitObject[1].GetComponent<PlayerMovement>();
		player.TakeHit(throwbackAmount,dir);
		player.TakeDamage(damage);
		Destroy(gameObject);
	}

	void OnHitObstacle ( RaycastHit hit )
	{
		Destroy ( gameObject ) ;

		if ( !spawnMagnet )
		{
			GameObject bulletVFX = ( GameObject ) Instantiate ( hitVFX , hit.point , hit.transform.rotation ) ;
			Destroy ( bulletVFX , 2f ) ;
			return ;
		}
		
		GameObject vfx = ( GameObject ) Instantiate ( blackHole , hit.point , hit.collider.gameObject.transform.rotation ) ;
		Destroy ( vfx , vfx.GetComponent<Magnet>().magnetTime ) ;

		if ( ourPlayer != null )
		{
			ourPlayer.disableMagnet = false ;
		}
		spawnMagnet = false;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
	}

	public void SetThrowbackAmount(float amount)
	{
		throwbackAmount = amount;
	}

	public void SetDamageAmount(int amount)
	{
		damage = amount;
	}

	public void GetPlayerWeShotFrom(PlayerMovement player)
	{
		ourPlayer = player;
	}
}
