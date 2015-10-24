using UnityEngine;
using System.Collections;

public class ThunderSpark : MonoBehaviour {

	public GameObject spark;

	private float spawnTime;
	private float time; 
	public float finishTime;

	private int randPosi;
	private float randPosi_X;
	private float randPosi_Y;
	private float randPosi_Z;

	private Vector3 nVec;
	private Vector3 bVec;
	private Quaternion lineRotate;

	public GameObject moveLine;

	// Use this for initialization
	void Start () {
		time = 0.0f;
		spawnTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		spawnTime += Time.deltaTime;
		bVec = nVec;
		randPosi_X = Random.Range (-9, 9);
		randPosi_Y = Random.Range (-2, 6);
		randPosi_Z = Random.Range (-3, 4);
		nVec = new Vector3 (randPosi_X, randPosi_Y, randPosi_Z);
		if (spawnTime > 0.3 ) {
			AudioManager.Instance.PlayAudio("SEThunder");
			Instantiate(spark, new Vector3(this.transform.position.x + randPosi_X, this.transform.position.y + randPosi_Y, this.transform.position.z + randPosi_Z), this.transform.rotation);
			//CompareVector(nVec, bVec);
			//Instantiate(moveLine, new Vector3(this.transform.position.x + randPosi_X, this.transform.position.y + randPosi_Y, this.transform.position.z + randPosi_Z), lineRotate);
			spawnTime = 0;
		}
		if (time >= finishTime)
			Destroy (gameObject);
	}

	void CompareVector(Vector3 n, Vector3 b){
		float x, y, z;

		if (n.x > b.x)
			x = 1.0f;
		else
			x = -1.0f;

		if (n.y > b.y)
			y = 1.0f;
		else
			y = -1.0f;

		if (n.z > b.z)
			z = 1.0f;
		else
			z = -1.0f;

		lineRotate.eulerAngles = new Vector3 (90 * x, 90 * y, 90 * z);
	}
}
