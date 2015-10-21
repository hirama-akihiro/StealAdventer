using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillImageScript : MonoBehaviour {

    /// <summary>
    /// プレイヤーのゲームオブジェクト
    /// </summary>
    public GameObject player;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

    }

    /// <summary>
    /// ステータスのスキル画像を現在プレイヤーが持っているスキルのものに変更
    /// </summary>
    public void ChangeSkillImage()
    {
        if (player.GetComponent<CharacterStatus>().skillObject != null)
        {
            transform.FindChild("SkillImage").gameObject.SetActive(true);
            this.GetComponentInChildren<RawImage>().texture = player.GetComponent<CharacterStatus>().skillObject.GetComponent<SkillObjectScript>().SkilImage;
        }
        else
            transform.FindChild("SkillImage").gameObject.SetActive(false);
    }
}
