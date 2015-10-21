using UnityEngine;
using System.Collections;

public class Golem_B : MonoBehaviour {

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
	enum Attack {Throw=0};
	private Attack attack;
	
	/// <summary>
	/// Skillオブジェクト
	/// </summary>
	public GameObject stone;
	
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
	/// capsuleCollider
	/// </summary>
	private CapsuleCollider capsuleCollider;
	
	/// <summary>
	/// 攻撃位置スクリプト
	/// </summary>
	private SkillGeneratePointScript skillGeneratePoint;
	
	#region 計測用変数
	private float deathTime = 10;
	private float attackTimer;
	private float startAttack = 8;
	private bool isAttack = false;
	private bool isRunning = false;
	private int attackPatern;
	#endregion
	
	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	
	#endregion
	
	void Start(){
		status = GetComponent<CharacterStatus>();
		status.NowAngle = CharacterStatus.CharacterAngle.Right;
		status.NowState = (int)CharacterState.Idling;
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
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
		case (int)CharacterState.Idling:
		{
			myAnimation.Play("idle");
			if (attackTimer <= 0){
				status.NowState = (int)CharacterState.Attacking;
				CheckAngle();
				myAnimation.Play("hpunch");
			}
			break;
		}
		case (int)CharacterState.Moving:
		{
			break;
		}
		case (int)CharacterState.Attacking:
		{
			//攻撃発射
			if(myAnimation["hpunch"].normalizedTime > 0.35f && !isAttack){
				skillGeneratePoint.GenerateSkill(stone, LayerNames.EnemySkill);
				isAttack = true;
			}
			//攻撃終了
			if(myAnimation["hpunch"].time >= 0.48f){
				status.NowState = (int)CharacterState.Idling;
				attackTimer = 2.0f;
				isAttack = false;
			}		
			break;
		}
		case (int)CharacterState.Death:
		{
			deathTime -= Time.deltaTime;
			myRigidBody.isKinematic = true;
			if(deathTime <= 0){
				/* エネミー撃破数加算処理 */
				ScoreManager.Instance.DefeatEnemy();
				Destroy(gameObject);
			}
			break;
		}
		}
		
		//死亡
		if (status.NowHP <= 0 && status.NowState != (int)CharacterState.Death)
		{
			myAnimation.Play("death");
			capsuleCollider.center = new Vector3(0.0f, 0.1f, 0.0f);
			capsuleCollider.radius = 0.3f;
			capsuleCollider.height = 0.3f;
			status.NowState = (int)CharacterState.Death;
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
