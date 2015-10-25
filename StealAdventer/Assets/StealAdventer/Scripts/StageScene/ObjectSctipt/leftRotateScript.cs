using UnityEngine;
using System.Collections;

public class leftRotateScript : MonoBehaviour {

	/// <summary>
	/// 回転数
	/// </summary>
	private int rotateCount = 0;

	/// <summary>
	/// 回転角度
	/// </summary>
	public float ungle;

    // Use this for initialization
    void Start()
    {
		IsTrapOn = false;
    }

    // Update is called once per frame
    public void Update()
    {
		if (IsTrapOn) { Rotate(); }
    }

	/// <summary>
	/// 回転 
	/// </summary>
	public void Rotate()
	{
		if (rotateCount <= ungle) {
			transform.Rotate (new Vector3 (0, 0, -1), 1);
			rotateCount++;
		}
	}

	public bool IsTrapOn { get; set; }
}