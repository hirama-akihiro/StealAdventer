using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeOutScript : MonoBehaviour
{
	private bool isFadeOut;
	public float fadeOutTime;
	public float blinkInter;
	int cnt;
	
	// Use this for initialization
	void Start()
	{
		cnt = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!isFadeOut) 
		{
			return;
		}
		float alpha = (float)(0.5 + System.Math.Sin(2 * 3.14 / (blinkInter * 60f) * cnt) * 0.5);
		GetComponent<Image>().color = new Color(1, 1, 1, alpha);
		cnt++;
	}

	public IEnumerator StartFadeOut()
	{
		cnt = 0;
		isFadeOut = true;
		yield return new WaitForSeconds(fadeOutTime);
		isFadeOut = false;
		GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		enabled = false;
	}
}
