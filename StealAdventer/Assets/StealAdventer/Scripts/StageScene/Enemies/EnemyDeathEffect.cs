using UnityEngine;
using System.Collections;

public class EnemyDeathEffect : MonoBehaviour {

	//持続時間
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
