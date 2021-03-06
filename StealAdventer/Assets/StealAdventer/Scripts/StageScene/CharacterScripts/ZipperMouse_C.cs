﻿using UnityEngine;
using System.Collections;

public class ZipperMouse_C : MonoBehaviour
{
	#region Field
	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;

	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4, Explosion = 5 };
		
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject skillObject;

	/// <summary>
	/// 爆発エフェクト
	/// </summary>
	public GameObject explosionObject;
	
	/// <summary>
	/// Animation
	/// </summary>
	private Animation myAnimation;
	
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	private GameObject player;

	/// <summary>
	/// rigidBody
	/// </summary>
	private Rigidbody myRigidBody;

	/// <summary>
	/// 攻撃位置スクリプト
	/// </summary>
	private SkillGeneratePointScript skillGeneratePoint;

	/// <summary>
	/// 死亡時エフェクト
	/// </summary>
	public GameObject deathEffect;

	/// <summary>
	/// ドロップアイテム
	/// </summary>
	public GameObject dropItem;

	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 10;
	private float explodeTimer = 3;
	private bool isAttack = false;
	private bool isExplode = false;
	#endregion

	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	public int dropProb;

	#endregion

	void Start(){
		status = GetComponent<CharacterStatus>();
		status.NowAngle = CharacterStatus.CharacterAngle.Right;
		status.NowState = (int)CharacterState.Moving;
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.Find("SDUnityChan");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
	}
	
	// Update is called once per frame
	void Update () {

		attackTimer -= Time.deltaTime;
		mutekiTime -= Time.deltaTime;
		
		// 無敵時間の点滅処理
		if (mutekiTime > 0)
		{
			if (blinkerTime < 0)
			{
				// 自身と子オブジェクトのRendererを取得
				Renderer[] objList = GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in objList)
				{
					renderer.enabled = !renderer.enabled;
				}
				blinkerTime = blinkerInterval;
			}
			blinkerTime -= Time.deltaTime;
		}
		else {
			Renderer[] objList = GetComponentsInChildren<Renderer> ();
			foreach (Renderer renderer in objList) {
				if(!renderer.enabled)
					renderer.enabled = !renderer.enabled;
			}
		}
		
		switch (status.NowState) {
		case (int)CharacterState.Moving:
		{
			isAttack = false;
			myAnimation.Play("run");
			transform.position = new Vector3(transform.position.x + (status.xSpeed / 40) * (float)status.NowAngle, transform.position.y, transform.position.z);
			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 3 && attackTimer <= 0) {
				status.NowState = (int)CharacterState.Attacking;
				attackTimer = startAttack;
				myAnimation.Play("attack");
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			//攻撃発射
			if(myAnimation["attack"].normalizedTime > 0.35f && !isAttack){
				skillGeneratePoint.GenerateSkill(skillObject, LayerNames.EnemySkill);
				isAttack = true;
			}

			//移動モードに移行
			if(myAnimation["attack"].time > myAnimation["attack"].length){
				status.NowState = (int)CharacterState.Moving;
			}
			break;
		}
		case (int)CharacterState.Death:
		{
			transform.localScale = new Vector3(1, 1, 1);
			myAnimation.Play("idle");
			deathTime -= Time.deltaTime;
			myRigidBody.isKinematic = true;
			if(deathTime <= 0){
				int drop = Random.Range(0, dropProb);
				if(drop == 0)
					Instantiate(dropItem, this.transform.position, Quaternion.Euler(0, 90, 0));
				Instantiate(deathEffect, this.transform.position, Quaternion.Euler(0, 90, 0));
				Destroy(gameObject);
			}
			break;
		}
		case (int)CharacterState.Explosion:
		{
			myAnimation.Play("wound");
			explodeTimer -= Time.deltaTime;
			if(explodeTimer <= 0){
				/*
				status.NowState = (int)CharacterState.Moving;
				status.NowHP = 1;
				*/
				transform.localScale = new Vector3(1, 1, 1);
				status.NowState = (int)CharacterState.Death;
				transform.rotation = Quaternion.Euler(180, 90, 0);
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
				Instantiate(explosionObject, transform.position, transform.rotation);
			}
			transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, transform.localScale.z + 0.01f);
			break;
		}	
		}

		//死亡
		/*
		if (status.NowHP <= 0 && status.NowState != (int)CharacterState.Death)
		{
			status.NowState = (int)CharacterState.Death;
			transform.rotation = Quaternion.Euler(180, 90, 0);
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
		}
		*/

		//爆発
		/*
		if (status.NowHP <= status.maxHP / 3 && status.NowState == (int)CharacterState.Moving && !isExplode) {
			status.NowState = (int)CharacterState.Explosion;
			isExplode = true;
		}
		*/

		//ＨＰ０で自爆
		if(status.nowHP <= 0 && isExplode == false){
			status.NowState = (int)CharacterState.Explosion;
			isExplode = true;
			/* エネミー撃破数加算処理 */
			ScoreManager.Instance.DefeatEnemy();
		}
	}
	
	void OnTriggerEnter(Collider c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		if (layerName == "ReflectionArea") {
			transform.Rotate (0, 180, 0);
			if (status.NowAngle == CharacterStatus.CharacterAngle.Left)
				status.NowAngle = CharacterStatus.CharacterAngle.Right;
			else
				status.NowAngle = CharacterStatus.CharacterAngle.Left;
		}
	}

	private void OnTriggerStay(Collider collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.PlayerSkill)
		{
			if (mutekiTime < 0)
			{
				status.NowHP -= collision.gameObject.GetComponent<SkillParam>().damege;
				mutekiTime = 1; // 現状無敵時間を2秒にしている：長いかも
			}
		}
	}
}
