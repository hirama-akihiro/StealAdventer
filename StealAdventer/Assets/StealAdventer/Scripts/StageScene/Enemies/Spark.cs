using UnityEngine;
using System.Collections;

public class Spark : MonoBehaviour {

	public float durationTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;
		if (durationTime <= 0)
			Destroy (gameObject);
	}
}
