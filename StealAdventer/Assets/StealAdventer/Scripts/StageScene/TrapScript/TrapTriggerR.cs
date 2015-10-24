using UnityEngine;
using System.Collections;

public class TrapTriggerR : MonoBehaviour {
	
	public GameObject[] rightfloors;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		foreach (GameObject rightfloor in rightfloors) {
			rightfloor.GetComponent<rightRotateScript>().IsTrapOn = true;
		}
	}
}
