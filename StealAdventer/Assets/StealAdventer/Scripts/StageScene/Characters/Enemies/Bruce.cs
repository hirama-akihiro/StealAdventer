using UnityEngine;
using System.Collections;

public class Bruce : Enemy {

	#region アニメーションフレーム
	private float stop_s = 0.01f;
	private float stop_e = 0.06f;

	private float blow_s = 0.061f;
	private float blow_e = 0.112f;
	private float blow_a = 0.08f;

	private float scissor_s = 0.19f;
	private float scissor_e = 0.245f;
	private float scissor_a = 0.215f;

	private float death_s = 0.85f;
	private float death_e = 0.93f;
	#endregion

	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};
	
	/// <summary>
	/// 攻撃パターン
	/// </summary>
	enum Attack {Slash=0 };

	private Attack attack;
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject slash;
	
	/// <summary>
	/// capsuleCollider
	/// </summary>
	private CapsuleCollider capsuleCollider;
	
	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private bool isAttack = false;
	private int attackPatern;
	#endregion
	
	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	public int dropProb;
	
	#endregion

	// Use this for initialization
	void Start () {
		myAnimation = GetComponent<Animation> ();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Idling;
		myRigidBody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		player = GameObject.Find("SDUnityChan");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (nowHP <= 0)
		{
			contactDamage = 0;
			gameObject.layer = LayerMask.NameToLayer("Character(DeadEnemy)");
		}

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
		
		switch (nowState) {
		case (int)CharacterState.Idling:
		{
			CheckAngle();
			isAttack = false;
			if (myAnimation ["Take 0010"].normalizedTime > stop_e)
				myAnimation["Take 0010"].speed = -1;
			if (myAnimation ["Take 0010"].normalizedTime < stop_s)
				myAnimation["Take 0010"].speed = 1;

			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 5 && attackTimer < 0){
				nowState = (int)CharacterState.Moving;
				//myAnimation["Take 0010"].speed = 1;
			}

			break;
		}
		case (int)CharacterState.Moving:
		{
			CheckAngle();
			myAnimation.Play("Run");
			transform.position = new Vector3(transform.position.x + (xSpeed / 80) * (float)nowAngle, transform.position.y, transform.position.z);

			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 2) {
				nowState = (int)CharacterState.Attacking;
				SelectAttack();
				CheckAngle();
				myAnimation["Take 0010"].speed = 1;
			}
			if(Mathf.Abs (this.transform.position.x - player.transform.position.x) >= 10){
				nowState = (int)CharacterState.Idling;
				myAnimation["Take 0010"].normalizedTime = stop_s;
				attackTimer = 0.2f;
				myAnimation["Take 0010"].speed = 1;
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			switch(attack){
			case Attack.Slash://斬る
			{
				//攻撃判定発生
				if(myAnimation["Take 0010"].normalizedTime > scissor_a && !isAttack){
					isAttack = true;
					skillGeneratePoint.GenerateSkill(slash, LayerNames.EnemySkill);
				}

				//攻撃終了
				if(myAnimation["Take 0010"].normalizedTime > scissor_e){
					nowState = (int)CharacterState.Moving;
					myAnimation["Take 0010"].normalizedTime = stop_s;
					attackTimer = 0.2f;
					myAnimation["Take 0010"].speed = 1;
				}
				break;
			}
			}
			
			break;
		}
		case (int)CharacterState.Death:
		{
			//transform.localScale = new Vector3(1, 1, 1);
			deathTime -= Time.deltaTime;
			myRigidBody.isKinematic = true;
			if(deathTime <= 0){
				int drop = Random.Range(0, dropProb);
				if(drop == 0)
					Instantiate(dropItem, this.transform.position, Quaternion.Euler(0, 90, 0));
				Instantiate(deathEffect, this.transform.position, Quaternion.Euler(0, 90, 0));
				Destroy(gameObject);
			}

			if(myAnimation["Take 0010"].normalizedTime > death_e){
				myAnimation["Take 0010"].speed = 0;
			}

			break;
		}
		}
		
		//死亡
		if (nowHP <= 0 && nowState != (int)CharacterState.Death)
		{
			/* エネミー撃破数加算処理 */
			ScoreManager.Instance.DefeatEnemy();
			nowState = (int)CharacterState.Death;
			myAnimation.Play("Take 0010");
			myAnimation["Take 0010"].normalizedTime = death_s;
		}

		if (Mathf.Abs (this.transform.position.x - player.transform.position.x) >= 20) {
			Destroy(gameObject);
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
		attackPatern = Random.Range (0, 1);
		switch (attackPatern) {
		case 0:
		{
			attack = Attack.Slash;
			myAnimation.Play("Take 0010");
			myAnimation["Take 0010"].normalizedTime = scissor_s;
			break;
		}
		}
	}

	private void RefrainMotion(float s, float e){
		if (myAnimation ["Take 0010"].normalizedTime > e) {
			myAnimation["Take 0010"].speed = -1;
		}
		if (myAnimation ["Take 0010"].normalizedTime < s) {
			myAnimation["Take 0010"].speed = 1;
		}
	}
	
	void OnTriggerEnter(Collider c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		if (layerName == "ReflectionArea") {
			transform.Rotate (0, 180, 0);
			if (nowAngle == CharacterAngle.Left)
				nowAngle = CharacterAngle.Right;
			else
				nowAngle = CharacterAngle.Left;
		}
	}
	
	private void OnTriggerStay(Collider collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.PlayerSkill)
		{
			if (mutekiTime < 0)
			{
				nowHP -= collision.gameObject.GetComponent<SkillParam>().damege;
				mutekiTime = 1; // 現状無敵時間を2秒にしている：長いかも
			}
		}
	}
}
