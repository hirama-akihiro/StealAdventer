using UnityEngine;
using System.Collections;

public class HealEffectScript : MonoBehaviour {

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    private GameObject player;

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayAudio("Heal");
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
            transform.position = player.transform.position;
	}

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
