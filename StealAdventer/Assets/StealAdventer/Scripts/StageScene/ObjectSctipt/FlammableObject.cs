using UnityEngine;
using System.Collections;

public class FlammableObject : MonoBehaviour {

    /// <summary>
    /// 炎上エフェクトオブジェクト
    /// </summary>
    public GameObject EffectObject;

	private GameObject tmpEffectObject;

    /// <summary>
    /// 炎上しているかどうか
    /// </summary>
    private bool isBurning;

	public Vector3 offset;

	// Use this for initialization
	void Start () {
        isBurning = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 炎上処理
    /// </summary>
    public void Burning()
    {
        if (!isBurning)
        {
            tmpEffectObject = (GameObject)Instantiate(EffectObject, transform.position + offset, EffectObject.transform.rotation);
            isBurning = true;
        }
    }

    void OnDestroy()
    {
        Destroy(tmpEffectObject);
    }
}
