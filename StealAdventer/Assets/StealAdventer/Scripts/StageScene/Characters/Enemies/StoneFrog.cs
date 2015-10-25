using UnityEngine;
using System.Collections;

public class StoneFrog : NormalEnemy {
	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};

	/// <summary>
	/// 攻撃パターン
	/// </summary>
	enum Attack { Spin = 0 };
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject damageField;
	
	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 8;
	private int attackPatern;
	#endregion
	
	private float blinkerTime;
	public int dropProb;
	
	#endregion
	
	protected override void Start(){
		base.Start();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Idling;
	}

	// Update is called once per frame
	protected override void Update()
	{
		if (nowHP <= 0)
		{
			contactDamage = 0;
			gameObject.layer = LayerMask.NameToLayer("Character(DeadEnemy)");
		}

		attackTimer -= Time.deltaTime;

		switch (nowState)
		{
			case (int)CharacterState.Idling:
				{
					myAnimation.Play("Idle");
					if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 1)
						nowState = (int)CharacterState.Moving;
					break;
				}
			case (int)CharacterState.Moving:
				{
					myAnimation.Play("Walk");
					transform.position = new Vector3(transform.position.x + (xSpeed / 80) * (float)nowAngle, transform.position.y, transform.position.z);

					//プレイヤーが離れたら止まる
					if (Mathf.Abs(this.transform.position.x - player.transform.position.x) >= 10)
					{
						myAnimation.Play("Idle");
						nowState = (int)CharacterState.Idling;
					}

					if (attackTimer <= 0)
					{
						nowState = (int)CharacterState.Attacking;
						attackTimer = startAttack;
						CheckAngle();
						myAnimation.Play("Attack2");
						skillGeneratePoint.GenerateSkill(damageField, LayerNames.EnemySkill);
					}
					break;
				}
			case (int)CharacterState.Attacking:
				{
					if (!myAnimation.isPlaying)
						nowState = (int)CharacterState.Moving;

					break;
				}
			case (int)CharacterState.Death:
				{
					//transform.localScale = new Vector3(1, 1, 1);
					deathTime -= Time.deltaTime;
					myRigidBody.isKinematic = true;
					if (deathTime <= 0)
					{
						int drop = Random.Range(0, dropProb);
						if (drop == 0)
							Instantiate(dropItem, this.transform.position, Quaternion.Euler(0, 90, 0));
						Instantiate(deathEffect, this.transform.position, Quaternion.Euler(0, 90, 0));
						Destroy(gameObject);
					}
					break;
				}
		}

		//死亡
		if (nowHP <= 0 && nowState != (int)CharacterState.Death)
		{
			myAnimation.Play("Death");
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
}
