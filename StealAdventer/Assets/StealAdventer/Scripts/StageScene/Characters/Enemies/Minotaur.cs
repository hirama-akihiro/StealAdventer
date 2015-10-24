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
	
	private CharacterState nowState;

	private Attack attack;

	/// <summary>
	/// RigidBody
	/// </summary>
	private Rigidbody myRigidbody;

	/// <summary>
	/// Animation
	/// </summary>
	private Animation anim;

	/// <summary>
	/// プレイヤーのオブジェクト
	/// </summary>
	private GameObject player;

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

	/// <summary>
	/// 防御力
	/// </summary>
	private int def;

	private SkillGeneratePointScript skillGeneratePoint;
	public GameObject DF_NormalAttack;
	public GameObject DF_Stamp;
	public GameObject Stone;
	public GameObject flamePillar;
	private bool spawnDF;
	private GameObject leftWall;
	private GameObject rightWall;
	private GameObject bossStageCenter;

	private int oldHP;
	private bool damaged;
	private int damageTime;

	public float blinkerInterval;
	private float blinkerTime;
	public float mutekiTime;
	
	// Use this for initialization
	void Awake () {
		blitz = GetComponent<BlitzMode> ();
		nowAngle = CharacterAngle.Right;
		nowState = CharacterState.Standing;
		myRigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animation> ();
		player = GameObject.Find("SDUnityChan");
		leftWall = GameObject.Find("BossStage/MovableArea/LeftWall");
		rightWall = GameObject.Find("BossStage/MovableArea/RightWall");
		bossStageCenter = GameObject.Find("BossStageCenter");
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
		spawnDF = false;
		oldHP = maxHP;
		damaged = false;
		mode = Mode.Normal;
		if(nowHP < maxHP / 2)
			mode = Mode.Blitz;
		def = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (player.transform.position.x);

		// 移動制限
		//if (transform.position.x > 0) { transform.position = Vector3.zero; }

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

		blitzTime -= Time.deltaTime;
		if (mode == Mode.Blitz && blitzTime <= 0) {
			Instantiate (blitz.spark, transform.position, transform.rotation);
			blitzTime = 0.3f;
		}

		//モードチェンジ
		if (mode == Mode.Normal && nowHP < maxHP / 2) {
			AudioManager.Instance.PlayAudio("roar02");
			AudioManager.Instance.PlayAudio("thunder");
			mode = Mode.Blitz;
			blitz.changeBlitz();
			def=0;
		}


		//状態による行動記述
		switch (nowState) {
		case CharacterState.Standing:
		{
			myRigidbody.isKinematic = true;
			//attackTimer -= 1;
			atkTimer -= Time.deltaTime;
			anim.Play("Idle_1");
			CheckAngle();

			if(atkTimer <= 0)
			{
				spawnDF = false;
				if(mode == Mode.Normal){
					atkRand = Random.Range(0, 6);
					/*if (Mathf.Abs (this.transform.position.x - player.transform.position.x) >= 5.5){
						atkRand = 3;
					}*/

					//atkRand = 4;

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
					//atkRand = 3;
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
		case CharacterState.Moving:
		{
			myRigidbody.isKinematic = true;
			anim.Play("RunCycle");
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
		case CharacterState.Attacking:
		{
			switch(attack) {
			case Attack.NormalAttack://通常攻撃
			{
				if(anim["Attack_1"].normalizedTime > 0.35f && spawnDF == false){
					spawnDF = true;
					AudioManager.Instance.PlayAudio("roar01");
					skillGeneratePoint.GenerateSkill(DF_NormalAttack, LayerNames.EnemySkill);
				}

				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			case Attack.Stamp://振り下ろし
			{
				if(anim["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					AudioManager.Instance.PlayAudio("roar01");
					skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
				}

				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			case Attack.JumpStamp://ジャンプ振り下ろし
			{
				if(anim["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
				}
				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			case Attack.Rush://突進
			{
				//attackTimer -= 1;
				atkTimer -= Time.deltaTime;
				if(atkTimer < 2.5f){
					anim.Play("RunCycle");
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
				if(anim["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
					AudioManager.Instance.PlayAudio("roar01");
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(Stone, LayerNames.EnemySkill);
				}
				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			case Attack.FlamePillar://下から炎が出るやつ
			{
				if(anim["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
					spawnDF = true;
					skillGeneratePoint.GenerateSkill(flamePillar, LayerNames.EnemySkill);
				}

				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			case Attack.ThunderAttack1:
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer <= 0){
					AudioManager.Instance.PlayAudio("thunder");
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
					AudioManager.Instance.PlayAudio("thunder");
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
						AudioManager.Instance.PlayAudio("thunder");
						rushCount++;
						transform.position = new Vector3(player.transform.position.x - 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_1");
						CheckAngle();
					}
				}
				else if(rushCount == 1){
					if(anim["Attack_1"].normalizedTime > 0.35f && spawnDF == false){
						AudioManager.Instance.PlayAudio("roar01");
						AudioManager.Instance.PlayAudio("thunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_NormalAttack, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_3");
						CheckAngle();
					}
					/*if(!anim.isPlaying){
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 1, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_3");
						CheckAngle();
					}*/
				}
				else if(rushCount == 2){
					if(anim["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
						AudioManager.Instance.PlayAudio("thunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x - 4, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_2");
						CheckAngle();
					}
					/*if(!anim.isPlaying){
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x - 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_2");
						CheckAngle();
					}*/
				}
				else if(rushCount == 3){
					if(anim["Attack_2"].normalizedTime > 0.45f && spawnDF == false){
						AudioManager.Instance.PlayAudio("roar01");
						AudioManager.Instance.PlayAudio("thunder");
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(flamePillar, LayerNames.EnemySkill);
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 2, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_3");
						CheckAngle();
					}
					/*if(!anim.isPlaying){
						rushCount++;
						spawnDF = false;
						Instantiate(blitz.spark, transform.position, transform.rotation);
						transform.position = new Vector3(player.transform.position.x + 1, b_position.y, transform.position.z);
						Instantiate(blitz.spark, transform.position, transform.rotation);
						anim.Play("Attack_3");
						CheckAngle();
					}*/
				}
				else if(rushCount == 4){
					if(anim["Attack_3"].normalizedTime > 0.45f && spawnDF == false){
						spawnDF = true;
						skillGeneratePoint.GenerateSkill(DF_Stamp, LayerNames.EnemySkill);
					}
					if(!anim.isPlaying){
						nowState = CharacterState.Standing;
					}
				}
				break;
			}
			case Attack.BlitzGazor:
			{
				atkTimer -= Time.deltaTime;
				if(atkTimer <= 0 && transform.position.y > 15){
					//float center = (rightWall.transform.position.x - leftWall.transform.position.x)/2 + leftWall.transform.position.x;
					float center = bossStageCenter.transform.position.x;
					if(player.transform.position.x <= center)
						transform.position = new Vector3(center + 4, b_position.y, transform.position.z);
					else
						transform.position = new Vector3(center - 4, b_position.y, transform.position.z);
					//transform.position = new Vector3(player.transform.position.x - 2, b_position.y, transform.position.z);
					//Instantiate(blitz.spark, transform.position, transform.rotation);
					anim.Play("Attack_2");
					CheckAngle();
				}

				if(anim["Attack_2"].normalizedTime > 0.45f && anim["Attack_2"].normalizedTime < 0.60f){
					AudioManager.Instance.PlayAudio("thunder");
					//spawnDF = true;
					skillGeneratePoint.GenerateSkill(blitz.BlitzGazor, LayerNames.EnemySkill);
				}
				if(!anim.isPlaying){
					nowState = CharacterState.Standing;
				}
				break;
			}
			}
			break;
		}
		case CharacterState.Damage:
		{
			break;
		}
		case CharacterState.Death:
		{
			anim.Play("Die");
			break;
		}
		}
		
		if (IsDestroy) {
			anim.Play("Die");
			nowState = CharacterState.Death;
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
		nowState = CharacterState.Moving;
		anim.Play("RunCycle");
		CheckAngle();
		//attackTimer = 100;
	}

	/// <summary>
	/// 攻撃開始時の処理
	/// </summary>
	public void StartAttack(){
		nowState = CharacterState.Attacking;
		CheckAngle ();
	}

	/// <summary>
	/// フック
	/// </summary>
	public void NormalAttack(){
		StartAttack ();
		//attackTimer = 100;
		atkTimer = 2.0f;
		attack = Attack.NormalAttack;
		anim.Play ("Attack_1");
	}

	/// <summary>
	/// 振り下ろし
	/// </summary>
	public void Stamp(){
		StartAttack ();
		//attackTimer = 99;
		atkTimer = 2.0f;
		attack = Attack.Stamp;
		anim.Play("Attack_3");
	}
	
	public void JumpStamp(){
		myRigidbody.isKinematic = false;
		StartAttack ();
		//attackTimer = 100;
		attack = Attack.JumpStamp;
		myRigidbody.AddForce(Vector3.up*8, ForceMode.VelocityChange);
		myRigidbody.AddForce(Vector3.right*3*(int)nowAngle, ForceMode.VelocityChange);

		anim.Play("Attack_3");
	}

	/// <summary>
	/// 突進
	/// </summary>
	public void Rush(){
		StartAttack ();
		//attackTimer = 200;
		atkTimer = 5.0f;
		attack = Attack.Rush;
		anim.Play("Idle_2");
	}

	/// <summary>
	/// 岩投げ
	/// </summary>
	public void Throw(){
		StartAttack ();
		//attackTimer = 100;
		atkTimer = 2.0f;
		attack = Attack.Throw;
		anim.Play("Attack_2");
	}

	//下から炎が出るやつ
	public void FlamePillar(){
		StartAttack ();
		atkTimer = 3.0f;
		attack = Attack.FlamePillar;
		anim.Play("Attack_2");
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
		anim [a].speed = s;
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
	/// 衝突時に一回だけ呼ばれるメソッド
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggernEnter(Collider collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.PlayerSkill)
		{
			myRigidbody.isKinematic = true;
			if (mutekiTime < 0)
			{
				nowHP -= collision.gameObject.GetComponent<SkillParam>().damege - def;
				mutekiTime = 1; // 現状無敵時間を2秒にしている：長いかも
			}
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
				nowHP -= collision.gameObject.GetComponent<SkillParam>().damege - def;
				mutekiTime = 1; // 現状無敵時間を2秒にしている：長いかも
			}
		}
	}

	private void OnTriggerStay(Collider collision)
	{
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (layerName == LayerNames.PlayerSkill)
        {
            if (mutekiTime < 0)
            {
                nowHP -= collision.gameObject.GetComponent<SkillParam>().damege - def;
                mutekiTime = 1; // 現状無敵時間を2秒にしている：長いかも
            }
        }
	}

	/// <summary>
	/// ゲーム終了時のメソッド
	/// </summary>
	public override void GameEnd()
	{
		enabled = false;
		CapsuleCollider cCollider;
		cCollider = GetComponent<CapsuleCollider>();
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
		if (IsDestroy)
		{
			anim.Play("Die");
			nowState = CharacterState.Death;
		}
	}
}
