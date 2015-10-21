using UnityEngine;
using System.Collections;

public class SkillBaseScript : MonoBehaviour {

	/// <summary>
	/// スキル移動速度
	/// </summary>
	public float skillSpeed = 5.0f;


	// Use this for initialization
	void Start()
	{
        //GetComponent<Rigidbody>().velocity = transform.right * skillSpeed;
		//spawn = GameObject.Find("SkillGeneratePoint");
		GetComponent<Rigidbody>().velocity = GameObject.Find("SkillGeneratePoint").transform.right * skillSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	/// <summary>
	/// 引数の方向にスキルが発射される
	/// </summary>
	/// <param name="direction"></param>
	public void Move(Vector3 direction)
	{
        GetComponent<Rigidbody>().velocity = direction.normalized * skillSpeed;
	}

	private void OnCollisionEnter()
    {
		// 衝突時に自身を削除する
        //Destroy(this.gameObject);
    }
}
