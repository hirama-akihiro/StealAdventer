using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class UnityChanController : Player {

	/// <summary>
	/// 残機数
	/// </summary>
	public int stock;

	/// <summary>
	/// 残基メージ
	/// </summary>
	public List<GameObject> stockImages;

	/// <summary>
	/// スキル発生地点
	/// </summary>
	private SkillGeneratePointScript skillGeneratePoint;

	/// <summary>
	/// キャラクターの状態
	/// </summary>
	enum CharacterState { Idling = 0, Running = 1, Jumping = 2, NormalAttack = 3, SkillAttack = 4, StealAttack = 5 };

	/// <summary>
	/// アニメーション
	/// </summary>
	private Animator myAnimator;

	/// <summary>
	/// RigidBody
	/// </summary>
	private Rigidbody myRigidbody;

	/// <summary>
	/// ジャンプ中ならtrue
	/// </summary>
	private bool isJump;

	/// <summary>
	/// 移動中ならTrue
	/// </summary>
	private bool isRun;

	/// <summary>
	/// 移動可能かどうか
	/// </summary>
	public bool isMovable;

	/// <summary>
	/// 現フレームスキルのクールタイム
	/// </summary>
	private float nowCoolTime = 0;

	private float nowUpperCoolTime = 0;

	/// <summary>
	/// スキルのクールタイム
	/// </summary>
	private float maxCoolTime = 1;

	/// <summary>
	/// クールタイム表示用のオブジェクト
	/// </summary>
	public GameObject coolTimeRateObject;

	/// <summary>
	/// クールタイム表示用のImage
	/// </summary>
	private Image coolTimeImage;

	/// <summary>
	/// 無敵時間
	/// </summary>
	private float mutekiTime = -1;

	/// <summary>
	/// 点滅間隔
	/// </summary>
	public float blinkerInterval;

	/// <summary>
	/// アッパー用ノーマルハンド
	/// </summary>
	public GameObject normalHandUpper;

	/// <summary>
	/// スキルゲット時のエフェクト
	/// </summary>
	public GameObject skillGetEffect;

	protected override void Awake()
	{
		base.Awake();
		skillGeneratePoint = GetComponent<SkillGeneratePointScript>();
		nowAngle = CharacterAngle.Right;
		nowState = (int)CharacterState.Idling;
		coolTimeImage = coolTimeRateObject.GetComponent<Image>();
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody>();
		isJump = false;
		isRun = false;
		IsControllable = true;
		isMovable = true;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public override void Update () {
		// 基底クラスの呼び出し
		base.Update();

		// 体力が0の時にリスポーンする
		if (nowHP < 0 && mutekiTime < 0)
		{
			stock--;
			ScoreManager.Instance.PlayerDead();
			myAnimator.SetBool("GameOver", true);
			mutekiTime = 1;
			IsWorping = true;
			for (int i = 0; i < stockImages.Count; i++)
			{
				//if (i >= stock) { stockImages[i].GetComponent<Image>().enabled = false; }
				if (i >= stock) 
				{ 
					//stockImages[i].GetComponent<Image>().enabled = false;
					StartCoroutine(stockImages[i].GetComponent<FadeOutScript>().StartFadeOut());
				}
			}
			if (stock == 0) { GameEnder.Instance.isGameOver = true; }
			else
			{
				isMovable = false;
				StartCoroutine(RespawnManager.Instance.Respawn());
			}
		}

		// 操作不能の時は現状から何もしない:出来無い
		if (!IsControllable) { return; }

		// アニメーションチェック
		myAnimator.SetBool("Running", isRun);
		myAnimator.SetBool("Jump", isJump);

		if (nowCoolTime < 0) { nowCoolTime = 0; }
		else { nowCoolTime -= Time.deltaTime; }
		coolTimeImage.fillAmount = 1 - (nowCoolTime / maxCoolTime);
		nowUpperCoolTime -= Time.deltaTime;
	}

	protected override void OnNoMove()
	{
		isRun = false;
		nowState = (int)CharacterState.Idling;
		CharacterMove(0, nowAngle);
	}

	protected override void OnMove()
	{
		if (!isMovable) { return; }

		isRun = true;
		if (UserInput.Instance.UnityChanLeftMove) { nowAngle = CharacterAngle.Left; }
		else { nowAngle = CharacterAngle.Right; }
		CharacterRotateAngle(nowAngle);
		CharacterMove(xSpeed, nowAngle);
		
	}

	/// <summary>
	/// ジャンプ操作時に呼ばれるメソッド
	/// </summary>
	protected override void OnJump()
	{
		if (isJump || !isMovable) { return; }

		nowState = (int)CharacterState.Jumping;
		isJump = true;
		myRigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);
		SEManager.Instance.PlayAudio("JumpVoice");
	}

	/// <summary>
	/// スキル攻撃時に呼ばれるメソッド
	/// </summary>
	protected override void OnSkillAttack()
	{
		if (nowCoolTime > 0 || skillObject == null || !isMovable) { return; }
		if (IsWorping) { return; }

		nowState = (int)CharacterState.SkillAttack;
		skillGeneratePoint.GenerateSkill(skillObject.GetComponent<SkillObjectScript>().AttackSkilObject, LayerNames.PlayerSkill);
		maxCoolTime = skillObject.GetComponent<SkillObjectScript>().AttackSkilObject.GetComponent<SkillParam>().coolTime;
		nowCoolTime = maxCoolTime;
		if (skillObject.GetComponent<SkillObjectScript>().AttackSkilObject.GetComponent<SkillParam>().skillType == SkillParam.SkillType.Long)
		{
			isMovable = false;
			CharacterMove(0, nowAngle);
			AudioManager.Instance.PlayAudio("LongSkillvoice");
		}
		else { AudioManager.Instance.PlayAudio("ShortSkillVoice"); }	
	}

	/// <summary>
	/// スキルボタン話した時に呼ばれるメソッド
	/// </summary>
	protected override void OnSkillAttackEnd()
	{
		isMovable = true;
	}

	/// <summary>
	/// スティールハンド時に呼ばれるメソッド
	/// </summary>
	protected override void OnStealHand()
	{
		if (isJump || !isMovable) { return; }

		nowState = (int)CharacterState.StealAttack;
		skillGeneratePoint.GenerateStealHand();
		CharacterMove(0, nowAngle);
		IsControllable = false;
		isMovable = false;
		isRun = false;
		AudioManager.Instance.PlayAudio("ShortSkillVoice");		
	}

	/// <summary>
	/// アッパーハンド攻撃時に呼ばれるメソ度
	/// </summary>
	protected override void OnUpperHand()
	{
		if (nowUpperCoolTime > 0) { return; }

		nowState = (int)CharacterState.NormalAttack;
		GameObject upperPos = GameObject.Find("UpperPoint");
		GameObject hand = Instantiate(normalHandUpper) as GameObject;
		if (nowAngle == CharacterAngle.Left)
		{
			//hand.transform.position += new Vector3(0, 0, -1);
			hand.transform.Rotate(180, 0, 0);

		}
		IsControllable = false;
		isMovable = false;
		nowUpperCoolTime = normalHandUpper.GetComponent<SkillParam>().coolTime;
		AudioManager.Instance.PlayAudio("NormalAttackVoice");
	}

	/// <summary>
	/// 衝突時に一回だけ呼ばれるメソッド
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.EnemySkill)
		{
			DamegeMethod(collision.gameObject.GetComponent<SkillParam>().damege);
		}
		else if (layerName == LayerNames.Stage_TrapObject)
		{
			DamegeMethod(collision.gameObject.GetComponent<SkillParam>().damege);
		}
		else if(layerName == LayerNames.BossEnemy || layerName == LayerNames.NormalEnemy)
		{
			DamegeMethod(collision.gameObject.GetComponent<Character>().contactDamage);
		}

	}

	/// <summary>
	/// 衝突している間ずっと呼ばれるメソッド
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionStay(Collision collision)
	{
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.Stage_FloorObject)
		{
			// 静止状態なら移動速度を止める(横方向のみ)
			if (nowState == (int)CharacterState.Idling)
			{
				myRigidbody.velocity = new Vector3(0, myRigidbody.velocity.y, myRigidbody.velocity.z);
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if (layerName == LayerNames.EnemySkill)
		{
			DamegeMethod(collider.gameObject.GetComponent<SkillParam>().damege);
		}
	}

	/// <summary>
	/// ダメージ処理を行うメソッド
	/// </summary>
	/// <param name="damege"></param>
	private void DamegeMethod(int damege)
	{
		if (mutekiTime > 0) { return; }
		if (damege <= 0) { return; }

		myRigidbody.isKinematic = true;
		nowHP -= damege;
		ScoreManager.Instance.AddDamagedScore(damege);
		mutekiTime = 1;
		AudioManager.Instance.PlayAudio("DamageVoice");
		myRigidbody.isKinematic = false;
		if (nowHP > 0) { StartCoroutine("MutekiFlashing"); }
		else { mutekiTime = -1; }
	}

	/// <summary>
	/// キャラクターの向きをAngleで決定
	/// </summary>
	/// <param name="angle"></param>
	private void CharacterRotateAngle(CharacterAngle angle)
	{
		switch(angle)
		{
			case CharacterAngle.Left:
				transform.rotation = Quaternion.Euler(0, 270, 0);
				break;
			case CharacterAngle.Right:
				transform.rotation = Quaternion.Euler(0, 90, 0);
				break;
		}
	}

	/// <summary>
	/// キャラクターを向きにxSpeedで移動させる
	/// </summary>
	/// <param name="xSpeed"></param>
	/// <param name="angle"></param>
	private void CharacterMove(float xSpeed, CharacterAngle angle)
	{
		Vector3 velocity = myRigidbody.velocity;
		myRigidbody.velocity = new Vector3((int)NowAngle * xSpeed, velocity.y, velocity.z);
	}

	/// <summary>
	/// 無敵時間中に点滅させるコルーチン
	/// </summary>
	/// <returns></returns>
	private IEnumerator MutekiFlashing()
	{
		gameObject.layer = LayerMask.NameToLayer(LayerNames.MutekiPlayer);
		while (mutekiTime > 0)
		{
			SetRendererEnable(true);
			yield return new WaitForSeconds(blinkerInterval);
			SetRendererEnable(false);
			yield return new WaitForSeconds(blinkerInterval);
			mutekiTime -= blinkerInterval * 2;
		}

		SetRendererEnable(true);
		gameObject.layer = LayerMask.NameToLayer(LayerNames.Player);
	}

	/// <summary>
	/// キャラクターのレンダラーアルファ値を設定するメソッド
	/// </summary>
	/// <param name="alpha"></param>
	public void SetRendererEnable(bool enable)
	{
		Renderer[] objList = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in objList)
		{
			renderer.enabled = enable;
		}
	}

	public void InstansiateGetSkillEffect()
	{
		if (skillGetEffect)
		{
			GameObject effect = Instantiate(skillGetEffect, transform.position, transform.rotation) as GameObject;
			effect.transform.parent = transform;
		}
	}

	public bool IsRespanable()
	{
		return myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;
	}

	public void FullHeal() { nowHP = maxHP; }

	#region ManagerClassCallMethod

	/// <summary>
	/// プレイヤーが死んでいるかどうか
	/// </summary>
	/// <returns></returns>
	public override bool IsDestroy { get{ return stock <= 0 && nowHP <= 0; } }
	
	/// <summary>
	/// アニメーションを外側から操作する関数
	/// </summary>
	/// <param name="animName"></param>
	/// <param name="tf"></param>
	public void SetBool(string animName, bool tf) { myAnimator.SetBool(animName, tf); }

	/// <summary>
	/// ユーザの操作が可能かどうか
	/// </summary>
	public bool IsControllable { get; set; }

	/// <summary>
	/// ジャンプ可能かどうか
	/// </summary>
	public bool IsJump { get { return isJump; } set { isJump = value; } }

	/// <summary>
	/// ワープ中かどうか
	/// </summary>
	public bool IsWorping { get; set; }

	public float MutekiTime { get { return mutekiTime; } set { mutekiTime = value; } }
	#endregion
}
