using UnityEngine;
using System.Collections;

public class StoneFrog : MonoBehaviour {
	#region Field
	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;
	
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};
	
	/// <summary>
	/// 攻撃パターン
	/// </summary>
	enum Attack {Spin=0};
	private Attack attack;
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject damageField;
	
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
	/// 死亡時エフェクト
	/// </summary>
	public GameObject deathEffect;
	
	/// <summary>
	/// 攻撃位置スクリプト
	/// </summary>
	private SkillGeneratePointScript skillGeneratePoint;

	/// <summary>
	/// ドロップアイテム
	/// </summary>
	public GameObject dropItem;
	
	#region 計測用変数
	private float deathTime = 2;
	private float attackTimer;
	private float startAttack = 8;
	private bool isAttack = false;
	private bool isRunning = false;
	private int attackPatern;
	#endregion
	
	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	public int dropProb;
	
	#endregion
	
	void Start(){
		status = GetComponent<CharacterStatus>();
		status.NowAngle = CharacterStatus.CharacterAngle.Right;
		status.NowState = (int)CharacterState.Idling;
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.Find("SDUnityChan");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (status.nowHP <= 0)
		{
			status.contactDamage = 0;
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
		
		switch (status.NowState) {
		case (int)CharacterState.Idling:
		{
			myAnimation.Play("Idle");
			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 1)
				status.NowState = (int)CharacterState.Moving;
			break;
		}
		case (int)CharacterState.Moving:
		{
			//CheckAngle();
			isAttack = false;
			myAnimation.Play("Walk");
			transform.position = new Vector3(transform.position.x + (status.xSpeed / 80) * (float)status.NowAngle, transform.position.y, transform.position.z);

			//プレイヤーが離れたら止まる
			if (Mathf.Abs (this.transform.position.x - player.transform.position.x) >= 10) {
				myAnimation.Play("Idle");
				status.NowState = (int)CharacterState.Idling;
			}

			if(attackTimer <= 0){
				status.NowState = (int)CharacterState.Attacking;
				attackTimer = startAttack;
				CheckAngle();
				myAnimation.Play("Attack2");
				skillGeneratePoint.GenerateSkill(damageField, LayerNames.EnemySkill);
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			switch(attack){
			case Attack.Spin://回転攻撃
			{
				if(!myAnimation.isPlaying)
					status.NowState = (int)CharacterState.Moving;

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
			break;
		}
		}
		
		//死亡
		if (status.NowHP <= 0 && status.NowState != (int)CharacterState.Death)
		{
			myAnimation.Play("Death");
			status.NowState = (int)CharacterState.Death;
			/* エネミー撃破数加算処理 */
			ScoreManager.Instance.DefeatEnemy();
			//transform.rotation = Quaternion.Euler(180, 90, 0);
			//transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
		}
	}
	
	void CheckAngle(){
		if (player.transform.position.x > transform.position.x) {
			status.NowAngle = CharacterStatus.CharacterAngle.Right;
			transform.rotation = Quaternion.Euler(0, 90, 0);
		} else {
			status.NowAngle = CharacterStatus.CharacterAngle.Left;
			transform.rotation = Quaternion.Euler(0, 270, 0);
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
