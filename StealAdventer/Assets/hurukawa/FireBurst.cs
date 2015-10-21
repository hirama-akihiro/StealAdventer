using UnityEngine;
using System.Collections;

public class FireBurst : MonoBehaviour {

	//持続時間
	public float durationTime;

	private BoxCollider boxCollider;
	
	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;

		if (durationTime <= 4)
			Destroy (boxCollider);

		if (durationTime <= 0)
			Destroy (gameObject);
	}
	
	public void Shot(GameObject spawn)
	{
	}
}
