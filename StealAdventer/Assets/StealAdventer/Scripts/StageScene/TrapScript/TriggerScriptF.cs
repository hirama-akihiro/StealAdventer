using UnityEngine;
using System.Collections;

public class TriggerScriptF : MonoBehaviour {
	
	public GameObject CG;
	
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
			CG.GetComponent<CylinderGenerator> ().IsTrapOn = false;
		}
	}
}
