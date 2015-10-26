using UnityEngine;
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
		AudioManager.I.PlayAudio("BGMFight", AudioManager.PlayMode.Repeat);
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
					AudioManager.I.PlayAudio("VoiceThree");
					break;
				case "2":
					countDownImageObject.GetComponent<Image>().sprite = countDownTwo;
					AudioManager.I.PlayAudio("VoiceTwo");
					break;
				case "1":
					countDownImageObject.GetComponent<Image>().sprite = countDownOne;
					AudioManager.I.PlayAudio("VoiceOne");
					break;
				case "0":
					countDownImageObject.GetComponent<Image>().sprite = countDownStart;
					AudioManager.I.PlayAudio("VoiceStart");
					break;
			}
		}
		countDownImageObject.GetComponent<Image>().color = new Color(1, 1, 1, timer - Mathf.Floor(timer));

		if(timer < 0.0f)
		{
			playerObject.SendMessage("GameStart");
            dataCanvas.SetActive(true);
            ScoreManager.I.GameStart();
            GameObject.Find("ElapsedTimeText").GetComponent<ElapsedTimeScript>().GameStart();
			countDownImageObject.GetComponent<Image>().enabled = false;
			enabled = false;
		}

		timer -= Time.deltaTime;
		prevTime = nowTime;
	}
}
