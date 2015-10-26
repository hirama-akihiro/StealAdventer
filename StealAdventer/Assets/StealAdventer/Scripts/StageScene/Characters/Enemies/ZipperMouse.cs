using UnityEngine;
using System.Collections;

public class ZipperMouse : NormalEnemy {
	
	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};
	
	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 8;
	private bool isAttack = false;
	#endregion
	
	private float blinkerTime;
	public int dropProb;
	#endregion
	
	protected override void Start(){
		base.Start();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Moving;
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
			case (int)CharacterState.Moving:
				{
					isAttack = false;
					myAnimation.Play("run");
					CashedTransform.position = new Vector3(CashedTransform.position.x + (xSpeed / 40) * (float)nowAngle, CashedTransform.position.y, CashedTransform.position.z);
					if (Mathf.Abs(CashedTransform.position.x - player.transform.position.x) <= 3 && attackTimer <= 0)
					{
						nowState = (int)CharacterState.Attacking;
						attackTimer = startAttack;
						myAnimation.Play("attack");
					}
					break;
				}
			case (int)CharacterState.Attacking:
				{
					//攻撃発射
					if (myAnimation["attack"].normalizedTime > 0.35f && !isAttack)
					{
						skillGeneratePoint.GenerateSkill(skillObject.GetComponent<SkillObjectScript>().attackSkilObject, LayerNames.EnemySkill);
						isAttack = true;
					}

					//移動モードに移行
					if (myAnimation["attack"].time > myAnimation["attack"].length)
					{
						nowState = (int)CharacterState.Moving;
					}
					break;
				}
			case (int)CharacterState.Death:
				{
					CashedTransform.localScale = new Vector3(1, 1, 1);
					myAnimation.Play("idle");
					deathTime -= Time.deltaTime;
					myRigidBody.isKinematic = true;
					if (deathTime <= 0)
					{
						int drop = Random.Range(0, dropProb);
						if (drop == 0)
							Instantiate(dropItem, CashedTransform.position, Quaternion.Euler(0, 90, 0));
						Instantiate(deathEffect, CashedTransform.position, Quaternion.Euler(0, 90, 0));
						Destroy(gameObject);
					}
					break;
				}
		}

		//死亡
		if (nowHP <= 0 && nowState != (int)CharacterState.Death)
		{
			/* エネミー撃破数加算処理 */
			ScoreManager.I.DefeatEnemy();
			nowState = (int)CharacterState.Death;
			CashedTransform.rotation = Quaternion.Euler(180, 90, 0);
			CashedTransform.position = new Vector3(CashedTransform.position.x, CashedTransform.position.y + 0.8f, CashedTransform.position.z);
		}
	}
}
