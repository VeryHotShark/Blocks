using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHealthCounter : MonoBehaviour
{
	public Text pressToRestart;
	public Text playerWon;
	public Text playerOneHealth;
	public Text playerTwoHealth;

	public Text playerOneScore;
	public Text playerTwoScore;

	Shadow playerOneShadow;
	Shadow playerTwoShadow;

	int pointCounterOne = 0;
	int pointCounterTwo = 0;

	int playerOneHealthBeforeShot;
	int playerTwoHealthBeforeShot;

	public static bool firstTimeAssign;
	public static bool firstCoroutine;
	IEnumerator AnimCoroutine;
	// Use this for initialization
	void Start ()
	{
		firstTimeAssign = true;
		firstCoroutine = true;
		PlayerMovement.OnDeath += UpdateText;
		PlayerMovement.OnHit += UpdateHealth;

		if (playerOneScore == null || playerTwoScore == null)
			return;
		
		playerOneScore.text = pointCounterOne.ToString();
		playerTwoScore.text = pointCounterTwo.ToString();
		pressToRestart.text = "best of " + GameManager.instance.bestOf.ToString();

		Shadow[] shadows = playerWon.GetComponents<Shadow>();
		playerTwoShadow = shadows[0];
		playerOneShadow = shadows[1];
	}

	// Update is called once per frame
	void UpdateText (GameObject player)
	{
		if(GameManager.isOver)
			return;

		if (playerOneScore == null || playerTwoScore == null)
			return;

		if(player.name == "PlayerOne")
		{
			pointCounterTwo++;
		}
		else
		{
			pointCounterOne++;
		}
		UpdateScore();
		GameManager.instance.CheckForWinner(pointCounterOne,pointCounterTwo);
	}

	void UpdateScore()
	{
		playerOneScore.text = pointCounterOne.ToString();
		playerTwoScore.text = pointCounterTwo.ToString();
	}

	void UpdateHealth(GameObject player)
	{
		if(firstTimeAssign)
		{
			playerOneHealthBeforeShot = player.GetComponent<PlayerMovement>().GetHealthBeforeShot;
			playerTwoHealthBeforeShot = player.GetComponent<PlayerMovement>().GetHealthBeforeShot;
			firstTimeAssign = false;
		}

		if(firstCoroutine)
		{
			if(player.name == "PlayerOne")
			{
				playerOneHealthBeforeShot = player.GetComponent<PlayerMovement>().GetHealthBeforeShot;
				playerOneHealth.text = player.GetComponent<PlayerMovement>().GetHealth.ToString() + "%";
			}
			else
			{
				playerTwoHealthBeforeShot = player.GetComponent<PlayerMovement>().GetHealthBeforeShot;
				playerTwoHealth.text = player.GetComponent<PlayerMovement>().GetHealth.ToString() + "%";
			}
			firstCoroutine = false;
		}

		if(AnimCoroutine != null)
		{
			StopCoroutine(AnimCoroutine);
		}

		AnimCoroutine = WaitForFinish(player);
		StartCoroutine(AnimCoroutine);
	}

	IEnumerator WaitForFinish(GameObject player)
	{
		yield return StartCoroutine(HealthAnimation(player));
	}

	IEnumerator HealthAnimation ( GameObject player )
	{

		int playerHealth = player.GetComponent<PlayerMovement> ( ).GetHealth ;
		int playerHealthBeforeShot ;

		if ( player.name == "PlayerOne" )
		{
			playerHealthBeforeShot = playerOneHealthBeforeShot ;
		}
		else
		{
			playerHealthBeforeShot = playerTwoHealthBeforeShot ;
		}

		while ( playerHealthBeforeShot != playerHealth )
		{
			if ( playerHealthBeforeShot > playerHealth )
			{
				playerHealthBeforeShot-- ;
			}
			else
			{
				playerHealthBeforeShot++;
			}

			if(player.name == "PlayerOne")
			{
				if ( playerHealthBeforeShot > playerHealth )
				{
					playerOneHealthBeforeShot--;
				}
				else
				{
					playerOneHealthBeforeShot++;
				}
				//playerOneHealthBeforeShot--;
				playerOneHealth.text = playerHealthBeforeShot.ToString() + "%";
			}
			else
			{
				if ( playerHealthBeforeShot > playerHealth )
				{
					playerTwoHealthBeforeShot--;
				}
				else
				{
					playerTwoHealthBeforeShot++;
				}
				//playerTwoHealthBeforeShot--;
				playerTwoHealth.text = playerHealthBeforeShot.ToString() + "%";
			}
			yield return new WaitForSeconds (0.03f);
		}

		/*	if(player.name == "Player")
		{
			playerOneHealth.text = playerHealthBeforeShot.ToString() + "%";
		}
		else
		{
			playerTwoHealth.text = playerHealthBeforeShot.ToString() + "%";
		}*/
	}

	public void UpdateWinner(string winText, int scoreOfTheWinner)
	{
		if (playerWon == null || pressToRestart == null)
			return;

		playerOneScore.enabled = false;
		playerTwoScore.enabled = false;
		playerWon.text = winText;
		if(scoreOfTheWinner == pointCounterOne)
		{
			playerOneShadow.enabled = true;
		}
		else
		{
			playerTwoShadow.enabled = true;
		}
		pressToRestart.text = "\npress r to restart\npress m to go to menu";
	}

	void OnDestroy()
	{
		PlayerMovement.OnDeath -= UpdateText;
		PlayerMovement.OnHit -= UpdateHealth;
	}
}
