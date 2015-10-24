using UnityEngine;
using System.Collections;

public class TrapTriggerL : MonoBehaviour {

	leftRotateScript left;
	
	public GameObject[] leftfloors;
	
	// Use this for initialization
	void Start () {
		left = new leftRotateScript ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		foreach (GameObject leftfloor in leftfloors) {
			leftfloor.GetComponent<leftRotateScript>().IsTrapOn = true;
		}
		
		//Debug.Log ("true");
		//left.Rotate ();
		//right.Rotate ();
	}
}
