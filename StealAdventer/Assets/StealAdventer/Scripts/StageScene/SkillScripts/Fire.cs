using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

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
    private GameObject spawn;

    /// <summary>
    /// 攻撃のSE名
    /// </summary>
    public string seName = "Burning";

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayAudio(seName);
    }
	
	// Update is called once per frame
	void Update () {
        if (!AudioManager.Instance.IsPlaying(seName))
            AudioManager.Instance.PlayAudio(seName);
        posUpdate();
    }

	private void posUpdate()
	{
		if (Input.GetKey(KeyCode.S))
		{
			transform.position = new Vector3(spawn.transform.position.x + (int)spawn.GetComponent<CharacterStatus>().NowAngle, spawn.transform.position.y + 0.5f, 0);
			transform.rotation = spawn.transform.rotation;
			transform.Rotate(0.0f, 90.0f, 90.0f);
		}
		else
		{
			AudioManager.Instance.StopAudio(seName);
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Stage_FlammableObject)
		{
			effectObject = GameObject.Instantiate(effectObjectOrigin);
			effectObject.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z - 0.5f);
			Destroy(effectObject, 3.0f);
			Destroy(c.gameObject, 3.0f);
		}
	}

    private void ColliderOn()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void Shot(GameObject spawn)
    {
        this.spawn = spawn;
        Invoke("ColliderOn", 0.3f);
    }
}
