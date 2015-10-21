using UnityEngine;
using System.Collections;

public class TriggerScriptF : MonoBehaviour {

	CylinderGenerator cylinderGenerator;
	
	public GameObject CG;
	
	// Use this for initialization
	void Start () {
		cylinderGenerator = new CylinderGenerator ();
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
			
			//Debug.Log ("false");
		}
	}
}
