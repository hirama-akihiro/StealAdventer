using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponentInChildren<Text>().text = "×" + ScoreManager.Instance.Coins;
	}
}
