using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {

	private bool isTouch;
	public GameObject respawnEffect;
	public GameObject effectPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider collider)
	{
		if (isTouch) { return; }
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if (layerName != LayerNames.Player && layerName != LayerNames.MutekiPlayer) { return ;}
		RespawnManager.I.respawnPosition = transform.position; 
		if (respawnEffect) { Instantiate(respawnEffect,effectPoint.transform.position, transform.rotation); }
		AudioManager.I.PlayAudio("SERespawnPoint");
		isTouch = true;
	}
}
