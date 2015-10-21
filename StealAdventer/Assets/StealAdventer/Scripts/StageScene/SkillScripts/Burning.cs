using UnityEngine;
using System.Collections;

public class Burning : MonoBehaviour {

    /// <summary>
    /// 炎上エフェクト(オリジナル)
    /// </summary>
    public GameObject effectObjectOrigin;

    /// <summary>
    /// 炎上エフェクト(コピー)
    /// </summary>
    private GameObject effectObject;

    /// <summary>
    /// 炎上エフェクト
    /// </summary>
    private bool burning;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void OnCollisionEnter(Collision c)
    {
        if(LayerMask.LayerToName(c.gameObject.layer) == "" && !burning){
            burning = true;
            effectObject = GameObject.Instantiate(effectObjectOrigin);
            effectObject.transform.position = this.transform.position;
            StartCoroutine(ObjDestroy(c));
        }
    }

    private IEnumerator ObjDestroy(Collision c)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(effectObject);
        Destroy(c.gameObject);
        burning = false;
    }
}
