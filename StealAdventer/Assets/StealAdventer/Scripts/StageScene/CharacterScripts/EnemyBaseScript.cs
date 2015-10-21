using UnityEngine;
using System.Collections;

/// <summary>
/// 各エネミーキャラクター持つ共通の機能
/// XNAでいう基底クラスに該当
/// </summary>
public class EnemyBaseScript: MonoBehaviour {

	/// <summary>
	/// キャラクターステータス
	/// </summary>
	private CharacterStatus status;

	// Use this for initialization
	void Start () {
		status = GetComponent<CharacterStatus>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collider)
	{
		string layerName = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerName == "Skill(Player)")
		{
		}
	}
}
