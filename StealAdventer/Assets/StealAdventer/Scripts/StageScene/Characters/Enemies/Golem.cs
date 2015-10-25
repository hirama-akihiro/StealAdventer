using UnityEngine;
using System.Collections;

public class Golem : NormalEnemy {

	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};

	/// <summary>
	/// 攻撃パターン
	/// </summary>
	enum Attack {Punch=0, Throw=1};
	private Attack attack;
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject stone;
	public GameObject punch;
	
	/// <summary>
	/// capsuleCollider
	/// </summary>
	private CapsuleCollider capsuleCollider;

	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 8;
	private bool isAttack = false;
	private bool isRunning = false;
	private int attackPatern;
	private float attackingTime;
	#endregion
	
	private float blinkerTime;
	public int dropProb;
	
	#endregion
	
	protected override void Start(){
		base.Start();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Moving;
		capsuleCollider = GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (nowHP <= 0)
		{
			contactDamage = 0;
			gameObject.layer = LayerMask.NameToLayer("Character(DeadEnemy)");
		}

		attackTimer -= Time.deltaTime;
		
		switch (nowState) {
		case (int)CharacterState.Idling:
		{
			if (Mathf.Abs (CashedTransform.position.x - player.transform.position.x) <= 10)
				nowState = (int)CharacterState.Moving;
			break;
		}
		case (int)CharacterState.Moving:
		{
			isAttack = false;
			myAnimation.Play("walk");
			CashedTransform.position = new Vector3(CashedTransform.position.x + (xSpeed / 80) * (float)nowAngle, CashedTransform.position.y, CashedTransform.position.z);

			if(attackTimer <= 0){
				nowState = (int)CharacterState.Attacking;
				attackTimer = startAttack;
				SelectAttack();
				CheckAngle();
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			attackingTime += Time.deltaTime;
			switch(attack){
			case Attack.Punch://パンチ
			{
				//プレイヤーの近くまで走る
				if(!isRunning)
					CashedTransform.position = new Vector3(CashedTransform.position.x + (xSpeed / 20) * (float)nowAngle, CashedTransform.position.y, CashedTransform.position.z);
				//近寄ったらパンチ
				if(Mathf.Abs (CashedTransform.position.x - player.transform.position.x) <= 1 && !isRunning){
					isRunning = true;
					myAnimation.Play("punch");
				}
				if(myAnimation["punch"].normalizedTime > 0.45f && !isAttack){
					skillGeneratePoint.GenerateSkill(punch, LayerNames.EnemySkill);
					isAttack = true;
				}
				//移動モードに移行
				if(myAnimation["punch"].time >= 0.3){
					nowState = (int)CharacterState.Moving;
				}
				break;
			}
			case Attack.Throw://投げ攻撃
			{
				//攻撃発射
				if(myAnimation["hpunch"].normalizedTime > 0.35f && !isAttack){
					skillGeneratePoint.GenerateSkill(stone, LayerNames.EnemySkill);
					isAttack = true;
				}
				//移動モードに移行
				if(myAnimation["hpunch"].time >= 0.4f){
					nowState = (int)CharacterState.Moving;
				}
				break;
			}
			}

			//何かしら止まったら時間経過で強制的に移動モードへ
			if(attackingTime >= 1.0f)
				nowState = (int)CharacterState.Moving;

			break;
		}
		case (int)CharacterState.Death:
		{
			deathTime -= Time.deltaTime;
			myRigidBody.isKinematic = true;
			if(deathTime <= 0){
				int drop = Random.Range(0, dropProb);
				if(drop == 0)
					Instantiate(dropItem, CashedPosition, Quaternion.Euler(0, 90, 0));
				Instantiate(deathEffect, CashedPosition, Quaternion.Euler(0, 90, 0));
				Destroy(gameObject);
			}
			break;
		}
		}
		
		//死亡
		if (nowHP <= 0 && nowState != (int)CharacterState.Death)
		{
			myAnimation.Play("death");
			capsuleCollider.center = new Vector3(0.0f, 0.1f, 0.0f);
			capsuleCollider.radius = 0.3f;
			capsuleCollider.height = 0.3f;
			nowState = (int)CharacterState.Death;
			ScoreManager.Instance.DefeatEnemy();
		}
	}

	void CheckAngle(){
		if (player.transform.position.x > transform.position.x) {
			nowAngle = CharacterAngle.Right;
			transform.rotation = Quaternion.Euler(0, 90, 0);
		} else {
			nowAngle = CharacterAngle.Left;
			transform.rotation = Quaternion.Euler(0, 270, 0);
		}
	}

	//攻撃を選び準備する関数
	void SelectAttack(){
		attackingTime = 0;
		attack = Attack.Punch;
		myAnimation.Play("run");
		isRunning = false;
	}
}
