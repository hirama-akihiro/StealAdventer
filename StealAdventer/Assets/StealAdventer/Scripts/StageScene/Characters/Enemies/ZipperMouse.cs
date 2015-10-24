using UnityEngine;
using System.Collections;

public class ZipperMouse : Enemy {
	
	#region Field
	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Moving = 1, Attacking = 2, Damage = 3, Death = 4};
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject skillObject;

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
	private float startAttack = 8;
	private bool isAttack = false;
	#endregion
	
	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	public int dropProb;
	
	#endregion
	
	void Start(){
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Moving;
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody> ();
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
				skillGeneratePoint.GenerateSkill(skillObject, LayerNames.EnemySkill);
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
		}

		//死亡
		if (nowHP <= 0 && nowState != (int)CharacterState.Death)
		{
			/* エネミー撃破数加算処理 */
			ScoreManager.Instance.DefeatEnemy();
			nowState = (int)CharacterState.Death;
			transform.rotation = Quaternion.Euler(180, 90, 0);
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
		}

		//プレイヤーが遠く離れると消滅
		//if (Mathf.Abs (this.transform.position.x - player.transform.position.x) >= 20) {
		//	Destroy(gameObject);
		//}
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
