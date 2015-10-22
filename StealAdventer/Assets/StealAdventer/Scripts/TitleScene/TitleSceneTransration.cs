using UnityEngine;
using System.Collections;

public class TitleSceneTransration : MonoBehaviour {
	private bool isLoadLevel;
	// Use this for initialization
	void Start () {
		AudioManager.Instance.StopAudio();
		AudioManager.Instance.PlayAudio("Title");
	}
	
	// Update is called once per frame
	void Update () {
		if (isLoadLevel) {
			return;
		}
		if (UserInput.Instance.PressAnyKey)
		{
			isLoadLevel = true;
			// TitleScene -> StageScene
			AudioManager.Instance.PlayAudio("seButton");
			//FadeManager.Instance.LoadLevel("PurposeScene", 0.5f);
			Application.LoadLevel("PurposeScene");
		}
	}}
