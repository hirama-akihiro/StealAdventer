using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour {

	/// <summary>
	/// 爆弾を投げる力
	/// </summary>
	public float power;
	
	/// <summary>
	/// 爆弾投射角度
	/// </summary>
	public float degree;

	public float durationTime;
	private Rigidbody myRigidBody;

	private GameObject spawn;

	private GameObject mino;
	private GameObject player;

	float power_x;
	float power_y;

	public bool IsReflect = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;
		if (durationTime <= 0)
			Destroy (gameObject);
	}

	public void Shot(GameObject spawn)
	{

		player = GameObject.Find("SDUnityChan");
		int angle;
		if(gameObject.transform.position.x <= player.transform.position.x)
			angle = -1;
		else
			angle = 1;
		this.spawn = spawn;
		gameObject.transform.position = this.spawn.transform.position;
		power_x = Mathf.Cos(Mathf.PI / 180.0f * degree) * angle * power;
		power_y = Mathf.Sin(Mathf.PI / 180.0f * degree) * power;
		gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(power_x, power_y, 0));
	}

	private void OnCollisionStay(Collision collision)
	{
		myRigidBody = GetComponent<Rigidbody> ();
		string layerName = LayerMask.LayerToName(collision.gameObject.layer);
		if (layerName == LayerNames.Stage_FloorObject || layerName == LayerNames.Stage_TrapObject)
		{
			gameObject.layer = LayerMask.NameToLayer("Stage(FloorObject)");
			// 地面落下後は移動させない
			myRigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			//rigidBody.isKinematic = true;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("UpperHand") && gameObject.layer == LayerMask.NameToLayer(LayerNames.EnemySkill))
		{
			gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-power_x * 2, power_y * 2, 0));
		}
	}
}
