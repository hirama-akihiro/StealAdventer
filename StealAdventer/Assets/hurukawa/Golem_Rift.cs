using UnityEngine;
using System.Collections;

public class Golem_Rift : MonoBehaviour {

	private float speed;

	private float time;

	private Rigidbody myRigidBody;

	// Use this for initialization
	void Start () {
		time = 3.0f;
		speed = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + (speed / 80), transform.position.y, transform.position.z);
		time -= Time.deltaTime;
		if (time <= 0) {
			speed *= -1;
			time = 3.0f;
		}
	}
}
