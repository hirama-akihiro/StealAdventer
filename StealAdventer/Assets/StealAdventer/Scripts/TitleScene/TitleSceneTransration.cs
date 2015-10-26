using UnityEngine;
using System.Collections;

public class TitleSceneTransration : MonoBehaviour
{
	/// <summary>
	/// シーンのロード状態か
	/// </summary>
	private bool isLoadLevel;

	// Use this for initialization
	void Start()
	{
		AudioManager.I.StopAudio();
		AudioManager.I.PlayAudio("BGMTitle");
	}

	// Update is called once per frame
	void Update()
	{
		if (isLoadLevel) { return; }

		if (UserInput.I.PressAnyKey)
		{
			isLoadLevel = true;
			AudioManager.I.PlayAudio("SEButtonClick");
			FadeManager.Instance.LoadLevel("PurposeScene", 0.5f);
		}
	}
}
