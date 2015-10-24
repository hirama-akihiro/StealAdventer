using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

    /// <summary>
	/// オブジェクト消滅直前の点滅時間
	/// </summary>
    public float blinkerTime;

    /// <summary>
	/// オブジェクト消滅直前の点滅間隔
	/// </summary>
    public float blinkerInterval;

    /// <summary>
	/// 表示(または非表示)状態開始からの時間
	/// </summary>
    private float blinkElapsedTime;

    /// <summary>
    /// 岩を投げる力
    /// </summary>
    public float power;

    /// <summary>
    /// 岩投射角度
    /// </summary>
    public float degree;

    /// <summary>
    /// 武器のゲームオブジェクト
    /// </summary>
    private GameObject spawn;

    /// <summary>
    /// 岩消滅までの時間
    /// </summary>
    public float liveTime;

    /// <summary>
    /// オブジェクト生成からの時間
    /// </summary>
    private float elapsedTime = 0.0f;

	private float power_x;
	private float power_y;


	private float maxYPos;
	private Rigidbody myRigidbody;

    // Use this for initialization
    void Start () {
		myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= (liveTime - blinkerTime))
        {
            if (blinkerInterval <= blinkElapsedTime)
            {
                blinkElapsedTime = 0.0f;
                // 自身と子オブジェクトのRendererを取得
                Renderer[] objList = GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in objList)
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
            blinkElapsedTime += Time.deltaTime;
            blinkerTime -= Time.deltaTime;
            if (blinkerTime <= 0.0f)
                Destroy(gameObject);
        }
    }

    public void Shot(GameObject spawn)
    {
        this.spawn = spawn;
        gameObject.transform.position = this.spawn.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        power_x = Mathf.Cos(Mathf.PI / 180.0f * degree) * (int)this.spawn.GetComponent<Character>().NowAngle * power;
        power_y = Mathf.Sin(Mathf.PI / 180.0f * degree) * power;
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(power_x, power_y, 0));
    }

	public void OnTriggerEnter(Collider collider)
	{
		if (LayerMask.LayerToName(collider.gameObject.layer) == LayerNames.Stage_FloorObject || LayerMask.LayerToName(collider.gameObject.layer) == LayerNames.Stage_TrapObject && LayerMask.LayerToName(gameObject.layer) != LayerNames.Stage_FloorObject)
		{
			gameObject.layer = LayerMask.NameToLayer(LayerNames.Stage_FloorObject);
			myRigidbody.isKinematic = true;
			// 地面落下後は移動させない
			//myRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			//myRigidbody.isKinematic = true;
			if (!AudioManager.Instance.IsPlaying("Stone"))
				AudioManager.Instance.PlayAudio("Stone");
		}
	}
}
