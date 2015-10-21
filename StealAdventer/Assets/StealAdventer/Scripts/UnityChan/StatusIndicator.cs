using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterStatus))]
public class StatusIndicator : MonoBehaviour {

	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;

	public GameObject hpTextObject;
	private Text hpText;

	// Use this for initialization
	void Start () {
		status = GetComponent<CharacterStatus>();
		//hpText = hpTextObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		//hpText.text = "HP:" + status.nowHP;
	}

	/// <summary>
	/// ゲーム開始時のメソッド
	/// </summary>
	public void GameStart() { enabled = true; }

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public void GameEnd() { enabled = false; }
}
