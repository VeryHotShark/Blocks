using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour {

	public GameObject fadeCanvas;
	public Text countDown;
	public float countDownSec; 

	public bool playScene;
	public bool fadeAnim;
	public float desiredZPos;
	public float camSeconds;
	public bool menuAnim;

	LevelManager myLevelManager;
	Camera myCam;
	Vector3 initialPos;
	float timer;

	// Use this for initialization
	void Start () 
	{
		Cursor.visible = false;

		myLevelManager = GameObject.FindObjectOfType<LevelManager> ();
		myCam = Camera.main;
		if (countDown != null)
			timer = countDownSec;

		if(fadeAnim)
			StartCoroutine (CamAnimation ());

		if(menuAnim)
			StartCoroutine (CamAnimation ());
	}

	// Update is called once per frame
	void Update () {
		if (countDown == null)
			return;

		if(TriggerBox.playerOneIsReady && TriggerBox.playerTwoIsReady)
		{
			timer -= Time.deltaTime;
			int intTime = Mathf.CeilToInt (timer);
			countDown.text = intTime.ToString ();
			if(intTime <= 0)
			{
				FadeInOut fade = fadeCanvas.GetComponent<FadeInOut> ();
				fade.fadeWay = FadeInOut.Fade.FadeOut;
				fade.delay = 0.2f;
				desiredZPos = -250f;
				StartCoroutine (CamAnimation ());
			}
		}
		else
		{
			timer = countDownSec;
			countDown.text = timer.ToString ();
		}
	}

	IEnumerator CamAnimation()
	{
		fadeCanvas.SetActive (true);
		TriggerBox.playerOneIsReady = false;
		TriggerBox.playerTwoIsReady = false;
		initialPos = myCam.transform.localPosition;
		float speed = 1f / camSeconds;
		float percent = 0f;
		while(percent < 1f)
		{
			percent += Time.deltaTime * speed;
			myCam.transform.localPosition = Vector3.Lerp (initialPos, new Vector3 (initialPos.x, initialPos.y, desiredZPos), percent);
			yield return null;
		}

		if(!fadeAnim)
			myCam.transform.localPosition = new Vector3 (initialPos.x, initialPos.y, desiredZPos);
		if(!fadeAnim && !playScene && !menuAnim)
			SceneManager.LoadScene (myLevelManager.getCurrentMapName);
		if(!fadeAnim && countDown != null)
		{
			timer = countDownSec;
		}

		menuAnim = false;
		fadeAnim = false;
		if(fadeCanvas.GetComponent<FadeInOut>().fadeWay == FadeInOut.Fade.FadeIn)
		{
			fadeCanvas.SetActive (false);
		}
	}

	public void ReloadScene(bool reload)
	{
		FadeInOut fade = fadeCanvas.GetComponent<FadeInOut> ();
		fade.fadeWay = FadeInOut.Fade.FadeOut;
		fade.delay = 0.2f;
		desiredZPos = -250f;
		StartCoroutine (waitTillFinish (reload));
	}

	IEnumerator waitTillFinish(bool reload)
	{
		yield return StartCoroutine (CamAnimation ());
		if(reload)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			SceneManager.LoadScene("Menu");
		}
	}
}
