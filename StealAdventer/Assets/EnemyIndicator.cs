using UnityEngine;
using System.Collections;

public class EnemyIndicator : MonoBehaviour {

	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;

	/// <summary>
	/// GUISkin
	/// </summary>
	public GUISkin skin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//void OnGUI()
	//{
	//	status = GetComponent<CharacterStatus>();
	//	string text = "HP:" + status.NowHP.ToString();
	//	int sw = Screen.width;
	//	int sh = Screen.height;
	//	Rect rect = new Rect(sw - sw / 4, 0, sw / 2, sh / 4);

	//	// HPの描画
	//	GUI.skin = skin;
	//	GUI.Label(rect, text, "Status");
	//}

	/// <summary>
	/// ゲーム開始時のメソッド
	/// </summary>
	public void GameStart() { enabled = true; }

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public void GameEnd() { enabled = false; }
}
