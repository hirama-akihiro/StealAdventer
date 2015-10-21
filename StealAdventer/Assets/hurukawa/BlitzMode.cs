using UnityEngine;
using System.Collections;

public class BlitzMode : MonoBehaviour {

	private Minotaur minotaur;

	public GameObject changeEffect;
	public GameObject moveEffect;
	public GameObject spark;
	public GameObject BlitzGazor;

	private GameObject mino ;

	private Quaternion zeroRotate;

	// Use this for initialization
	void Start () {
		mino = GameObject.Find("Minotaur");
		zeroRotate.eulerAngles = new Vector3 (270, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// 通常攻撃
	/// </summary>
	public void BlitzAttack_1(){
	}

	public void changeBlitz(){
		Instantiate (changeEffect, mino.transform.position, zeroRotate);
		//Instantiate (changeEffect, spawn.transform.position, spawn.transform.rotation);
	}
}
