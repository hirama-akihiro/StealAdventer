using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour
{
	#region Field
	/// <summary>
	/// キャラクターの最大体力パラメータ
	/// </summary>
	public int maxHP;

	/// <summary>
	/// キャラクターの現在体力パラメータ
	/// </summary>
	public int nowHP;

	/// <summary>
	/// 接触ダメージ
	/// </summary>
	public int contactDamage;

	/// <summary>
	/// X方向への移動速度
	/// </summary>
	public float xSpeed = 3.0f;

	/// <summary>
	/// Y方向への移動速度
	/// </summary>
	public float ySpeed = 3.0f;

	/// <summary>
	/// ジャンプ力
	/// </summary>
	public float jumpPower = 7.0f;

	/// <summary>
	/// キャラクターの向き
	/// </summary>
	public enum CharacterAngle { Left = -1, Right = 1 };

	/// <summary>
	/// 現在のキャラクターの向き
	/// </summary>
	private CharacterAngle nowAngle;
	
	/// <summary>
	/// キャラクターの現在の向き
	/// </summary>
	private int nowState;

	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject skillObject;
	#endregion

	// Use this for initialization
	void Start () {
		nowHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		// 体力が0の時キャラクターを削除
		if (nowHP <= 0) { /* Destroy(this.gameObject); */}
	}

	public virtual bool IsDestroy() { return nowHP <= 0; }

	/// <summary>
	/// ゲーム開始時のメソッド
	/// </summary>
	public void GameStart() { enabled = true; }

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public void GameEnd() { enabled = false; }

	#region Property
	/// <summary>
	/// 現在の体力
	/// </summary>
	public int NowHP { get { return nowHP; } set { nowHP = value; } }

	/// <summary>
	/// キャラクターの向き
	/// </summary>
	public CharacterAngle NowAngle { get { return nowAngle; } set { nowAngle = value; } }

	/// <summary>
	/// キャラクターの現在の状態
	/// </summary>
	public int NowState { get { return nowState; } set { nowState = value; } }
	#endregion
}
