using UnityEngine;
using System.Collections;

public class DestroyArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerName == LayerNames.Player || layerName == LayerNames.MutekiPlayer)
		{
			if (!collider.gameObject.GetComponent<UnityChanController>().IsWorping)
			{
				collider.gameObject.GetComponent<CharacterStatus>().nowHP = -1;
			}
		}
		if (layerName == LayerNames.NormalEnemy)
		{
			Destroy(collider.gameObject);
		}
	}
}
