using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ElapsedTimeScript : MonoBehaviour {

    /// <summary>
    /// ゲームプレイ中かどうか
    /// </summary>
    private bool isGamePlaying;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isGamePlaying)
            this.GetComponent<Text>().text = new DateTime(0).Add(ScoreManager.Instance.GetElapsedTime()).ToString("mm:ss");
	}

    public void GameStart()
    {
        isGamePlaying = true;
    }

    public void GameEnd()
    {
		isGamePlaying = false;
        this.GetComponent<Text>().text = "00:00";
    }
}
