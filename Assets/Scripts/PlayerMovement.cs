using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public enum Player {PlayerOne,PlayerSecond};
	public Player index;
	[Header("PlayerInput")]
	public KeyCode shoot ;
	public KeyCode ChargeThrust ;

	[Header("PlayerVariables")]
	public int startHealth = 100;
	public float slowDownSpeed = 6f;
	public float secondsToCharge = 2f;
	public float chargeLoadSpeed = 2f;
	public Vector2 chargeMinMax  = new Vector2 (0f,2f);
	public float coolDown = 3f;
	public float thrust = 1f;
	public float kickPower = 2f;
	public float smoothMoveTime = .1f;
	public float setSpeed = 12f;
	public float turnSpeed = 4f;

	public float animTime;
	public float pickUpLast;
	public Projectile bullet;
	public Transform[] spawnPos;
	public GameObject dieVFX;
	public GameObject thrustVFX;
	public GameObject pointer;
	public GameObject ui;
	public GameObject shieldUI;
	public GameObject bulletUI;
	public AudioClip[] clips;
	public Image loadbar;

	AudioSource audioSource;
	Vector2 shotChargeMinMax = new Vector2(0f,1f);
	bool multipleShots;
	bool isMagnetic;
	bool immune ;
	bool lerpColor;
	bool isDead;
	bool isDeadByShot;
	bool firstSpawn;
	Color spawnColor;

	Transform spawnMiddlePos;
	Rigidbody myRigid;
	Vector3 deadPos;
	Vector3 moveVelocity;
	Vector3 initialPos;
	Quaternion initialRot;
	Material myMat;
	Color originalColor;
	BoxCollider myBox;
	MeshRenderer myMesh;
	IEnumerator animBarUp;

	float smoothInputMagnitude;
	float smoothInputVelocity;
	float angle;
	float chargeAmount;
	float coolDownTimer;
	float moveVer;
	float moveHor;
	float shotChargeAmount;
	float chargeSpeed;
	float moveSpeed;
	int health;
	int healthBeforeGettingShot;



	public static event System.Action<GameObject> OnDeath;
	public static event System.Action<GameObject> OnHit;


	// Use this for initialization
	void Start ( )
	{
		bulletUI.SetActive(false);
		shieldUI.SetActive(false);
		spawnMiddlePos = GetComponentInChildren<Transform> ( ) ;
		audioSource = GetComponent<AudioSource>();
		myRigid = GetComponent<Rigidbody> ( ) ;
		myMat = GetComponent<Renderer>().material;
		myMesh = gameObject.GetComponent<MeshRenderer>();
		myBox = gameObject.GetComponent<BoxCollider>();

		if ( index == Player.PlayerOne )
		{
			angle = 90f ;
			ChargeThrust = KeyCode.Space ;
			shoot = KeyCode.V ;
		}
		else 
		{
			angle = -90f ;
			ChargeThrust = KeyCode.Keypad0 ;
			shoot = KeyCode.Keypad3;
		}

		health = startHealth;
		healthBeforeGettingShot = health;

		if ( OnHit != null )
		{
			OnHit ( gameObject ) ;
		}

		//ScoreHealthCounter.firstCoroutine = true;
		originalColor = myMat.color;
		initialPos = transform.position;
		initialRot = transform.rotation;
		spawnColor = myMat.color;
		spawnColor.a = 0;
		moveSpeed = setSpeed;
		isDeadByShot = false;
		isDead = false;
		lerpColor = false;
		multipleShots = false;
		immune = false;
		firstSpawn = true;
		StartCoroutine ( SpawnPlayer ( 2f ) ) ;
		firstSpawn = false;
	}
	
	// Update is called once per frame
	void Update ( )
	{
		CheckIfDead ( ) ;
		if ( !isDead && !isDeadByShot )
		{
			Movement ( ) ;
			ThrustAndCharge ( ) ;
			ChargeShoot();
			//Shoot ( ) ;
		}

		if(isDeadByShot)
		{
			transform.position = deadPos;
		}
	}

	void Movement ( )
	{
		if ( index == Player.PlayerOne )
		{
			moveHor = Input.GetAxisRaw ( "Horizontal" ) ;
			moveVer = Input.GetAxisRaw ( "Vertical" ) ;
		}
		else
		{
			moveHor = Input.GetAxisRaw("HorizontalSecond");
			moveVer = Input.GetAxisRaw("VerticalSecond");
		}

		Vector3 moveDir = new Vector3 ( moveHor , 0f , moveVer ).normalized ;

		float movementMagnitude = moveDir.magnitude ;
		smoothInputMagnitude = Mathf.SmoothDamp ( smoothInputMagnitude , movementMagnitude , ref smoothInputVelocity , smoothMoveTime ) ;
		//moveVelocity = moveDir * moveSpeed * smoothInputMagnitude;

		float targetAngle = Mathf.Atan2 ( moveDir.x , moveDir.z ) * Mathf.Rad2Deg ;
		angle = Mathf.LerpAngle (  angle , targetAngle , Time.deltaTime * turnSpeed * movementMagnitude ) ;

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

	/*	void CheckGroundAngle()
	{
		Ray ray = new Ray(transform.position,Vector3.down * 2f);
		RaycastHit hitInfo;
		
		if(Physics.Raycast(ray,out hitInfo, 2f))
		{
			transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
		}
	}*/
	
	/*void Shoot()
	{
		if(Input.GetKeyDown(shoot))
		{
			Instantiate(bullet,spawnMiddlePos.position,spawnMiddlePos.rotation);
		}
	}*/

	void ThrustAndCharge ( )
	{
		coolDownTimer -= Time.deltaTime ;

		if ( Input.GetKey ( ChargeThrust ) )
		{
			if ( coolDownTimer <= 0f )
			{
				chargeAmount += Time.deltaTime / chargeLoadSpeed ;
				myMat.color = Color.Lerp ( originalColor , Color.yellow , chargeAmount / chargeMinMax.y ) ;
			}
			Debug.Log(chargeAmount);
		}

		chargeAmount = Mathf.Clamp ( chargeAmount , chargeMinMax.x , chargeMinMax.y ) ;


		if ( Input.GetKeyUp ( ChargeThrust ) )
		{
			if ( coolDownTimer <= 0f )
			{
				audioSource.clip = clips[5];
				audioSource.Play();
				StartCoroutine ( AnimateBarDown ( ) ) ;
				if ( chargeAmount < chargeMinMax.y )
				{
					chargeAmount = thrust ;
				}

				if ( moveVelocity.x < 1f && moveVelocity.z < 1f )
				{
					myRigid.AddForce ( transform.forward * chargeAmount * 15f , ForceMode.Impulse ) ;
				}
				else
				{
					myRigid.AddForce ( moveVelocity * chargeAmount , ForceMode.Impulse ) ;
				}
				GameObject vfx = Instantiate(thrustVFX,transform.position + transform.forward * transform.lossyScale.z / 2f,transform.rotation * Quaternion.Euler(0f,180f,0f)) as GameObject;
				Destroy(vfx,1f);
				coolDownTimer = coolDown ;
				chargeAmount = 0f ;
				myMat.color = originalColor ;
			}
		}

		if ( loadbar.fillAmount == 0f )
		{
			animBarUp = AnimateBarUp();
			StartCoroutine(animBarUp);
		}
	}

	IEnumerator AnimateBarDown()
	{
		float animSpeed = 1f/ animTime;
		float percent = 0;
		while(percent < 1)
		{
			percent += Time.deltaTime * animSpeed;
			loadbar.fillAmount = Mathf.Lerp(1f,0f,percent);
			yield return null;
		}
	}

	IEnumerator AnimateBarUp()
	{
		float animSpeed = 1f/ (coolDown - animTime);
		float percent = 0;
		while(percent < 1)
		{
			percent += Time.deltaTime * animSpeed;
			loadbar.fillAmount = Mathf.Lerp(0f, 1f,percent);
			yield return null;
		}
	}

	void ChargeShoot ( )
	{
		if ( Input.GetKeyDown ( shoot ) )
		{
			lerpColor = true ;
			chargeSpeed = 1f / secondsToCharge ;
		}

		if ( lerpColor )
		{
			shotChargeAmount += Time.deltaTime * chargeSpeed ;
			myMat.color = Color.Lerp ( originalColor , Color.black , shotChargeAmount ) ;
			moveSpeed = Mathf.Lerp(setSpeed,slowDownSpeed,shotChargeAmount);
		}

		//Debug.Log(shotChargeAmount);
		shotChargeAmount = Mathf.Clamp ( shotChargeAmount , shotChargeMinMax.x , shotChargeMinMax.y ) ;

		if ( Input.GetKeyUp ( shoot ) )
		{
			if ( shotChargeAmount < shotChargeMinMax.y )
			{

				if ( multipleShots )
				{
					for ( int i = 0 ; i < spawnPos.Length ; i++ )
					{
						Projectile projectile = Instantiate ( bullet , spawnPos [ i ].position , spawnPos [ i ].rotation ) as Projectile;
						if(isMagnetic)
						{
							projectile.spawnMagnet = true;
							projectile.GetPlayerWeShotFrom(this);
						}
					}
				}
				else
				{
					 Projectile projectile = Instantiate ( bullet , spawnMiddlePos.position , spawnMiddlePos.rotation ) as Projectile;
					if(isMagnetic)
						{
							projectile.spawnMagnet = true;
							projectile.GetPlayerWeShotFrom(this);
						}
				}

				myRigid.AddForce(-transform.forward * kickPower, ForceMode.Impulse);
				bullet.spawnMagnet = false;
				audioSource.clip = clips[0];
				audioSource.Play();
			}
			else
			{
				Projectile projectile = Instantiate(bullet,spawnMiddlePos.position,spawnMiddlePos.rotation) as Projectile;
				projectile.transform.localScale = Vector3.one;
				projectile.SetThrowbackAmount(30f);
				projectile.SetDamageAmount(50);
				myRigid.AddForce(-transform.forward * kickPower * 3f, ForceMode.Impulse);
				CameraShake.isShaking = true;
				audioSource.clip = clips[1];
				audioSource.Play();
			}
			myMat.color = originalColor;
			shotChargeAmount = shotChargeMinMax.x;
			lerpColor = false;
			moveSpeed = setSpeed;
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "HealthPickUp")
		{
			audioSource.clip = clips[2];
			audioSource.Play();
			health += 50;
			if(OnHit != null)
			{
				OnHit(gameObject);
			}
			Destroy(col.gameObject);
		}
		else if(col.tag == "TriplePickUp")
		{
			audioSource.clip = clips[2];
			audioSource.Play();
			multipleShots = true;
			bulletUI.SetActive(true);
			StartCoroutine(DeactivateUI(bulletUI,pickUpLast,multipleShots));
			Destroy(col.gameObject);
		}
		else if(col.tag == "ShieldPickUp")
		{
			audioSource.clip = clips[2];
			audioSource.Play();
			immune = true;
			shieldUI.SetActive(true);
			StartCoroutine(DeactivateUI(shieldUI,pickUpLast,immune));
			Destroy(col.gameObject);
		}
		else if(col.tag == "MagnetPickUp")
		{
			audioSource.clip = clips[2];
			audioSource.Play();
			isMagnetic = true;
			//shieldUI.SetActive(true);
			//StartCoroutine(DeactivateUI(shieldUI,pickUpLast,immune));
			Destroy(col.gameObject);
		}
	}

	IEnumerator DeactivateUI ( GameObject ui , float sec , bool ability )
	{
		yield return new WaitForSeconds ( sec ) ;
		if(bool.Equals(ability,multipleShots))
		{
			multipleShots = false;
		}
		else
		{
			immune = false;
		}
		ui.SetActive(false);
	}

	void CheckIfDead ( )
	{
		if ( transform.position.y < -15f )
		{
			isDead = true ;
			if ( isDead )
			{
				GameObject vfx = (GameObject)Instantiate(dieVFX, transform.position,Quaternion.identity);
				vfx.GetComponent<Renderer>().material = myMat;
				Destroy(vfx,3f);
				StartCoroutine ( SpawnPlayer ( 2f ) ) ;
				audioSource.clip = clips[4];
				audioSource.Play();
			}
		}
	}

	public void TakeHit(float throwback, Vector3 dir)
	{
		myRigid.AddForce(dir * throwback,ForceMode.Impulse);
		audioSource.clip = clips[3];
		audioSource.Play();
	}
	
	public void TakeDamage(int amount)
	{
		if(immune)
			return;

		health -= amount;
		healthBeforeGettingShot = health + amount;

		if(OnHit != null)
		{
			OnHit(gameObject);
		}

		if(health <= 0)
		{
			isDeadByShot = true;
			deadPos = transform.position;
			GameObject vfx = (GameObject)Instantiate(dieVFX, transform.position,Quaternion.identity);
			vfx.GetComponent<Renderer>().material = myMat;
			Destroy(vfx,3f);
			audioSource.clip = clips[4];
			audioSource.Play();
			if(isDeadByShot)
			{
				bulletUI.SetActive(false);
				shieldUI.SetActive(false);
				ui.SetActive(false);
				pointer.SetActive(false);
				myRigid.detectCollisions = false;
				myRigid.isKinematic = true;
				myBox.enabled = false;
				myMesh.enabled = false;
				StartCoroutine(WaitCoroutine(2f));
			}
		}
	}

	IEnumerator WaitCoroutine(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		StartCoroutine(SpawnPlayer(2f));
	}

	IEnumerator SpawnPlayer ( float safeTime )
	{
		if(animBarUp != null)
		{
			StopCoroutine(animBarUp);
		}

		if ( !firstSpawn )
		{
			if ( OnDeath != null )
			{
				OnDeath ( gameObject ) ;
			}
		}
		ui.SetActive(true);
		bulletUI.SetActive(false);
		shieldUI.SetActive(false);
		pointer.SetActive(true);
		myRigid.isKinematic = false;
		myRigid.detectCollisions = true;
		myBox.enabled = true;
		myMesh.enabled = true;
		transform.position = initialPos;
		myRigid.velocity = Vector3.zero;
		isDead = false;
		isDeadByShot = false;
		immune = false;
		multipleShots = false;
		gameObject.layer = 2;
		health = startHealth;
		healthBeforeGettingShot = health;
		loadbar.fillAmount = 1f;
		coolDownTimer = 0f;
		ScoreHealthCounter.firstCoroutine = true;

		if(index == Player.PlayerOne)
		{
			angle = 90f;
		}
		else
		{
			angle = -90f;
		}

		if(OnHit != null)
		{
			OnHit(gameObject);
		}

		float respawnFadeSpeed = 1f / safeTime;
		float percent = 0f;

		while(percent <= 1f)
		{
			percent += Time.deltaTime * respawnFadeSpeed;
			myMat.color = Color.Lerp(originalColor, spawnColor, Mathf.PingPong(percent * 4f,1f));
			yield return null;
		}
		gameObject.layer = 8;
	}

	public bool IsPlayerDead{get{return isDeadByShot;}}

	public bool disableMagnet{get{return isMagnetic;}set{isMagnetic = value;}}

	public Vector3 GetInitPos{get{return initialPos;}}

	public Quaternion GetInitRot{get{return initialRot;}}

	public int GetHealth{get{return health;}}

	public int GetHealthBeforeShot{get{return healthBeforeGettingShot;}}
}
