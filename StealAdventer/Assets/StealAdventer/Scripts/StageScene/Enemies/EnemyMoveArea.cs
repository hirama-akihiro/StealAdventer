using UnityEngine;
using System.Collections;

public class EnemyMoveArea : MonoBehaviour {

	public float X_Area;
	public float Y_Area;

	private Transform startPosition;

	private GameObject player;

	// Use this for initialization
	void Awake () {
		startPosition = this.transform;
		player = GameObject.Find("SDUnityChan");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (startPosition.position.x - player.transform.position.x) >= X_Area) {
			Destroy(gameObject);
		}
		/*if (Mathf.Abs (startPosition.position.y - player.transform.position.y) >= Y_Area) {
			Destroy(gameObject);
		}*/
		//Debug.Log (startPosition.position.x);
	}
}
