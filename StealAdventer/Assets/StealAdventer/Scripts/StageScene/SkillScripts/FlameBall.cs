using UnityEngine;
using System.Collections;

public class FlameBall : MonoBehaviour {

    /// <summary>
    /// スキル移動速度
    /// </summary>
    public float skillSpeed = 5.0f;

    /// <summary>
    /// 炎上エフェクト(オリジナル)
    /// </summary>
    public GameObject effectObjectOrigin;

    /// <summary>
    /// 炎上エフェクト(コピー)
    /// </summary>
    private GameObject effectObject;

    /// <summary>
    /// 武器のゲームオブジェクト
    /// </summary>
    public GameObject spawn;

    /// <summary>
    /// スキル発射時のプレイヤーの向き
    /// </summary>
    private int spawnVector;

	// Use this for initialization
	void Start () {
        //spawn = GameObject.Find("SkillGeneratePoint");
        //GetComponent<Rigidbody>().velocity = spawn.transform.right * skillSpeed;
        AudioManager.Instance.PlayAudio("SEFrameBall");
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0.0f, 0.0f, spawnVector * 0.15f));
    }

    private void OnTriggerEnter(Collider c)
    {
        //Debug.Log(c.gameObject.layer);
        if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Stage_FlammableObject)
        {
            //effectObject = GameObject.Instantiate(effectObjectOrigin);
            //effectObject.transform.position = c.transform.position;
			c.gameObject.GetComponent<FlammableObject>().Burning();
			Destroy(c.gameObject, 1.5f);
        }
        Destroy(gameObject);
    }

    public void Shot(GameObject spawn)
    {
        spawnVector = (int)spawn.GetComponent<Character>().NowAngle;
    }
}
