using UnityEngine;
using System.Collections;

/// <summary>
/// キャラクター基底クラス
/// </summary>
public class Character : MonoBehaviour {

	/// <summary>
	/// キャッシュTransform
	/// </summary>
	private Transform cashedTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// キャッシュTransform
	/// </summary>
	public Transform CashedTransform
	{
		get
		{
			if (cashedTransform == null) { cashedTransform = GetComponent<Transform>(); }
			return cashedTransform;
		}
	}
}
