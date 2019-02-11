using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayer : MonoBehaviour {

	[Header("Controls")]
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode right = KeyCode.RightArrow;
	public KeyCode up = KeyCode.UpArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode kick = KeyCode.KeypadEnter;
	public KeyCode ChargeThrust = KeyCode.Keypad0;

	[Header("PlayerVariables")]
	public float kickPower = 1f;
	public float chargeLoadSpeed = 2f;
	public Vector2 chargeMinMax = new Vector2 (0f,6f);
	public float coolDown = 3f;
	public float thrust = 2f;
	public float smoothMoveTime = .2f;
	public float moveSpeed = 8f;
	public float turnSpeed = 8f;

	Rigidbody myRigid;
	Vector3 moveVelocity;
	Material myMat;
	Color originalColor;

	float smoothInputMagnitude;
	float smoothInputVelocity;
	float angle;
	float chargeAmount;
	float coolDownTimer;
	float moveHorizontal;
	float moveVertical;

	// Use this for initialization
	void Start () {
		myRigid = GetComponent<Rigidbody>();
		myMat = GetComponent<Renderer>().material;
		originalColor = myMat.color;
	}
	
	// Update is called once per frame
	void Update ( )
	{
		Movement();
		ThrustAndCharge();
	}

	void Movement()
	{
		/*if(Input.GetKey(left))
		{
			moveHorizontal = -1f;
		}

		if(Input.GetKey(right))
		{
			moveHorizontal = 1f;
		}

		if(Input.GetKeyUp(left))
		{
			moveHorizontal = 0f;
		}

		if(Input.GetKeyUp(right))
		{
			moveHorizontal = 0f;
		}

		if(Input.GetKey(down))
		{
			moveVertical = -1f;
		}

		if(Input.GetKey(up))
		{
			moveVertical = 1f;
		}

		if(Input.GetKeyUp(down))
		{
			moveVertical = 0f;
		}

		if(Input.GetKeyUp(up))
		{
			moveVertical = 0f;
		}

		if(Input.GetKey(left) && Input.GetKey(right))
		{
			moveHorizontal = 0f;
		}

		if(Input.GetKey(up) && Input.GetKey(down))
		{
			moveVertical = 0f;
		}*/

		float moveHor = Input.GetAxisRaw("HorizontalSecond");
		float moveVer = Input.GetAxisRaw("VerticalSecond");

		Vector3 moveDir = new Vector3 ( moveHor , 0f , moveVer ).normalized ;

		float movementMagnitude = moveDir.magnitude ;
		smoothInputMagnitude = Mathf.SmoothDamp ( smoothInputMagnitude , movementMagnitude , ref smoothInputVelocity , smoothMoveTime ) ;
		//moveVelocity = moveDir * moveSpeed * smoothInputMagnitude;

		float targetAngle = Mathf.Atan2 ( moveDir.x , moveDir.z ) * Mathf.Rad2Deg ;
		angle = Mathf.LerpAngle ( angle , targetAngle , Time.deltaTime * turnSpeed * movementMagnitude ) ;

		moveVelocity = transform.forward * moveSpeed * smoothInputMagnitude ;
	}

	void FixedUpdate ()
	{
		myRigid.MoveRotation(Quaternion.Euler(Vector3.up * angle));
		myRigid.MovePosition(myRigid.position + moveVelocity * Time.fixedDeltaTime);
	}

	/*void Thrust()
	{
		coolDownTimer -= Time.deltaTime;

		if ( Input.GetKeyDown ( KeyCode.Space ) )
		{
			if ( coolDownTimer <= 0f )
			{
				myRigid.AddForce ( moveVelocity * thrust , ForceMode.Impulse );
				coolDownTimer = coolDown;
			}
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			chargeAmount += Time.deltaTime * chargeLoadSpeed;
			myMat.color = Color.Lerp ( myMat.color , Color.yellow ,chargeAmount * Time.deltaTime * chargeLoadSpeed / chargeMinMax.y) ;
		}

		chargeAmount = Mathf.Clamp(chargeAmount,chargeMinMax.x,chargeMinMax.y);

		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			myRigid.AddForce(moveVelocity * chargeAmount,ForceMode.Impulse);
			chargeAmount = 0f;
			myMat.color = originalColor;
		}
  */

	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag.Equals("Ball"))
		{
			Vector3 kickDir = (col.GetComponentInParent<Transform>().position - transform.position).normalized;
			Rigidbody ballRigid = col.GetComponentInParent<Rigidbody>();
			if(Input.GetKeyDown(kick))
			{
				ballRigid.AddForce(kickDir * kickPower,ForceMode.Impulse);
			}
		}
	}

	void ThrustAndCharge ( )
	{
		coolDownTimer -= Time.deltaTime;

		if ( Input.GetKey ( ChargeThrust ) )
		{
			if ( coolDownTimer <= 0f )
			{
				chargeAmount += Time.deltaTime * chargeLoadSpeed ;
				myMat.color = Color.Lerp ( myMat.color , Color.yellow , chargeAmount * Time.deltaTime * chargeLoadSpeed / chargeMinMax.y ) ;
			}
		}

		chargeAmount = Mathf.Clamp(chargeAmount,chargeMinMax.x,chargeMinMax.y);


		if ( Input.GetKeyUp (ChargeThrust) )
		{
			if ( coolDownTimer <= 0f )
			{
				if(chargeAmount <= 2f)
				{
					chargeAmount = thrust;
				}
				myRigid.AddForce ( moveVelocity * chargeAmount , ForceMode.Impulse );
				coolDownTimer = coolDown;
				chargeAmount = 0f;
				myMat.color = originalColor;
			}
		}
	}
}
