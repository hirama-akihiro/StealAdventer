using UnityEngine;
using System.Collections;

public class Enemy : Character {

	#region Field Value
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	protected GameObject player;

	/// <summary>
	/// Animation
	/// </summary>
	protected Animation myAnimation;

	/// <summary>
	/// rigidBody
	/// </summary>
	protected Rigidbody myRigidBody;

	/// <summary>
	/// 攻撃位置スクリプト
	/// </summary>
	protected SkillGeneratePointScript skillGeneratePoint;

	/// <summary>
	/// 死亡時エフェクト
	/// </summary>
	public GameObject deathEffect;

	/// <summary>
	/// ドロップアイテム
	/// </summary>
	public GameObject dropItem;
	#endregion

	// Use this for initialization
	protected virtual void Start () {
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody>();
		player = GameObject.Find("SDUnityChan");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
