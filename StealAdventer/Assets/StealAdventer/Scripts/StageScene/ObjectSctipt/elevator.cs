using UnityEngine;
using System.Collections;

public class elevator : MonoBehaviour {

    private Vector3 pos;
	public float speed;
	private GameObject player;
	// Use this for initialization
	void Start () {
        pos = transform.position;
		player = GameObject.Find ("SDUnityChan");
	}
	
	// Update is called once per frame
	void Update () {
        /*var speed = 4f;			// 往復速度(目安)
        var movableRange = 22f;	// 移動可能距離
        var time = 10f;			// 往復時間
        transform.position = new Vector3(pos.x, PingPong(speed, time, movableRange) + pos.y, pos.z);*/
		if (transform.position.y >= 11f) {
			speed *= -1.0f;
		}
		if (transform.position.y <= -11f) {
			speed *= -1.0f;
		}
		transform.position = new Vector3(transform.position.x,transform.position.y + speed,transform.position.z);
	}

    /*private float PingPong(float moveSpeed, float moveTime, float range)
    {
        return Mathf.PingPong(Time.time * Mathf.Abs(moveSpeed), Mathf.Abs(range));
    }*/

	private void OnCollisionStay(Collision c)
	{
		if (LayerMask.LayerToName (c.gameObject.layer) == LayerNames.Player) {
			Debug.Log (gameObject.name);
			c.transform.position = new Vector3(c.transform.position.x,c.transform.position.y,c.transform.position.z);
			c.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
		}
	}

	private void OnCollisionEnter(Collision c)
	{
		
	}
}
