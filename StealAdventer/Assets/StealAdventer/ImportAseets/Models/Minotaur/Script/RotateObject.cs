using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	private Transform thisTransform;
	void Start () {
		thisTransform = transform;
	}

	void Update () {
		if(Input.GetMouseButton(0)){
        	thisTransform.Rotate(Vector3.up *-15* Input.GetAxis("Mouse X"));
      	}
	}
}
