using UnityEngine;

public class CameraShake : MonoBehaviour {

	public float power = 0.2f;
	public float duration = 0.1f;
	public float slowDownAmount = 1.0f;
	public float timeToDestination = 0.1f;
	public static bool isShaking;

	Transform myCam;
	Vector3 initialPos;
	Vector3 velocity;
	float initialDuration;

	// Use this for initialization
	void Start ()
	{
		isShaking = false;
		myCam = Camera.main.transform;
		initialDuration = duration;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isShaking)
		{
			initialPos = myCam.localPosition;
			if(duration > 0)
			{
				myCam.localPosition = initialPos + Random.insideUnitSphere * power;
				duration -= Time.deltaTime * slowDownAmount;
			}
			else
			{
				isShaking = false;
				duration = initialDuration;
				myCam.localPosition = Vector3.SmoothDamp(myCam.localPosition,initialPos, ref velocity, timeToDestination);
			}
		}	
	}
}
