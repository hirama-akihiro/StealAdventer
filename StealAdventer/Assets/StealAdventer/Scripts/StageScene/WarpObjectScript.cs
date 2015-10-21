using UnityEngine;
using System.Collections;

public class WarpObjectScript : MonoBehaviour {

	/// <summary>
	/// 移動先の座標オブジェクト
	/// </summary>
	public GameObject terminalPoint;

	/// <summary>
	/// ワープの移動先座標
	/// </summary>
	private Vector3 movePos;

	/// <summary>
	/// 移動開始時間
	/// </summary>
	private float moveStartTime;

	/// <summary>
	/// 移動に要する時間
	/// </summary>
	public float moveTime;

	/// <summary>
	/// プレイヤーのゲームオブジェクト
	/// </summary>
	private GameObject player;

	/// <summary>
	/// ワープ中かどうか
	/// </summary>
	private bool isWorping;

	// Use this for initialization
	void Start () {
		isWorping = false;
		movePos = terminalPoint.transform.position;
		player = GameObject.Find ("SDUnityChan");
	}
	
	// Update is called once per frame
	void Update () {
		if (isWorping) 
		{
			float diff = Time.timeSinceLevelLoad - moveStartTime;
			if (diff > moveTime)
			{
				isWorping = false;
				player.GetComponent<CapsuleCollider>().isTrigger = false;
				player.GetComponent<UnityChanController>().IsWorping = false;
				player.GetComponent<Rigidbody>().useGravity = true;
				player.GetComponent<UnityChanController>().SetRendererEnable(true);
				player.transform.position = movePos;
			}
			float rate = diff / moveTime;
			player.transform.position = Vector3.Lerp(transform.position, movePos, rate);
		}
	}

	void OnTriggerEnter(Collider other) {
		if ((LayerMask.LayerToName (other.gameObject.layer) == LayerNames.Player || LayerMask.LayerToName(other.gameObject.layer) == LayerNames.MutekiPlayer) && !player.GetComponent<UnityChanController>().IsWorping) {
			moveStartTime = Time.timeSinceLevelLoad;
			isWorping = true;
			player.GetComponent<CapsuleCollider>().isTrigger = true;
			player.GetComponent<Rigidbody>().useGravity = false;
			player.GetComponent<UnityChanController>().IsWorping = true;
			player.GetComponent<UnityChanController>().SetRendererEnable(false);
		}
	}
}
