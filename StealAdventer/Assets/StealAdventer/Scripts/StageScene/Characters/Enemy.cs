using UnityEngine;
using System.Collections;

public class Enemy : Character {

	#region Field Value
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	protected GameObject player;

	/// <summary>
	/// Animation
	/// </summary>
	protected Animation myAnimation;

	/// <summary>
	/// rigidBody
	/// </summary>
	protected Rigidbody myRigidBody;

	/// <summary>
	/// 死亡時エフェクト
	/// </summary>
	public GameObject deathEffect;

	/// <summary>
	/// ドロップアイテム
	/// </summary>
	public GameObject dropItem;
	#endregion

	// Use this for initialization
	protected override void Start () {
		base.Start();
		myAnimation = GetComponent<Animation>();
		myRigidBody = GetComponent<Rigidbody>();
		player = GameObject.Find("SDUnityChan");
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	private void DamageMethod(int damage)
	{
		if(mutekiTime > 0) { return; }
		if(damage <= 0) { return; }

		nowHP -= damage;
		mutekiTime = 1;
		if(nowHP > 0) { StartCoroutine("MutekiFlashing"); }
		else { mutekiTime = -1; }
	}

	/// <summary>
	/// 無敵時間中に点滅させるコルーチン
	/// </summary>
	/// <returns></returns>
	private IEnumerator MutekiFlashing()
	{
		while (mutekiTime > 0)
		{
			SetRendererEnable(true);
			yield return new WaitForSeconds(blinkerInterval);
			SetRendererEnable(false);
			yield return new WaitForSeconds(blinkerInterval);
			mutekiTime -= blinkerInterval * 2;
		}
		SetRendererEnable(true);
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if (layerName == "ReflectionArea")
		{
			CashedTransform.Rotate(0, 180, 0);
			if (nowAngle == CharacterAngle.Left)
				nowAngle = CharacterAngle.Right;
			else
				nowAngle = CharacterAngle.Left;
		}
		if (layerName == LayerNames.PlayerSkill)
		{
			DamageMethod(collider.gameObject.GetComponent<SkillParam>().damege);
		}
	}
}
