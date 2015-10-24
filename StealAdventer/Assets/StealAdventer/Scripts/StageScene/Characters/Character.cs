using UnityEngine;
using System.Collections;

/// <summary>
/// キャラクター基底クラス
/// </summary>
public class Character : MonoBehaviour {

	/// <summary>
	/// キャッシュTransform
	/// </summary>
	private Transform cashedTransform;

	#region CharacterStatus
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
	public float xSpeed;

	/// <summary>
	/// Y方向への移動速度
	/// </summary>
	public float ySpeed;

	/// <summary>
	/// ジャンプ力
	/// </summary>
	public float jumpPower;

	/// <summary>
	/// キャラクターの向き
	/// </summary>
	public enum CharacterAngle { Left = -1, Right = 1 };

	/// <summary>
	/// 現在のキャラクターの向き
	/// </summary>
	protected CharacterAngle nowAngle;

	/// <summary>
	/// キャラクターの現在の向き
	/// </summary>
	protected int nowState;

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
	
	}

	/// <summary>
	/// キャッシュTransform
	/// </summary>
	public Transform CashedTransform
	{
		get
		{
			if (cashedTransform == null) { cashedTransform = GetComponent<Transform>(); }
			return cashedTransform;
		}
	}

	/// <summary>
	/// 体力が0以下か
	/// </summary>
	/// <returns></returns>
	public virtual bool IsDestroy { get { return nowHP <= 0; } }

	/// <summary>
	/// ゲーム開始時のメソッド
	/// </summary>
	public virtual void GameStart() { enabled = true; }

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public virtual void GameEnd() { enabled = false; }

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
