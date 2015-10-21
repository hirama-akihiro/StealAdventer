using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Vector3 velocity = myRigidbody.velocity;
			myRigidbody.velocity = new Vector3(-4, velocity.y, velocity.z);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Vector3 velocity = myRigidbody.velocity;
			myRigidbody.velocity = new Vector3(4, velocity.y, velocity.z);
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			Vector3 velocity = myRigidbody.velocity;
			myRigidbody.velocity = new Vector3(velocity.x, 4, velocity.z);
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			Vector3 velocity = myRigidbody.velocity;
			myRigidbody.velocity = new Vector3(velocity.x, -4, velocity.z);
		}
	}
}
