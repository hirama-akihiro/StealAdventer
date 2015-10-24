using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    /// <summary>
    /// コイン1枚あたりのポイント
    /// </summary>
    public int point;

    /// <summary>
    /// コインの回転速度
    /// </summary>
    public float speed = 10.0f;

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    private GameObject player;

    private int i = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 20.0f, gameObject.transform.rotation.z, 0.0f);
        transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 10.0f, gameObject.transform.rotation.z);
        if (player != null)
        {
            i++;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
        }
	}

    private void OnCollisionEnter(Collision c)
    {
        if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Player)
        {
            player = c.gameObject;
            //ScoreManager.Instance.AddScore(point);
            ScoreManager.Instance.AddCoin(point);
            AudioManager.Instance.PlayAudio("SECoin");
            GetComponent<SphereCollider>().isTrigger = true;
            Destroy(gameObject,1.0f);
        }
    }

	private void OnTriggerEnter(Collider c)
	{
		if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Player || LayerMask.LayerToName(c.gameObject.layer) == LayerNames.MutekiPlayer)
		{
			player = c.gameObject;
			//ScoreManager.Instance.AddScore(point);
			ScoreManager.Instance.AddCoin(point);
			AudioManager.Instance.PlayAudio("SECoin");
			GetComponent<SphereCollider>().isTrigger = true;
			Destroy(gameObject,1.0f);
		}
	}
}
