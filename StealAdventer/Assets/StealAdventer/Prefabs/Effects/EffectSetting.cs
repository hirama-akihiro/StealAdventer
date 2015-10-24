using UnityEngine;
using System.Collections;

public class EffectSetting : MonoBehaviour {

	public float surviveTime = 1.5f;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, surviveTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
