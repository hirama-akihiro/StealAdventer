using UnityEngine;
using System.Collections;

public class Minotaur : Enemy {

	/// <summary>
	/// ブリッツモードスクリプト
	/// </summary>
	private BlitzMode blitz;
	
	enum CharacterState { Standing=0, Moving=1, Attacking=2, Damage=3, Death=4 };
	
	enum Attack { NormalAttack=0, JumpStamp=1, Stamp=2, Rush=3, Throw=4,
		ThunderAttack1=5, ThunderAttack2=6, ThunderRush=7, BlitzGazor=8,
		FlamePillar=9};

	/// <summary>
	/// 攻撃の選択用
	/// </summary>
	private int atkRand;
	
	enum Mode {Normal, Blitz};

	/// <summary>
	/// モードチェンジ用
	/// </summary>
	private Mode mode;

	private Attack attack;

	/// <summary>
	/// RigidBody
	/// </summary>
	private Rigidbody myRigidbody;

	/// <summary>
	/// モーションの時間を計る
	/// </summary>
	private float atkTimer;

	/// <summary>
	/// 高速移動直前の位置
	/// </summary>
	private Vector3 b_position;

	/// <summary>
	/// ブリッツモード時のエフェクト用
	/// </summary>
	private float blitzTime;

	/// <summary>
	/// ThunderRush用
	/// </summary>
	private int rushCount;

	public GameObject DF_NormalAttack;
	public GameObject DF_Stamp;
	public GameObject Stone;
	public GameObject flamePillar;
	private bool spawnDF;
	private GameObject bossStageCenter;

	private int oldHP;
	private bool damaged;
	private int damageTime;
	private float blinkerTime;
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		blitz = GetComponent<BlitzMode> ();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Standing;
		myRigidbody = GetComponent<Rigidbody>();
		myAnimation = GetComponent<Animation> ();
		player = GameObject.Find("SDUnityChan");
		bossStageCenter = GameObject.Find("BossStageCenter");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
		spawnDF = false;
		oldHP = maxHP;
		damaged = false;
		mode = Mode.Normal;
		if (nowHP < maxHP / 2) { mode = Mode.Blitz; };
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		blitzTime -= Time.deltaTime;
		if (mode == Mode.Blitz && blitzTime <= 0) {
			Instantiate (blitz.spark, transform.position, transform.rotation);
			blitzTime = 0.3f;
		}

		//モードチェンジ
		if (mode == Mode.Normal && nowHP < maxHP / 2) {
			AudioManager.I.PlayAudio("SERoar02");
			AudioManager.I.PlayAudio("SEThunder");
			mode = Mode.Blitz;
			blitz.changeBlitz();
		}

		//状態による行動記述
		switch (nowState) {
		case (int)CharacterState.Standing:
		{
			myRigidbody.isKinematic = true;
			//attackTimer -= 1;
			atkTimer -= Time.deltaTime;
			myAnimation.Play("Idle_1");
			CheckAngle();

			if(atkTimer <= 0)
			{
				spawnDF = false;
				if(mode == Mode.Normal){
					atkRand = Random.Range(0, 6);
					switch(atkRand)
					{
					case 0://通常攻撃
						StartMoving(); break;
					case 1://ジャンプスタンプ
						StartMoving(); break;
					case 2://スタンプ
						StartMoving(); break;
					case 3://突進
						Rush(); break;
					case 4://投げ
						Throw(); break;
					case 5://下から火が出るやつ
						FlamePillar(); break;
					}
				}
				else if(mode == Mode.Blitz){
					atkRand = Random.Range(0, 4);
					switch(atkRand)
					{
					case 0:
						ThunderAttack1(); break;
					case 1:
						ThunderAttack2(); break;
					case 2:
						ThunderRush(); break;
					case 3:
						BlitzGazor(); break;
					}
				}
			}
			break;
		}
		case (int)CharacterState.Moving:
		{
			myRigidbody.isKinematic = true;
			myAnimation.Play("RunCycle");
			CheckAngle();
			transform.position = new Vector3(transform.position.x + (xSpeed / 20) * (float)nowAngle, transform.position.y, transform.position.z);

			switch(atkRand){
			case 0:
			{
				if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 3) {
					CheckAngle();
					NormalAttack();
				}
				break;
			}
			case 1:
			{
				if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 3) {
					CheckAngle();
					Stamp();
				}
				break;
			}
			case 2:
			{
				if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 3) {
					CheckAngle();
					Stamp();
				}
				break;
			}
			}
			break;
		}
		case (int)CharacterState.Attacking:
		{
			switch(attack) {
			case Attack.NormalAttack://通常攻撃
			{
				if(myAnimation["Attack_1"].normalizedTime > 0.35f && spawnDF == false){
					spawnDF = true;
					AudioManager.I.PlayAudio("SERoar01");
					skillGeneratePoint.GenerateSkill(DF_NormalAttack, LayerNames.EnemySkill);
				}

				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			case Attack.Stamp://振り下ろし
			{
				if(myAnimation["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					AudioManager.I.PlayAudio("SERoar01");
					skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
				}

				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			case Attack.JumpStamp://ジャンプ振り下ろし
			{
				if(myAnimation["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
				}
				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			case Attack.Rush://突進
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer < 2.5f){
					myAnimation.Play("RunCycle");
					transform.position = new Vector3(transform.position.x + (xSpeed / 10) * (float)nowAngle, transform.position.y, transform.position.z);
					if (Mathf.Abs (this.transform.position.x - player.transform.position.x) <= 2.5) {
						if(nowHP < maxHP / 2)
							Stamp();
						else
							NormalAttack();
						CheckAngle();
					}
				}
				CheckAngle();
				break;
			}
			case Attack.Throw://投げ
			{
				if(myAnimation["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
					AudioManager.I.PlayAudio("SERoar01");
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(Stone, LayerNames.EnemySkill);
				}
				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			case Attack.FlamePillar://下から炎が出るやつ
			{
				if(myAnimation["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(flamePillar, LayerNames.EnemySkill);
				}

				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			case Attack.ThunderAttack1:
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer <= 0){
					AudioManager.I.PlayAudio("SEThunder");
					transform.position = new Vector3(player.transform.position.x - 2, b_position.y, transform.position.z);
					Instantiate(blitz.spark, transform.position, transform.rotation);
					NormalAttack();
				}
				break;
			}
			case Attack.ThunderAttack2:
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer <= 0){
					AudioManager.I.PlayAudio("SEThunder");
					transform.position = new Vector3(player.transform.position.x + 3, b_position.y, transform.position.z);
					Instantiate(blitz.spark, transform.position, transform.rotation);
					Stamp();
				}
				break;
			}
			case Attack.ThunderRush:
			{
				if(rushCount==0){
					atkTimer -= Time.deltaTime;
					if(atkTimer <= 0){
						AudioManager.I.PlayAudio("SEThunder");
						rushCount++;
						transform.position = new Vector3(player.transform.position.x - 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						myAnimation.Play("Attack_1");
						CheckAngle();
					}
				}
				else if(rushCount == 1){
					if(myAnimation["Attack_1"].normalizedTime > 0.35f && spawnDF == false){
						AudioManager.I.PlayAudio("SERoar01");
						AudioManager.I.PlayAudio("SEThunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_NormalAttack, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						myAnimation.Play("Attack_3");
						CheckAngle();
					}
				}
				else if(rushCount == 2){
					if(myAnimation["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
						AudioManager.I.PlayAudio("SEThunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x - 4, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						myAnimation.Play("Attack_2");
						CheckAngle();
					}
				}
				else if(rushCount == 3){
					if(myAnimation["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
						AudioManager.I.PlayAudio("SERoar01");
						AudioManager.I.PlayAudio("SEThunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(flamePillar, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						myAnimation.Play("Attack_3");
						CheckAngle();
					}
				}
				else if(rushCount == 4){
					if(myAnimation["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
					}
					if(!myAnimation.isPlaying){
						nowState = (int)CharacterState.Standing;
					}
				}
				break;
			}
			case Attack.BlitzGazor:
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer <= 0 && transform.position.y > 15){
					float center = bossStageCenter.transform.position.x;
					if(player.transform.position.x <= center)
						transform.position = new Vector3(center + 4, b_position.y, transform.position.z);
					else
						transform.position = new Vector3(center - 4, b_position.y, transform.position.z);
					myAnimation.Play("Attack_2");
					CheckAngle();
				}

				if(myAnimation["Attack_2"].normalizedTime > 0.45f && myAnimation["Attack_2"].normalizedTime < 0.60f){
					AudioManager.I.PlayAudio("SEThunder");
					skillGeneratePoint.GenerateSkill(blitz.BlitzGazor, LayerNames.EnemySkill);
				}
				if(!myAnimation.isPlaying){
					nowState = (int)CharacterState.Standing;
				}
				break;
			}
			}
			break;
		}
		case (int)CharacterState.Damage:
		{
			break;
		}
		case (int)CharacterState.Death:
		{
			myAnimation.Play("Die");
			break;
		}
		}
		
		if (IsDestroy) {
			myAnimation.Play("Die");
			nowState = (int)CharacterState.Death;
		}

		if (damaged) {
			damageTime -= 1;
			nowHP = oldHP;
			if (damageTime < 0)
				damaged = false;
		} else {
			checkDamage(nowHP);
		}
	}

	int checkDirection (){
		return (int)Mathf.Abs (this.transform.position.x - player.transform.position.x);
	}

	/// <summary>
	/// ミノタウロスをプレイヤーの方へ向ける	
	/// </summary>
	public void CheckAngle (){
		if (player.transform.position.x > transform.position.x) {
			nowAngle = CharacterAngle.Right;
			transform.rotation = Quaternion.Euler(0, 90, 0);
		} else {
			nowAngle = CharacterAngle.Left;
			transform.rotation = Quaternion.Euler(0, 270, 0);
		}
	}

	/// <summary>
	/// 移動開始
	/// </summary>
	public void StartMoving(){
		nowState = (int)CharacterState.Moving;
		myAnimation.Play("RunCycle");
		CheckAngle();
	}

	/// <summary>
	/// 攻撃開始時の処理
	/// </summary>
	public void StartAttack(){
		nowState = (int)CharacterState.Attacking;
		CheckAngle ();
	}

	/// <summary>
	/// フック
	/// </summary>
	public void NormalAttack(){
		StartAttack ();
		atkTimer = 2.0f;
		attack = Attack.NormalAttack;
		myAnimation.Play ("Attack_1");
	}

	/// <summary>
	/// 振り下ろし
	/// </summary>
	public void Stamp(){
		StartAttack ();
		atkTimer = 2.0f;
		attack = Attack.Stamp;
		myAnimation.Play("Attack_3");
	}
	
	public void JumpStamp(){
		myRigidbody.isKinematic = false;
		StartAttack ();
		attack = Attack.JumpStamp;
		myRigidbody.AddForce(Vector3.up*8, ForceMode.VelocityChange);
		myRigidbody.AddForce(Vector3.right*3*(int)nowAngle, ForceMode.VelocityChange);

		myAnimation.Play("Attack_3");
	}

	/// <summary>
	/// 突進
	/// </summary>
	public void Rush(){
		StartAttack ();
		atkTimer = 5.0f;
		attack = Attack.Rush;
		myAnimation.Play("Idle_2");
	}

	/// <summary>
	/// 岩投げ
	/// </summary>
	public void Throw(){
		StartAttack ();
		atkTimer = 2.0f;
		attack = Attack.Throw;
		myAnimation.Play("Attack_2");
	}

	//下から炎が出るやつ
	public void FlamePillar(){
		StartAttack ();
		atkTimer = 3.0f;
		attack = Attack.FlamePillar;
		myAnimation.Play("Attack_2");
	}

	/// <summary>
	/// 高速移動→フック
	/// </summary>
	public void ThunderAttack1(){
		StartAttack ();
		atkTimer = 3.0f;
		Instantiate (blitz.moveEffect, transform.position, transform.rotation);
		b_position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Instantiate (blitz.spark, transform.position, transform.rotation);
		transform.position = new Vector3 (transform.position.x, 20.0f, transform.position.z);
		attack = Attack.ThunderAttack1;
	}

	public void ThunderAttack2(){
		StartAttack ();
		atkTimer = 3.0f;
		Instantiate (blitz.moveEffect, transform.position, transform.rotation);
		b_position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Instantiate (blitz.spark, transform.position, transform.rotation);
		transform.position = new Vector3 (transform.position.x, 20.0f, transform.position.z);
		attack = Attack.ThunderAttack2;
	}

	public void ThunderRush(){
		rushCount = 0;
		StartAttack ();
		atkTimer = 3.0f;
		Instantiate (blitz.moveEffect, transform.position, transform.rotation);
		b_position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		transform.position = new Vector3 (transform.position.x, 20.0f, transform.position.z);
		attack = Attack.ThunderRush;
	}

	public void BlitzGazor(){
		StartAttack ();
		atkTimer = 3.0f;
		Instantiate (blitz.moveEffect, transform.position, transform.rotation);
		b_position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Instantiate (blitz.spark, transform.position, transform.rotation);
		transform.position = new Vector3 (transform.position.x, 20.0f, transform.position.z);
		attack = Attack.BlitzGazor;
	}

	/// <summary>
	/// アニメーションの速度変更	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="s">S.</param>
	public void ChangeAnimationSpeed(string a, float s){
		myAnimation [a].speed = s;
	}

	/// <summary>
	/// ダメージを受けたか見る	/// </summary>
	/// <param name="hp">Hp.</param>
	public void checkDamage(int hp){
		if (oldHP > hp) {
			damaged = true;
			damageTime = 10;
			oldHP = nowHP;
		}
	}
	
	/// <summary>
	/// 衝突している間ずっと呼ばれるメソッド
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionStay(Collision collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.PlayerSkill)
		{
			if (mutekiTime < 0)
			{
				DamageMethod(collision.gameObject.GetComponent<SkillParam>().damege);
			}
		}
	}

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public override void GameEnd()
	{
		enabled = false;
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
		if (IsDestroy)
		{
			myAnimation.Play("Die");
			nowState = (int)CharacterState.Death;
		}
	}
}
