using UnityEngine;
using System.Collections;

public class NormalHandUpper : MonoBehaviour {

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

	/// <summary>
	/// 現在の角度
	/// </summary>
	private float nowRotate;

	private Vector3 offset = new Vector3(0.7f, 0, 0.7f);

	
	/// <summary>
	/// プレイヤーオブジェクト
	/// </summary>
	public GameObject playerObject;

	// Use this for initialization
	void Start () {
		nowRotate = 90.0f;
		startTime = Time.time;
		playerObject = GameObject.Find("SDUnityChan");
	}
	
	// Update is called once per frame
	void Update()
	{
		nowRotate += rotateSpeed * Time.deltaTime;
		int sign = (int)playerObject.GetComponent<UnityChanController>().status.NowAngle;
		transform.position = playerObject.transform.position + sign * offset + new Vector3(sign * 0.4f * Mathf.Sin(nowRotate * Mathf.Deg2Rad), -2 * Mathf.Cos(nowRotate * Mathf.Deg2Rad), 0);
		transform.Rotate(0, rotateSpeed * Time.deltaTime * 0.5f, 0);

		if (IsDestory())
		{
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
