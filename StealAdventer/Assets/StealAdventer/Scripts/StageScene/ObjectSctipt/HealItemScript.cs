using UnityEngine;
using System.Collections;

public class HealItemScript : MonoBehaviour {

    /// <summary>
    /// HP回復量
    /// </summary>
    public int healValue;

    /// <summary>
    /// 回復エフェクト
    /// </summary>
    public GameObject healEffect;

    /// <summary>
    /// 上下運動の下限
    /// </summary>
    public float moveLimitUnder;

    /// <summary>
    /// 上下運動の上限
    /// </summary>
    public float moveLimitOver;

    /// <summary>
    /// 上下運動の速度
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// 生成地点(運動の中心)
    /// </summary>
    private Vector3 basePosition;

    // Use this for initialization
    void Start () {
        basePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed, transform.position.z);
        if ((transform.position.y - basePosition.y) > moveLimitOver || (transform.position.y - basePosition.y) < moveLimitUnder)
            moveSpeed *= -1;
	}

    void OnTriggerEnter(Collider c)
    {
		if(LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Player || LayerMask.LayerToName(c.gameObject.layer) == LayerNames.MutekiPlayer)
        {
            GameObject effect;
            int hp = c.gameObject.GetComponent<CharacterStatus>().nowHP + healValue;
            c.gameObject.GetComponent<CharacterStatus>().nowHP = Mathf.Min(hp, c.gameObject.GetComponent<CharacterStatus>().maxHP);
            effect = (GameObject)Instantiate(healEffect, c.gameObject.transform.position, healEffect.transform.rotation);
            effect.GetComponent<HealEffectScript>().SetPlayer(c.gameObject);
            Destroy(effect, 1.50f);
            Destroy(gameObject);
        }
    }
}
