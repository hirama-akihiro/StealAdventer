using UnityEngine;
using System.Collections;

/// <summary>
/// スティールハンドクラスのスクリプト
/// </summary>
public class StealHandScript : MonoBehaviour {
	/// <summary>
	/// Stealしたスキルオブジェクト
	/// </summary>
	public GameObject stealSkillObject;

	/// <summary>
	/// Stealしたスキルを描画するオブジェクト
	/// </summary>
	public GameObject stealSkillDrawObject;

	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	public GameObject playerObject;

	/// <summary>
	/// 生成時の座標
	/// </summary>
	private Vector3 startPos;

	/// <summary>
	/// 到達目標座標
	/// </summary>
	private Vector3 endPos;

	/// <summary>
	/// 生成時の時間
	/// </summary>
	private float startTime;

	/// <summary>
	/// ハンド生成時間(往復移動時間)
	/// </summary>
	public float moveTime;

	/// <summary>
	/// 移動速度
	/// </summary>
	public int speed;

	/// <summary>
	/// 移動可能範囲
	/// </summary>
	public int movableRange;

	/// <summary>
	/// ハンドの向き
	/// </summary>
	private int sign;

	/// <summary>
	/// スキルを奪っているかどうか
	/// </summary>
	private bool isStealSkill;

	/// <summary>
	/// 前進中か
	/// </summary>
	private bool isAdvance;

    /// <summary>
	/// ステータスなどのデータ表示用キャンバス
	/// </summary>
    private GameObject dataCanvas;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		startPos = transform.position;
		sign = transform.localEulerAngles.y > 180 ? -1 : 1;	// 左向き:-1,右向き:1
		endPos = startPos + sign * new Vector3(movableRange, 0, 0);
		isStealSkill = false;
		isAdvance = true;
        dataCanvas = GameObject.Find("DataCanvas");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (endPos - startPos).normalized * speed * Time.deltaTime;
		if (Vector3.Distance(transform.position, endPos) < 0.2f && isAdvance && !isStealSkill)
		{
			SwapVector3(ref startPos, ref endPos);
			isAdvance = false;
		}
		//transform.position = new Vector3(sign * PingPong(speed, surviveTime, movableRange) + startPos.x, startPos.y, startPos.z);
		

		// 生存時間経過で自身を削除
		if (IsDestory)
		{
			playerObject = GameObject.Find("SDUnityChan");
			// スキルを奪えなかった場合，特に上書きとかしない
			if (stealSkillObject != null) {
                playerObject.GetComponent<CharacterStatus>().skillObject = stealSkillObject;
                dataCanvas.transform.FindChild("Skill").GetComponent<SkillImageScript>().ChangeSkillImage();
				playerObject.SendMessage("InstansiateGetSkillEffect");
				AudioManager.Instance.PlayAudio("SkillGet");
            }
			playerObject.GetComponent<UnityChanController>().IsControllable = true;
			playerObject.GetComponent<UnityChanController>().isMovable = true;
			Destroy(gameObject);
		}
	}

	private void SwapVector3(ref Vector3 left, ref Vector3 right)
	{
		Vector3 tmp = left;
		left = right;
		right = tmp;
	}

	/// <summary>
	/// 自身の反復運動処理
	/// </summary>
	/// <param name="moveSpeed"></param>
	/// <param name="moveTime"></param>
	/// <param name="range"></param>4	
	/// <returns></returns>
	private float PingPong(float moveSpeed, float moveTime, float range)
	{
		return Mathf.PingPong(ElapsedTime * Mathf.Abs(moveSpeed), Mathf.Abs(range));
	}

	/// <summary>
	/// 生成時からの経過時間
	/// </summary>
	/// <returns></returns>
	private float ElapsedTime { get { return Time.time - startTime; } }

	/// <summary>
	/// 死んでいるか
	/// </summary>
	/// <returns></returns>
	private bool IsDestory { get { return !isAdvance && Vector3.Distance(transform.position, endPos) < 0.2f; } }

	/// <summary>
	/// 衝突時に1回だけ呼ばれるメソッド
	/// </summary>
	/// <param name="collider"></param>
	private void OnTriggerEnter(Collider collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		// スキルを奪取しているなら何もしない
		if (isStealSkill) { return; }

		stealSkillObject = collider.GetComponent<CharacterStatus>().skillObject;
		if (stealSkillObject == null) { return; }

		stealSkillDrawObject = Instantiate(stealSkillObject.GetComponent<SkillObjectScript>().DisplaySkillObject);
		stealSkillDrawObject.transform.position = transform.position;
		stealSkillDrawObject.transform.parent = this.gameObject.transform;
		stealSkillDrawObject.layer = LayerMask.NameToLayer("NoCollision");
		// 奪ったスキルの当たり判定を無効
		var skillRigidBody = stealSkillDrawObject.GetComponent<Rigidbody>();
		if (skillRigidBody) { skillRigidBody.useGravity = false; }
		var spheaCollider = stealSkillDrawObject.GetComponent<SphereCollider>();
		if (spheaCollider) { spheaCollider.isTrigger = true; }
		var boxCollider = stealSkillDrawObject.GetComponent<BoxCollider>();
		if (boxCollider) { boxCollider.isTrigger = true; }

		endPos = transform.position;
		SwapVector3(ref startPos, ref endPos);
		isAdvance = false;
		isStealSkill = true;
	}
}
