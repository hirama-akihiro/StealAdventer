using UnityEngine;
using System.Collections;

public class FlamePillar : MonoBehaviour {

	public GameObject fireBurst;
	private GameObject player;

	private bool generating;

	private float timer;

	private int count;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("SDUnityChan");
		generating = false;
		transform.position = player.transform.position;
		timer = 0.5f;
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		
		if (!generating && timer <= 0) {
			generating = true;
			timer = 1.0f;
			//transform.position = player.transform.position;
		}

		if (generating && timer <= 0) {
			AudioManager.I.PlayAudio("syaggan");
			Instantiate(fireBurst, transform.position, transform.rotation);
			generating = false;
			timer = 1f;
			count += 1;
			transform.position = player.transform.position;
		}

		if (count >= 1)
			Destroy (gameObject);



	}
	
	public void Shot(GameObject spawn)
	{
	}
}
