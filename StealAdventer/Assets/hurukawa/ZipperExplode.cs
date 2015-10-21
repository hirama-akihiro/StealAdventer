using UnityEngine;
using System.Collections;

public class ZipperExplode : MonoBehaviour {

	public float durationTime;

	private SphereCollider sphreCollider;

	// Use this for initialization
	void Start () {
		sphreCollider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;

		if (durationTime <= 3)
			Destroy (sphreCollider);

		if (durationTime <= 0)
			Destroy (gameObject);
	}
}
