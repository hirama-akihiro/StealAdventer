using UnityEngine;
using System.Collections;

public class DragonScript : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
