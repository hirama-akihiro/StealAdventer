using UnityEngine;
using System.Collections;

public class RespawnManager : SingletonMonoBehavior<RespawnManager>{

	/// <summary>
	/// ユーザが操作するプレイヤーオブジェクト
	/// </summary>
	public GameObject playerObject;

	/// <summary>
	/// 死んだ座標
	/// </summary>
	private Vector3 deadPosition;

	/// <summary>
	/// リスポーン座標
	/// </summary>
	public Vector3 respawnPosition;

	public GameObject respawnEffect;

	/// <summary>
	/// リスポーン中かどうか
	/// </summary>
	private bool isRespawning;

	private float respawnTime = 1.0f;

	private float startTime;

	private Minotaur minotaur;

	// Use this for initialization
	void Start () {
		minotaur = GameObject.Find("Minotaur").GetComponent<Minotaur>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isRespawning) { return; }

		float diff = Time.timeSinceLevelLoad - startTime;
		float rate = diff / respawnTime;
		playerObject.transform.position = Vector3.Lerp(deadPosition, respawnPosition, rate);
		if(diff > respawnTime)
		{
			isRespawning = false;

			// リスポーンエフェクト
			if (respawnEffect) Instantiate(respawnEffect, respawnPosition, transform.rotation);

			playerObject.transform.position = respawnPosition;
			playerObject.GetComponent<CapsuleCollider>().isTrigger = false;
			playerObject.GetComponent<UnityChanController>().IsWorping = false;
			playerObject.GetComponent<UnityChanController>().SetRendererEnable(true);
			playerObject.GetComponent<Rigidbody>().useGravity = true;
			playerObject.GetComponent<UnityChanController>().MutekiTime = -1;
			playerObject.GetComponent<UnityChanController>().FullHeal();
			playerObject.GetComponent<UnityChanController>().isMovable = true;
			playerObject.GetComponent<Rigidbody>().isKinematic = false;
			playerObject.GetComponent<CapsuleCollider>().isTrigger = false;
			AudioManager.I.PlayAudio("VoiceRespawn");
		}
	}

	/// <summary>
	/// プレイヤーオブジェクトをリスポン
	/// </summary>
	public IEnumerator Respawn()
	{
		playerObject.GetComponent<Rigidbody>().isKinematic = true;
		playerObject.GetComponent<CapsuleCollider>().isTrigger = true;
		yield return new WaitForSeconds(3f);
		isRespawning = true;
		deadPosition = playerObject.transform.position;
		startTime = Time.timeSinceLevelLoad;
		playerObject.GetComponent<UnityChanController>().IsWorping = true;
		playerObject.GetComponent<UnityChanController>().SetRendererEnable(false);
		playerObject.GetComponent<Rigidbody>().useGravity = false;
		playerObject.GetComponent<UnityChanController>().SetBool("GameOver", false);
		TargetCamera.I.isCompliance = true;
		minotaur.enabled = false;
	}
}
