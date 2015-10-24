using UnityEngine;
using System.Collections;

public class SkillGeneratePointScript: MonoBehaviour
{
    /// <summary>
    /// GameObject型SkillObject
    /// </summary>
    private GameObject skillObject;

	/// <summary>
	/// Stealの際にエネミーへ飛ばすハンドオブジェクト(オリジナル)
	/// </summary>
	public GameObject orgStealHand;

	/// <summary>
	/// Stealの際にエネミーへ飛ばすハンドオブジェクト(インスタンス)
	/// </summary>
	private GameObject cpyStealHand;

	/// <summary>
	/// スキル発射オブジェクト
	/// </summary>
	public Transform spawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
    }

	public void GenerateStealHand()
	{
		cpyStealHand = Instantiate(orgStealHand);
		cpyStealHand.transform.position = spawn.position + new Vector3(0, -0.25f, 0);
		cpyStealHand.transform.Rotate(0, spawn.rotation.y * 180, 0);
	}

	/// <summary>
	/// 弾を発射
	/// </summary>
	public void GenerateSkill(GameObject skillObject, string layerName)
	{
		this.skillObject = Instantiate(skillObject);
		this.skillObject.transform.position = spawn.position;
		this.skillObject.layer = LayerMask.NameToLayer(layerName);

		this.skillObject.SendMessage("Shot", gameObject);
	}
}
