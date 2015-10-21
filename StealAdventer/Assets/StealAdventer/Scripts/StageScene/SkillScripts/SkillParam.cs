using UnityEngine;
using System.Collections;

public class SkillParam : MonoBehaviour {

	/// <summary>
	/// ダメージ量
	/// </summary>
	public int damege;

	/// <summary>
	/// クールタイム
	/// </summary>
	public float coolTime;

	/// <summary>
	/// 
	/// </summary>
	public enum SkillType { Short, Long };

	/// <summary>
	/// スキルタイプ
	/// </summary>
	public SkillType skillType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
