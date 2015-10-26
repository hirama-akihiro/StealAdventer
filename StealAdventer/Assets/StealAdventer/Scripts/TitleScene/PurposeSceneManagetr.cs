using UnityEngine;
using System.Collections;

public class PurposeSceneManagetr : MonoBehaviour {

	private bool isLoadLevel;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (isLoadLevel) {
			return;
		}
		if (UserInput.I.PressAnyKey)
		{
			isLoadLevel = true;
			// TitleScene -> StageScene
			AudioManager.I.PlayAudio("seButton");
			//FadeManager.Instance.LoadLevel("InfoScene", 0.5f);
			Application.LoadLevel("InfoScene");
		}
	}
}
