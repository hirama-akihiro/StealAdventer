using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    /// <summary>
    /// 蒸発エフェクト(オリジナル)
    /// </summary>
    public GameObject effectObjectOrigin;

    /// <summary>
    /// 蒸発エフェクト(コピー)
    /// </summary>
    private GameObject effectObject;

    /// <summary>
    /// 武器のゲームオブジェクト
    /// </summary>
    private GameObject spawn;

    /// <summary>
    /// 攻撃のSE名
    /// </summary>
    public string SEName = "Bubble";

    // Use this for initialization
    void Start () {
        //Invoke("ColliderOn", 0.3f);
        AudioManager.Instance.PlayAudio(SEName);
    }
	
	// Update is called once per frame
	void Update () {
        posUpdate();
        if (!AudioManager.Instance.IsPlaying(SEName))
            AudioManager.Instance.PlayAudio(SEName);
    }

    private void posUpdate()
    {
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(spawn.transform.position.x + ((int)spawn.GetComponent<Character>().NowAngle * 2.0f), spawn.transform.position.y + 0.5f, spawn.transform.position.z);
            transform.rotation = spawn.transform.rotation;
            transform.Rotate(0, 0, 90f);
        }
        else
        {
            AudioManager.Instance.StopAudio(SEName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        //Debug.Log(c.gameObject.layer);
        if (LayerMask.LayerToName(c.gameObject.layer) == "Fire")
        {
            AudioManager.Instance.PlayAudio("SEShoka");
            effectObject = GameObject.Instantiate(effectObjectOrigin);
            effectObject.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z - 0.5f);
            Destroy(effectObject,3.0f);
            Destroy(c.gameObject);  
        }
    }

    private void ColliderOn()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void Shot(GameObject spawn)
    {
        this.spawn = spawn;
        //Invoke("ColliderOn", 0.3f);
    }
}
