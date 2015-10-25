using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Flashing : MonoBehaviour
{
	public float fadeTime;
	int cnt;

	// Use this for initialization
	void Start()
	{
		cnt = 0;
	}

	// Update is called once per frame
	void Update()
	{
		float alpha = (float)(0.5 + System.Math.Sin(2 * 3.14 / (fadeTime * 60f) * cnt) * 0.5);
		GetComponent<Text>().color = new Color(1, 1, 1, alpha);
		cnt++;
	}
}
