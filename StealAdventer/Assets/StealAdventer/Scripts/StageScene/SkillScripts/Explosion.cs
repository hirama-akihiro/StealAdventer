using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    /// <summary>
    /// コライダーオフまでの時間
    /// </summary>
    public float colliderOffTime;

	// Use this for initialization
	void Start () {
        StartCoroutine(ColliderOff());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Stage_BreakableObject)
        {
            Destroy(c.gameObject);
        }
        else if(LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Stage_FlammableObject)
        {
            c.GetComponent<FlammableObject>().Burning();
            Destroy(c.gameObject, 3.0f);
        }
    }

    IEnumerator ColliderOff()
    {
        yield return new WaitForSeconds(colliderOffTime);
        this.GetComponent<SphereCollider>().enabled = false;
    }
}
