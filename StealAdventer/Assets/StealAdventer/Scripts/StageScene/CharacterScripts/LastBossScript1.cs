using UnityEngine;
using System.Collections;

public class LastBossScript : MonoBehaviour {

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
			// StageScene2 -> CregitScene
			Application.LoadLevel(3);
		}
	}
}
