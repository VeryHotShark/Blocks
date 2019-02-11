using System.Collections;
using UnityEngine;

public class FadeInOut : MonoBehaviour {

	public enum Fade {FadeIn, FadeOut};
	public Fade fadeWay;

	public float fadeSeconds;
	public float delay;

	CanvasGroup myCanvas;
	float clear = 0f;
	float full = 1f;
	// Use this for initialization
	void OnEnable()
	{
		myCanvas = GetComponent<CanvasGroup>();
		StartCoroutine (StartFade (delay));
	}
	
	IEnumerator StartFade(float delay)
	{
		yield return new WaitForSeconds(delay);
		float fadeSpeed = 1f/ fadeSeconds;
		float percent = 0f;
		if (fadeWay == Fade.FadeIn)
		{
			myCanvas.alpha = full;
			while(percent < 1f)
			{
				percent += Time.deltaTime * fadeSpeed;
				myCanvas.alpha = Mathf.Lerp(full,clear,percent);
				yield return null;
			}
		} 
		else if(fadeWay == Fade.FadeOut)
		{
			myCanvas.alpha = clear;
			while(percent < 1f)
			{
				percent += Time.deltaTime * fadeSpeed;
				myCanvas.alpha = Mathf.Lerp(clear,full,percent);
				yield return null;
			}
		}
	}
}
