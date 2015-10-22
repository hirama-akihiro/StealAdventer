using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillDisplay : MonoBehaviour {

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    public GameObject player;

    /// <summary>
    /// スキル表示パネル
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// 表示スキル
    /// </summary>
    private GameObject displayObject;

	/// <summary>
	/// カメラオブジェクト
	/// </summary>
    public Camera camera;

    // Use this for initialization
    void Start () {
        SkillChange();
	}
	
	// Update is called once per frame
	void Update () {
        if (displayObject != null)
        {
            Vector3 position = transform.GetComponent<RectTransform>().position;
            position.z = -1.0f - camera.transform.position.z;
            displayObject.transform.position = camera.ScreenToWorldPoint(position);
        }
    }

    /// <summary>
    /// HPを表示する対象のゲームオブジェクト
    /// </summary>
    void SkillChange()
    {
        if (player.GetComponent<CharacterStatus>().skillObject.GetComponent<SkillObjectScript>().DisplaySkillObject != null)
        {
            displayObject = Instantiate(player.GetComponent<CharacterStatus>().skillObject.GetComponent<SkillObjectScript>().DisplaySkillObject);
            Vector3 position = transform.GetComponent<RectTransform>().position;
            position.z = -1.0f - camera.transform.position.z;
            displayObject.transform.position = camera.ScreenToWorldPoint(position);
            Debug.Log(displayObject.transform.position.x);
            Debug.Log(displayObject.transform.position.y);
            Debug.Log(displayObject.transform.position.z);
            Debug.Log(position.x);
            Debug.Log(position.y);
            Debug.Log(position.z);

        }
        else
            displayObject = null;
    }
}
