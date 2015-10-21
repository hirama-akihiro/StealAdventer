using UnityEngine;
using System.Collections;

public class SlashableObject : MonoBehaviour {

    /// <summary>
	/// オブジェクト破壊時の点滅時間
	/// </summary>
    public float blinkerTime;

    /// <summary>
	/// オブジェクト破壊時の点滅間隔
	/// </summary>
    public float blinkerInterval;

    /// <summary>
	/// 表示(または非表示)状態開始からの時間
	/// </summary>
    private float blinkElapsedTime;

    /// <summary>
	/// オブジェクトが切断されたかどうか
	/// </summary>
    private bool isSlashed = false;

    // Use this for initialization
    void Start () {
        blinkElapsedTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(isSlashed)
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
            if(blinkerTime <= 0.0f)
                Destroy(gameObject);
        }
	}

    public void Slashing()
    {
        isSlashed = true;
    }

}
