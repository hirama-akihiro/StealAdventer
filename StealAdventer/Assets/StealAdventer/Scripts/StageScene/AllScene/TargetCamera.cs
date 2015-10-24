using UnityEngine;
using System.Collections;

public class TargetCamera : SingletonMonoBehavior<TargetCamera> {

	/// <summary>
	/// 追従するターゲット
	/// </summary>
	public Transform target;

	/// <summary>
	/// ターゲットからのオフセット
	/// </summary>
	public Vector3 offset;

	/// <summary>
	/// 追従する下限領域
	/// </summary>
	public float underLimit;

	public bool isCompliance;

	// Use this for initialization
	void Start () {
		isCompliance = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isCompliance) { return; }
		if (!target) { return; }
		Vector3 targetCameraPos = target.position + offset;
		transform.position = targetCameraPos;
	}
}
