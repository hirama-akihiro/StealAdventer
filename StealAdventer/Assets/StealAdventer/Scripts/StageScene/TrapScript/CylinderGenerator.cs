using UnityEngine;
using System.Collections;

public class CylinderGenerator : MonoBehaviour {

	public GameObject cylinder;
	private int count;
	private float count1;
	//private int limit;
	public float generateSec;
	// Use this for initialization
	public bool IsTrapOn{ get; set;}

	void Start () {
		IsTrapOn = false;
		count1 = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsTrapOn == true) {
			count1 += Time.deltaTime;
			if (generateSec <= count1) {
				Instantiate (cylinder, transform.position, cylinder.transform.rotation);
				count1 = 0;
			}
		}
	}
}
