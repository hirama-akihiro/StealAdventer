using UnityEngine;
using System.Collections;

public class SkillGetObject : MonoBehaviour {

	/// <summary>
	/// スキルオブジェクト
	/// </summary>
	public GameObject skillObjet;

	/// <summary>
	/// データ表示様キャンバス
	/// </summary>
	private GameObject dataCanvas;

	// Use this for initialization
	void Start () {
		dataCanvas = GameObject.Find("DataCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter(Collider collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerName == LayerNames.Player || layerName == LayerNames.MutekiPlayer)
		{
			if (skillObjet)
			{
				collider.gameObject.GetComponent<Character>().skillObject = skillObjet;
				dataCanvas.transform.FindChild("Skill").GetComponent<SkillImageScript>().ChangeSkillImage();
				collider.gameObject.SendMessage("InstansiateGetSkillEffect");
				AudioManager.Instance.PlayAudio("SkillGet");
			}
		}
	}
}
