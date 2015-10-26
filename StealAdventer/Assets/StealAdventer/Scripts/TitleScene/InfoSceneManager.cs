using UnityEngine;
using System.Collections;

public class InfoSceneManager : MonoBehaviour {


	private bool isLoadLevel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLoadLevel) {
			return;
		}
		if (UserInput.I.PressAnyKey)
		{
			isLoadLevel = true;
			AudioManager.I.PlayAudio("SEButtonClick");
			FadeManager.Instance.LoadLevel("StageScene", 0.5f);
		}	
	}
}
