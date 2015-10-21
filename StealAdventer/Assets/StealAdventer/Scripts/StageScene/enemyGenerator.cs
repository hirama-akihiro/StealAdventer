using UnityEngine;
using System.Collections;

public class enemyGenerator : MonoBehaviour {

	private GameObject player;
	private GameObject e;
	public GameObject enemy;

	public GameObject generateEffect;

	private bool isSpawn;

	public float X_Area;
	public float Y_Area;

	public bool isRapidGenerate;
	private bool isGenerate;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("SDUnityChan");
		isSpawn = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (isRapidGenerate)
		{
			if (!e && !isGenerate)
			{
				isGenerate = true;
				StartCoroutine(DelayGenerate());
			}
		}
		else
		{
			if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <= X_Area && !isSpawn && !e)
			{
				e = Instantiate(enemy, transform.position, transform.rotation) as GameObject;
				Instantiate(generateEffect, transform.position, transform.rotation);
				isSpawn = true;
			}
			if (Mathf.Abs(this.transform.position.x - player.transform.position.x) >= X_Area)
			{
				isSpawn = false;
			}
		}

		/*
		if (Mathf.Abs (this.transform.position.y - player.transform.position.y) <= Y_Area && !isSpawn) {
			Instantiate(enemy, transform.position, transform.rotation);
			isSpawn = true;
		}
		if (Mathf.Abs (this.transform.position.y - player.transform.position.y) >= Y_Area && isSpawn) {
			isSpawn = false;
		}
		*/

		/*
		if (Mathf.Abs (this.transform.position.x - enemy.transform.position.x) >= 10) {
			isSpawn = false;
			Debug.Log("ok");
		}
		*/
	}

	private IEnumerator DelayGenerate()
	{
		yield return new WaitForSeconds(2);
		isGenerate = false;
		e = Instantiate(enemy, transform.position, transform.rotation) as GameObject;
		Instantiate(generateEffect, transform.position, transform.rotation);
	}
}
