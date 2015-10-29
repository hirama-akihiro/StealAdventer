using UnityEngine;
using System.Collections;

public class ZipperMouse_C : NormalEnemy
{
	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4, Explosion = 5 };

	/// <summary>
	/// 爆発エフェクト
	/// </summary>
	public GameObject explosionObject;

	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 10;
	private float explodeTimer = 3;
	private bool isAttack = false;
	private bool isExplode = false;
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
	protected override void Update () {

		attackTimer -= Time.deltaTime;
		
		switch (nowState) {
		case (int)CharacterState.Moving:
		{
			isAttack = false;
			myAnimation.Play("run");
			transform.position = new Vector3(transform.position.x + (xSpeed / 40) * (float)nowAngle, transform.position.y, transform.position.z);
			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 3 && attackTimer <= 0) {
				nowState = (int)CharacterState.Attacking;
				attackTimer = startAttack;
				myAnimation.Play("attack");
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			//攻撃発射
			if(myAnimation["attack"].normalizedTime > 0.35f && !isAttack){
				skillGeneratePoint.GenerateSkill(skillObject.GetComponent<SkillObjectScript>().attackSkilObject, LayerNames.EnemySkill);
				isAttack = true;
			}

			//移動モードに移行
			if(myAnimation["attack"].time > myAnimation["attack"].length){
				nowState = (int)CharacterState.Moving;
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
				transform.localScale = new Vector3(1, 1, 1);
				nowState = (int)CharacterState.Death;
				transform.rotation = Quaternion.Euler(180, 90, 0);
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
				Instantiate(explosionObject, transform.position, transform.rotation);
			}
			transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, transform.localScale.z + 0.01f);
			break;
		}	
		}

		//ＨＰ０で自爆
		if(nowHP <= 0 && isExplode == false){
			nowState = (int)CharacterState.Explosion;
			isExplode = true;
			/* エネミー撃破数加算処理 */
			ScoreManager.I.DefeatEnemy();
		}
	}
}
