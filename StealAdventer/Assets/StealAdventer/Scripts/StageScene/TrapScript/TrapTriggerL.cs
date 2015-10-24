using UnityEngine;
using System.Collections;

public class TrapTriggerL : MonoBehaviour {

	leftRotateScript left;
	
	/// <summary>
	/// 左側フロアー
	/// </summary>
	public GameObject[] leftFloors;
	
	// Use this for initialization
	void Start () {
		left = gameObject.AddComponent<leftRotateScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		foreach (GameObject leftfloor in leftFloors) {
			leftfloor.GetComponent<leftRotateScript>().IsTrapOn = true;
		}
	}
}
