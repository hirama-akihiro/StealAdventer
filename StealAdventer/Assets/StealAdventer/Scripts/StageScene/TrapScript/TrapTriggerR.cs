using UnityEngine;
using System.Collections;

public class TrapTriggerR : MonoBehaviour {
	
	rightRotateScript right;
	
	public GameObject[] rightfloors;
	
	// Use this for initialization
	void Start () {
		right = new rightRotateScript ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		foreach (GameObject rightfloor in rightfloors) {
			rightfloor.GetComponent<rightRotateScript>().IsTrapOn = true;
		}
		
		//Debug.Log ("true");
		//left.Rotate ();
		//right.Rotate ();
	}
}
