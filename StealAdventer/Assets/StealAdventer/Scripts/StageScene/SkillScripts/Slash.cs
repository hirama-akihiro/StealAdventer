using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {

    // Use this for initialization
    void Start () {
        AudioManager.Instance.PlayAudio("swing");
        Destroy(gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Shot(GameObject spawn)
    {
        gameObject.transform.Translate(new Vector3(-2.0f * (int)spawn.GetComponent<CharacterStatus>().NowAngle, 0.5f, 0.0f));
        //gameObject.transform.rotation = new Quaternion(0.0f, 90.0f, 0.0f, 0.0f);
        gameObject.transform.Rotate(new Vector3(90.0f * (1 + (int)spawn.GetComponent<CharacterStatus>().NowAngle), 90.0f, 0.0f));
    }

    private void OnTriggerEnter(Collider c)
    {
        if (LayerMask.LayerToName(c.gameObject.layer) == LayerNames.Stage_SlashableObject)
        {
            //c.GetComponent<SlashableObject>().Slashing();
			Destroy(c.gameObject);
        }
    }
}
