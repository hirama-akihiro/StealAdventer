using UnityEngine;
using System.Collections;

public class MediumBossScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if (layerName == "Skill(Player)")
		{
			// StageScene1 -> StageScene2
			Application.LoadLevel(2);
		}
	}
}
