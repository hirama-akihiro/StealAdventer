using UnityEngine;
using System.Collections;

public class NormalHandHook : MonoBehaviour {

	/// <summary>
	/// 生成時の時間
	/// </summary>
	private float startTime;

	/// <summary>
	/// ハンド生成時間(往復移動時間)
	/// </summary>
	public float surviveTime;

	/// <summary>
	/// 移動速度
	/// </summary>
	public float rotateSpeed;

	private float nowRotate;

	private Vector3 startPos;
	
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	public GameObject playerObject;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		startPos = transform.position;
		nowRotate = 0.0f;
		playerObject = GameObject.Find("SDUnityChan");
		if (playerObject.GetComponent<UnityChanController>().status.NowAngle == CharacterStatus.CharacterAngle.Left)
		{
			transform.Rotate(0, 180, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		nowRotate += rotateSpeed;
		if (playerObject.GetComponent<UnityChanController>().status.NowAngle == CharacterStatus.CharacterAngle.Right)
		{
			transform.position = playerObject.transform.position + new Vector3(Mathf.Sin(nowRotate * Mathf.Deg2Rad), 0, -Mathf.Cos(nowRotate * Mathf.Deg2Rad));
			transform.Rotate(0, rotateSpeed, 0);
		}
		else
		{
			transform.position = playerObject.transform.position - new Vector3(Mathf.Sin(nowRotate * Mathf.Deg2Rad), 0, -Mathf.Cos(nowRotate * Mathf.Deg2Rad));
			transform.Rotate(0, rotateSpeed, 0);
		}
		if (IsDestory())
		{
			playerObject = GameObject.Find("SDUnityChan");
			playerObject.GetComponent<UnityChanController>().IsControllable = true;
			playerObject.GetComponent<UnityChanController>().isMovable = true;
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// 生成時からの経過時間
	/// </summary>
	/// <returns></returns>
	private float ElapsedTime() { return Time.time - startTime; }

	/// <summary>
	/// 死んでいるか
	/// </summary>
	/// <returns></returns>
	private bool IsDestory() { return (Time.time - startTime) > surviveTime; }
}
