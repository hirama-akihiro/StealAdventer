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
		if (UserInput.Instance.PressSpaceKey)
		{
			isLoadLevel = true;
			// TitleScene -> StageScene
			AudioManager.Instance.PlayAudio("seButton");
			//FadeManager.Instance.LoadLevel("StageScene", 0.5f);
			Application.LoadLevel("StageScene");
		}	
	}
}
