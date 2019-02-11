using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Range(-1,1)]
	public int sceneNumber;
	public static bool isOver;

	public int bestOf = 5;

	public bool testMode;
	public static GameManager instance;

	ScoreHealthCounter UI;
	MenuSceneManager sceneManager;

	// Use this for initialization
	void Awake()
	{
		sceneManager = GetComponent<MenuSceneManager> ();
		UI = GetComponent<ScoreHealthCounter>();
		isOver = false;
		if(instance == null)
		{
			instance = this;
		}
	}

	void Update()
	{
		if(isOver || testMode)
		{
			if(Input.GetKeyDown(KeyCode.R))
			{
				sceneManager.ReloadScene (true);
			}
			if(Input.GetKeyDown(KeyCode.M))
			{
				sceneManager.ReloadScene (false);
			}
		}
	}

	public void CheckForWinner(int playerOneScore,int playerTwoScore)
	{
		if(playerOneScore == bestOf)
		{
			isOver = true;
			UI.UpdateWinner("red Won",playerOneScore);
		}
		else if(playerTwoScore == bestOf)
		{
			isOver = true;
			UI.UpdateWinner("blue Won",playerTwoScore);
		}
	}
}
