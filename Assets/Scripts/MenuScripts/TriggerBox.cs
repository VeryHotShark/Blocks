using UnityEngine;

public class TriggerBox : MonoBehaviour {

	[Range(0,1)] public int triggerIndex;
	public static bool playerOneIsReady = false;
	public static bool playerTwoIsReady = false;

	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player")
		{
			if(triggerIndex == 0)
			{
				playerOneIsReady = true;
			}
			else
			{
				playerTwoIsReady = true;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			if(triggerIndex == 0)
			{
				playerOneIsReady = false;
			}
			else
			{
				playerTwoIsReady = false;
			}
		}
	}
}
