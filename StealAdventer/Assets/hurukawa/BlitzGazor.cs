using UnityEngine;
using System.Collections;

public class BlitzGazor : MonoBehaviour {

	//public GameObject gazor;

	public float durationTime;

	private GameObject spawn;
	
	private GameObject mino;

	private GameObject player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;
		if (durationTime <= 0)
			Destroy (gameObject);
	}

	public void Shot(GameObject spawn)
	{
		/*
		mino = GameObject.Find ("Minotaur");
		this.spawn = spawn;
		gameObject.transform.position = this.spawn.transform.position;
		if (this.spawn.transform.position.x >= mino.transform.position.x)
			transform.rotation = Quaternion.Euler (0, 90, 0);
		else
			transform.rotation = Quaternion.Euler (0, -90, 0);
		*/
		player = GameObject.Find("SDUnityChan");
		int angle;
		if(gameObject.transform.position.x <= player.transform.position.x)
			angle = -1;
		else
			angle = 1;
		transform.rotation = Quaternion.Euler (0, -90 * angle, 0);
	}
}
