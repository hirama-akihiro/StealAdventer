﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStarter : MonoBehaviour {

    /// <summary>
	/// ステータスなどのデータ表示用キャンバス
	/// </summary>
    public GameObject dataCanvas;

	public GameObject playerObject;
	public GameObject bossObject;
	private float timer;
	private int nowTime;
	private int prevTime;

	public GameObject countDownImageObject;
	public Sprite countDownThree;
	public Sprite countDownTwo;
	public Sprite countDownOne;
	public Sprite countDownStart;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.PlayAudio("Fight", AudioManager.PlayMode.Repeat);
		timer = 4f;
	}
	
	// Update is called once per frame
	void Update () {

		nowTime = Mathf.CeilToInt(timer - 1.0f);
		if (prevTime != (int)nowTime)
		{
			switch (nowTime.ToString())
			{
				case "3":
					countDownImageObject.GetComponent<Image>().sprite = countDownThree;
					AudioManager.Instance.PlayAudio("Three");
					break;
				case "2":
					countDownImageObject.GetComponent<Image>().sprite = countDownTwo;
					AudioManager.Instance.PlayAudio("Two");
					break;
				case "1":
					countDownImageObject.GetComponent<Image>().sprite = countDownOne;
					AudioManager.Instance.PlayAudio("One");
					break;
				case "0":
					countDownImageObject.GetComponent<Image>().sprite = countDownStart;
					AudioManager.Instance.PlayAudio("Start");
					break;
			}
		}
		countDownImageObject.GetComponent<Image>().color = new Color(1, 1, 1, timer - Mathf.Floor(timer));

		if(timer < 0.0f)
		{
			playerObject.SendMessage("GameStart");
            dataCanvas.SetActive(true);
            ScoreManager.Instance.GameStart();
            GameObject.Find("ElapsedTimeText").GetComponent<ElapsedTimeScript>().GameStart();
			countDownImageObject.GetComponent<Image>().enabled = false;
			enabled = false;
		}

		timer -= Time.deltaTime;
		prevTime = nowTime;
	}
}
