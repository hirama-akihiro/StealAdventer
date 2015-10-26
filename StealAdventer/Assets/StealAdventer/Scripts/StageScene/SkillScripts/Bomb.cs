using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    /// <summary>
    /// 爆弾を投げる力
    /// </summary>
    public float power;

    /// <summary>
    /// 爆弾投射角度
    /// </summary>
    public float degree;

    /// <summary>
    /// 爆発エフェクト(オリジナル)
    /// </summary>
    public GameObject effectObjectOrigin;

    /// <summary>
    /// 爆発エフェクト(コピー)
    /// </summary>
    private GameObject effectObject;

    /// <summary>
    /// 武器のゲームオブジェクト
    /// </summary>
    private GameObject spawn;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider c)
    {
        effectObject = GameObject.Instantiate(effectObjectOrigin);
        effectObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);

        AudioManager.I.PlayAudio("SEExplosion");
        Destroy(effectObject, 1.5f);
        Destroy(gameObject);
    }

    public void Shot(GameObject spawn)
    {
        this.spawn = spawn;
        gameObject.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 0.5f, spawn.transform.position.z);
        float power_x = Mathf.Cos(Mathf.PI / 180.0f * degree) * (int)this.spawn.GetComponent<Character>().NowAngle * power;
        float power_y = Mathf.Sin(Mathf.PI / 180.0f * degree) * power;
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(power_x, power_y, 0));        
    }
}
