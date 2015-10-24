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
		AudioManager.Instance.StopAudio();
		AudioManager.Instance.PlayAudio("Title");
	}

	// Update is called once per frame
	void Update()
	{
		if (isLoadLevel) { return; }

		if (UserInput.Instance.PressAnyKey)
		{
			isLoadLevel = true;
			AudioManager.Instance.PlayAudio("SEButtonClick");
			FadeManager.Instance.LoadLevel("PurposeScene", 0.5f);
		}
	}
}
